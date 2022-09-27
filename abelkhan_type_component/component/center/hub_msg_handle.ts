import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');
import svr = require('./svrmanager');
import hub = require('./hubmanager');
import close_handle = require('./closehandle');

export class hub_msg_handle{
    private hub_call_center_module : center_protcol.hub_call_center_module;
    private svrmng : svr.svrmanager;
    private hubmng : hub.hubmanager;
    private close_handle : close_handle.closehandle;
    constructor(modules:abelkhan.modulemng, svrs:svr.svrmanager, hubs:hub.hubmanager, _close_handle:close_handle.closehandle){
        this.hub_call_center_module = new center_protcol.hub_call_center_module(modules);
        this.svrmng = svrs;
        this.hubmng = hubs;
        this.close_handle = _close_handle;

        this.hub_call_center_module.cb_closed = this.closed.bind(this);
    }

    public closed(){
        this.hubmng.hub_closed(this.hub_call_center_module.current_ch);
        if (this.hubmng.check_all_hub_closed()){
            this.svrmng.close_db();
            this.close_handle.is_close = true;
        }
    }
}
