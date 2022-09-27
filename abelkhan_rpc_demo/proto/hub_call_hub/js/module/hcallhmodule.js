/*this rsp file is codegen by abelkhan for js*/

function rsp_hcallh(_hub, _remote_hub_name, _uuid)
{
    this.hub_handle = _hub;
    this.remote_hub_name = _remote_hub_name;
    this.uuid = _uuid;
    this.call = function()
    {
        _hub.hubs.call_hub(_remote_hub_name, "hcallh", "hcallh_rsp", _uuid);
    }
    this.err = function()
    {
        _hub.hubs.call_hub(_remote_hub_name, "hcallh", "hcallh_err", _uuid);
    }
}

function hcallh(_hub)
{
    var event_cb = require("event_cb");
    event_cb.event_cb.call(this);

    this.module_name = "hcallh";
    this.hub_handle = _hub;
    _hub.modules.add_module("hcallh", this);

    this.hcallh = function(remote_hub_name, uuid)
    {
        _hub.modules.rsp = new rsp_hcallh(_hub, remote_hub_name, uuid);
        this.call_event("hcallh", []);
        _hub.modules.rsp = null;
    }

}
module.exports.hcallh = hcallh;

