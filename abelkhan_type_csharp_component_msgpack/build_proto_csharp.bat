cd ./abelkhan_type
python gencsharpclient.py ../trinityprotocol/tcpprotcol/client_protcol ../trinityprotocol/tcpprotcol/csharp
python gencsharp.py ../trinityprotocol/tcpprotcol/client_protcol ../server_protocol/csharp_server
python gencsharp.py ../server_protocol/server_protcol ../server_protocol/csharp_server

pause