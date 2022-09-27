/*this rsp file is codegen by abelkhan for js*/

function rsp_ccallh(_hub, _uuid)
{
    this.hub_handle = _hub;
    this.uuid = _uuid;
    this.call = function(argv0)
    {
        _hub.gates.call_client(_hub.gates.current_client_uuid, "ccallh", "ccallh_rsp", _uuid, argv0);
    }
    this.err = function()
    {
        _hub.gates.call_client(_hub.gates.current_client_uuid, "ccallh", "ccallh_err", _uuid);
    }
}
module.exports.rsp_ccallh = rsp_ccallh;

function ccallh(_hub)
{
    var event_cb = require("event_cb");
    event_cb.event_cb.call(this);

    this.module_name = "ccallh";
    this.hub_handle = _hub;
    _hub.modules.add_module("ccallh", this);

    this.ccallh = function(uuid)
    {
        _hub.modules.rsp = new rsp_ccallh(_hub, uuid);
        this.call_event("ccallh", []);
        _hub.modules.rsp = null;
    }

}
module.exports.ccallh = ccallh;
