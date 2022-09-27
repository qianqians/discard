import abelkhan = require("../../abelkhan_type/ts/abelkhan");
import { v1 as uuidv1 } from 'uuid'
/*this enum code is codegen by abelkhan codegen for ts*/

/*this struct code is codegen by abelkhan codegen for typescript*/
/*this caller code is codegen by abelkhan codegen for typescript*/
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_dbproxy_call_hub extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_dbproxy_call_hub");
        modules.reg_module(this);

    }
}

export class dbproxy_call_hub_caller extends abelkhan.Icaller {
    public rsp_cb_dbproxy_call_hub_handle : rsp_cb_dbproxy_call_hub;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("dbproxy_call_hub", _ch);
        this.rsp_cb_dbproxy_call_hub_handle = new rsp_cb_dbproxy_call_hub(modules);
    }

    public reg_hub_sucess(){
        let _argv_e8c36cb0_9f46_11ea_81ca_a85e451255ad:any[] = [];
        this.call_module_method("reg_hub_sucess", _argv_e8c36cb0_9f46_11ea_81ca_a85e451255ad);
    }

    public ack_create_persisted_object(callbackid:string, is_create_sucessed:boolean){
        let _argv_e8c36cb1_9f46_11ea_ba44_a85e451255ad:any[] = [];
        _argv_e8c36cb1_9f46_11ea_ba44_a85e451255ad.push(callbackid);
        _argv_e8c36cb1_9f46_11ea_ba44_a85e451255ad.push(is_create_sucessed);
        this.call_module_method("ack_create_persisted_object", _argv_e8c36cb1_9f46_11ea_ba44_a85e451255ad);
    }

    public ack_updata_persisted_object(callbackid:string, is_update_sucessed:boolean){
        let _argv_e8c36cb2_9f46_11ea_ae5e_a85e451255ad:any[] = [];
        _argv_e8c36cb2_9f46_11ea_ae5e_a85e451255ad.push(callbackid);
        _argv_e8c36cb2_9f46_11ea_ae5e_a85e451255ad.push(is_update_sucessed);
        this.call_module_method("ack_updata_persisted_object", _argv_e8c36cb2_9f46_11ea_ae5e_a85e451255ad);
    }

    public ack_get_object_count(callbackid:string, count:number){
        let _argv_e8c36cb3_9f46_11ea_a0b5_a85e451255ad:any[] = [];
        _argv_e8c36cb3_9f46_11ea_a0b5_a85e451255ad.push(callbackid);
        _argv_e8c36cb3_9f46_11ea_a0b5_a85e451255ad.push(count);
        this.call_module_method("ack_get_object_count", _argv_e8c36cb3_9f46_11ea_a0b5_a85e451255ad);
    }

    public ack_get_object_info(callbackid:string, object_info:string){
        let _argv_e8c36cb4_9f46_11ea_baf2_a85e451255ad:any[] = [];
        _argv_e8c36cb4_9f46_11ea_baf2_a85e451255ad.push(callbackid);
        _argv_e8c36cb4_9f46_11ea_baf2_a85e451255ad.push(object_info);
        this.call_module_method("ack_get_object_info", _argv_e8c36cb4_9f46_11ea_baf2_a85e451255ad);
    }

    public ack_get_object_info_end(callbackid:string){
        let _argv_e8c36cb5_9f46_11ea_9237_a85e451255ad:any[] = [];
        _argv_e8c36cb5_9f46_11ea_9237_a85e451255ad.push(callbackid);
        this.call_module_method("ack_get_object_info_end", _argv_e8c36cb5_9f46_11ea_9237_a85e451255ad);
    }

    public ack_remove_object(callbackid:string, is_remove_sucessed:boolean){
        let _argv_e8c36cb6_9f46_11ea_b23c_a85e451255ad:any[] = [];
        _argv_e8c36cb6_9f46_11ea_b23c_a85e451255ad.push(callbackid);
        _argv_e8c36cb6_9f46_11ea_b23c_a85e451255ad.push(is_remove_sucessed);
        this.call_module_method("ack_remove_object", _argv_e8c36cb6_9f46_11ea_b23c_a85e451255ad);
    }

}
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_hub_call_dbproxy extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_hub_call_dbproxy");
        modules.reg_module(this);

    }
}

export class hub_call_dbproxy_caller extends abelkhan.Icaller {
    public rsp_cb_hub_call_dbproxy_handle : rsp_cb_hub_call_dbproxy;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("hub_call_dbproxy", _ch);
        this.rsp_cb_hub_call_dbproxy_handle = new rsp_cb_hub_call_dbproxy(modules);
    }

    public reg_hub(hub_name:string){
        let _argv_e8c36cb7_9f46_11ea_b5ce_a85e451255ad:any[] = [];
        _argv_e8c36cb7_9f46_11ea_b5ce_a85e451255ad.push(hub_name);
        this.call_module_method("reg_hub", _argv_e8c36cb7_9f46_11ea_b5ce_a85e451255ad);
    }

    public create_persisted_object(db:string, collection:string, object_info:string, callbackid:string){
        let _argv_e8c36cb8_9f46_11ea_b678_a85e451255ad:any[] = [];
        _argv_e8c36cb8_9f46_11ea_b678_a85e451255ad.push(db);
        _argv_e8c36cb8_9f46_11ea_b678_a85e451255ad.push(collection);
        _argv_e8c36cb8_9f46_11ea_b678_a85e451255ad.push(object_info);
        _argv_e8c36cb8_9f46_11ea_b678_a85e451255ad.push(callbackid);
        this.call_module_method("create_persisted_object", _argv_e8c36cb8_9f46_11ea_b678_a85e451255ad);
    }

    public updata_persisted_object(db:string, collection:string, query_json:string, object_info:string, callbackid:string){
        let _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad:any[] = [];
        _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad.push(db);
        _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad.push(collection);
        _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad.push(query_json);
        _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad.push(object_info);
        _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad.push(callbackid);
        this.call_module_method("updata_persisted_object", _argv_e8c36cb9_9f46_11ea_9764_a85e451255ad);
    }

    public get_object_count(db:string, collection:string, query_json:string, callbackid:string){
        let _argv_e8c36cba_9f46_11ea_8a47_a85e451255ad:any[] = [];
        _argv_e8c36cba_9f46_11ea_8a47_a85e451255ad.push(db);
        _argv_e8c36cba_9f46_11ea_8a47_a85e451255ad.push(collection);
        _argv_e8c36cba_9f46_11ea_8a47_a85e451255ad.push(query_json);
        _argv_e8c36cba_9f46_11ea_8a47_a85e451255ad.push(callbackid);
        this.call_module_method("get_object_count", _argv_e8c36cba_9f46_11ea_8a47_a85e451255ad);
    }

    public get_object_info(db:string, collection:string, query_json:string, callbackid:string){
        let _argv_e8c36cbb_9f46_11ea_a9da_a85e451255ad:any[] = [];
        _argv_e8c36cbb_9f46_11ea_a9da_a85e451255ad.push(db);
        _argv_e8c36cbb_9f46_11ea_a9da_a85e451255ad.push(collection);
        _argv_e8c36cbb_9f46_11ea_a9da_a85e451255ad.push(query_json);
        _argv_e8c36cbb_9f46_11ea_a9da_a85e451255ad.push(callbackid);
        this.call_module_method("get_object_info", _argv_e8c36cbb_9f46_11ea_a9da_a85e451255ad);
    }

    public remove_object(db:string, collection:string, query_json:string, callbackid:string){
        let _argv_e8c36cbc_9f46_11ea_94de_a85e451255ad:any[] = [];
        _argv_e8c36cbc_9f46_11ea_94de_a85e451255ad.push(db);
        _argv_e8c36cbc_9f46_11ea_94de_a85e451255ad.push(collection);
        _argv_e8c36cbc_9f46_11ea_94de_a85e451255ad.push(query_json);
        _argv_e8c36cbc_9f46_11ea_94de_a85e451255ad.push(callbackid);
        this.call_module_method("remove_object", _argv_e8c36cbc_9f46_11ea_94de_a85e451255ad);
    }

}
/*this module code is codegen by abelkhan codegen for typescript*/
export class dbproxy_call_hub_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("dbproxy_call_hub");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("reg_hub_sucess", this.reg_hub_sucess.bind(this));
        this.reg_method("ack_create_persisted_object", this.ack_create_persisted_object.bind(this));
        this.reg_method("ack_updata_persisted_object", this.ack_updata_persisted_object.bind(this));
        this.reg_method("ack_get_object_count", this.ack_get_object_count.bind(this));
        this.reg_method("ack_get_object_info", this.ack_get_object_info.bind(this));
        this.reg_method("ack_get_object_info_end", this.ack_get_object_info_end.bind(this));
        this.reg_method("ack_remove_object", this.ack_remove_object.bind(this));
        this.cb_reg_hub_sucess = null;

        this.cb_ack_create_persisted_object = null;

        this.cb_ack_updata_persisted_object = null;

        this.cb_ack_get_object_count = null;

        this.cb_ack_get_object_info = null;

        this.cb_ack_get_object_info_end = null;

        this.cb_ack_remove_object = null;

    }

    public cb_reg_hub_sucess : ()=>void | null;
    reg_hub_sucess(inArray:any[]){
        let _argv_e8c36cbd_9f46_11ea_81d6_a85e451255ad:any[] = [];
        if (this.cb_reg_hub_sucess){
            this.cb_reg_hub_sucess.apply(null, _argv_e8c36cbd_9f46_11ea_81d6_a85e451255ad);
        }
    }

    public cb_ack_create_persisted_object : (callbackid:string, is_create_sucessed:boolean)=>void | null;
    ack_create_persisted_object(inArray:any[]){
        let _argv_e8c36cbe_9f46_11ea_bbc4_a85e451255ad:any[] = [];
        _argv_e8c36cbe_9f46_11ea_bbc4_a85e451255ad.push(inArray[0]);
        _argv_e8c36cbe_9f46_11ea_bbc4_a85e451255ad.push(inArray[1]);
        if (this.cb_ack_create_persisted_object){
            this.cb_ack_create_persisted_object.apply(null, _argv_e8c36cbe_9f46_11ea_bbc4_a85e451255ad);
        }
    }

    public cb_ack_updata_persisted_object : (callbackid:string, is_update_sucessed:boolean)=>void | null;
    ack_updata_persisted_object(inArray:any[]){
        let _argv_e8c36cbf_9f46_11ea_9d5a_a85e451255ad:any[] = [];
        _argv_e8c36cbf_9f46_11ea_9d5a_a85e451255ad.push(inArray[0]);
        _argv_e8c36cbf_9f46_11ea_9d5a_a85e451255ad.push(inArray[1]);
        if (this.cb_ack_updata_persisted_object){
            this.cb_ack_updata_persisted_object.apply(null, _argv_e8c36cbf_9f46_11ea_9d5a_a85e451255ad);
        }
    }

    public cb_ack_get_object_count : (callbackid:string, count:number)=>void | null;
    ack_get_object_count(inArray:any[]){
        let _argv_e8c36cc0_9f46_11ea_8e87_a85e451255ad:any[] = [];
        _argv_e8c36cc0_9f46_11ea_8e87_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc0_9f46_11ea_8e87_a85e451255ad.push(inArray[1]);
        if (this.cb_ack_get_object_count){
            this.cb_ack_get_object_count.apply(null, _argv_e8c36cc0_9f46_11ea_8e87_a85e451255ad);
        }
    }

    public cb_ack_get_object_info : (callbackid:string, object_info:string)=>void | null;
    ack_get_object_info(inArray:any[]){
        let _argv_e8c36cc1_9f46_11ea_933a_a85e451255ad:any[] = [];
        _argv_e8c36cc1_9f46_11ea_933a_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc1_9f46_11ea_933a_a85e451255ad.push(inArray[1]);
        if (this.cb_ack_get_object_info){
            this.cb_ack_get_object_info.apply(null, _argv_e8c36cc1_9f46_11ea_933a_a85e451255ad);
        }
    }

    public cb_ack_get_object_info_end : (callbackid:string)=>void | null;
    ack_get_object_info_end(inArray:any[]){
        let _argv_e8c36cc2_9f46_11ea_bc1d_a85e451255ad:any[] = [];
        _argv_e8c36cc2_9f46_11ea_bc1d_a85e451255ad.push(inArray[0]);
        if (this.cb_ack_get_object_info_end){
            this.cb_ack_get_object_info_end.apply(null, _argv_e8c36cc2_9f46_11ea_bc1d_a85e451255ad);
        }
    }

    public cb_ack_remove_object : (callbackid:string, is_remove_sucessed:boolean)=>void | null;
    ack_remove_object(inArray:any[]){
        let _argv_e8c36cc3_9f46_11ea_bb80_a85e451255ad:any[] = [];
        _argv_e8c36cc3_9f46_11ea_bb80_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc3_9f46_11ea_bb80_a85e451255ad.push(inArray[1]);
        if (this.cb_ack_remove_object){
            this.cb_ack_remove_object.apply(null, _argv_e8c36cc3_9f46_11ea_bb80_a85e451255ad);
        }
    }

}
export class hub_call_dbproxy_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("hub_call_dbproxy");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("reg_hub", this.reg_hub.bind(this));
        this.reg_method("create_persisted_object", this.create_persisted_object.bind(this));
        this.reg_method("updata_persisted_object", this.updata_persisted_object.bind(this));
        this.reg_method("get_object_count", this.get_object_count.bind(this));
        this.reg_method("get_object_info", this.get_object_info.bind(this));
        this.reg_method("remove_object", this.remove_object.bind(this));
        this.cb_reg_hub = null;

        this.cb_create_persisted_object = null;

        this.cb_updata_persisted_object = null;

        this.cb_get_object_count = null;

        this.cb_get_object_info = null;

        this.cb_remove_object = null;

    }

    public cb_reg_hub : (hub_name:string)=>void | null;
    reg_hub(inArray:any[]){
        let _argv_e8c36cc4_9f46_11ea_9602_a85e451255ad:any[] = [];
        _argv_e8c36cc4_9f46_11ea_9602_a85e451255ad.push(inArray[0]);
        if (this.cb_reg_hub){
            this.cb_reg_hub.apply(null, _argv_e8c36cc4_9f46_11ea_9602_a85e451255ad);
        }
    }

    public cb_create_persisted_object : (db:string, collection:string, object_info:string, callbackid:string)=>void | null;
    create_persisted_object(inArray:any[]){
        let _argv_e8c36cc5_9f46_11ea_bae0_a85e451255ad:any[] = [];
        _argv_e8c36cc5_9f46_11ea_bae0_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc5_9f46_11ea_bae0_a85e451255ad.push(inArray[1]);
        _argv_e8c36cc5_9f46_11ea_bae0_a85e451255ad.push(inArray[2]);
        _argv_e8c36cc5_9f46_11ea_bae0_a85e451255ad.push(inArray[3]);
        if (this.cb_create_persisted_object){
            this.cb_create_persisted_object.apply(null, _argv_e8c36cc5_9f46_11ea_bae0_a85e451255ad);
        }
    }

    public cb_updata_persisted_object : (db:string, collection:string, query_json:string, object_info:string, callbackid:string)=>void | null;
    updata_persisted_object(inArray:any[]){
        let _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad:any[] = [];
        _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad.push(inArray[1]);
        _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad.push(inArray[2]);
        _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad.push(inArray[3]);
        _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad.push(inArray[4]);
        if (this.cb_updata_persisted_object){
            this.cb_updata_persisted_object.apply(null, _argv_e8c36cc6_9f46_11ea_9e14_a85e451255ad);
        }
    }

    public cb_get_object_count : (db:string, collection:string, query_json:string, callbackid:string)=>void | null;
    get_object_count(inArray:any[]){
        let _argv_e8c36cc7_9f46_11ea_8ca2_a85e451255ad:any[] = [];
        _argv_e8c36cc7_9f46_11ea_8ca2_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc7_9f46_11ea_8ca2_a85e451255ad.push(inArray[1]);
        _argv_e8c36cc7_9f46_11ea_8ca2_a85e451255ad.push(inArray[2]);
        _argv_e8c36cc7_9f46_11ea_8ca2_a85e451255ad.push(inArray[3]);
        if (this.cb_get_object_count){
            this.cb_get_object_count.apply(null, _argv_e8c36cc7_9f46_11ea_8ca2_a85e451255ad);
        }
    }

    public cb_get_object_info : (db:string, collection:string, query_json:string, callbackid:string)=>void | null;
    get_object_info(inArray:any[]){
        let _argv_e8c36cc8_9f46_11ea_8b37_a85e451255ad:any[] = [];
        _argv_e8c36cc8_9f46_11ea_8b37_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc8_9f46_11ea_8b37_a85e451255ad.push(inArray[1]);
        _argv_e8c36cc8_9f46_11ea_8b37_a85e451255ad.push(inArray[2]);
        _argv_e8c36cc8_9f46_11ea_8b37_a85e451255ad.push(inArray[3]);
        if (this.cb_get_object_info){
            this.cb_get_object_info.apply(null, _argv_e8c36cc8_9f46_11ea_8b37_a85e451255ad);
        }
    }

    public cb_remove_object : (db:string, collection:string, query_json:string, callbackid:string)=>void | null;
    remove_object(inArray:any[]){
        let _argv_e8c36cc9_9f46_11ea_a5c8_a85e451255ad:any[] = [];
        _argv_e8c36cc9_9f46_11ea_a5c8_a85e451255ad.push(inArray[0]);
        _argv_e8c36cc9_9f46_11ea_a5c8_a85e451255ad.push(inArray[1]);
        _argv_e8c36cc9_9f46_11ea_a5c8_a85e451255ad.push(inArray[2]);
        _argv_e8c36cc9_9f46_11ea_a5c8_a85e451255ad.push(inArray[3]);
        if (this.cb_remove_object){
            this.cb_remove_object.apply(null, _argv_e8c36cc9_9f46_11ea_a5c8_a85e451255ad);
        }
    }

}
