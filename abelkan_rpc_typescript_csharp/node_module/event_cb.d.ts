export class event_cb{
    add_event_listen(event:string, mothed:Function) : void;
    clear_event() : void;
    call_event(event:string, argvs:any[]) : void;
}