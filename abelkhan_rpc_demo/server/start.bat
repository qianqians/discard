start center.exe config.txt center
ping 127.0.0.1 -n 3 -w 1000 > nul
start dbproxy.exe config.txt dbproxy
ping 127.0.0.1 -n 3 -w 1000 > nul
start websocket_gate.exe config.txt gate
ping 127.0.0.1 -n 3 -w 1000 > nul