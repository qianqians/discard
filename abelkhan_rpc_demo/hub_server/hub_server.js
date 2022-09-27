var hub = require('../lib/hub.js');
var h2hcaller = require('../proto/hub_call_hub/js/caller/hcallhcaller.js')
var h2ccaller = require('../proto/hub_call_client/js/caller/hcallccaller.js')
var c2hmodule = require('../proto/client_call_hub/js/module/ccallhmodule.js')

var args = process.argv.splice(2);
var _hub = new hub.hub(args);
var _c2hmodule = new c2hmodule.ccallh(_hub)
var _h2ccaller = new h2ccaller.hcallc(_hub)
var _h2hcaller = new h2hcaller.hcallh(_hub);

_c2hmodule.add_event_listen("ccallh", function(){
    console.log("ccallh begin");

    _hub.modules.rsp.call("hello world!");

    _h2hcaller.get_hub('hub_server0').hcallh().callBack(function(){
        console.log("client login");
    },
    function(){
        console.log("error");
    })

    _h2ccaller.get_client(_hub.gates.current_client_uuid).hcallc("again hello world!");

    console.log("ccallh end");
});

process.on('uncaughtException', function (err) {
    console.log(err);
    console.log(err.stack);
});

_hub.poll();
