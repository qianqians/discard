import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center = require('../../protcol/ts/center');

export class centerproxy{
	private _center_caller : center.center_caller;
	public is_reg_sucess : boolean;
	constructor(ch:abelkhan.Ichannel, _modules:abelkhan.modulemng){
		this._center_caller = new center.center_caller(ch, _modules);
		this.is_reg_sucess = false;
	}

	public reg_dbproxy(svr_name:string, ip:string, port:number){
		this._center_caller.reg_server("dbproxy", svr_name, ip, port);
	}

}
