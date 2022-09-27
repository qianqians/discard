/*this rsp file is codegen by abelkhan for c++*/

#include <string>
#include <functional>
#include <memory>

#include <boost/any.hpp>
#include <boost/signals2.hpp>

#include <module.h>

#include <client.h>

namespace rsp
{
    class hcallc_module : public common::imodule, public std::enable_shared_from_this<hcallc_module> 
    {
    public:
        std::string module_name;
    public:
        hcallc_module()
        {
        }

        void Init(std::shared_ptr<client::client> _client)
        {
            module_name = "hcallc";
            _client->modules.add_module("hcallc", shared_from_this());

            reg_cb("hcallc", std::bind(&hcallc_module::hcallc, this, std::placeholders::_1));
        }

    public:
        boost::signals2::signal<void(std::string argv0)> sighcallc;
        void hcallc(std::shared_ptr<std::vector<boost::any> > argvs)
        {
            auto argv0 = boost::any_cast<std::string >((*argvs)[0]);
            sighcallc(argv0);
        }

    };
}
