import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import mongodbproxy = require('./mongodbproxy');
import hubmanager = require('./hubmanager');
import dbproxy_protcol = require('../../protcol/ts/dbproxy');
import log = require('../../service/log/log');

export class hub_msg_handle{
    private _hubmng : hubmanager.hubmanager;
    private _mongodbproxy : mongodbproxy.mongodbproxy;
    private hub_call_dbproxy_module : dbproxy_protcol.hub_call_dbproxy_module;
    constructor(modules:abelkhan.modulemng, _hubs:hubmanager.hubmanager, _dbproxy:mongodbproxy.mongodbproxy){
        this._hubmng = _hubs;
        this._mongodbproxy = _dbproxy;

        this.hub_call_dbproxy_module = new dbproxy_protcol.hub_call_dbproxy_module(modules);
        this.hub_call_dbproxy_module.cb_reg_hub = this.reg_hub.bind(this);
        this.hub_call_dbproxy_module.cb_create_persisted_object = this.create_persisted_object.bind(this);
        this.hub_call_dbproxy_module.cb_updata_persisted_object = this.updata_persisted_object.bind(this);
        this.hub_call_dbproxy_module.cb_remove_object = this.remove_object.bind(this);
        this.hub_call_dbproxy_module.cb_get_object_count = this.get_object_count.bind(this);
        this.hub_call_dbproxy_module.cb_get_object_info = this.get_object_info.bind(this);
    }

    public reg_hub(hub_name:string){
        log.getLogger().trace("hub:%s connected", hub_name);

		let _hubproxy = this._hubmng.reg_hub(this.hub_call_dbproxy_module.current_ch, hub_name);
		_hubproxy.reg_hub_sucess ();
    }
    
    public create_persisted_object(db:string, collection:string, object_info:string, callbackid:string){
        log.getLogger().trace("begin create_persisted_object");

        let _hubproxy = this._hubmng.get_hub(this.hub_call_dbproxy_module.current_ch);
        if (_hubproxy == null){
            log.getLogger().error("hubproxy is null");
            return;
        }

        this._mongodbproxy.save(db, collection, object_info, (is_save_sucess:boolean)=>{
            _hubproxy.ack_create_persisted_object(callbackid, is_save_sucess);
        })

        log.getLogger().trace("end create_persisted_object");
    }

    public updata_persisted_object(db:string, collection:string, query_json:string, object_info:string, callbackid:string){
        log.getLogger().trace("begin updata_persisted_object");

        let _hubproxy = this._hubmng.get_hub(this.hub_call_dbproxy_module.current_ch);
        if (_hubproxy == null){
            log.getLogger().error("hubproxy is null");
            return;
        }

        this._mongodbproxy.update(db, collection, query_json, object_info, (is_update_sucess:boolean)=>{
            _hubproxy.ack_updata_persisted_object(callbackid, is_update_sucess);
        });

        log.getLogger().trace("end updata_persisted_object");
    }

    public remove_object(db:string, collection:string, query_json:string, callbackid:string){
        log.getLogger().trace("begin remove_object");

        let _hubproxy = this._hubmng.get_hub(this.hub_call_dbproxy_module.current_ch);
        if (_hubproxy == null){
            log.getLogger().error("hubproxy is null");
            return;
        }

        this._mongodbproxy.remove(db, collection, query_json, (is_remove_sucess:boolean)=>{
            _hubproxy.ack_remove_object(callbackid, is_remove_sucess);
        });

        log.getLogger().trace("end remove_object");
    }

    public get_object_count(db:string, collection:string, query_json:string, callbackid:string){
        log.getLogger().trace("begin get_object_count");

        let _hubproxy = this._hubmng.get_hub(this.hub_call_dbproxy_module.current_ch);
        if (_hubproxy == null){
            log.getLogger().error("hubproxy is null");
            return;
        }

        this._mongodbproxy.count(db, collection, query_json, (count:number)=>{
            _hubproxy.ack_get_object_count(callbackid, count);
        });

        log.getLogger().trace("end get_object_count");
    }

    public get_object_info(db:string, collection:string, query_json:string, callbackid:string){
        log.getLogger().trace("begin get_object_info");

        let _hubproxy = this._hubmng.get_hub(this.hub_call_dbproxy_module.current_ch);
        if (_hubproxy == null){
            log.getLogger().error("hubproxy is null");
            return;
        }

        this._mongodbproxy.find(db, collection, query_json, (json_result:any[])=>{
            let count = 0;
            if (json_result.length == 0){
                _hubproxy.ack_get_object_info(callbackid, JSON.stringify([]));
            }
            else{
                let _datalist = [];
                for (let data of json_result)
                {
                    _datalist.push(data);

                    count++;
                    if (count >= 100){
                        _hubproxy.ack_get_object_info(callbackid, JSON.stringify(_datalist));

                        count = 0;
                        _datalist = [];
                    }
                }
                if (count > 0 && count < 100)
                {
                    _hubproxy.ack_get_object_info(callbackid, JSON.stringify(_datalist));
                }
            }
            _hubproxy.ack_get_object_info_end(callbackid);

        });

        log.getLogger().trace("end get_object_info");
    }
}
