/*this req file is codegen by abelkhan for js*/

var seq = Math.round(Math.random()*1000);
function cb_ccallh_cb()
{
    this.event_ccallh_handle_cb = null;
    this.cb = function(argv0)
    {
        if (this.event_ccallh_handle_cb !== null)
        {
            this.event_ccallh_handle_cb(argv0);
        }
    }

    this.event_ccallh_handle_err = null;
    this.err = function()
    {
        if (this.event_ccallh_handle_err != null)
        {
            this.event_ccallh_handle_err();
        }
    }

    this.callBack = function(cb, err)
    {
        this.event_ccallh_handle_cb = cb;
        this.event_ccallh_handle_err = err;
    }
}

/*this cb code is codegen by abelkhan for js*/
function cb_ccallh_handle()
{
    this.map_ccallh = {};
    this.ccallh_rsp = function(uuid, argv0)
    {
        var rsp = this.map_ccallh[uuid];
        rsp.cb(argv0);
    }

    this.ccallh_err = function(uuid)
    {
        var rsp = this.map_ccallh[uuid];
        rsp.err();
    }

}

function ccallh(_client_handle)
{
    this.client_handle = _client_handle;
    this.cb_ccallh_handle = new cb_ccallh_handle();
    _client_handle.modules.add_module("ccallh", this.cb_ccallh_handle);

    this.get_hub = function(hub_name){
        return new ccallh_hubproxy(hub_name, _client_handle, this.cb_ccallh_handle);
    }
}

function ccallh_hubproxy (hub_name, _client_handle, _cb_ccallh_handle)
{
    this.hub_name = hub_name;
    this.cb_ccallh_handle = _cb_ccallh_handle;
    this.client_handle = _client_handle;

    this.ccallh = function()
    {
        seq++;
        var uuid = seq.toString();
        _client_handle.call_hub(hub_name, "ccallh", "ccallh", uuid);

        var cb_ccallh_obj = new cb_ccallh_cb();
        this.cb_ccallh_handle.map_ccallh[uuid] = cb_ccallh_obj;

        return cb_ccallh_obj;
    }

}
