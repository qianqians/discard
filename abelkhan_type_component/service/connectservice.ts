import net = require('net');
import log = require('./log/log');
import signals = require('./signals/signals');
import abelkhan = require('../abelkhan_type/ts/abelkhan');
import channel = require('./channel');

export class connectservice{
    private _modules : abelkhan.modulemng;
    public signals_disconnect : signals.signals<channel.channel>;

    constructor(modules:abelkhan.modulemng){
        this._modules = modules;
        this.signals_disconnect = new signals.signals<channel.channel>();
    }
    
    public connect(ip:string, port:number, cb:(ch:channel.channel)=>void){
        log.getLogger().trace("begin connect host:%s, port:%d", ip, port);

        var sock = new net.Socket();
        sock.connect(port, ip, ()=>{
            log.getLogger().trace("connectting host:%s, port:%d", ip, port);

            var ch = new channel.channel(sock, this._modules);
            ch.signals_disconnect.connect((ch:channel.channel)=>{
                this.signals_disconnect.emit(ch);
            });
    
            cb(ch);

            log.getLogger().trace("end connect host:%s, port:%d", ip, port);
        });
    }

}
