import abelkhan = require("../abelkhan_type/ts/abelkhan");
import net = require("net");
import log = require("./log/log");
import signals = require("./signals/signals");

export class channel_onrecv{
    public signals_data : signals.signals<Buffer>;
    private _modules : abelkhan.modulemng;
    private _ch : abelkhan.Ichannel;
    private data : any;
    constructor(ch:abelkhan.Ichannel, _modules:abelkhan.modulemng){
        this.signals_data = new signals.signals<Buffer>();

        this._ch = ch;
        this._modules = _modules;
        this.data = null;
    }

    public on_recv(data:Buffer){
        try
        {
            log.getLogger().trace("begin on data");
            
            var new_data = data;
            if (this.data !== null){
                new_data = Buffer.concat([this.data, new_data]);
            }

            while(new_data.length > 4){
                var len = new_data[0] | new_data[1] << 8 | new_data[2] << 16 | new_data[3] << 24;

                if ( (len + 4) > new_data.length ){
                    break;
                }

                var json_data = new_data.slice(4, (len + 4));
                this.signals_data.emit(json_data);
                var json_str = json_data.toString('utf-8');
                log.getLogger().trace(json_str);
                this._modules.process_event(this._ch, JSON.parse(json_str));
                
                if ( new_data.length > (len + 4) ){
                    new_data = new_data.slice(len + 4);
                }
                else{
                    new_data = null;
                    break;
                }
            }

            this.data = new_data;

            log.getLogger().trace("end on data");
        }
        catch(err)
        {
            log.getLogger().error(err);
        }
    }
}
