import path = require('path');
import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import config = require('../../service/config/config');
import log = require('../../service/log/log');
import signals = require('../../service/signals/signals');
import cryptacceptservice = require('../../service/cryptacceptservice');
import cryptchannel = require('../../service/cryptchannel');
import connectservice = require('../../service/connectservice');
import enetservice = require('../../service/enetservice');
import center_msg_handle = require('./center_msg_handle');
import centerproxy = require('./centerproxy');
import closehandle = require('./closehandle');
import dbproxy_msg_handle = require('./dbproxy_msg_handle');
import dbproxyproxy = require('./dbproxyproxy');
import hub_msg_handle = require('./hub_msg_handle');
import hub_protcol = require('../../protcol/ts/hub');
import xor_key_protcol = require('../../protcol/ts/xor_key');

export class outAddr{
    public host : string;
    public port : number;
    constructor(host:string, port:number){
        this.host = host;
        this.port = port;
    }
}

function gen_xor_byte(){
    let key = 0;
    while(key === 0){
        key = Math.floor((Math.random() * 256));
    }
    return key;
}

function gen_xor_key(){
    let xor_key0 = gen_xor_byte();
    let xor_key1 = gen_xor_byte();
    let xor_key2 = gen_xor_byte();
    let xor_key3 = gen_xor_byte();

    return Math.abs(xor_key0 << 24 | xor_key1 << 16 | xor_key2 << 8 | xor_key3);
}

export class hub{
    public signals_close : signals.signals<void>;
    public signals_connect_db : signals.signals<void>;
    public signals_reload : signals.signals<void>;
    public signals_hubproxy : signals.signals<hub_msg_handle.hubproxy>;
    public signals_client_channel : signals.signals<abelkhan.Ichannel>;
    public signals_client_disconnect : signals.signals<abelkhan.Ichannel>;
    public modules : abelkhan.modulemng;
    public name : string;
    public hub_type : string;
    public _outAddr : outAddr|null;
    public xor_key : number;
    private closehandle : closehandle.closehandle;
    private connectservice : connectservice.connectservice;
    private center_msg_handle : center_msg_handle.center_msg_handle;
    private centerproxy : centerproxy.centerproxy;
    private dbproxy_msg_handle : dbproxy_msg_handle.dbproxy_msg_handle;
    private dbproxyproxy : dbproxyproxy.dbproxyproxy;
    private enetservice : enetservice.enetservice;
    private hub_msg_handle : hub_msg_handle.hub_msg_handle;
    private cryptacceptservice : cryptacceptservice.cryptacceptservice;
    private _timetmp : number;
    constructor(cfg_file:string, cfg_name:string){
        let _root_cfg = config.config(cfg_file);
		let _config = _root_cfg[cfg_name];
        let _center_config = _root_cfg["center"];
        
        log.configLogger(path.join(_config["log_dir"], _config["log_file"]), _config["log_level"]);
        log.getLogger().trace("config logger!");
        
        this.name = _config["name"];
        this.hub_type = _config["hub_type"];
        
        this.signals_close = new signals.signals<void>();
        this.signals_connect_db = new signals.signals<void>();
        this.signals_reload = new signals.signals<void>();
        this.signals_hubproxy = new signals.signals<hub_msg_handle.hubproxy>();
        this.signals_client_channel = new signals.signals<abelkhan.Ichannel>();
        this.signals_client_disconnect = new signals.signals<abelkhan.Ichannel>();
        
		this.modules = new abelkhan.modulemng();
        this.closehandle = new closehandle.closehandle();
        this.connectservice = new connectservice.connectservice(this.modules);

        this.enetservice = new enetservice.enetservice(_config["ip"], _config["port"], this.modules);
        this.hub_msg_handle = new hub_msg_handle.hub_msg_handle(this.modules);
        this.hub_msg_handle.signals_hubproxy.connect((hubproxy:hub_msg_handle.hubproxy)=>{
            this.signals_hubproxy.emit(hubproxy);
        });

        this.connectservice.connect(_center_config["ip"], _center_config["port"], (ch:abelkhan.Ichannel)=>{
            this.centerproxy = new centerproxy.centerproxy(ch, this.modules);
            this.center_msg_handle = new center_msg_handle.center_msg_handle(this.modules, this.closehandle, this.centerproxy);
            this.center_msg_handle.signals_close.connect(()=>{
                this.signals_close.emit();
            });
            this.center_msg_handle.signals_reload.connect(()=>{
                this.signals_reload.emit();
            });
            this.center_msg_handle.signals_svr.connect((svr_info:center_msg_handle.server_info)=>{
                if (svr_info.type == "dbproxy"){
                    if (!_config["dbproxy"]){
                        return;
                    }

                    if (svr_info.name != _config["dbproxy"]){
                        return;
                    }

                    this.connectservice.connect(svr_info.ip, svr_info.port, (ch:abelkhan.Ichannel)=>{
                        this.dbproxyproxy = new dbproxyproxy.dbproxyproxy(ch, this.modules);
                        this.dbproxy_msg_handle = new dbproxy_msg_handle.dbproxy_msg_handle(this.modules, this.dbproxyproxy);
                        this.dbproxyproxy.reg_hub(this.name);
                        this.dbproxy_msg_handle.signals_connect_db.connect(()=>{
                            this.signals_connect_db.emit();
                        });
                    });
                }
                else if (svr_info.type == "hub"){
                    this.enetservice.connect(svr_info.ip, svr_info.port, (ch:abelkhan.Ichannel)=>{
                        let _caller = new hub_protcol.hub_call_hub_caller(ch, this.modules);
                        _caller.reg_hub(this.name, this.hub_type).callBack(()=>{
                            log.getLogger().info("reg hub:%s sucess", svr_info.name);
                        }, ()=>{
                            log.getLogger().error("reg hub:%s faild", svr_info.name);
                        });
                    });
                }
            });
            this.centerproxy.reg_hub(this.name, _config["ip"], _config["port"]);
        });

        this._outAddr = null;
        this.xor_key = 0;
        if (_config["out_host"] && _config["out_port"]){
            this._outAddr = new outAddr(_config["out_host"], _config["out_port"]);
            this.xor_key = _root_cfg["default_key"];

            this.cryptacceptservice = new cryptacceptservice.cryptacceptservice(this._outAddr.port, this.xor_key, this.modules);
            this.cryptacceptservice.signals_connect.connect((ch:cryptchannel.cryptchannel)=>{
                let new_key = gen_xor_key();
                let xor_key_caller = new xor_key_protcol.xor_key_caller(ch, this.modules);
                xor_key_caller.refresh_xor_key(new_key);
                ch.set_xor_key(new_key);
                this.signals_client_channel.emit(ch);
            });
            this.cryptacceptservice.signals_disconnect.connect((ch:abelkhan.Ichannel)=>{
                this.signals_client_disconnect.emit(ch);
            });
        }

        this._timetmp = Date.now();
    }

    public close(){
        setTimeout(()=>{this.closehandle.is_close = true;}, 3000);
    }

    public poll(){
        this.enetservice.poll();
        
        if (this.closehandle.is_close){
            setTimeout(()=>{
                process.exit();
            }, 200);
        }

        var _tmp_now = Date.now();
        var _tmp_time = _tmp_now - this._timetmp;
        this._timetmp = _tmp_now;
        if (_tmp_time < 50){
            setTimeout(this.poll.bind(this), 5);
        }
        else{
            setImmediate(this.poll.bind(this));
        }
    }
}