
export class signals<T>{
    private _signals : ((t:T)=>void)[];
    constructor(){
        this._signals = [];
    }
    
    public connect(fn:(t:T)=>void){
        this._signals.push(fn);
    }

    public emit(t:T){
        for(let fn of this._signals){
            fn(t);
        }
    }
}