import center = require('../../component/center/center');
import log = require('../../service/log/log');

process.on('uncaughtException', function (err) {
    log.getLogger().error(err);
    log.getLogger().error(err.stack);
});


(function(){
    let args = process.argv.splice(2);
    let _center = new center.center(args[0], args[1]);
    _center.poll();
})();