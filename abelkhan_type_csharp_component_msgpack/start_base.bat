start ./bin/netcoreapp3.1/center_server.exe ./config/config_base.txt center
sleep 10
start ./bin/netcoreapp3.1/dbproxy_server.exe ./config/config_base.txt dbproxy
sleep 2