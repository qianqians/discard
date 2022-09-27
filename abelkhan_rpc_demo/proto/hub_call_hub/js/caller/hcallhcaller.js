/*this req file is codegen by abelkhan for js*/

function cb_hcallh()
{
    this.event_hcallh_handle_cb = null;
    this.cb = function()
    {
        if (this.event_hcallh_handle_cb !== null)
        {
            this.event_hcallh_handle_cb();
        }
    }

    this.event_hcallh_handle_err = null;
    this.err = function()
    {
        if (this.event_hcallh_handle_err != null)
        {
            this.event_hcallh_handle_err();
        }
    }

    this.callBack = function(cb, err)
    {
        this.event_hcallh_handle_cb = cb;
        this.event_hcallh_handle_err = err;
    }
}

/*this cb code is codegen by abelkhan for js*/
function cb_hcallh_handle()
{
    this.map_hcallh = {};
    this.hcallh_rsp = function(uuid)
    {
        var rsp = this.map_hcallh[uuid];
        rsp.cb();
    }

    this.hcallh_err = function(uuid)
    {
        var rsp = this.map_hcallh[uuid];
        rsp.err();
    }

}

function hcallh(_hub_handle)
{
    this.hub_handle = _hub_handle;
    this.cb_hcallh_handle = new cb_hcallh_handle();
    _hub_handle.modules.add_module("hcallh", this.cb_hcallh_handle);

    this.get_hub = function(hub_name){
        return new hcallh_hubproxy(hub_name, _hub_handle, this.cb_hcallh_handle);
    }
}
module.exports.hcallh = hcallh;

function hcallh_hubproxy (hub_name, _hub_handle, _hcallh_handle)
{
    this.hub_name = hub_name;
    this.hub_handle = _hub_handle;

    this.hcallh = function()
    {
        const uuidv1 = require('uuid/v1');
        var uuid = uuidv1();

        _hub_handle.hubs.call_hub(hub_name, "hcallh", "hcallh", _hub_handle.name, uuid);

        var cb_hcallh_obj = new cb_hcallh();
        _hcallh_handle.map_hcallh[uuid] = cb_hcallh_obj;

        return cb_hcallh_obj;
    }

}
