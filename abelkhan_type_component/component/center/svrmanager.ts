import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import center_protcol = require('../../protcol/ts/center');

export class svrproxy
{
    public type : string;
    public name : string;
    public ip : string;
    public port : number;
    private center_call_server_caller: center_protcol.center_call_server_caller;
    constructor(ch:abelkhan.Ichannel, type:string, name:string, ip:string, port:number, modules:abelkhan.modulemng){
        this.type = type;
        this.name = name;
        this.ip = ip;
        this.port = port;

        this.center_call_server_caller = new center_protcol.center_call_server_caller(ch, modules);
    }

    public reg_server_sucess()
    {
        this.center_call_server_caller.reg_server_sucess();
    }

    public close_server()
    {
        this.center_call_server_caller.close_server();
    }
}

export class svrmanager
{
    private modules : abelkhan.modulemng;
    private dbproxys : svrproxy[];
    private svrproxys : Map<abelkhan.Ichannel, svrproxy>;

    constructor(modules:abelkhan.modulemng){
        this.modules = modules;
        this.dbproxys = [];
        this.svrproxys = new Map<abelkhan.Ichannel, svrproxy>();
    }

    public reg_svr(ch:abelkhan.Ichannel, type:string, name:string, ip:string, port:number){
        let _svrproxy = new svrproxy(ch, type, name, ip, port, this.modules)
        this.svrproxys.set(ch, _svrproxy);
        if (type == "dbproxy"){
            this.dbproxys.push(_svrproxy);
        }
        return _svrproxy;
    }

    public close_db(){
        for(let _proxy of this.dbproxys){
            _proxy.close_server();
        }
    }

    public for_each_svr(fn:(_proxy:svrproxy)=>void){
        this.svrproxys.forEach((_proxy:svrproxy)=>{
            fn(_proxy);
        });
    }

    public get_svr(ch:abelkhan.Ichannel){
        return this.svrproxys.get(ch);
    }
}
