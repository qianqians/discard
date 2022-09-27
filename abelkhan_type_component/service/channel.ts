import log = require("./log/log");
import net = require("net");
import abelkhan = require("../abelkhan_type/ts/abelkhan");
import channel_onrecv = require("./channel_onrecv");
import signals = require("./signals/signals");

export class channel{
    private sock : net.Socket;
    private c : channel_onrecv.channel_onrecv;
    public signals_disconnect : signals.signals<channel>;
    constructor(s:net.Socket, _modules:abelkhan.modulemng){
        this.sock = s;
        this.c = new channel_onrecv.channel_onrecv(this, _modules);

        this.signals_disconnect = new signals.signals<channel>();

        this.sock.on("data", (data:Buffer)=>{
            this.c.on_recv(data);
        });

        this.sock.on('close', ()=>{
            this.signals_disconnect.emit(this);
        });

        this.sock.on('error', (error)=>{
            this.signals_disconnect.emit(this);
        });
    }
    
    public push(event){
        var json_str = JSON.stringify(event);
        var json_buff = Buffer.from(json_str, 'utf-8');

        var send_header = Buffer.alloc(4);
        send_header.writeUInt8(json_buff.length & 0xff, 0);
        send_header.writeUInt8((json_buff.length >> 8) & 0xff, 1);
        send_header.writeUInt8((json_buff.length >> 16) & 0xff, 2);
        send_header.writeUInt8((json_buff.length >> 24) & 0xff, 3);
        var send_data = Buffer.concat([send_header, json_buff]);

        this.sock.write(send_data);

        log.getLogger().trace(json_str);
    }    
    
}
