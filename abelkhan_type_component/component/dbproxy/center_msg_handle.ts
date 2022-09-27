import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center = require('../../protcol/ts/center');
import closehandle = require('./closehandle');
import centerproxy = require('./centerproxy');

export class center_msg_handle{
	private center_call_server_module : center.center_call_server_module;
	private _close_handle : closehandle.closeHandle;
	private _centerproxy : centerproxy.centerproxy;
	constructor(modules:abelkhan.modulemng, _close_handle:closehandle.closeHandle, _centerproxy:centerproxy.centerproxy){
		this._close_handle = _close_handle;
		this._centerproxy = _centerproxy;

		this.center_call_server_module = new center.center_call_server_module(modules);
		this.center_call_server_module.cb_close_server = this.close_server.bind(this);
		this.center_call_server_module.cb_reg_server_sucess = this.reg_server_sucess.bind(this);
	}

	public close_server(){
		this._close_handle._is_close = true;
	}

	public reg_server_sucess(){
		this._centerproxy.is_reg_sucess = true;
	}
}
