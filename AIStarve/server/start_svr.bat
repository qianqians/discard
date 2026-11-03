start ./redis/redis-server.exe
start ./bin/Debug/center_svr.exe ./config/center.txt center &
start ./bin/Debug/dbproxy_svr.exe ./config/dbproxy.txt dbproxy &
start ./bin/Debug/gate_svr.exe ./config/gate.txt gate &
start ./bin/Debug/server.exe ./config/server.txt server &
pause