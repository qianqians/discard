/*this req file is codegen by abelkhan codegen for c++*/

#ifndef _ccallh_req_h
#define _ccallh_req_h

#include <string>
#include <functional>
#include <memory>

#include <boost/any.hpp>
#include <boost/uuid/uuid.hpp>
#include <boost/uuid/uuid_generators.hpp>
#include <boost/uuid/uuid_io.hpp>
#include <boost/lexical_cast.hpp>
#include <boost/signals2.hpp>

#include <module.h>

#include <client.h>

namespace req
{
class cb_ccallh_func{
public:
    boost::signals2::signal<void(std::string)> sigccallhcb;
    void cb(std::string argvs0){
        sigccallhcb(argvs0);
    }

    boost::signals2::signal<void()> sigccallherr;
    void err(){
        sigccallherr();
    }

    void callBack(std::function<void(std::string)> cb, std::function<void()> err){
        sigccallhcb.connect(cb);
        sigccallherr.connect(err);
    }

};
/*req cb code, codegen by abelkhan codegen*/
class cb_ccallh : public common::imodule {
public:
    cb_ccallh(){
        reg_cb("ccallh_rsp", std::bind(&cb_ccallh::ccallh_rsp, this, std::placeholders::_1));
        reg_cb("ccallh_err", std::bind(&cb_ccallh::ccallh_err, this, std::placeholders::_1));
    }

    std::map<std::string, std::shared_ptr<cb_ccallh_func> > map_ccallh;
    void ccallh_rsp(std::shared_ptr<std::vector<boost::any> > argvs){
        auto cb_uuid = boost::any_cast<std::string>((*argvs)[0]);
        auto argv1 = boost::any_cast<std::string>((*argvs)[1]);
        std::shared_ptr<cb_ccallh_func> func_cb = map_ccallh[cb_uuid];
        func_cb->cb(argv1);
    }
    void ccallh_err(std::shared_ptr<std::vector<boost::any> > argvs){
        auto cb_uuid = boost::any_cast<std::string>((*argvs)[0]);
        std::shared_ptr<cb_ccallh_func> func_cb = map_ccallh[cb_uuid];
        func_cb->err();
    }
};

class ccallh_hubproxy;
class ccallh {
private:
    std::shared_ptr<client::client> client_handle_ptr;
    std::shared_ptr<cb_ccallh> cb_ccallh_handle;

public:
    ccallh(std::shared_ptr<client::client> _client) {
        client_handle_ptr = _client;
        cb_ccallh_handle = std::make_shared<cb_ccallh>();
        client_handle_ptr->modules.add_module("ccallh", cb_ccallh_handle);
    }

    ~ccallh(){
    }

    std::shared_ptr<ccallh_hubproxy> get_hub(std::string hub_name) {
        return std::make_shared<ccallh_hubproxy>(hub_name, client_handle_ptr, cb_ccallh_handle);
    }
};

class ccallh_hubproxy {
public:
    std::string hub_name;
    std::shared_ptr<cb_ccallh> cb_ccallh_handle;
    std::shared_ptr<client::client> client_handle_ptr;

public:
    ccallh_hubproxy(std::string _hub_name, std::shared_ptr<client::client> _client_handle_ptr, std::shared_ptr<cb_ccallh> _cb_ccallh){
        hub_name = _hub_name;
        cb_ccallh_handle = _cb_ccallh;
        client_handle_ptr = _client_handle_ptr;
    }

    std::shared_ptr<cb_ccallh_func> ccallh(){
        boost::uuids::random_generator g;
        auto uuid = boost::lexical_cast<std::string>(g());
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back(uuid);
        client_handle_ptr->call_hub(hub_name, "ccallh", "ccallh", v);
        auto cb_func_obj = std::make_shared<cb_ccallh_func>();
        cb_ccallh_handle->map_ccallh.insert(std::make_pair(uuid, cb_func_obj));
        return cb_func_obj;
    }

};

}

#endif
