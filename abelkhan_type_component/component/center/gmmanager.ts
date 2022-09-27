import abelkhan = require('../../abelkhan_type/ts/abelkhan');

export class gmmanager{
	private gms : Map<abelkhan.Ichannel, string>;
	constructor(){
		this.gms = new Map<abelkhan.Ichannel, string>();
	}

	public reg_gm(gmname:string, ch:abelkhan.Ichannel){
		this.gms.set(ch, gmname);
	}

	public check_gm(gmname:string, ch:abelkhan.Ichannel){
		let gm_name = this.gms.get(ch);
		if (!gm_name){
			return false;
		}

		return gm_name === gmname;
	}
}