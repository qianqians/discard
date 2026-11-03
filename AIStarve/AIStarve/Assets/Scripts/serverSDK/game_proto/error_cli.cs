using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MsgPack.Serialization;

namespace Abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

    public enum em_error{
        timeout = -2,
        db_error = -1,
        success = 0,
        scene_in_other_server = 1,
        fence_is_exist = 2,
        role_is_import = 3,
        have_role_protect = 4,
        cultivated_is_grow = 5,
        scene_sales_vegetable_failed = 6
    }
/*this struct code is codegen by abelkhan codegen for c#*/
/*this module code is codegen by abelkhan codegen for c#*/
    public class error_code_ntf_module : Common.IModule {
        public Client.Client _client_handle;
        public error_code_ntf_module(Client.Client client_handle_) 
        {
            _client_handle = client_handle_;
            _client_handle.modulemanager.add_mothed("error_code_ntf_error_code", error_code);
        }

        public event Action<em_error> on_error_code;
        public void error_code(IList<MsgPack.MessagePackObject> inArray){
            var _err_code = (em_error)((MsgPack.MessagePackObject)inArray[0]).AsInt32();
            if (on_error_code != null){
                on_error_code(_err_code);
            }
        }

    }

}
