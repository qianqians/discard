start ts-node ./server/center/center.ts ./config/config_base.txt center
sleep 10
start ts-node ./server/dbproxy/dbproxy.ts ./config/config_base.txt dbproxy
sleep 2