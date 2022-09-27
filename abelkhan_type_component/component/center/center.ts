import path = require('path');
import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import config = require('../../service/config/config');
import log = require('../../service/log/log');
import signals = require('../../service/signals/signals');
import acceptservice = require('../../service/acceptservice');
import closeHandle = require('./closehandle');
import gm_msg_handle = require('./gm_msg_handle');
import gms = require('./gmmanager');
import hub_msg_handle = require('./hub_msg_handle');
import hubs = require('./hubmanager');
import svr_msg_handle = require('./svr_msg_handle');
import svrs = require('./svrmanager');

export class center{
    public signals_svr_disconnect : signals.signals<svrs.svrproxy>;

    private modules : abelkhan.modulemng;
    private _closeHandle : closeHandle.closehandle;
    private _accept_svr_service : acceptservice.acceptservice;
    private _svr_msg_handle : svr_msg_handle.svr_msg_handle;
    private _svrmanager : svrs.svrmanager;
    private _hub_msg_handle : hub_msg_handle.hub_msg_handle;
    private _hubmanager : hubs.hubmanager;
    private _accept_gm_service : acceptservice.acceptservice;
    private _gm_msg_handle : gm_msg_handle.gm_msg_handle;
    private _gmmanager : gms.gmmanager;
    private _timetmp : number;

    constructor(cfg_file:string, cfg_name:string){
        let _root_cfg = config.config(cfg_file);
        let _config = _root_cfg[cfg_name];

        this._closeHandle = new closeHandle.closehandle();

        log.configLogger(path.join(_config["log_dir"], _config["log_file"]), _config["log_level"]);
        log.getLogger().trace("config logger!");

        this.modules = new abelkhan.modulemng();

        this.signals_svr_disconnect = new signals.signals<svrs.svrproxy>();
        this._svrmanager = new svrs.svrmanager(this.modules);
        this._hubmanager = new hubs.hubmanager(this.modules);
        this._svr_msg_handle = new svr_msg_handle.svr_msg_handle(this.modules, this._svrmanager, this._hubmanager);
        this._hub_msg_handle = new hub_msg_handle.hub_msg_handle(this.modules, this._svrmanager, this._hubmanager, this._closeHandle);
        this._accept_svr_service = new acceptservice.acceptservice(_config["ip"], _config["port"], this.modules);
        this._accept_svr_service.signals_disconnect.connect((ch)=>{
            let _proxy = this._svrmanager.get_svr(ch);
            if (_proxy){
                if (_proxy.type == "hub"){
                    let _hubproxy = this._hubmanager.get_hub(ch);
                    if (_hubproxy.is_closed && this._closeHandle.is_closing){
                        return;
                    }
                    this._hubmanager.hub_closed(ch);
                }

                this.signals_svr_disconnect.emit(_proxy);
            }
        });

        this._gmmanager = new gms.gmmanager();
        this._gm_msg_handle = new gm_msg_handle.gm_msg_handle(this.modules, this._svrmanager, this._hubmanager, this._gmmanager, this._closeHandle);
        this._accept_gm_service = new acceptservice.acceptservice(_config["gm_ip"], _config["gm_port"], this.modules);

        this._timetmp = Date.now();
    }

    public poll(){
        if (this._closeHandle.is_close){
            setTimeout(()=>{
                process.exit();
            }, 2000);
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