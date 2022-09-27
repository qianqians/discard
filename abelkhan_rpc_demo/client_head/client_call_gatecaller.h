/*this caller file is codegen by juggle for c++*/
#ifndef _client_call_gate_caller_h
#define _client_call_gate_caller_h
#include <sstream>
#include <tuple>
#include <string>
#include "Icaller.h"
#include "Ichannel.h"
#include <boost/any.hpp>
#include <memory>

namespace caller
{
class client_call_gate : public juggle::Icaller {
public:
    client_call_gate(std::shared_ptr<juggle::Ichannel> _ch) : Icaller(_ch) {
        module_name = "client_call_gate";
    }

    ~client_call_gate(){
    }

    void connect_server(std::string argv0,int64_t argv1){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("connect_server");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv1);
        ch->push(v);
    }

    void cancle_server(){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("cancle_server");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        ch->push(v);
    }

    void enable_heartbeats(){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("enable_heartbeats");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        ch->push(v);
    }

    void disable_heartbeats(){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("disable_heartbeats");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        ch->push(v);
    }

    void connect_hub(std::string argv0,std::string argv1){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("connect_hub");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv1);
        ch->push(v);
    }

    void disconnect_hub(std::string argv0,std::string argv1){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("disconnect_hub");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv1);
        ch->push(v);
    }

    void forward_client_call_hub(std::string argv0,std::string argv1,std::string argv2,std::shared_ptr<std::vector<boost::any> > argv3){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("forward_client_call_hub");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv1);
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv2);
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv3);
        ch->push(v);
    }

    void heartbeats(int64_t argv0){
        auto v = std::make_shared<std::vector<boost::any> >();
        v->push_back("client_call_gate");
        v->push_back("heartbeats");
        v->push_back(std::make_shared<std::vector<boost::any> >());
        (boost::any_cast<std::shared_ptr<std::vector<boost::any> > >((*v)[2]))->push_back(argv0);
        ch->push(v);
    }

};

}

#endif
