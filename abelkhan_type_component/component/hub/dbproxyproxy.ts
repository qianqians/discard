import { v1 as uuidv1 } from 'uuid'
import abelkhan = require('../../abelkhan_type/ts/abelkhan');
import dbproxy_protcol = require('../../protcol/ts/dbproxy');

export class dbproxyproxy{
    public hub_call_dbproxy_caller : dbproxy_protcol.hub_call_dbproxy_caller;
    public callback_set : Map<string, Function>;
    public end_cb_set : Map<string, Function>;
    private _collection : Collection;
    constructor(ch:abelkhan.Ichannel, modules:abelkhan.modulemng){
        this.hub_call_dbproxy_caller = new dbproxy_protcol.hub_call_dbproxy_caller(ch, modules);
        this.callback_set = new Map<string, Function>();
        this.end_cb_set = new Map<string, Function>();
        this._collection = new Collection("_", "_", this);
    }

    public reg_hub(name:string){
        this.hub_call_dbproxy_caller.reg_hub(name);
    }

    public getCollection(db:string, collection:string){
        this._collection.set_db_collection(db, collection);
        return this._collection;
    }

    public begin_callback(callbackid:string){
        return this.callback_set.get(callbackid);
    }

    public end_callback(callbackid:string){
        if (this.callback_set.has(callbackid)){
            this.callback_set.delete(callbackid);
        }
    }

    public end_get_object_info_callback(callbackid:string){
        this.end_callback(callbackid);

        if (this.end_cb_set.has(callbackid)){
            let cb = this.end_cb_set.get(callbackid);
            this.end_cb_set.delete(callbackid);
            return cb;
        }

        return null;
    }

}

export class Collection{
    private _db : string;
    private _collection : string;
    private _dbproxy : dbproxyproxy;
    constructor(db:string, collection:string, dbproxy:dbproxyproxy){
        this._db = db;
        this._collection = collection;
        this._dbproxy = dbproxy;
    }

    public set_db_collection(db:string, collection:string){
        this._db = db;
        this._collection = collection;
    }

    public createPersistedObject(object_info:any, _handle:(is_create_sucess)=>void){
        let callbackid = uuidv1();
        this._dbproxy.hub_call_dbproxy_caller.create_persisted_object(this._db, this._collection, JSON.stringify(object_info), callbackid);
        this._dbproxy.callback_set.set(callbackid, _handle);
    }

    public updataPersistedObject(query_json:any, updata_info:any, _handle:(is_update_sucess)=>void){
        let callbackid = uuidv1();
        this._dbproxy.hub_call_dbproxy_caller.updata_persisted_object(this._db, this._collection, JSON.stringify(query_json), JSON.stringify(updata_info), callbackid);
        this._dbproxy.callback_set.set(callbackid, _handle);
    }

    public getObjectCount(query_json:any, _handle:(count:number)=>void){
        let callbackid = uuidv1();
        this._dbproxy.hub_call_dbproxy_caller.get_object_count(this._db, this._collection, JSON.stringify(query_json), callbackid);
        this._dbproxy.callback_set.set(callbackid, _handle);
    }

    public getObjectInfo(query_json:any, _handle:(data:any[])=>void, _end:()=>void){
        let callbackid = uuidv1();
        this._dbproxy.hub_call_dbproxy_caller.get_object_info(this._db, this._collection, JSON.stringify(query_json), callbackid);
        this._dbproxy.callback_set.set(callbackid, _handle);
        this._dbproxy.end_cb_set.set(callbackid, _end);
    }

    public removeObject(query_json:any, _handle:(is_del_sucess)=>void){
        let callbackid = uuidv1();
        this._dbproxy.hub_call_dbproxy_caller.remove_object(this._db, this._collection, JSON.stringify(query_json), callbackid);
        this._dbproxy.callback_set.set(callbackid, _handle);
    }
}