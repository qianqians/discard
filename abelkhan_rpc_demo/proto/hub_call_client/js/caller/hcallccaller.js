/*this ntf file is codegen by ablekhan for js*/

function hcallc(hub_ptr){
    this.get_client = function(uuid){
        return new hcallc_cliproxy(uuid, hub_ptr);
    }
    this.get_multicast = function(uuids){
        return new hcallc_cliproxy_multi(uuids, hub_ptr);
    }
    this.get_broadcast = function(){
        return new hcallc_broadcast(hub_ptr);
    }
}
module.exports.hcallc = hcallc;
module.exports.hcallc_cliproxy = hcallc_cliproxy;
module.exports.hcallc_cliproxy_multi = hcallc_cliproxy_multi;
module.exports.hcallc_broadcast = hcallc_broadcast;

function hcallc_cliproxy(uuid, hub_ptr){
    this.uuid = uuid;

    this.hcallc = function(argv0){
        hub_ptr.gates.call_client(uuid, "hcallc", "hcallc", argv0);
    }
}
function hcallc_cliproxy_multi(uuids, hub_ptr){
    this.uuids = uuids;
}
function hcallc_broadcast(hub_ptr){
}
