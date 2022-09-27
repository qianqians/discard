function gateproxy(ch, hub){
    this.caller = new hub_call_gate_caller(ch);
    this.hub = hub;

    this.reg_hub = function(){
        this.caller.reg_hub(this.hub.uuid, this.hub.name);
	}

    this.connect_sucess = function(client_uuid){
        this.caller.connect_sucess(client_uuid);
    }

    this.disconnect_client = function(uuid){
        this.caller.disconnect_client(uuid);
    }

    this.forward_hub_call_client = function(uuid, module, func, argv){
        this.caller.forward_hub_call_client(uuid, module, func, argv);
    }

    this.forward_hub_call_group_client = function(uuids, module, func, argv){
		this.caller.forward_hub_call_group_client(uuids, module, func, argv);
	}

	this.forward_hub_call_global_client = function(module, func, argv){
		this.caller.forward_hub_call_global_client(module, func, argv);
	}
}

function directproxy(direct_ch){
    this.caller = new hub_call_client_caller(direct_ch);

    this.call_client = function(module, func, argv){
        this.caller.call_client(module, func, argv);
    }
}

function gatemng(conn, hub){
    eventobj.call(this);
    this.conn = conn;
    this.hub = hub;

    this.current_client_uuid = ""
    this.clients = {};

	this.gates = {};

    this.direct_ch = {};

    this.connect_gate = function(uuid, ip, port){
        getLogger().trace("connect_gate ip:%s, port:%d", ip, port);
		this.conn.connect(ip, port, (ch)=>{
        //this.conn.connect(ip, port, this, function(ch){
            this.gates[uuid] = new gateproxy(ch, hub);
            ch.gateproxy = this.gates[uuid];
            this.gates[uuid].reg_hub();
        });
    }

    this.client_direct_connect = function(client_uuid, direct_ch){
        if (this.direct_ch[client_uuid]){
            return;
        }

        getLogger().trace("reg direct client:%s", client_uuid);

        direct_ch.directproxy = new directproxy(direct_ch);
        direct_ch.client_uuid = client_uuid;
        this.direct_ch[client_uuid] = direct_ch.directproxy;

        hub.call_event("direct_client_connect", [client_uuid]);
    }

    this.client_connect = function(client_uuid, gate_ch){
        if (!gate_ch.gateproxy){
            return;
        }

        if (this.clients[client_uuid]){
            return;
        }

        getLogger().trace("reg client:%s", client_uuid);

        this.clients[client_uuid] = gate_ch.gateproxy;
        gate_ch.gateproxy.connect_sucess(client_uuid);

        hub.call_event("client_connect", [client_uuid]);
    }

    this.client_disconnect = function(client_uuid){
        if (this.clients[client_uuid]){
            hub.call_event("client_disconnect", [client_uuid]);

            delete this.clients[client_uuid];
        }
    }

    this.client_exception = function(client_uuid){
        hub.call_event("client_exception", [client_uuid]);
    }

    this.disconnect_client = function(uuid){
        if (this.clients[uuid]){
            this.clients[uuid].disconnect_client(uuid);
            delete this.clients[uuid];
        }
    }

    this.call_client = function(uuid, _module, func){
		if (this.direct_ch[uuid] ){
            this.direct_ch[uuid].call_client(_module, func, [].slice.call(arguments, 3));
            return;
        }

		if (this.clients[uuid]){
            this.clients[uuid].forward_hub_call_client(uuid, _module, func, [].slice.call(arguments, 3));
        }
    }

    this.call_group_client = function(uuids, _module, func){
        var argvs = [].slice.call(arguments, 3);

        let tmp_uuids = [];
        for(let uuid of uuids){
            if (this.direct_ch[uuid] ){
                this.direct_ch[uuid].call_client(_module, func, argvs);
                continue;
            }

            tmp_uuids.push(uuid);
        }

        let tmp_gates = [];
        for(let uuid of tmp_uuids){
            if (this.clients[uuid] && tmp_gates.indexOf(this.clients[uuid]) === -1 ){
                tmp_gates.push(this.clients[uuid]);
            }
        }

        for(let gate_proxy of tmp_gates){
			gate_proxy.forward_hub_call_group_client(uuids, _module, func, argvs);
		}
	}

	this.call_global_client = function(_module, func){
        var argvs = [].slice.call(arguments, 2)
		for(let uuid in this.gates){
			this.gates[uuid].forward_hub_call_global_client(_module, func, argvs);
		}
	}

}
