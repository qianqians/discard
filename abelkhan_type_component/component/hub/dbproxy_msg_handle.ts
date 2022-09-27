import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import signals = require('../../service/signals/signals');
import dbproxy_protcol = require('../../protcol/ts/dbproxy');
import dbproxyproxy = require('./dbproxyproxy');

export class dbproxy_msg_handle{
	public signals_connect_db : signals.signals<void>;
	private dbproxyproxy : dbproxyproxy.dbproxyproxy;
	private dbproxy_call_hub_module : dbproxy_protcol.dbproxy_call_hub_module;
	constructor(modules:abelkhan.modulemng, dbproxyproxy : dbproxyproxy.dbproxyproxy){
		this.signals_connect_db = new signals.signals<void>();
		this.dbproxyproxy = dbproxyproxy;
		
		this.dbproxy_call_hub_module = new dbproxy_protcol.dbproxy_call_hub_module(modules);
		this.dbproxy_call_hub_module.cb_reg_hub_sucess = this.reg_hub_sucess.bind(this);
		this.dbproxy_call_hub_module.cb_ack_create_persisted_object = this.ack_create_persisted_object.bind(this);
		this.dbproxy_call_hub_module.cb_ack_updata_persisted_object = this.ack_updata_persisted_object.bind(this);
		this.dbproxy_call_hub_module.cb_ack_get_object_count = this.ack_get_object_count.bind(this);
		this.dbproxy_call_hub_module.cb_ack_get_object_info = this.ack_get_object_info.bind(this);
		this.dbproxy_call_hub_module.cb_ack_get_object_info_end = this.ack_get_object_info_end.bind(this);
		this.dbproxy_call_hub_module.cb_ack_remove_object = this.ack_remove_object.bind(this);
	}

	public reg_hub_sucess(){
        this.signals_connect_db.emit();
	}

	public ack_create_persisted_object(callbackid:string, is_create_sucess:boolean){
		let _handle = <(is_create_sucess:boolean)=>void>(this.dbproxyproxy.begin_callback(callbackid));
		_handle(is_create_sucess);
		this.dbproxyproxy.end_callback(callbackid);
	}

	public ack_updata_persisted_object(callbackid:string, is_updata_sucess:boolean){
		let _handle = <(is_create_sucess:boolean)=>void>(this.dbproxyproxy.begin_callback(callbackid));
		_handle(is_updata_sucess);
		this.dbproxyproxy.end_callback(callbackid);
	}
	
	public ack_get_object_count(callbackid:string, count:number){
		let _handle = <(count:number)=>void>(this.dbproxyproxy.begin_callback(callbackid));
		_handle(count);
		this.dbproxyproxy.end_callback(callbackid);
	}

	public ack_get_object_info(callbackid:string, json_obejct_array:string){
		let _handle = <(data:any[])=>void>(this.dbproxyproxy.begin_callback(callbackid));
		_handle(JSON.parse(json_obejct_array));
	}

	public ack_get_object_info_end(callbackid:string){
		let _end = <()=>void>(this.dbproxyproxy.end_get_object_info_callback(callbackid));
		_end();
	}

	public ack_remove_object(callbackid:string, is_del_sucess:boolean){
		let _handle = <(is_del_sucess:boolean)=>void>(this.dbproxyproxy.begin_callback(callbackid));
		_handle(is_del_sucess);
		this.dbproxyproxy.end_callback(callbackid);
	}
}