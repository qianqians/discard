import mongodb = require('mongodb');

export class mongodbproxy{
    private _dbproxy : mongodb.MongoClient;
    private _dbproxy_is_init : boolean;
    private _cb : (()=>void)[];
    constructor(url:string){
        this._dbproxy_is_init = false;
        this._cb = [];

        mongodb.MongoClient.connect(url, { useNewUrlParser: true, useUnifiedTopology: true }).then((client:mongodb.MongoClient)=>{
            this._dbproxy = client;
            this._dbproxy_is_init = true;
            for(let cb of this._cb){
                cb();
            }
        }).catch((err:mongodb.MongoError)=>{
            if (err){
                throw err;
            }
        });
    }

    public create_index(db:string, collection:string, key:string, is_unique:boolean){
        let cb = ()=>{
            if (this._dbproxy_is_init){
                let _db = this._dbproxy.db(db);
                let _c = _db.collection(collection);
                _c.createIndex(key, {unique:is_unique});
            }
        }
        
        if (this._dbproxy_is_init){
            cb();
        }
        else{
            this._cb.push(cb);
        }
    }

    public save(db:string, collection:string, json_data:string, callback:(is_save_sucess:boolean)=>void){
        let cb = ()=>{
            if (this._dbproxy_is_init){
                let _db = this._dbproxy.db(db);
                let _c = _db.collection(collection);
                _c.insertOne(JSON.parse(json_data)).then((result:mongodb.InsertOneWriteOpResult<any>)=>{
                    callback(true);
                }).catch((err:mongodb.MongoError)=>{
                    callback(false);
                });
            }
        }

        if (this._dbproxy_is_init){
            cb();
        }
        else{
            this._cb.push(cb);
        }
    }

    public update(db:string, collection:string, json_query:string, json_update:string, callback:(is_update_sucess:boolean)=>void){
        let cb = ()=>{
            if (this._dbproxy_is_init){
                let _db = this._dbproxy.db(db);
                let _c = _db.collection(collection);
                _c.updateOne(JSON.parse(json_query), JSON.parse(json_update)).then((result:mongodb.UpdateWriteOpResult)=>{
                    callback(true);
                }).catch((err:mongodb.MongoError)=>{
                    callback(false);
                });
            }
        }

        if (this._dbproxy_is_init){
            cb();
        }
        else{
            this._cb.push(cb);
        }
    }

    public find(db:string, collection:string, json_query:string, callback:(json_result:any[])=>void){
        let cb = ()=>{
            if (this._dbproxy_is_init){
                let _db = this._dbproxy.db(db);
                let _c = _db.collection(collection);
                let _cursor = _c.find(JSON.parse(json_query));
                _cursor.toArray().then((result:any[])=>{
                    callback(result);
                }).catch((err:mongodb.MongoError)=>{
                    callback([]);
                });
            }
        }

        if (this._dbproxy_is_init){
            cb();
        }
        else{
            this._cb.push(cb);
        }
    }

    public count(db:string, collection:string, json_query:string, callback:(count:number)=>void){
        let cb = ()=>{
            if (this._dbproxy_is_init){
                let _db = this._dbproxy.db(db);
                let _c = _db.collection(collection);
                let _cursor = _c.find(JSON.parse(json_query));
                _cursor.count().then((_count:number)=>{
                    callback(_count);
                }).catch((err:mongodb.MongoError)=>{
                    callback(0);
                });
            }
        }

        if (this._dbproxy_is_init){
            cb();
        }
        else{
            this._cb.push(cb);
        }
    }

    public remove(db:string, collection:string, json_query:string, callback:(is_remove_sucess:boolean)=>void){
        let cb = ()=>{
            if (this._dbproxy_is_init){
                let _db = this._dbproxy.db(db);
                let _c = _db.collection(collection);
                _c.deleteOne(JSON.parse(json_query)).then((result:mongodb.DeleteWriteOpResultObject)=>{
                    callback(true);
                }).catch((err:mongodb.MongoError)=>{
                    callback(false);
                });
            }
        }

        if (this._dbproxy_is_init){
            cb();
        }
        else{
            this._cb.push(cb);
        }
    }
}
