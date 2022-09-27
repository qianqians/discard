import abelkhan = require("../../abelkhan_type/ts/abelkhan");
import { v1 as uuidv1 } from 'uuid'
/*this enum code is codegen by abelkhan codegen for ts*/

/*this struct code is codegen by abelkhan codegen for typescript*/
/*this caller code is codegen by abelkhan codegen for typescript*/
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_xor_key extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_xor_key");
        modules.reg_module(this);

    }
}

export class xor_key_caller extends abelkhan.Icaller {
    public rsp_cb_xor_key_handle : rsp_cb_xor_key;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("xor_key", _ch);
        this.rsp_cb_xor_key_handle = new rsp_cb_xor_key(modules);
    }

    public refresh_xor_key(xor_key:number){
        let _argv_e8ba6c00_9f46_11ea_b8b7_a85e451255ad:any[] = [];
        _argv_e8ba6c00_9f46_11ea_b8b7_a85e451255ad.push(xor_key);
        this.call_module_method("refresh_xor_key", _argv_e8ba6c00_9f46_11ea_b8b7_a85e451255ad);
    }

}
/*this module code is codegen by abelkhan codegen for typescript*/
export class xor_key_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("xor_key");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("refresh_xor_key", this.refresh_xor_key.bind(this));
        this.cb_refresh_xor_key = null;

    }

    public cb_refresh_xor_key : (xor_key:number)=>void | null;
    refresh_xor_key(inArray:any[]){
        let _argv_e8bae130_9f46_11ea_8f3b_a85e451255ad:any[] = [];
        _argv_e8bae130_9f46_11ea_8f3b_a85e451255ad.push(inArray[0]);
        if (this.cb_refresh_xor_key){
            this.cb_refresh_xor_key.apply(null, _argv_e8bae130_9f46_11ea_8f3b_a85e451255ad);
        }
    }

}
