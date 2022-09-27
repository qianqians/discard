import path = require('path');
import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import config = require('../../service/config/config');
import log = require('../../service/log/log');
import acceptservice = require('../../service/acceptservice');
import connectservice = require('../../service/connectservice');
import closeHandle = require('./closehandle');
import centerproxy = require('./centerproxy');
import center_msg_handle = require('./center_msg_handle');
import hubmanager = require('./hubmanager');
import hub_msg_handle = require('./hub_msg_handle');
import mongodbproxy = require('./mongodbproxy');

export class dbproxy{
	private modules : abelkhan.modulemng;
	private _mongodbproxy : mongodbproxy.mongodbproxy;
	private _closeHandle : closeHandle.closeHandle;
	private _centerproxy : centerproxy.centerproxy;
	private _center_msg_handle : center_msg_handle.center_msg_handle;
	private _center_connectservice : connectservice.connectservice;
	private _hubmanager : hubmanager.hubmanager;
	private _hub_msg_handle : hub_msg_handle.hub_msg_handle;
	private _hub_acceptservice : acceptservice.acceptservice;
	private _timetmp : number;
	constructor(cfg_file:string, cfg_name:string){
		let _root_cfg = config.config(cfg_file);
		let _config = _root_cfg[cfg_name];
		let _center_config = _root_cfg["center"];

        log.configLogger(path.join(_config["log_dir"], _config["log_file"]), _config["log_level"]);
		log.getLogger().trace("config logger!");
		
		this.modules = new abelkhan.modulemng();
		this._closeHandle = new closeHandle.closeHandle();
		this._mongodbproxy = new mongodbproxy.mongodbproxy(_config["db_url"]);

		if (_config["index"]){
			for(let index of _config["index"]){
				this._mongodbproxy.create_index(index["db"], index["collection"], index["key"], index["is_unique"]);
			}
		}

		this._hubmanager = new hubmanager.hubmanager(this.modules);
		this._hub_msg_handle = new hub_msg_handle.hub_msg_handle(this.modules, this._hubmanager, this._mongodbproxy);
		this._hub_acceptservice = new acceptservice.acceptservice(_config["ip"], _config["port"], this.modules);

		this._center_connectservice = new connectservice.connectservice(this.modules);
		this._center_connectservice.connect(_center_config["ip"], _center_config["port"], (ch:abelkhan.Ichannel)=>{
			this._centerproxy = new centerproxy.centerproxy(ch, this.modules);
			this._center_msg_handle = new center_msg_handle.center_msg_handle(this.modules, this._closeHandle, this._centerproxy);
			this._centerproxy.reg_dbproxy(_config["name"], _config["ip"], _config["port"]);
		});

		this._timetmp = Date.now();
	}

	public poll(){
        if (this._closeHandle.is_closed()){
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
