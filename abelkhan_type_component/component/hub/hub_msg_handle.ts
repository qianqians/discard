import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import signals = require('../../service/signals/signals');
import hub_protcol = require('../../protcol/ts/hub');

export class hubproxy{
    public ch : abelkhan.Ichannel;
    public hub_name : string;
    public hub_type : string;
    constructor(ch:abelkhan.Ichannel, hub_name: string, hub_type: string){
        this.ch = ch;
        this.hub_name = hub_name;
        this.hub_type = hub_type;
    }

}

export class hub_msg_handle{
    public signals_hubproxy : signals.signals<hubproxy>;
    private hub_call_hub_module : hub_protcol.hub_call_hub_module;
    constructor(modules:abelkhan.modulemng){
        this.signals_hubproxy = new signals.signals<hubproxy>();

        this.hub_call_hub_module = new hub_protcol.hub_call_hub_module(modules);
        this.hub_call_hub_module.cb_reg_hub = this.reg_hub.bind(this);
    }

    public reg_hub(hub_name: string, hub_type: string){
        let ch = this.hub_call_hub_module.current_ch;
        let rsp = <hub_protcol.rsp_reg_hub>(this.hub_call_hub_module.rsp);
        rsp.rsp();

        this.signals_hubproxy.emit(new hubproxy(ch, hub_name, hub_type));
    }
}
