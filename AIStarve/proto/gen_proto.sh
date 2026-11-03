cd ./rpc/
python3 genc2h.py ../proto/client_call_hub/ csharp "" ../../server/server/game_proto ../proto/common
python3 genh2c.py ../proto/hub_call_client/ csharp "" ../../server/server/game_proto ../proto/common

python3 genc2h.py ../proto/client_call_hub/ csharp ../../AIStarve/Assets/Scripts/serverSDK/game_proto "" ../proto/common
python3 genh2c.py ../proto/hub_call_client/ csharp ../../AIStarve/Assets/Scripts/serverSDK/game_proto "" ../proto/common

cd ../