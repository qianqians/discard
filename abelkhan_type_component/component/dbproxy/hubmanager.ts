import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import dbproxy_protcol = require('../../protcol/ts/dbproxy');

export class hubproxy{
	private hub_name : string;
	private dbproxy_caller_hub : dbproxy_protcol.dbproxy_call_hub_caller;
	constructor(ch:abelkhan.Ichannel, hub_name:string, modules:abelkhan.modulemng){
		this.hub_name = hub_name;
		this.dbproxy_caller_hub = new dbproxy_protcol.dbproxy_call_hub_caller(ch, modules);
	}

	public reg_hub_sucess(){
		this.dbproxy_caller_hub.reg_hub_sucess();
	}

	public ack_create_persisted_object(cbid:string, is_create_sucess:boolean){
		this.dbproxy_caller_hub.ack_create_persisted_object(cbid, is_create_sucess);
	}

	public ack_updata_persisted_object(callbackid:string, is_update_sucess:boolean){
		this.dbproxy_caller_hub.ack_updata_persisted_object(callbackid, is_update_sucess);
	}

    public ack_get_object_count(callbackid:string, count:number){
        this.dbproxy_caller_hub.ack_get_object_count(callbackid, count);
    }

	public ack_get_object_info(callbackid:string, json_str_object_info_list:string){
		this.dbproxy_caller_hub.ack_get_object_info(callbackid, json_str_object_info_list);
	}

	public ack_get_object_info_end(callbackid:string){
		this.dbproxy_caller_hub.ack_get_object_info_end(callbackid);
	}

    public ack_remove_object(callbackid:string, is_remove_sucess:boolean){
        this.dbproxy_caller_hub.ack_remove_object(callbackid, is_remove_sucess);
    }
}

export class hubmanager{
	private modules : abelkhan.modulemng;
	private hubproxys : Map<abelkhan.Ichannel, hubproxy>;
	constructor(modules:abelkhan.modulemng){
		this.modules = modules;
		this.hubproxys = new Map<abelkhan.Ichannel, hubproxy>();
	}

	public reg_hub(ch:abelkhan.Ichannel, svr_name:string){
		let _proxy = new hubproxy(ch, svr_name, this.modules);
		this.hubproxys.set(ch, _proxy);
		return _proxy;
	}

	public get_hub(ch:abelkhan.Ichannel){
		return this.hubproxys.get(ch);
	}
}
