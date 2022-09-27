import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');

export class centerproxy{
    public is_reg_center_sucess : boolean;
    private center_caller : center_protcol.center_caller;
    private hub_call_center_caller : center_protcol.hub_call_center_caller;
    constructor(ch:abelkhan.Ichannel, modules:abelkhan.modulemng){
        this.is_reg_center_sucess = false;
        this.center_caller = new center_protcol.center_caller(ch, modules);
        this.hub_call_center_caller = new center_protcol.hub_call_center_caller(ch, modules);
    }

    public reg_hub(name:string, ip:string, port:number){
        this.center_caller.reg_server("hub", name, ip, port);
    }

    public closed(){
        this.hub_call_center_caller.closed();
    }
}