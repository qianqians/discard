function dbproxy_msg_handle(_hub){
    this.hub = _hub;

    this.reg_hub_sucess = function(){
        getLogger().trace("connect db sucess");
        this.hub.onConnectDB_event();
    }
    
    this.ack_create_persisted_object = function( callbackid, is_create_sucess ){
		var cb = this.hub.dbproxy.begin_callback(callbackid);
		cb(is_create_sucess);
		this.hub.dbproxy.end_callback(callbackid);
	}

	this.ack_updata_persisted_objec = function( callbackid ){
        var cb = this.hub.dbproxy.begin_callback(callbackid);
		cb();
        this.hub.dbproxy.end_callback(callbackid);
	}

    this.ack_get_object_count = function( callbackid,  count ){
        var cb = this.hub.dbproxy.begin_callback(callbackid);
        cb(count);
        this.hub.dbproxy.end_callback(callbackid);
    }

	this.ack_get_object_info = function( callbackid, json_obejct_array ){
		var cb = this.hub.dbproxy.begin_callback(callbackid);
		cb(json_obejct_array);
	}

	this.ack_get_object_info_end = function( callbackid ){
        var cb = this.hub.dbproxy.end_get_object_info_callback(callbackid);
        cb();
    }

    this.ack_remove_object = function( callbackid ){
        var cb = this.hub.dbproxy.begin_callback(callbackid);
        cb();
        this.hub.dbproxy.end_callback(callbackid);
    }
}
