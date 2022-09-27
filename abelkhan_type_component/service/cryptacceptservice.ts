import net = require('net');
import signals = require('./signals/signals');
import abelkhan = require('../abelkhan_type/ts/abelkhan');
import cryptchannel = require('./cryptchannel');

export class cryptacceptservice{
    private server : net.Server;
    
    public signals_connect : signals.signals<cryptchannel.cryptchannel>;
    public signals_disconnect : signals.signals<cryptchannel.cryptchannel>;
    
    constructor(port:number, xor_key:number, modules:abelkhan.modulemng){
        this.signals_connect = new signals.signals<cryptchannel.cryptchannel>();
        this.signals_disconnect = new signals.signals<cryptchannel.cryptchannel>();

        this.server = net.createServer((s:net.Socket)=>{
            let ch = new cryptchannel.cryptchannel(xor_key, s, modules);
            ch.signals_disconnect.connect((ch:cryptchannel.cryptchannel)=>{
                this.signals_disconnect.emit(ch);
            });

            this.signals_connect.emit(ch);

        }).listen(port, "0.0.0.0");
    }
}
