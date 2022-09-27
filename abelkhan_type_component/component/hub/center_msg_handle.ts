import closehandle = require('./closehandle');
import centerproxy = require('./centerproxy');
import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');
import signals = require('../../service/signals/signals');

export class server_info{
	public type:string;
	public name:string;
	public ip:string;
	public port:number;
	constructor(type:string, name:string, ip:string, port:number){
		this.type = type;
		this.name = name;
		this.ip = ip;
		this.port = port;
	}
}

export class center_msg_handle{
	public signals_close : signals.signals<void>;
	public signals_svr : signals.signals<server_info>;
	public signals_reload : signals.signals<void>;

	private _closehandle : closehandle.closehandle;
	private _centerproxy : centerproxy.centerproxy;
	private center_call_server_module : center_protcol.center_call_server_module;
	private center_call_hub_module : center_protcol.center_call_hub_module;
	constructor(modules:abelkhan.modulemng, _closehandle:closehandle.closehandle, _centerproxy:centerproxy.centerproxy){
		this.signals_close = new signals.signals<void>();
		this.signals_svr = new signals.signals<server_info>();
		this.signals_reload = new signals.signals<void>();

		this._closehandle = _closehandle;
		this._centerproxy = _centerproxy;

		this.center_call_server_module = new center_protcol.center_call_server_module(modules);
		this.center_call_server_module.cb_reg_server_sucess = this.reg_server_sucess.bind(this);
		this.center_call_server_module.cb_close_server = this.close_server.bind(this);

		this.center_call_hub_module = new center_protcol.center_call_hub_module(modules);
		this.center_call_hub_module.cb_distribute_server_address = this.distribute_server_address.bind(this);
		this.center_call_hub_module.cb_reload = this.reload.bind(this);
	}

	public reg_server_sucess(){
		this._centerproxy.is_reg_center_sucess = true;
	}

	public close_server(){
		this.signals_close.emit();
	}

	public distribute_server_address(type:string, name:string, ip:string, port:number){
		this.signals_svr.emit(new server_info(type, name, ip, port));
	}

	public reload(){
		this.signals_reload.emit();
	}

}