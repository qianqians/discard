import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');

export class hubproxy{
    public is_closed : boolean = false;

    private center_call_hub_caller : center_protcol.center_call_hub_caller;
    constructor(ch:abelkhan.Ichannel, modules:abelkhan.modulemng){
        this.center_call_hub_caller = new center_protcol.center_call_hub_caller(ch, modules);
    }

    public distribute_server_address(type:string, name:string, ip:string, port:number){
        this.center_call_hub_caller.distribute_server_address(type, name, ip, port);
	}

    public reload(){
        this.center_call_hub_caller.reload();
    }
}

export class hubmanager{
    private modules : abelkhan.modulemng;
    private hubproxys : Map<abelkhan.Ichannel, hubproxy>;
    
    constructor(modules:abelkhan.modulemng){
        this.modules = modules;
        this.hubproxys = new Map<abelkhan.Ichannel, hubproxy>();
    }

    public reg_hub(ch:abelkhan.Ichannel){
		let _hubproxy = new hubproxy(ch, this.modules);
		this.hubproxys.set(ch, _hubproxy);
		return _hubproxy;
    }
    
    public get_hub(ch:abelkhan.Ichannel){
        return this.hubproxys.get(ch);
    }

    public for_each_hub(fn:(_proxy:hubproxy)=>void){
        this.hubproxys.forEach((_proxy:hubproxy)=>{
            fn(_proxy);
        });
    }

    public hub_closed(ch:abelkhan.Ichannel){
        let _proxy = this.hubproxys.get(ch);
        if (_proxy){
            _proxy.is_closed = true;
        }
    }

    public check_all_hub_closed(){
        let _all_closed = true;
        this.hubproxys.forEach((_proxy:hubproxy)=>{
            if (!_proxy.is_closed){
                _all_closed = false;
            }
        });
        return _all_closed;
    }

}
