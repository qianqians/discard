using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using MsgPack.Serialization;

namespace Abelkhan
{
/*this enum code is codegen by abelkhan codegen for c#*/

    public enum Direction{
        None = 0,
        Up = 1,
        Down = 2,
        Left = 3,
        Right = 4
    }
    public enum FenceType{
        Fence = 1,
        FenceGate = 2,
        FenceGatePWD = 3
    }
    public enum AvatarType{
        Self = 0,
        Friend = 1,
        Enemy = 2,
        Npc = 3,
        unknow = 4
    }
/*this struct code is codegen by abelkhan codegen for c#*/
    public class scene_hub_info
    {
        public string scene_name;
        public string scene_guid;
        public string scene_hub_id;
        public static MsgPack.MessagePackObjectDictionary scene_hub_info_to_protcol(scene_hub_info _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("scene_name", _struct.scene_name);
            _protocol.Add("scene_guid", _struct.scene_guid);
            _protocol.Add("scene_hub_id", _struct.scene_hub_id);
            return _protocol;
        }
        public static scene_hub_info protcol_to_scene_hub_info(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct5311f08d_a515_37bc_916b_6b3181601492 = new scene_hub_info();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "scene_name"){
                    _struct5311f08d_a515_37bc_916b_6b3181601492.scene_name = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "scene_guid"){
                    _struct5311f08d_a515_37bc_916b_6b3181601492.scene_guid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "scene_hub_id"){
                    _struct5311f08d_a515_37bc_916b_6b3181601492.scene_hub_id = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
            }
            return _struct5311f08d_a515_37bc_916b_6b3181601492;
        }
    }

    public class Pos
    {
        public float X;
        public float Y;
        public static MsgPack.MessagePackObjectDictionary Pos_to_protcol(Pos _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("X", _struct.X);
            _protocol.Add("Y", _struct.Y);
            return _protocol;
        }
        public static Pos protcol_to_Pos(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct29dd0b96_9335_398d_808e_0308d72c7d9a = new Pos();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "X"){
                    _struct29dd0b96_9335_398d_808e_0308d72c7d9a.X = ((MsgPack.MessagePackObject)i.Value).AsSingle();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Y"){
                    _struct29dd0b96_9335_398d_808e_0308d72c7d9a.Y = ((MsgPack.MessagePackObject)i.Value).AsSingle();
                }
            }
            return _struct29dd0b96_9335_398d_808e_0308d72c7d9a;
        }
    }

    public class Building
    {
        public string data_type = "UserBuilding";
        public string UserGuid;
        public string SceneID;
        public string BuildingGuid;
        public Int32 BuildingId;
        public string BuildingResource;
        public Pos topLeft;
        public Pos bottomRight;
        public static MsgPack.MessagePackObjectDictionary Building_to_protcol(Building _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("SceneID", _struct.SceneID);
            _protocol.Add("BuildingGuid", _struct.BuildingGuid);
            _protocol.Add("BuildingId", _struct.BuildingId);
            _protocol.Add("BuildingResource", _struct.BuildingResource);
            _protocol.Add("topLeft", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.topLeft)));
            _protocol.Add("bottomRight", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.bottomRight)));
            return _protocol;
        }
        public static Building protcol_to_Building(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structe37b319b_d83e_3aee_bc74_701941f721c3 = new Building();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "SceneID"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.SceneID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "BuildingGuid"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.BuildingGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "BuildingId"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.BuildingId = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "BuildingResource"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.BuildingResource = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "topLeft"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.topLeft = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "bottomRight"){
                    _structe37b319b_d83e_3aee_bc74_701941f721c3.bottomRight = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _structe37b319b_d83e_3aee_bc74_701941f721c3;
        }
    }

    public class Fence
    {
        public string data_type = "UserFence";
        public string UserGuid;
        public string SceneID;
        public string FenceGuid;
        public Int32 FenceId;
        public string FenceName;
        public FenceType FenceType;
        public string pwd;
        public Pos topLeft;
        public Pos bottomRight;
        public static MsgPack.MessagePackObjectDictionary Fence_to_protcol(Fence _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("SceneID", _struct.SceneID);
            _protocol.Add("FenceGuid", _struct.FenceGuid);
            _protocol.Add("FenceId", _struct.FenceId);
            _protocol.Add("FenceName", _struct.FenceName);
            _protocol.Add("FenceType", (Int32)_struct.FenceType);
            _protocol.Add("pwd", _struct.pwd);
            _protocol.Add("topLeft", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.topLeft)));
            _protocol.Add("bottomRight", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.bottomRight)));
            return _protocol;
        }
        public static Fence protcol_to_Fence(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8 = new Fence();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "SceneID"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.SceneID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "FenceGuid"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.FenceGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "FenceId"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.FenceId = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "FenceName"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.FenceName = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "FenceType"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.FenceType = (FenceType)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "pwd"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.pwd = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "topLeft"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.topLeft = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "bottomRight"){
                    _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8.bottomRight = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _struct6b7c8548_5d21_315c_a6ed_b41891d0e4d8;
        }
    }

    public class Cultivated
    {
        public string data_type = "UserCultivated";
        public string UserGuid;
        public string SceneID;
        public string CultivatedGuid;
        public string VegetableGuid;
        public Pos topLeft;
        public Pos bottomRight;
        public static MsgPack.MessagePackObjectDictionary Cultivated_to_protcol(Cultivated _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("SceneID", _struct.SceneID);
            _protocol.Add("CultivatedGuid", _struct.CultivatedGuid);
            _protocol.Add("VegetableGuid", _struct.VegetableGuid);
            _protocol.Add("topLeft", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.topLeft)));
            _protocol.Add("bottomRight", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.bottomRight)));
            return _protocol;
        }
        public static Cultivated protcol_to_Cultivated(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct74a29833_7279_3327_b8b3_18bbe2335691 = new Cultivated();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "SceneID"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.SceneID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "CultivatedGuid"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.CultivatedGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "VegetableGuid"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.VegetableGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "topLeft"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.topLeft = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "bottomRight"){
                    _struct74a29833_7279_3327_b8b3_18bbe2335691.bottomRight = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _struct74a29833_7279_3327_b8b3_18bbe2335691;
        }
    }

    public class Vegetable
    {
        public string data_type = "UserVegetable";
        public string UserGuid;
        public string SceneID;
        public string VegetableGuid;
        public Int32 perfabId;
        public string VegetableResource;
        public TimeInfo growiTime;
        public Int64 ripenTime;
        public Int32 count;
        public Int32 subtype;
        public Pos topLeft;
        public Pos bottomRight;
        public static MsgPack.MessagePackObjectDictionary Vegetable_to_protcol(Vegetable _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("SceneID", _struct.SceneID);
            _protocol.Add("VegetableGuid", _struct.VegetableGuid);
            _protocol.Add("perfabId", _struct.perfabId);
            _protocol.Add("VegetableResource", _struct.VegetableResource);
            _protocol.Add("growiTime", new MsgPack.MessagePackObject(TimeInfo.TimeInfo_to_protcol(_struct.growiTime)));
            _protocol.Add("ripenTime", _struct.ripenTime);
            _protocol.Add("count", _struct.count);
            _protocol.Add("subtype", _struct.subtype);
            _protocol.Add("topLeft", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.topLeft)));
            _protocol.Add("bottomRight", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.bottomRight)));
            return _protocol;
        }
        public static Vegetable protcol_to_Vegetable(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structd53b7306_7a0f_3d35_8705_949bcdd189d2 = new Vegetable();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "SceneID"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.SceneID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "VegetableGuid"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.VegetableGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "perfabId"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.perfabId = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "VegetableResource"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.VegetableResource = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "growiTime"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.growiTime = TimeInfo.protcol_to_TimeInfo(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "ripenTime"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.ripenTime = ((MsgPack.MessagePackObject)i.Value).AsInt64();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "count"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.count = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "subtype"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.subtype = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "topLeft"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.topLeft = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "bottomRight"){
                    _structd53b7306_7a0f_3d35_8705_949bcdd189d2.bottomRight = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _structd53b7306_7a0f_3d35_8705_949bcdd189d2;
        }
    }

    public class Role
    {
        public string data_type = "UserRole";
        public string SceneID;
        public string UserGuid;
        public string RoleGuid;
        public Int32 RoleId;
        public string RoleResource;
        public static MsgPack.MessagePackObjectDictionary Role_to_protcol(Role _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("SceneID", _struct.SceneID);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("RoleGuid", _struct.RoleGuid);
            _protocol.Add("RoleId", _struct.RoleId);
            _protocol.Add("RoleResource", _struct.RoleResource);
            return _protocol;
        }
        public static Role protcol_to_Role(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structe15dab07_4671_3806_9f26_9880fe20019d = new Role();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _structe15dab07_4671_3806_9f26_9880fe20019d.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "SceneID"){
                    _structe15dab07_4671_3806_9f26_9880fe20019d.SceneID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _structe15dab07_4671_3806_9f26_9880fe20019d.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "RoleGuid"){
                    _structe15dab07_4671_3806_9f26_9880fe20019d.RoleGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "RoleId"){
                    _structe15dab07_4671_3806_9f26_9880fe20019d.RoleId = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "RoleResource"){
                    _structe15dab07_4671_3806_9f26_9880fe20019d.RoleResource = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
            }
            return _structe15dab07_4671_3806_9f26_9880fe20019d;
        }
    }

    public class SceneChange
    {
        public List<Building> remove_buildings;
        public List<Building> add_buildings;
        public List<Cultivated> remove_cultivated;
        public List<Cultivated> add_cultivated;
        public Fence old_fences;
        public Fence new_fences;
        public static MsgPack.MessagePackObjectDictionary SceneChange_to_protcol(SceneChange _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            if (_struct.remove_buildings != null) {
                var _array_remove_buildings = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.remove_buildings){
                    _array_remove_buildings.Add( new MsgPack.MessagePackObject(Building.Building_to_protcol(v_)));
                }
                _protocol.Add("remove_buildings", new MsgPack.MessagePackObject(_array_remove_buildings));
            }
            if (_struct.add_buildings != null) {
                var _array_add_buildings = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.add_buildings){
                    _array_add_buildings.Add( new MsgPack.MessagePackObject(Building.Building_to_protcol(v_)));
                }
                _protocol.Add("add_buildings", new MsgPack.MessagePackObject(_array_add_buildings));
            }
            if (_struct.remove_cultivated != null) {
                var _array_remove_cultivated = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.remove_cultivated){
                    _array_remove_cultivated.Add( new MsgPack.MessagePackObject(Cultivated.Cultivated_to_protcol(v_)));
                }
                _protocol.Add("remove_cultivated", new MsgPack.MessagePackObject(_array_remove_cultivated));
            }
            if (_struct.add_cultivated != null) {
                var _array_add_cultivated = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.add_cultivated){
                    _array_add_cultivated.Add( new MsgPack.MessagePackObject(Cultivated.Cultivated_to_protcol(v_)));
                }
                _protocol.Add("add_cultivated", new MsgPack.MessagePackObject(_array_add_cultivated));
            }
            _protocol.Add("old_fences", new MsgPack.MessagePackObject(Fence.Fence_to_protcol(_struct.old_fences)));
            _protocol.Add("new_fences", new MsgPack.MessagePackObject(Fence.Fence_to_protcol(_struct.new_fences)));
            return _protocol;
        }
        public static SceneChange protcol_to_SceneChange(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct1f8be172_1c9e_38c1_8426_9d337a341fdf = new SceneChange();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "remove_buildings"){
                    _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.remove_buildings = new List<Building>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.remove_buildings.Add(Building.protcol_to_Building(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "add_buildings"){
                    _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.add_buildings = new List<Building>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.add_buildings.Add(Building.protcol_to_Building(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "remove_cultivated"){
                    _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.remove_cultivated = new List<Cultivated>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.remove_cultivated.Add(Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "add_cultivated"){
                    _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.add_cultivated = new List<Cultivated>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.add_cultivated.Add(Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "old_fences"){
                    _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.old_fences = Fence.protcol_to_Fence(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "new_fences"){
                    _struct1f8be172_1c9e_38c1_8426_9d337a341fdf.new_fences = Fence.protcol_to_Fence(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _struct1f8be172_1c9e_38c1_8426_9d337a341fdf;
        }
    }

    public class UserInformation
    {
        public string data_type = "UserInformation";
        public string SceneID;
        public string UserName;
        public string UserTableID;
        public string UserGuid;
        public string Avatar;
        public Int32 Gender;
        public Int32 Level;
        public Int32 coin;
        public Pos Pos;
        public Direction dir;
        public static MsgPack.MessagePackObjectDictionary UserInformation_to_protcol(UserInformation _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("SceneID", _struct.SceneID);
            _protocol.Add("UserName", _struct.UserName);
            _protocol.Add("UserTableID", _struct.UserTableID);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("Avatar", _struct.Avatar);
            _protocol.Add("Gender", _struct.Gender);
            _protocol.Add("Level", _struct.Level);
            _protocol.Add("coin", _struct.coin);
            _protocol.Add("Pos", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.Pos)));
            _protocol.Add("dir", (Int32)_struct.dir);
            return _protocol;
        }
        public static UserInformation protcol_to_UserInformation(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct07924b8f_25bc_32a4_b436_da6af6116572 = new UserInformation();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "SceneID"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.SceneID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserName"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.UserName = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserTableID"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.UserTableID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Avatar"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.Avatar = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Gender"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.Gender = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Level"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.Level = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "coin"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.coin = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Pos"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.Pos = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "dir"){
                    _struct07924b8f_25bc_32a4_b436_da6af6116572.dir = (Direction)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
            }
            return _struct07924b8f_25bc_32a4_b436_da6af6116572;
        }
    }

    public class UserSceneInfo
    {
        public AvatarType type;
        public UserInformation info;
        public static MsgPack.MessagePackObjectDictionary UserSceneInfo_to_protcol(UserSceneInfo _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("type", (Int32)_struct.type);
            _protocol.Add("info", new MsgPack.MessagePackObject(UserInformation.UserInformation_to_protcol(_struct.info)));
            return _protocol;
        }
        public static UserSceneInfo protcol_to_UserSceneInfo(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structf8e44cd9_94f7_3548_b5bf_54cb065dfe38 = new UserSceneInfo();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "type"){
                    _structf8e44cd9_94f7_3548_b5bf_54cb065dfe38.type = (AvatarType)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "info"){
                    _structf8e44cd9_94f7_3548_b5bf_54cb065dfe38.info = UserInformation.protcol_to_UserInformation(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _structf8e44cd9_94f7_3548_b5bf_54cb065dfe38;
        }
    }

    public class NPCInformation
    {
        public string data_type = "NPCInformation";
        public string NPCName;
        public string NPCTableID;
        public string NPCGuid;
        public string ownerGuid;
        public string Avatar;
        public Int32 Gender;
        public Int32 Level;
        public Pos Pos;
        public Direction dir;
        public static MsgPack.MessagePackObjectDictionary NPCInformation_to_protcol(NPCInformation _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("data_type", _struct.data_type);
            _protocol.Add("NPCName", _struct.NPCName);
            _protocol.Add("NPCTableID", _struct.NPCTableID);
            _protocol.Add("NPCGuid", _struct.NPCGuid);
            _protocol.Add("ownerGuid", _struct.ownerGuid);
            _protocol.Add("Avatar", _struct.Avatar);
            _protocol.Add("Gender", _struct.Gender);
            _protocol.Add("Level", _struct.Level);
            _protocol.Add("Pos", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.Pos)));
            _protocol.Add("dir", (Int32)_struct.dir);
            return _protocol;
        }
        public static NPCInformation protcol_to_NPCInformation(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce = new NPCInformation();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "data_type"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.data_type = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "NPCName"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.NPCName = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "NPCTableID"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.NPCTableID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "NPCGuid"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.NPCGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "ownerGuid"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.ownerGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Avatar"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.Avatar = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Gender"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.Gender = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Level"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.Level = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Pos"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.Pos = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "dir"){
                    _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce.dir = (Direction)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
            }
            return _structb1a569a1_8c76_374e_b2e1_1c336ec0b7ce;
        }
    }

    public class NPCSceneInfo
    {
        public AvatarType type;
        public NPCInformation info;
        public static MsgPack.MessagePackObjectDictionary NPCSceneInfo_to_protcol(NPCSceneInfo _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("type", (Int32)_struct.type);
            _protocol.Add("info", new MsgPack.MessagePackObject(NPCInformation.NPCInformation_to_protcol(_struct.info)));
            return _protocol;
        }
        public static NPCSceneInfo protcol_to_NPCSceneInfo(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structbf95d06b_54f4_3822_8d36_64bacb8381f6 = new NPCSceneInfo();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "type"){
                    _structbf95d06b_54f4_3822_8d36_64bacb8381f6.type = (AvatarType)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "info"){
                    _structbf95d06b_54f4_3822_8d36_64bacb8381f6.info = NPCInformation.protcol_to_NPCInformation(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
            }
            return _structbf95d06b_54f4_3822_8d36_64bacb8381f6;
        }
    }

    public class SceneInfo
    {
        public string subscene_name;
        public List<Building> buildings;
        public List<Fence> fences;
        public List<Cultivated> Cultivateds;
        public List<Vegetable> Vegetables;
        public List<UserSceneInfo> users;
        public List<NPCSceneInfo> npcs;
        public static MsgPack.MessagePackObjectDictionary SceneInfo_to_protcol(SceneInfo _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("subscene_name", _struct.subscene_name);
            if (_struct.buildings != null) {
                var _array_buildings = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.buildings){
                    _array_buildings.Add( new MsgPack.MessagePackObject(Building.Building_to_protcol(v_)));
                }
                _protocol.Add("buildings", new MsgPack.MessagePackObject(_array_buildings));
            }
            if (_struct.fences != null) {
                var _array_fences = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.fences){
                    _array_fences.Add( new MsgPack.MessagePackObject(Fence.Fence_to_protcol(v_)));
                }
                _protocol.Add("fences", new MsgPack.MessagePackObject(_array_fences));
            }
            if (_struct.Cultivateds != null) {
                var _array_Cultivateds = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.Cultivateds){
                    _array_Cultivateds.Add( new MsgPack.MessagePackObject(Cultivated.Cultivated_to_protcol(v_)));
                }
                _protocol.Add("Cultivateds", new MsgPack.MessagePackObject(_array_Cultivateds));
            }
            if (_struct.Vegetables != null) {
                var _array_Vegetables = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.Vegetables){
                    _array_Vegetables.Add( new MsgPack.MessagePackObject(Vegetable.Vegetable_to_protcol(v_)));
                }
                _protocol.Add("Vegetables", new MsgPack.MessagePackObject(_array_Vegetables));
            }
            if (_struct.users != null) {
                var _array_users = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.users){
                    _array_users.Add( new MsgPack.MessagePackObject(UserSceneInfo.UserSceneInfo_to_protcol(v_)));
                }
                _protocol.Add("users", new MsgPack.MessagePackObject(_array_users));
            }
            if (_struct.npcs != null) {
                var _array_npcs = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.npcs){
                    _array_npcs.Add( new MsgPack.MessagePackObject(NPCSceneInfo.NPCSceneInfo_to_protcol(v_)));
                }
                _protocol.Add("npcs", new MsgPack.MessagePackObject(_array_npcs));
            }
            return _protocol;
        }
        public static SceneInfo protcol_to_SceneInfo(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct4f50ee72_55d3_3593_a651_f16393a46309 = new SceneInfo();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "subscene_name"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.subscene_name = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "buildings"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.buildings = new List<Building>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct4f50ee72_55d3_3593_a651_f16393a46309.buildings.Add(Building.protcol_to_Building(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "fences"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.fences = new List<Fence>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct4f50ee72_55d3_3593_a651_f16393a46309.fences.Add(Fence.protcol_to_Fence(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Cultivateds"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.Cultivateds = new List<Cultivated>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct4f50ee72_55d3_3593_a651_f16393a46309.Cultivateds.Add(Cultivated.protcol_to_Cultivated(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Vegetables"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.Vegetables = new List<Vegetable>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct4f50ee72_55d3_3593_a651_f16393a46309.Vegetables.Add(Vegetable.protcol_to_Vegetable(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "users"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.users = new List<UserSceneInfo>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct4f50ee72_55d3_3593_a651_f16393a46309.users.Add(UserSceneInfo.protcol_to_UserSceneInfo(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "npcs"){
                    _struct4f50ee72_55d3_3593_a651_f16393a46309.npcs = new List<NPCSceneInfo>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct4f50ee72_55d3_3593_a651_f16393a46309.npcs.Add(NPCSceneInfo.protcol_to_NPCSceneInfo(((MsgPack.MessagePackObject)v_).AsDictionary()));
                    }
                }
            }
            return _struct4f50ee72_55d3_3593_a651_f16393a46309;
        }
    }

    public class UserMoveInfo
    {
        public AvatarType type;
        public string UserGuid;
        public string NPCTableID;
        public Pos Pos;
        public Direction dir;
        public static MsgPack.MessagePackObjectDictionary UserMoveInfo_to_protcol(UserMoveInfo _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("type", (Int32)_struct.type);
            _protocol.Add("UserGuid", _struct.UserGuid);
            _protocol.Add("NPCTableID", _struct.NPCTableID);
            _protocol.Add("Pos", new MsgPack.MessagePackObject(Pos.Pos_to_protcol(_struct.Pos)));
            _protocol.Add("dir", (Int32)_struct.dir);
            return _protocol;
        }
        public static UserMoveInfo protcol_to_UserMoveInfo(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _structde5d91e1_5db9_3a5e_985d_644be21eb795 = new UserMoveInfo();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "type"){
                    _structde5d91e1_5db9_3a5e_985d_644be21eb795.type = (AvatarType)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "UserGuid"){
                    _structde5d91e1_5db9_3a5e_985d_644be21eb795.UserGuid = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "NPCTableID"){
                    _structde5d91e1_5db9_3a5e_985d_644be21eb795.NPCTableID = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "Pos"){
                    _structde5d91e1_5db9_3a5e_985d_644be21eb795.Pos = Pos.protcol_to_Pos(((MsgPack.MessagePackObject)i.Value).AsDictionary());
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "dir"){
                    _structde5d91e1_5db9_3a5e_985d_644be21eb795.dir = (Direction)((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
            }
            return _structde5d91e1_5db9_3a5e_985d_644be21eb795;
        }
    }

    public class TimeInfo
    {
        public Int32 year;
        public Int32 months;
        public Int32 day;
        public Int32 hour;
        public Int32 minute;
        public Int32 second;
        public static MsgPack.MessagePackObjectDictionary TimeInfo_to_protcol(TimeInfo _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("year", _struct.year);
            _protocol.Add("months", _struct.months);
            _protocol.Add("day", _struct.day);
            _protocol.Add("hour", _struct.hour);
            _protocol.Add("minute", _struct.minute);
            _protocol.Add("second", _struct.second);
            return _protocol;
        }
        public static TimeInfo protcol_to_TimeInfo(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct0ff6c200_4388_360e_8a89_66405e28c6f5 = new TimeInfo();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "year"){
                    _struct0ff6c200_4388_360e_8a89_66405e28c6f5.year = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "months"){
                    _struct0ff6c200_4388_360e_8a89_66405e28c6f5.months = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "day"){
                    _struct0ff6c200_4388_360e_8a89_66405e28c6f5.day = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "hour"){
                    _struct0ff6c200_4388_360e_8a89_66405e28c6f5.hour = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "minute"){
                    _struct0ff6c200_4388_360e_8a89_66405e28c6f5.minute = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "second"){
                    _struct0ff6c200_4388_360e_8a89_66405e28c6f5.second = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
            }
            return _struct0ff6c200_4388_360e_8a89_66405e28c6f5;
        }
    }

    public class QuestInfo
    {
        public Int32 id;
        public string chat;
        public List<Int32> reward;
        public List<Int32> next;
        public static MsgPack.MessagePackObjectDictionary QuestInfo_to_protcol(QuestInfo _struct){
        if (_struct == null) {
            return null;
        }

            var _protocol = new MsgPack.MessagePackObjectDictionary();
            _protocol.Add("id", _struct.id);
            _protocol.Add("chat", _struct.chat);
            if (_struct.reward != null) {
                var _array_reward = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.reward){
                    _array_reward.Add(v_);
                }
                _protocol.Add("reward", new MsgPack.MessagePackObject(_array_reward));
            }
            if (_struct.next != null) {
                var _array_next = new List<MsgPack.MessagePackObject>();
                foreach(var v_ in _struct.next){
                    _array_next.Add(v_);
                }
                _protocol.Add("next", new MsgPack.MessagePackObject(_array_next));
            }
            return _protocol;
        }
        public static QuestInfo protcol_to_QuestInfo(MsgPack.MessagePackObjectDictionary _protocol){
        if (_protocol == null) {
            return null;
        }

            var _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92 = new QuestInfo();
            foreach (var i in _protocol){
                if (((MsgPack.MessagePackObject)i.Key).AsString() == "id"){
                    _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92.id = ((MsgPack.MessagePackObject)i.Value).AsInt32();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "chat"){
                    _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92.chat = ((MsgPack.MessagePackObject)i.Value).AsString();
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "reward"){
                    _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92.reward = new List<Int32>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92.reward.Add(((MsgPack.MessagePackObject)v_).AsInt32());
                    }
                }
                else if (((MsgPack.MessagePackObject)i.Key).AsString() == "next"){
                    _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92.next = new List<Int32>();
                    var _protocol_array = ((MsgPack.MessagePackObject)i.Value).AsList();
                    foreach (var v_ in _protocol_array){
                        _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92.next.Add(((MsgPack.MessagePackObject)v_).AsInt32());
                    }
                }
            }
            return _struct57c11bc1_8eb6_3f85_af34_91db9ac2ec92;
        }
    }

/*this caller code is codegen by abelkhan codegen for c#*/

}
