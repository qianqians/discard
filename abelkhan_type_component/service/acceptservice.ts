import net = require('net');
import signals = require('./signals/signals');
import abelkhan = require('../abelkhan_type/ts/abelkhan');
import channel = require('./channel');

export class acceptservice{
    private server : net.Server;
    
    public signals_connect : signals.signals<channel.channel>;
    public signals_disconnect : signals.signals<channel.channel>;
    
    constructor(ip:string, port:number, modules:abelkhan.modulemng){
        this.signals_connect = new signals.signals<channel.channel>();
        this.signals_disconnect = new signals.signals<channel.channel>();

        this.server = net.createServer((s:net.Socket)=>{
            let ch = new channel.channel(s, modules);
            ch.signals_disconnect.connect((ch:channel.channel)=>{
                this.signals_disconnect.emit(ch);
            });

            this.signals_connect.emit(ch);

        }).listen(port, ip);
    }
}
