using System;

namespace lobby
{
    class createroomimpl
    {
        public delegate void create_mj_huanghuang_room_handle(string hub_name, Int64 room_id);

        static public void create_mj_huanghuang_room(string unionid, Int64 peopleNum, Int64 score, Int64 times, Int64 payRule, create_mj_huanghuang_room_handle cb)
        {
            var callback_id = System.Guid.NewGuid().ToString();

            hub.hub.hubs.call_hub("room1", "room", "could_create_mj_huanghuang_room", callback_id);
            hub.hub.hubs.call_hub("room2", "room", "could_create_mj_huanghuang_room", callback_id);
            hub.hub.hubs.call_hub("room3", "room", "could_create_mj_huanghuang_room", callback_id);
            hub.hub.hubs.call_hub("room4", "room", "could_create_mj_huanghuang_room", callback_id);

            lobby.create_mj_huanghuang_room_real_handle _handle = (string hub_name) => { create_mj_huanghuang_room_real(hub_name, unionid, peopleNum, score, times, payRule, cb); };
            lobby.could_create_room_callback.Add(callback_id, _handle);
        }

        static void create_mj_huanghuang_room_real(string hub_name, string unionid, Int64 peopleNum, Int64 score, Int64 times, Int64 payRule, create_mj_huanghuang_room_handle cb)
        {
            var callback_id = System.Guid.NewGuid().ToString();

            hub.hub.hubs.call_hub(hub_name, "room", "create_mj_huanghuang_room", unionid, peopleNum, score, times, payRule, callback_id);

            lobby.create_mj_huanghuang_room_callback_client_handle _handle = (Int64 room_id) => { cb(hub_name, room_id); };
            lobby.create_room_real_callback.Add(callback_id, _handle);
        }
    }
}
