export class closeHandle{
	public _is_close : Boolean;
	constructor(){
		this._is_close = false;
	}

	public is_closed(){
		return this._is_close;
	}

}