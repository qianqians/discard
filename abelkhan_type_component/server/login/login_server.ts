import hub = require('../../component/hub/hub');
import log = require('../../service/log/log');

process.on('uncaughtException', function (err) {
    log.getLogger().error(err);
    log.getLogger().error(err.stack);
});

(function(){
    let args = process.argv.splice(2);
    let _hub = new hub.hub(args[0], args[1]);

    _hub.poll();
})();