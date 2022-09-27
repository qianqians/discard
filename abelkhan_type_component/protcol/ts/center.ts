import abelkhan = require("../../abelkhan_type/ts/abelkhan");
import { v1 as uuidv1 } from 'uuid'
/*this enum code is codegen by abelkhan codegen for ts*/

/*this struct code is codegen by abelkhan codegen for typescript*/
/*this caller code is codegen by abelkhan codegen for typescript*/
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_center_call_hub extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_center_call_hub");
        modules.reg_module(this);

    }
}

export class center_call_hub_caller extends abelkhan.Icaller {
    public rsp_cb_center_call_hub_handle : rsp_cb_center_call_hub;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("center_call_hub", _ch);
        this.rsp_cb_center_call_hub_handle = new rsp_cb_center_call_hub(modules);
    }

    public distribute_server_address(type:string, name:string, ip:string, port:number){
        let _argv_e8c2a961_9f46_11ea_bedb_a85e451255ad:any[] = [];
        _argv_e8c2a961_9f46_11ea_bedb_a85e451255ad.push(type);
        _argv_e8c2a961_9f46_11ea_bedb_a85e451255ad.push(name);
        _argv_e8c2a961_9f46_11ea_bedb_a85e451255ad.push(ip);
        _argv_e8c2a961_9f46_11ea_bedb_a85e451255ad.push(port);
        this.call_module_method("distribute_server_address", _argv_e8c2a961_9f46_11ea_bedb_a85e451255ad);
    }

    public reload(){
        let _argv_e8c345a1_9f46_11ea_8e94_a85e451255ad:any[] = [];
        this.call_module_method("reload", _argv_e8c345a1_9f46_11ea_8e94_a85e451255ad);
    }

}
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_center_call_server extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_center_call_server");
        modules.reg_module(this);

    }
}

export class center_call_server_caller extends abelkhan.Icaller {
    public rsp_cb_center_call_server_handle : rsp_cb_center_call_server;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("center_call_server", _ch);
        this.rsp_cb_center_call_server_handle = new rsp_cb_center_call_server(modules);
    }

    public reg_server_sucess(){
        let _argv_e8c345a2_9f46_11ea_9ba5_a85e451255ad:any[] = [];
        this.call_module_method("reg_server_sucess", _argv_e8c345a2_9f46_11ea_9ba5_a85e451255ad);
    }

    public close_server(){
        let _argv_e8c345a3_9f46_11ea_95fb_a85e451255ad:any[] = [];
        this.call_module_method("close_server", _argv_e8c345a3_9f46_11ea_95fb_a85e451255ad);
    }

}
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_hub_call_center extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_hub_call_center");
        modules.reg_module(this);

    }
}

export class hub_call_center_caller extends abelkhan.Icaller {
    public rsp_cb_hub_call_center_handle : rsp_cb_hub_call_center;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("hub_call_center", _ch);
        this.rsp_cb_hub_call_center_handle = new rsp_cb_hub_call_center(modules);
    }

    public closed(){
        let _argv_e8c345a4_9f46_11ea_9b47_a85e451255ad:any[] = [];
        this.call_module_method("closed", _argv_e8c345a4_9f46_11ea_9b47_a85e451255ad);
    }

}
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_center extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_center");
        modules.reg_module(this);

    }
}

export class center_caller extends abelkhan.Icaller {
    public rsp_cb_center_handle : rsp_cb_center;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("center", _ch);
        this.rsp_cb_center_handle = new rsp_cb_center(modules);
    }

    public reg_server(type:string, name:string, ip:string, port:number){
        let _argv_e8c345a5_9f46_11ea_9024_a85e451255ad:any[] = [];
        _argv_e8c345a5_9f46_11ea_9024_a85e451255ad.push(type);
        _argv_e8c345a5_9f46_11ea_9024_a85e451255ad.push(name);
        _argv_e8c345a5_9f46_11ea_9024_a85e451255ad.push(ip);
        _argv_e8c345a5_9f46_11ea_9024_a85e451255ad.push(port);
        this.call_module_method("reg_server", _argv_e8c345a5_9f46_11ea_9024_a85e451255ad);
    }

}
/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_gm_center extends abelkhan.Imodule {
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_gm_center");
        modules.reg_module(this);

    }
}

export class gm_center_caller extends abelkhan.Icaller {
    public rsp_cb_gm_center_handle : rsp_cb_gm_center;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("gm_center", _ch);
        this.rsp_cb_gm_center_handle = new rsp_cb_gm_center(modules);
    }

    public confirm_gm(gm_name:string){
        let _argv_e8c345a6_9f46_11ea_b2d1_a85e451255ad:any[] = [];
        _argv_e8c345a6_9f46_11ea_b2d1_a85e451255ad.push(gm_name);
        this.call_module_method("confirm_gm", _argv_e8c345a6_9f46_11ea_b2d1_a85e451255ad);
    }

    public close_clutter(gmname:string){
        let _argv_e8c345a7_9f46_11ea_824d_a85e451255ad:any[] = [];
        _argv_e8c345a7_9f46_11ea_824d_a85e451255ad.push(gmname);
        this.call_module_method("close_clutter", _argv_e8c345a7_9f46_11ea_824d_a85e451255ad);
    }

    public reload(gmname:string){
        let _argv_e8c345a8_9f46_11ea_9339_a85e451255ad:any[] = [];
        _argv_e8c345a8_9f46_11ea_9339_a85e451255ad.push(gmname);
        this.call_module_method("reload", _argv_e8c345a8_9f46_11ea_9339_a85e451255ad);
    }

}
/*this module code is codegen by abelkhan codegen for typescript*/
export class center_call_hub_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("center_call_hub");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("distribute_server_address", this.distribute_server_address.bind(this));
        this.reg_method("reload", this.reload.bind(this));
        this.cb_distribute_server_address = null;

        this.cb_reload = null;

    }

    public cb_distribute_server_address : (type:string, name:string, ip:string, port:number)=>void | null;
    distribute_server_address(inArray:any[]){
        let _argv_e8c345a9_9f46_11ea_ad82_a85e451255ad:any[] = [];
        _argv_e8c345a9_9f46_11ea_ad82_a85e451255ad.push(inArray[0]);
        _argv_e8c345a9_9f46_11ea_ad82_a85e451255ad.push(inArray[1]);
        _argv_e8c345a9_9f46_11ea_ad82_a85e451255ad.push(inArray[2]);
        _argv_e8c345a9_9f46_11ea_ad82_a85e451255ad.push(inArray[3]);
        if (this.cb_distribute_server_address){
            this.cb_distribute_server_address.apply(null, _argv_e8c345a9_9f46_11ea_ad82_a85e451255ad);
        }
    }

    public cb_reload : ()=>void | null;
    reload(inArray:any[]){
        let _argv_e8c345aa_9f46_11ea_822a_a85e451255ad:any[] = [];
        if (this.cb_reload){
            this.cb_reload.apply(null, _argv_e8c345aa_9f46_11ea_822a_a85e451255ad);
        }
    }

}
export class center_call_server_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("center_call_server");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("reg_server_sucess", this.reg_server_sucess.bind(this));
        this.reg_method("close_server", this.close_server.bind(this));
        this.cb_reg_server_sucess = null;

        this.cb_close_server = null;

    }

    public cb_reg_server_sucess : ()=>void | null;
    reg_server_sucess(inArray:any[]){
        let _argv_e8c345ab_9f46_11ea_b6d1_a85e451255ad:any[] = [];
        if (this.cb_reg_server_sucess){
            this.cb_reg_server_sucess.apply(null, _argv_e8c345ab_9f46_11ea_b6d1_a85e451255ad);
        }
    }

    public cb_close_server : ()=>void | null;
    close_server(inArray:any[]){
        let _argv_e8c345ac_9f46_11ea_8e3d_a85e451255ad:any[] = [];
        if (this.cb_close_server){
            this.cb_close_server.apply(null, _argv_e8c345ac_9f46_11ea_8e3d_a85e451255ad);
        }
    }

}
export class hub_call_center_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("hub_call_center");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("closed", this.closed.bind(this));
        this.cb_closed = null;

    }

    public cb_closed : ()=>void | null;
    closed(inArray:any[]){
        let _argv_e8c345ad_9f46_11ea_aade_a85e451255ad:any[] = [];
        if (this.cb_closed){
            this.cb_closed.apply(null, _argv_e8c345ad_9f46_11ea_aade_a85e451255ad);
        }
    }

}
export class center_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("center");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("reg_server", this.reg_server.bind(this));
        this.cb_reg_server = null;

    }

    public cb_reg_server : (type:string, name:string, ip:string, port:number)=>void | null;
    reg_server(inArray:any[]){
        let _argv_e8c345ae_9f46_11ea_908e_a85e451255ad:any[] = [];
        _argv_e8c345ae_9f46_11ea_908e_a85e451255ad.push(inArray[0]);
        _argv_e8c345ae_9f46_11ea_908e_a85e451255ad.push(inArray[1]);
        _argv_e8c345ae_9f46_11ea_908e_a85e451255ad.push(inArray[2]);
        _argv_e8c345ae_9f46_11ea_908e_a85e451255ad.push(inArray[3]);
        if (this.cb_reg_server){
            this.cb_reg_server.apply(null, _argv_e8c345ae_9f46_11ea_908e_a85e451255ad);
        }
    }

}
export class gm_center_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("gm_center");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("confirm_gm", this.confirm_gm.bind(this));
        this.reg_method("close_clutter", this.close_clutter.bind(this));
        this.reg_method("reload", this.reload.bind(this));
        this.cb_confirm_gm = null;

        this.cb_close_clutter = null;

        this.cb_reload = null;

    }

    public cb_confirm_gm : (gm_name:string)=>void | null;
    confirm_gm(inArray:any[]){
        let _argv_e8c345af_9f46_11ea_b811_a85e451255ad:any[] = [];
        _argv_e8c345af_9f46_11ea_b811_a85e451255ad.push(inArray[0]);
        if (this.cb_confirm_gm){
            this.cb_confirm_gm.apply(null, _argv_e8c345af_9f46_11ea_b811_a85e451255ad);
        }
    }

    public cb_close_clutter : (gmname:string)=>void | null;
    close_clutter(inArray:any[]){
        let _argv_e8c345b0_9f46_11ea_b38c_a85e451255ad:any[] = [];
        _argv_e8c345b0_9f46_11ea_b38c_a85e451255ad.push(inArray[0]);
        if (this.cb_close_clutter){
            this.cb_close_clutter.apply(null, _argv_e8c345b0_9f46_11ea_b38c_a85e451255ad);
        }
    }

    public cb_reload : (gmname:string)=>void | null;
    reload(inArray:any[]){
        let _argv_e8c345b1_9f46_11ea_86f3_a85e451255ad:any[] = [];
        _argv_e8c345b1_9f46_11ea_86f3_a85e451255ad.push(inArray[0]);
        if (this.cb_reload){
            this.cb_reload.apply(null, _argv_e8c345b1_9f46_11ea_86f3_a85e451255ad);
        }
    }

}
