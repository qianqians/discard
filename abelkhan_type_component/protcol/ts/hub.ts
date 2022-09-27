import abelkhan = require("../../abelkhan_type/ts/abelkhan");
import { v1 as uuidv1 } from 'uuid'
/*this enum code is codegen by abelkhan codegen for ts*/

/*this struct code is codegen by abelkhan codegen for typescript*/
/*this caller code is codegen by abelkhan codegen for typescript*/
export class cb_reg_hub{
    public event_reg_hub_handle_cb : ()=>void | null;
    public event_reg_hub_handle_err : ()=>void | null;
    constructor(){
        this.event_reg_hub_handle_cb = null;
        this.event_reg_hub_handle_err = null;
    }

    callBack(_cb:()=>void, _err:()=>void)
    {
        this.event_reg_hub_handle_cb = _cb;
        this.event_reg_hub_handle_err = _err;
    }
}

/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_hub_call_hub extends abelkhan.Imodule {
    public map_reg_hub:Map<string, cb_reg_hub>;
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_hub_call_hub");
        modules.reg_module(this);

        this.map_reg_hub = new Map<string, cb_reg_hub>();
        this.reg_method("reg_hub_rsp", this.reg_hub_rsp.bind(this));
        this.reg_method("reg_hub_err", this.reg_hub_err.bind(this));
    }
    public reg_hub_rsp(inArray:any[]){
        let uuid = inArray[0];
        let _argv_e8c36cca_9f46_11ea_b5ea_a85e451255ad:any[] = [];
        var rsp = this.map_reg_hub.get(uuid);
        rsp.event_reg_hub_handle_cb.apply(null, _argv_e8c36cca_9f46_11ea_b5ea_a85e451255ad);
        this.map_reg_hub.delete(uuid);
    }
    public reg_hub_err(inArray:any[]){
        let uuid = inArray[0];
        let _argv_e8c36ccb_9f46_11ea_a4c3_a85e451255ad:any[] = [];
        var rsp = this.map_reg_hub.get(uuid);
        rsp.event_reg_hub_handle_err.apply(null, _argv_e8c36ccb_9f46_11ea_a4c3_a85e451255ad);
        this.map_reg_hub.delete(uuid);
    }
}

export class hub_call_hub_caller extends abelkhan.Icaller {
    public rsp_cb_hub_call_hub_handle : rsp_cb_hub_call_hub;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("hub_call_hub", _ch);
        this.rsp_cb_hub_call_hub_handle = new rsp_cb_hub_call_hub(modules);
    }

    public reg_hub(hub_name:string, hub_type:string){
        let uuid_e8c36ccc_9f46_11ea_ba0f_a85e451255ad = uuidv1();

        let _argv_e8c36ccd_9f46_11ea_b546_a85e451255ad:any[] = [uuid_e8c36ccc_9f46_11ea_ba0f_a85e451255ad];
        _argv_e8c36ccd_9f46_11ea_b546_a85e451255ad.push(hub_name);
        _argv_e8c36ccd_9f46_11ea_b546_a85e451255ad.push(hub_type);
        this.call_module_method("reg_hub", _argv_e8c36ccd_9f46_11ea_b546_a85e451255ad);
        let cb_reg_hub_obj = new cb_reg_hub();
        this.rsp_cb_hub_call_hub_handle.map_reg_hub.set(uuid_e8c36ccc_9f46_11ea_ba0f_a85e451255ad, cb_reg_hub_obj);

        return cb_reg_hub_obj;
    }

}
/*this module code is codegen by abelkhan codegen for typescript*/
export class rsp_reg_hub extends abelkhan.Icaller {
    private uuid : string;
    constructor(_ch:any, _uuid:string){
        super("rsp_cb_hub_call_hub", _ch);
        this.uuid = _uuid;
    }

    public rsp(){
        let _argv_e8c36ccf_9f46_11ea_968c_a85e451255ad:any[] = [this.uuid];
        this.call_module_method("reg_hub_rsp", _argv_e8c36ccf_9f46_11ea_968c_a85e451255ad);
    }

    public err(){
        let _argv_e8c36cd0_9f46_11ea_a2fe_a85e451255ad:any[] = [this.uuid];
        this.call_module_method("reg_hub_err", _argv_e8c36cd0_9f46_11ea_a2fe_a85e451255ad);
    }

}

export class hub_call_hub_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("hub_call_hub");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("reg_hub", this.reg_hub.bind(this));
        this.cb_reg_hub = null;

    }

    public cb_reg_hub : (hub_name:string, hub_type:string)=>void | null;
    reg_hub(inArray:any[]){
        let _cb_uuid = inArray[0];
        let _argv_e8c36cce_9f46_11ea_b30c_a85e451255ad:any[] = [];
        _argv_e8c36cce_9f46_11ea_b30c_a85e451255ad.push(inArray[1]);
        _argv_e8c36cce_9f46_11ea_b30c_a85e451255ad.push(inArray[2]);
        this.rsp = new rsp_reg_hub(this.current_ch, _cb_uuid);
        if (cb_reg_hub){
            cb_reg_hub.apply(null, _argv_e8c36cce_9f46_11ea_b30c_a85e451255ad);
        }
        this.rsp = null;
    }

}
