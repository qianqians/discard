import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import config = require('../../service/config/config');
import conn = require('../../service/connectservice');
import center = require('../../protcol/ts/center');
import rl = require('readline-sync');

export class center_proxy{
    private gm_center_caller : center.gm_center_caller;
    constructor(ch:abelkhan.Ichannel, modules:abelkhan.modulemng){
        this.gm_center_caller = new center.gm_center_caller(ch, modules);
    }

    public confirm_gm(gm_name:string){
        this.gm_center_caller.confirm_gm(gm_name);
    }

    public close_clutter(gm_name:string){
        this.gm_center_caller.close_clutter(gm_name);
    }

    public reload(gm_name:string){
        this.gm_center_caller.reload(gm_name);
    }
}

export class gm{
    public gm_name : string;
    public _center_proxy : center_proxy;
    public _is_init : boolean;
    private modules : abelkhan.modulemng;
    private _conn_center : conn.connectservice;

    constructor(cfg_file:string, cfg_name:string, gm_name:string){
        this.gm_name = gm_name;
        this._is_init = false;

        let _root_cfg = config.config(cfg_file);
        let _config = _root_cfg[cfg_name];

        this.modules = new abelkhan.modulemng();
        this._conn_center = new conn.connectservice(this.modules);
        this._conn_center.connect(_config["gm_ip"], _config["gm_port"], (ch:abelkhan.Ichannel)=>{
            this._center_proxy = new center_proxy(ch, this.modules);
            this._center_proxy.confirm_gm(gm_name);
            this._is_init = true;
        });
    }
}

function output_cmd()
{
    console.log("Enter gm cmd:");
    console.log(" close-----close clutter");
    console.log(" reload-----reload hub");
    console.log(" q----quit");
}

(function (){
    let args = process.argv.splice(2);
    let gm_name:string = "";
    let cmd:string[] = [];
    if (args.length > 3){
        gm_name = args[2];

        for (let i = 3; i < args.length; ++i){
            cmd.push(args[i]);
        }
    }
    else{
        gm_name = rl.question("Enter gm name:");
    }

    let _gm = new gm(args[0], args[1], gm_name);

    let cmd_callback:{[key:string]:()=>void} = {};
    cmd_callback["close"] = ()=>{
        _gm._center_proxy.close_clutter(_gm.gm_name);
    }
    cmd_callback["reload"] = ()=>{
        _gm._center_proxy.reload(_gm.gm_name);
    }

    let cb = ()=>{
        if (!_gm._is_init){
            setTimeout(() => {cb();}, 1000);
            return;
        }

        let running = true;
        while (running){
            if (cmd.length > 0){
                cmd_callback[cmd[0]]();
                running = false;
            }
            else{
                output_cmd();

                let cmd1 = rl.question("?");
                if (cmd1 == "q"){
                    process.exit();
                }

                cmd_callback[cmd1]();
            }
        }
    }

    cb();

})()
