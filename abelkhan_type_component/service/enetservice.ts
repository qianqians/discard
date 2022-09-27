/* jshint esversion: 6 */
import enet = require('./js_enet');
import log = require("./log/log");
import signals = require('./signals/signals');
import abelkhan = require('../abelkhan_type/ts/abelkhan');
import enetchannel = require('./enetchannel');

export class enetservice{
    private host : number;
    private modules : abelkhan.modulemng;
    public signals_connect : signals.signals<enetchannel.enetchannel>;
    private conn_cbs : {};
    private conns = {};
    constructor(ip:string, port:number, modules:abelkhan.modulemng){
        this.host = enet.enet_host_create(ip, port, 2048);
        log.getLogger().trace("enetservice host handle:%d", this.host);

        this.modules = modules;
        this.signals_connect = new signals.signals<enetchannel.enetchannel>();
    }

    public poll(){
        let event = enet.enet_host_service(this.host);
        if (event){
            switch(event.type)
            {
            case 1:
                {
                    let raddr = event.ip + ":" + event.port;
                    log.getLogger().trace("enetservice poll raddr:%s", raddr);
                    let ch = this.conns[raddr];
                    if (!ch){
                        ch = new enetchannel.enetchannel(this.host, event.host, event.port, this.modules);
                        this.conns[raddr] = ch;
                    }

                    let cb = this.conn_cbs[raddr];
                    if (cb){
                        delete this.conn_cbs[raddr];
                        cb(ch);
                    }
                    else{
                        this.signals_connect.emit(ch);
                    }
                }
                break;
            case 3:
                {
                    this.onRecv(event.data, event.ip, event.port);
                }
                break;
            }
        }
    }

    public onRecv(msg:Buffer, rhost:string, rport:number){
        log.getLogger().trace("message begin");

        if(msg.length >= 4){
            let raddr = rhost + ":" + rport;
            let ch = this.conns[raddr];
            if (!ch){
                log.getLogger().trace("message invalid ch end");
                return;
            }

            ch.on_recv(msg);
        }

        log.getLogger().trace("message end");
    }
    
    public connect = (rhost:string, rport:number, cb:(ch:enetchannel.enetchannel)=>void) =>{
        let raddr = rhost + ":" + rport;
        this.conn_cbs[raddr] = cb;

        log.getLogger().trace("enetservice connect raddr:%s", raddr);

        enet.enet_host_connect(this.host, rhost, rport);
    };
}
module.exports.enetservice = enetservice;