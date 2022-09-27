import dbproxy = require('../../component/dbproxy/dbproxy');
import log = require('../../service/log/log');

process.on('uncaughtException', function (err) {
    log.getLogger().error(err);
    log.getLogger().error(err.stack);
});


(function(){
    let args = process.argv.splice(2);
    let _proxy = new dbproxy.dbproxy(args[0], args[1]);
    _proxy.poll();
})();