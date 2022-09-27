import log4js = require('log4js');
export function configLogger(logfilepath:string, _level:string){
    log4js.configure({
        appenders: {
            normal: {
                type: 'file',
                filename: logfilepath,
                maxLogSize: 1024*1024*32,
                backups: 3,
                layout: {
                    type: 'pattern',
                    pattern: '%d %p %m%n',
                }
            }
        },
        categories: {default: { appenders: ['normal'], level: _level }}
    });
}

export function getLogger(){
    return log4js.getLogger('normal');
}