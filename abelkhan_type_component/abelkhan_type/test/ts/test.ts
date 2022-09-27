import abelkhan = require("../../abelkhan_type/ts/abelkhan");
import { v1 as uuidv1 } from 'uuid'
/*this enum code is codegen by abelkhan codegen for ts*/

/*this struct code is codegen by abelkhan codegen for typescript*/
export class test1
{
    public argv1 : number;
    public argv2 : string;
    public argv3 : number;
    public argv4 : number;

    constructor(_argv1 : number, _argv2 : string, _argv3 : number, _argv4 : number){
        this.argv1 = _argv1;
        this.argv2 = _argv2;
        this.argv3 = _argv3;
        this.argv4 = _argv4;
    }
}
export function test1_to_protcol(_struct:test1){
    let _protocol:any[] = [];
    _protocol.push(_struct.argv1);
    _protocol.push(_struct.argv2);
    _protocol.push(_struct.argv3);
    _protocol.push(_struct.argv4);
    return _protocol;
}
export function protcol_to_test1(_protocol:any[]){
    let _argv1 = _protocol[0] as number;
    let _argv2 = _protocol[1] as string;
    let _argv3 = _protocol[2] as number;
    let _argv4 = _protocol[3] as number;
    let _struct = new test1(
        _argv1,
        _argv2,
        _argv3,
        _argv4);
    return _struct;
}
export class test2
{
    public argv1 : number;
    public argv2 : test1;

    constructor(_argv1 : number, _argv2 : test1){
        this.argv1 = _argv1;
        this.argv2 = _argv2;
    }
}
export function test2_to_protcol(_struct:test2){
    let _protocol:any[] = [];
    _protocol.push(_struct.argv1);
    _protocol.push(test1_to_protcol(_struct.argv2));
    return _protocol;
}
export function protcol_to_test2(_protocol:any[]){
    let _argv1 = _protocol[0] as number;
    let _argv2 = protcol_to_test1(_protocol[1]);
    let _struct = new test2(
        _argv1,
        _argv2);
    return _struct;
}
/*this caller code is codegen by abelkhan codegen for typescript*/
export class cb_test3{
    public event_test3_handle_cb : (t1:test1)=>void | null;
    public event_test3_handle_err : (err:number)=>void | null;
    constructor(){
        this.event_test3_handle_cb = null;
        this.event_test3_handle_err = null;
    }

    callBack(_cb:(t1:test1)=>void, _err:(err:number)=>void)
    {
        this.event_test3_handle_cb = _cb;
        this.event_test3_handle_err = _err;
    }
}

/*this cb code is codegen by abelkhan for ts*/
export class rsp_cb_test extends abelkhan.Imodule {
    public map_test3:Map<string, cb_test3>;
    constructor(modules:abelkhan.modulemng){
        super("rsp_cb_test");
        modules.reg_module(this);

        this.map_test3 = new Map<string, cb_test3>();
        this.reg_method("test3_rsp", this.test3_rsp.bind(this));
        this.reg_method("test3_err", this.test3_err.bind(this));
    }
    public test3_rsp(inArray:any[]){
        let uuid = inArray[0];
        let _argv_84ca4e8f_9f37_11ea_b8b8_a85e451255ad:any[] = [];
        _argv_84ca4e8f_9f37_11ea_b8b8_a85e451255ad.push(protcol_to_test1(inArray[1]));
        var rsp = this.map_test3.get(uuid);
        rsp.event_test3_handle_cb.apply(null, _argv_84ca4e8f_9f37_11ea_b8b8_a85e451255ad);
        this.map_test3.delete(uuid);
    }
    public test3_err(inArray:any[]){
        let uuid = inArray[0];
        let _argv_84caeacf_9f37_11ea_bec2_a85e451255ad:any[] = [];
        _argv_84caeacf_9f37_11ea_bec2_a85e451255ad.push(inArray[1]);
        var rsp = this.map_test3.get(uuid);
        rsp.event_test3_handle_err.apply(null, _argv_84caeacf_9f37_11ea_bec2_a85e451255ad);
        this.map_test3.delete(uuid);
    }
}

export class test_caller extends abelkhan.Icaller {
    public rsp_cb_test_handle : rsp_cb_test;
    constructor(_ch:any, modules:abelkhan.modulemng){
        super("test", _ch);
        this.rsp_cb_test_handle = new rsp_cb_test(modules);
    }

    public test3(t2:test2){
        let uuid_84caead0_9f37_11ea_bb1b_a85e451255ad = uuidv1();

        let _argv_84caead1_9f37_11ea_b2cc_a85e451255ad:any[] = [uuid_84caead0_9f37_11ea_bb1b_a85e451255ad];
        _argv_84caead1_9f37_11ea_b2cc_a85e451255ad.push(test2_to_protcol(t2));
        this.call_module_method("test3", _argv_84caead1_9f37_11ea_b2cc_a85e451255ad);
        let cb_test3_obj = new cb_test3();
        this.rsp_cb_test_handle.map_test3.set(uuid_84caead0_9f37_11ea_bb1b_a85e451255ad, cb_test3_obj);

        return cb_test3_obj;
    }

    public test4(argv:test2[]){
        let _argv_84caead2_9f37_11ea_9056_a85e451255ad:any[] = [];
        let _array_84caead3_9f37_11ea_9405_a85e451255ad:any[] = [];
        for(let v_84caead4_9f37_11ea_bb08_a85e451255ad of _name){
            _array_84caead3_9f37_11ea_9405_a85e451255ad.push(test2_to_protcol(v_84caead4_9f37_11ea_bb08_a85e451255ad));
        }
        _argv_84caead2_9f37_11ea_9056_a85e451255ad.push(_array_84caead3_9f37_11ea_9405_a85e451255ad);
        this.call_module_method("test4", _argv_84caead2_9f37_11ea_9056_a85e451255ad);
    }

}
/*this module code is codegen by abelkhan codegen for typescript*/
export class rsp_test3 extends abelkhan.Icaller {
    private uuid : string;
    constructor(_ch:any, _uuid:string){
        super("rsp_cb_test", _ch);
        this.uuid = _uuid;
    }

    public rsp(t1:test1){
        let _argv_84caead6_9f37_11ea_a3b9_a85e451255ad:any[] = [this.uuid];
        _argv_84caead6_9f37_11ea_a3b9_a85e451255ad.push(test1_to_protcol(t1));
        this.call_module_method("test3_rsp", _argv_84caead6_9f37_11ea_a3b9_a85e451255ad);
    }

    public err(err:number){
        let _argv_84caead7_9f37_11ea_b4e5_a85e451255ad:any[] = [this.uuid];
        _argv_84caead7_9f37_11ea_b4e5_a85e451255ad.push(err);
        this.call_module_method("test3_err", _argv_84caead7_9f37_11ea_b4e5_a85e451255ad);
    }

}

export class test_module extends abelkhan.Imodule {
    private modules:abelkhan.modulemng;
    constructor(modules:abelkhan.modulemng){
        super("test");
        this.modules = modules;
        this.modules.reg_module(this);

        this.reg_method("test3", this.test3.bind(this));
        this.reg_method("test4", this.test4.bind(this));
        this.cb_test3 = null;

        this.cb_test4 = null;

    }

    public cb_test3 : (t2:test2)=>void | null;
    test3(inArray:any[]){
        let _cb_uuid = inArray[0];
        let _argv_84caead5_9f37_11ea_92bc_a85e451255ad:any[] = [];
        _argv_84caead5_9f37_11ea_92bc_a85e451255ad.push(protcol_to_test2(inArray[1]));
        this.rsp = new rsp_test3(this.current_ch, _cb_uuid);
        if (cb_test3){
            cb_test3.apply(null, _argv_84caead5_9f37_11ea_92bc_a85e451255ad);
        }
        this.rsp = null;
    }

    public cb_test4 : (argv:test2[])=>void | null;
    test4(inArray:any[]){
        let _argv_84caead8_9f37_11ea_8630_a85e451255ad:any[] = [];
        let _array_84caead9_9f37_11ea_8ee5_a85e451255ad:any[] = [];        for(let v_84caeada_9f37_11ea_9d8e_a85e451255ad of inArray[0]){
            _array_84caead9_9f37_11ea_8ee5_a85e451255ad.push(protcol_to_test2(v_84caeada_9f37_11ea_9d8e_a85e451255ad));
        }
        _argv_84caead8_9f37_11ea_8630_a85e451255ad.push(_array_84caead9_9f37_11ea_8ee5_a85e451255ad);
        if (this.cb_test4){
            this.cb_test4.apply(null, _argv_84caead8_9f37_11ea_8630_a85e451255ad);
        }
    }

}
