import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');
import svr = require('./svrmanager');
import hub = require('./hubmanager');
import gm = require('./gmmanager');
import close_handle = require('./closehandle');

export class gm_msg_handle{
    private gm_center_module : center_protcol.gm_center_module;
    private svrmng : svr.svrmanager;
    private hubmng : hub.hubmanager;
    private gmmng : gm.gmmanager;
    private close_handle : close_handle.closehandle;
    constructor(modules:abelkhan.modulemng, svrs:svr.svrmanager, hubs:hub.hubmanager, gms:gm.gmmanager, _close_handle:close_handle.closehandle){
        this.svrmng = svrs;
        this.hubmng = hubs;
        this.gmmng = gms;
        this.close_handle = _close_handle;
        this.gm_center_module = new center_protcol.gm_center_module(modules);

        this.gm_center_module.cb_confirm_gm = this.confirm_gm.bind(this);
        this.gm_center_module.cb_close_clutter = this.close_clutter.bind(this);
        this.gm_center_module.cb_reload = this.reload.bind(this);
    }

    public confirm_gm(gm_name:string){
		this.gmmng.reg_gm(gm_name, this.gm_center_module.current_ch);
    }
    
    public close_clutter(gmname:string){
		if (this.gmmng.check_gm (gmname, this.gm_center_module.current_ch)) {
            this.close_handle.is_closing = true;
			this.svrmng.for_each_svr((_svrproxy:svr.svrproxy) => {
                if (_svrproxy.type != "dbproxy") {
                    _svrproxy.close_server();
                }
			});

            if (this.hubmng.check_all_hub_closed()) {
                this.svrmng.close_db();
                this.close_handle.is_close = true;
            }
        }
    }
    
    public reload(gmname:string){
        if (this.gmmng.check_gm(gmname, this.gm_center_module.current_ch)){
            this.hubmng.for_each_hub((_proxy:hub.hubproxy) => {
                _proxy.reload();
            });
        }
    }
}
