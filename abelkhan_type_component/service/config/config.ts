import fs = require('fs');
export function config(cfgfilepath:string){
    var data = fs.readFileSync(cfgfilepath, 'utf8');
    var obj = JSON.parse(data);
    return obj;
}