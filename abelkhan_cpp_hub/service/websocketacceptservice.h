
#ifndef _websocket_acceptservice_h
#define _websocket_acceptservice_h

#include <thread>
#include <functional>
#include <exception>

#include <boost/signals2.hpp>

#include "msque.h"
#include "websocketchannel.h"
#include "process_.h"

namespace service
{

class webacceptservice {
public:
	webacceptservice(std::string ip, short port, bool is_ssl, std::string _certificate_chain_file, std::string _private_key_file, std::string _tmp_dh_file, std::shared_ptr<juggle::process> process)
		: th([this, is_ssl, port]() {
			if (is_ssl) {
				asio_tls_server = std::make_shared<websocketpp::server<websocketpp::config::asio_tls> >();
				asio_tls_server->init_asio();

				asio_tls_server->set_tls_init_handler(websocketpp::lib::bind(&webacceptservice::on_tls_init, this, websocketpp::lib::placeholders::_1));

				asio_tls_server->set_access_channels(websocketpp::log::alevel::none);
				asio_tls_server->set_error_channels(websocketpp::log::elevel::none);

				asio_tls_server->set_open_handler(websocketpp::lib::bind(&webacceptservice::onAccept, this, websocketpp::lib::placeholders::_1));
				asio_tls_server->set_close_handler(websocketpp::lib::bind(&webacceptservice::onChannelDisconn, this, websocketpp::lib::placeholders::_1));
				asio_tls_server->set_message_handler(websocketpp::lib::bind(&webacceptservice::onMsg, this, websocketpp::lib::placeholders::_1, websocketpp::lib::placeholders::_2));

				asio_tls_server->listen(port);
				asio_tls_server->start_accept();

				while (1) {
					try {
						asio_tls_server->run();
					}
					catch (std::exception e) {
						spdlog::error("err:{0}", e.what());
					}
				}
			}
			else {
				asio_server = std::make_shared<websocketpp::server<websocketpp::config::asio> >();
				asio_server->init_asio();

				asio_server->set_access_channels(websocketpp::log::alevel::none);
				asio_server->set_error_channels(websocketpp::log::elevel::none);

				asio_server->set_open_handler(websocketpp::lib::bind(&webacceptservice::onAccept, this, websocketpp::lib::placeholders::_1));
				asio_server->set_close_handler(websocketpp::lib::bind(&webacceptservice::onChannelDisconn, this, websocketpp::lib::placeholders::_1));
				asio_server->set_message_handler(websocketpp::lib::bind(&webacceptservice::onMsg, this, websocketpp::lib::placeholders::_1, websocketpp::lib::placeholders::_2));

				asio_server->listen(port);
				asio_server->start_accept();

				while (1) {
					try {
						asio_server->run();
					}
					catch (std::exception e) {
						spdlog::error("err:{0}", e.what());
					}
				}
			}
		}) 
	{
		_is_ssl = is_ssl;
		_process = process;

		certificate_chain_file = _certificate_chain_file;
		private_key_file = _private_key_file;
		tmp_dh_file = _tmp_dh_file;
	}

	websocketpp::lib::shared_ptr<websocketpp::lib::asio::ssl::context> on_tls_init(websocketpp::connection_hdl hdl) {
		namespace asio = websocketpp::lib::asio;

		auto ctx = websocketpp::lib::make_shared<asio::ssl::context>(asio::ssl::context::sslv23);
		try {
			ctx->set_options(
				asio::ssl::context::default_workarounds | 
				asio::ssl::context::no_sslv2 |
				asio::ssl::context::single_dh_use);
	
			ctx->use_certificate_chain_file(certificate_chain_file);
			ctx->use_private_key_file(private_key_file, asio::ssl::context::pem);
			ctx->use_tmp_dh_file(tmp_dh_file);
		}
		catch (std::exception& e) {
			std::cout << "Exception: " << e.what() << std::endl;
		}
		return ctx;
	}

	boost::signals2::signal<void(std::shared_ptr<juggle::Ichannel>)> sigchannelconnectexception;
	boost::signals2::signal<void(std::shared_ptr<juggle::Ichannel>)> sigchannelconnect;
	void onAccept(websocketpp::connection_hdl hdl) {
		if (_is_ssl) {
			auto ch = std::make_shared<webchannel>(asio_tls_server, hdl);
			ch->sigdisconn.connect(boost::bind(&webacceptservice::ChannelDisconn, this, _1));
			ch->sigconnexception.connect([this](std::shared_ptr<webchannel> _ch){
				_ch->disconnect();

				if (!sigchannelconnectexception.empty()) {
					sigchannelconnectexception(_ch);
				}

				gc_fn_que.push([this, _ch]() {
					_process->unreg_channel(_ch);
				});
			});

			_chs_mu.lock();
			_chs.insert(std::make_pair(hdl.lock().get(), ch));
			_chs_mu.unlock();

			_process->reg_channel(ch);

			sigchannelconnect(ch);
		}
		else {
			auto ch = std::make_shared<webchannel>(asio_server, hdl);
			ch->sigdisconn.connect(boost::bind(&webacceptservice::ChannelDisconn, this, _1));
			ch->sigconnexception.connect([this](std::shared_ptr<webchannel> _ch) {
				_ch->disconnect();

				if (!sigchannelconnectexception.empty()) {
					sigchannelconnectexception(_ch);
				}

				gc_fn_que.push([this, _ch]() {
					_process->unreg_channel(_ch);
				});
			});

			_chs_mu.lock();
			_chs.insert(std::make_pair(hdl.lock().get(), ch));
			_chs_mu.unlock();

			_process->reg_channel(ch);

			sigchannelconnect(ch);
		}
	}

	boost::signals2::signal<void(std::shared_ptr<juggle::Ichannel>)> sigchanneldisconnect;
	void onChannelDisconn(websocketpp::connection_hdl hdl) {
		_chs_mu.lock();
		auto ch = _chs[hdl.lock().get()];
		_chs.erase(hdl.lock().get());
		_chs_mu.unlock();

		if (!sigchanneldisconnect.empty()) {
			sigchanneldisconnect(ch);
		}

		gc_fn_que.push([this, ch]() {
			_process->unreg_channel(ch);
		});
	}

	void onMsg(websocketpp::connection_hdl hdl, websocketpp::server<websocketpp::config::asio>::message_ptr msg)
	{
		std::string str_msg = msg->get_payload();

		_chs_mu.lock();
		auto ch = _chs[hdl.lock().get()];
		_chs_mu.unlock();

		ch->recv(str_msg);
	}

	void ChannelDisconn(std::shared_ptr<webchannel> ch) {
		if (!sigchanneldisconnect.empty()) {
			sigchanneldisconnect(ch);
		}

		gc_fn_que.push([this, ch]() {
			_process->unreg_channel(ch);
		});
	}

	void poll(){
	}

	void gc_poll() {
		std::function<void()> gc_fn;
		while (gc_fn_que.pop(gc_fn))
		{
			gc_fn();
		}
	}

private:
	std::shared_ptr<websocketpp::server<websocketpp::config::asio_tls> > asio_tls_server;
	std::shared_ptr<websocketpp::server<websocketpp::config::asio> > asio_server;

	bool _is_ssl;

	std::shared_ptr<juggle::process> _process;

	std::string certificate_chain_file;
	std::string private_key_file;
	std::string tmp_dh_file;

	std::mutex _chs_mu;
	std::map<void*, std::shared_ptr<webchannel> > _chs;

	std::thread th;

	Fossilizid::container::msque<std::function<void()> > gc_fn_que;

};

}

#endif
