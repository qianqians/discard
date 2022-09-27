/* jshint esversion: 6 */

export function enet_initialize() : void;
export function enet_deinitialize() : void;
export function enet_host_create(ip:string, port:number, peercount:number) : number;
export function enet_client_create(peercount:number) : number;
export function enet_host_service(host:number) : any;
export function enet_peer_send(host:number, remote_host:number, remote_port:number, data:Buffer) : void;
export function enet_host_connect(host:number, remote_ip:string, remote_port:number) : void;