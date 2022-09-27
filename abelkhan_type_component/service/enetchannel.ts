/* jshint esversion: 6 */
import enet = require('./js_enet');
import log = require("./log/log");
import abelkhan = require("../abelkhan_type/ts/abelkhan");
import channel_onrecv = require("./channel_onrecv");

export class enetchannel{
    private host : number;
    private rhost : number;
    private rport : number;
    private c : channel_onrecv.channel_onrecv;
    constructor(host:number, _rhost:number, _rport:number, _modules:abelkhan.modulemng){
        this.host = host;
        this.rhost = _rhost;
        this.rport = _rport;

        this.c = new channel_onrecv.channel_onrecv(this, _modules);
    }

    public on_recv(msg:Buffer){
        log.getLogger().trace("on_recv begin");
        this.c.on_recv(msg);
        log.getLogger().trace("on_recv end");
    }

    public push(event){
        var json_str = JSON.stringify(event);
        var json_buff = Buffer.from(json_str, 'utf-8');

        var send_header = Buffer.alloc(4);
        send_header.writeUInt8((json_buff.length) & 0xff, 0);
        send_header.writeUInt8((json_buff.length >> 8) & 0xff, 1);
        send_header.writeUInt8((json_buff.length >> 16) & 0xff, 2);
        send_header.writeUInt8((json_buff.length >> 24) & 0xff, 3);
        var send_data = Buffer.concat([send_header, json_buff]);

        enet.enet_peer_send(this.host, this.rhost, this.rport, send_data);

        log.getLogger().trace("push", json_str);
    };

}