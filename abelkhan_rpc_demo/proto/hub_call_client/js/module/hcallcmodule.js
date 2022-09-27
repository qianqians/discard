/*this imp file is codegen by abelkhan for js*/
function hcallc(_client){
    event_cb.call(this);

    this.client_handle = _client;
    _client.modules.add_module("hcallc", this);

    this.hcallc = function(argv0)
    {
        this.call_event("hcallc", [argv0]);
    }

}
