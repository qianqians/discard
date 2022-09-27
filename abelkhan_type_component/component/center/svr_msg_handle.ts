import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');
import svr = require('./svrmanager');
import hub = require('./hubmanager');

export class svr_msg_handle{
    private center_module : center_protcol.center_module;
    private svrmng : svr.svrmanager;
    private hubmng : hub.hubmanager;
    constructor(modules:abelkhan.modulemng, svrs:svr.svrmanager, hubs:hub.hubmanager){
        this.center_module = new center_protcol.center_module(modules);
        this.svrmng = svrs;
        this.hubmng = hubs;

        this.center_module.cb_reg_server = this.reg_server.bind(this);
    }

    public reg_server(type:string, name:string, ip:string, port:number){
        this.hubmng.for_each_hub((_proxy:hub.hubproxy)=>{
            _proxy.distribute_server_address(type, name, ip, port);
        });

        if (type == "hub") {
            let _hubproxy = this.hubmng.reg_hub (this.center_module.current_ch);

            this.svrmng.for_each_svr((_proxy:svr.svrproxy) =>{
                _hubproxy.distribute_server_address(_proxy.type, _proxy.name, _proxy.ip, _proxy.port);
            });
        }

        let _svrproxy = this.svrmng.reg_svr(this.center_module.current_ch, type, name, ip, port);
		_svrproxy.reg_server_sucess();
    }
}