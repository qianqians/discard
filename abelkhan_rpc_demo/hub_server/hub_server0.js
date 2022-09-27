var hub = require('../lib/hub.js');
var h2hmodule = require('../proto/hub_call_hub/js/module/hcallhmodule.js')

var args = process.argv.splice(2);
var _hub = new hub.hub(args);
var _h2hmodule =new h2hmodule.hcallh(_hub);

_h2hmodule.add_event_listen("hcallh", function(){
    console.log("client login");
    _hub.modules.rsp.call();
});

process.on('uncaughtException', function (err) {
    console.log(err);
    console.log(err.stack);
});

_hub.poll();
