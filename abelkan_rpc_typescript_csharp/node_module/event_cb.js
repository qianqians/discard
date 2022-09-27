function event_cb(){
    this.events = {}

    this.add_event_listen = function(event, mothed){
        this.events[event] = mothed;
    }

    this.clear_event = function(){
        this.events = {}
    }

    this.call_event = function(event, argvs){
        if (this.events[event]){
            this.events[event].apply(null, argvs);
        }
    }
}
module.exports.event_cb = event_cb;
