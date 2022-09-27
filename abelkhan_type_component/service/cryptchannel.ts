import log = require("./log/log");
import net = require("net");
import abelkhan = require("../abelkhan_type/ts/abelkhan");
import channel_onrecv = require("./channel_onrecv");
import signals = require("./signals/signals");

export class cryptchannel{
    private sock : net.Socket;
    private c : channel_onrecv.channel_onrecv;
    public signals_disconnect : signals.signals<cryptchannel>;
    private xor_key0 : number;
    private xor_key1 : number;
    private xor_key2 : number;
    private xor_key3 : number;
    constructor(xor_key:number, s:net.Socket, _modules:abelkhan.modulemng){
        this.xor_key0 = (xor_key >> 24) & 0xff;
        this.xor_key1 = (xor_key >> 16) & 0xff;
        this.xor_key2 = (xor_key >> 8) & 0xff;
        this.xor_key3 = xor_key & 0xff;

        log.getLogger().trace("xor_key0:", this.xor_key0);
        log.getLogger().trace("xor_key1:", this.xor_key1);
        log.getLogger().trace("xor_key2:", this.xor_key2);
        log.getLogger().trace("xor_key3:", this.xor_key3);

        this.sock = s;
        this.c = new channel_onrecv.channel_onrecv(this, _modules);
        this.c.signals_data.connect((data:Buffer)=>{
            for(let i = 0; i < data.length; ++i){
                if ((i%4) == 0){
                    data[i] ^= this.xor_key0;
                } 
                else if ((i%4) == 1){
                    data[i] ^= this.xor_key1;
                }
                else if ((i%4) == 2){
                    data[i] ^= this.xor_key2;
                }
                else if ((i%4) == 3){
                    data[i] ^= this.xor_key3;
                }
            }
        });

        this.signals_disconnect = new signals.signals<cryptchannel>();

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

    public disconnect(){
        this.sock.destroy();
    }

    public set_xor_key(xor_key:number){
        this.xor_key0 = (xor_key >> 24) & 0xff;
        this.xor_key1 = (xor_key >> 16) & 0xff;
        this.xor_key2 = (xor_key >> 8) & 0xff;
        this.xor_key3 = xor_key & 0xff;
    }
    
    public push(event){
        var json_str = JSON.stringify(event);
        var json_buff = Buffer.from(json_str, 'utf-8');
        for(let i = 0; i < json_buff.length; ++i){
            if ((i%4) == 0){
                json_buff[i] ^= this.xor_key0;
            } 
            else if ((i%4) == 1){
                json_buff[i] ^= this.xor_key1;
            }
            else if ((i%4) == 2){
                json_buff[i] ^= this.xor_key2;
            }
            else if ((i%4) == 3){
                json_buff[i] ^= this.xor_key3;
            }
            log.getLogger().trace(json_buff[i]);
        }

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
