#include <memory>
#include <iostream>
#include <vector>
#include <string>

#include "client.h"
#include "ccallhcaller.hpp"
#include "hcallcmodule.hpp"


int main()
{
	auto client_handle = std::make_shared<client::client>(123456);

	client_handle->sigConnectGate.connect([client_handle]() {
		client_handle->connect_hub("hub_server");
		client_handle->connect_hub("hub_server0");
	});

	auto c2h_handle = std::make_shared<req::ccallh>(client_handle);
	std::vector<std::string> vec_con_hub;
	client_handle->sigConnectHub.connect([&vec_con_hub, client_handle, c2h_handle](std::string hub_name) {
		vec_con_hub.push_back(hub_name);
		if (std::find(vec_con_hub.begin(), vec_con_hub.end(), "hub_server") != vec_con_hub.end() &
			std::find(vec_con_hub.begin(), vec_con_hub.end(), "hub_server0") != vec_con_hub.end())
		{
			c2h_handle->get_hub("hub_server")->ccallh()->callBack([](std::string text) {
				std::cout << text << std::endl;
			}, []() {
				std::cout << "error" << std::endl;
			});
		}
	});


	auto h2c_module = std::make_shared<rsp::hcallc_module>();
	h2c_module->Init(client_handle);
	h2c_module->sighcallc.connect([](std::string test) {
		std::cout << test << std::endl;
	});

	client_handle->connect_server("127.0.0.1", 3236, client_handle->timer.Tick);

	uint64_t old_tick = client_handle->poll();
	while (1)
	{
		uint64_t tick = client_handle->poll();

		if ((tick - old_tick) < 50) {
			boost::this_thread::sleep(boost::get_system_time() + boost::posix_time::microseconds(5));
		}
		old_tick = tick;
	}

    return 0;
}

