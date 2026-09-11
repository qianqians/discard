using Abelkhan;
using avatar;
using battle_shop;
using config;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using OfflineMsg;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Player
{
    public class LoginException : System.Exception
    {
        public string ErrorInfo;
        public LoginException(string err) : base(err)
        {
            ErrorInfo = err;
        }
    }

    public class GetPlayerException : System.Exception
    {
        public string ErrorInfo;
        public GetPlayerException(string err) : base(err)
        {
            ErrorInfo = err;
        }
    }

    public class GetGuidFromDBError : System.Exception
    {
        public string error;

        public GetGuidFromDBError(string err) : base(err)
        {
            error = err;
        }
    }

    public class PlayerInfo : IHostingData
    {
        private UserData info;
        private long lastTickStrengthTime;

        public battle_shop_player battleShopPlayer;
        public PVELevelConfig pveLevelCfg;
        public bool isInitRound = false;
        public bool isPVEQuestEvent;

        public class TmpPVEData
        {
            public battle_shop_player TmpBattleShopPlayer;
            public PVELevelConfig TmpPVELevelCfg;
            public int TmpPVELevelIndex;
            public bool isInitRound = false;
            public bool isQuestEvent;
        }
        public TmpPVEData tmpPVEData;
        public bool isTmp = false;

        public bool oldPVEQuestEvent; 

        public static string Type()
        {
            return "player_info";
        }

        private static UserWeekAchievement NewUserWeekAchievement()
        {
            return new UserWeekAchievement()
            {
                wAchievData = new List<AchievementData>(),
                battleInfo = new List<BattleInfo>(),      // 周成就 战绩记录
                timeout = Timerservice.WeekEndTimetmp(),  // 周成就超时时间
            };
        }

        public static IHostingData Create()
        {
            return new PlayerInfo()
            {
                info = new UserData()
                {
                    User = new UserInformation()
                    {
                        UserName = string.Empty,
                        UserGuid = 0,
                    },
                    Achiev = new UserAchievement()
                    {
                        achievData = new List<AchievementData>(),
                        battleInfo = new List<BattleInfo>(),// 战绩记录
                    },
                    wAchiev = NewUserWeekAchievement(),
                    Strength = 100,
                    gold = 100,
                    diamond = 10,
                    score = 0,
                    quest = 10001,
                    bag = new Abelkhan.Bag() { ItemList = new() },
                    RoleList = new List<Role>(),
                    GuideSteps = new (),
                },
                isPVEQuestEvent = false,
                lastTickStrengthTime = Timerservice.Tick
            };
        }

        public static IHostingData Load(BsonDocument data)
        {
            var info = new PlayerInfo()
            {
                info = new UserData()
                {
                    User = new UserInformation()
                    {
                        UserName = string.Empty,
                        UserGuid = 0,
                    },
                    Achiev = new UserAchievement()
                    {
                        achievData = new List<AchievementData>(),
                        battleInfo = new List<BattleInfo>(),// 战绩记录
                    },
                    wAchiev = NewUserWeekAchievement(),
                    bag = new Abelkhan.Bag(),
                    Strength = 0,
                    gold = 0,
                    diamond = 0,
                    quest = 0, 
                    score = 0,
                    RoleList = new (),
                    GuideSteps = new(),
                },
            };

            var user = data.GetValue("User").AsBsonDocument;
            info.info.User = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<UserInformation>(user);

            if (data.Contains("Achiev"))
            {
                var achiev = data.GetValue("Achiev").AsBsonDocument;
                info.info.Achiev = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<UserAchievement>(achiev);
                info.CheckAchievementStatus();
            }

            if (data.Contains("wAchiev"))
            {
                var wAchiev = data.GetValue("wAchiev").AsBsonDocument;
                info.info.wAchiev = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<UserWeekAchievement>(wAchiev);
                info.CheckWeekAchievement();
            }

            info.info.Strength = data.GetValue("Strength").AsInt32;

            foreach (var role in data.GetValue("RoleList").AsBsonArray)
            {
                var r = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Role>(role.AsBsonDocument);
                info.info.RoleList.Add(r);
            }

            var itemList = new List<RoleCardInfo>();
            foreach (var item in data.GetValue("bag").AsBsonArray)
            {
                var roleID = item.AsBsonDocument.GetValue("roleID").AsInt32;
                var Number = item.AsBsonDocument.GetValue("Number").AsInt32;
                var isTatter = item.AsBsonDocument.GetValue("isTatter").AsBoolean;

                itemList.Add(new RoleCardInfo() { 
                    roleID= roleID,
                    Number = Number,
                    isTatter = isTatter
                });
            }
            info.info.bag.ItemList = itemList;

            if (data.Contains("guideSteps"))
            {
                var guideSteps = data.GetValue("guideSteps").AsBsonArray;
                foreach (var g in guideSteps)
                {
                    info.info.GuideSteps.Add((GuideStep)g.AsInt32);
                }
            }

            if (data.Contains("gold"))
            {
                info.info.gold = data.GetValue("gold").AsInt32;
            }
            else
            {
                info.info.gold = 100;
            }

            if (data.Contains("diamond"))
            {
                info.info.diamond = data.GetValue("diamond").AsInt32;
            }
            else
            {
                info.info.diamond = 10;
            }

            if (data.Contains("quest"))
            {
                info.info.quest = data.GetValue("quest").AsInt32;
            }
            else
            {
                info.info.quest = 10001;
            }

            if (data.Contains("isPVEQuestEvent"))
            {
                info.isPVEQuestEvent = data.GetValue("isPVEQuestEvent").AsBoolean;
            }
            else
            {
                info.isPVEQuestEvent = false;
            }

            if (data.Contains("RogueRoleList"))
            {
                var rogueRoleList = data.GetValue("RogueRoleList").AsBsonArray;
                foreach (var r in rogueRoleList)
                {
                    //info.RogueRoleList.Add(MongoDB.Bson.Serialization.BsonSerializer.Deserialize<Role>(r.ToBsonDocument()));
                }
            }

            if (data.Contains("PVELevelIndex"))
            {
                info.info.PVELevelIndex = data.GetValue("PVELevelIndex").AsInt32;
            }

            if (data.Contains("score"))
            {
                info.info.score = data.GetValue("score").AsInt32;
            }
            else
            {
                info.info.score = 0;
            }

            if (data.Contains("lastTickStrengthTime"))
            {
                info.lastTickStrengthTime = data.GetValue("lastTickStrengthTime").AsInt64;
            }
            else
            {
                info.lastTickStrengthTime = Timerservice.Tick;
            }
            
            if (data.Contains("isTmp"))
            {
                info.isTmp = data.GetValue("isTmp").AsBoolean;
            }

            if (data.Contains("BattleData"))
            {
                var BattleData = data.GetValue("BattleData").AsBsonDocument;
                info.battleShopPlayer = new battle_shop_player(null, null, info.BattleRoleGroup(), info.info.User);
                info.battleShopPlayer.BattleData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<UserBattleData>(BattleData);
            }

            info.tmpPVEData = new();
            if (data.Contains("TmpBattleData"))
            {
                var BattleData = data.GetValue("TmpBattleData").AsBsonDocument;
                info.tmpPVEData.TmpBattleShopPlayer = new battle_shop_player(null, null, info.BattleRoleGroup(), info.info.User);
                info.tmpPVEData.TmpBattleShopPlayer.BattleData = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<UserBattleData>(BattleData);
            }
            if (data.Contains("TmpPVELevelIndex"))
            {
                info.tmpPVEData.TmpPVELevelIndex = data.GetValue("TmpPVELevelIndex").AsInt32;
            }
            if (data.Contains("TmpPVEIsQuestEvent"))
            {
                info.tmpPVEData.isQuestEvent = data.GetValue("TmpPVEIsQuestEvent").AsBoolean;
            }

            return info;
        }

        public BsonDocument Store()
        {
            var roleList = new BsonArray();
            foreach(var id in info.RoleList)
            {
                roleList.Add(id.ToBsonDocument());
            }
            
            var rogueRoleList = new BsonArray();
            //foreach (var r in RogueRoleList)
            //{
            //    rogueRoleList.Add(r.ToBsonDocument());
            //}

            var itemList = new BsonArray();
            foreach (var item in info.bag.ItemList)
            {
                var i = new BsonDocument { { "roleID", item.roleID }, { "Number", item.Number }, { "isTatter", item.isTatter } };
                itemList.Add(i);
            }

            var tmpGuideSteps = new BsonArray();
            if (info.GuideSteps != null)
            {
                foreach (var g in info.GuideSteps)
                {
                    tmpGuideSteps.Add(g);
                }
            }

            var doc = new BsonDocument
            {
                { "User", info.User.ToBsonDocument() },
                { "Achiev", info.Achiev.ToBsonDocument() },
                { "wAchiev", info.wAchiev.ToBsonDocument() },
                { "Strength", info.Strength },
                { "gold", info.gold },
                { "diamond", info.diamond },
                { "quest", info.quest },
                { "score", info.score },
                { "isQuestEvent", isPVEQuestEvent },
                { "RoleList", roleList },
                { "bag", itemList },
                { "guideSteps", tmpGuideSteps },
                { "lastTickStrengthTime", lastTickStrengthTime },
                { "PVELevelIndex", info.PVELevelIndex },
                {  "isTmp", isTmp },
                { "RogueRoleList", rogueRoleList }
            };

            if (battleShopPlayer != null)
            {
                doc.Add("BattleData", battleShopPlayer.BattleData.ToBsonDocument());
            }

            if (tmpPVEData != null)
            {
                if (tmpPVEData.TmpBattleShopPlayer != null)
                {
                    doc.Add("TmpBattleData", tmpPVEData.TmpBattleShopPlayer.BattleData.ToBsonDocument());
                }
                doc.Add("TmpPVELevelIndex", tmpPVEData.TmpPVELevelIndex);
            }

            return doc;
        }

        public UserData Info()
        {
            return info;
        }

        public void AddRogueRole(List<Role> role)
        {
            //RogueRoleList.AddRange(role);
        }

        public bool CheckNotFinishPVELevel()
        {
            return config.Config.PVELevelConfigs.ContainsKey(info.quest);
        }

        public List<int> BattleRoleGroup()
        {
            return config.Config.RoleConfigs.Keys.ToList();
        }

        public void UpdateGuideStep(GuideStep step)
        {
            if (!info.GuideSteps.Contains(step))
            {
                info.GuideSteps.Add(step);
            }
        }

        public void AddRoleList(List<Role> roles)
        {
            info.RoleList.AddRange(roles);
        }
        
        public void AddStrength(int _strength)
        {
            info.Strength += _strength;
        }

        public int CostStrength(int strength) 
        {
            if ((info.Strength - strength) < 0)
            {
                return (int)em_error.no_enough_strength;
            }
            info.Strength -= strength;
            return (int)em_error.success;
        }
        
        public bool CheckStrength()
        {
            if (info.Strength >= 100)
            {
                return false;
            }

            bool s_change = false;
            while (true)
            {
                if ((lastTickStrengthTime + 2 * 60 * 1000) > Timerservice.Tick)
                {
                    break;
                }

                lastTickStrengthTime += 2 * 60 * 1000;
                ++info.Strength;
                s_change = true;

                if (info.Strength >= 100)
                { 
                    lastTickStrengthTime = Timerservice.Tick;
                    break;
                }
            }

            return s_change;
        }

        public AchievementData GetAchievementData(Achievement enumAchieve)
        {
            foreach(var a in info.Achiev.achievData)
            {
                if (a.emAchievement == enumAchieve)
                {
                    return a;
                }
            }

            var data = new AchievementData()
            {
                emAchievement = enumAchieve,
                status = AchievementAwardStatus.EMNotComplete,
                count = 0,
            };
            info.Achiev.achievData.Add(data);

            return data;
        }

        private void CheckAchievementStatus()
        {
            foreach (var info in info.Achiev.battleInfo)
            {
                CheckAchievement(info);
            }
        }

        private void CheckWeekAchievement()
        {
            foreach (var info in info.wAchiev.battleInfo)
            {
                CheckWeekAchievement(info);
            }

            foreach (var a in info.wAchiev.wAchievData)
            {
                if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), a.emAchievement), out var task))
                {
                    if (a.count >= task.Value && a.status != AchievementAwardStatus.EMRecv)
                    {
                        a.status = AchievementAwardStatus.EMComplete;
                    }
                }
            }
        }

        private bool CheckSuccessiveFiveVictory()
        {
            var data = GetAchievementData(Achievement.EMSuccessiveFiveVictory);
            if (data.status == AchievementAwardStatus.EMRecv)
            {
                return false;
            }

            var check = true;
            for(int i = info.Achiev.battleInfo.Count - 1; i < 0 && i < (info.Achiev.battleInfo.Count - 5); --i)
            {
                var battleInfo = info.Achiev.battleInfo[i];
                if (battleInfo.mod == BattleMod.Battle || battleInfo.mod == BattleMod.RankBattle)
                {
                    if (battleInfo.isVictory != BattleVictory.victory)
                    {
                        check = false; 
                        break;
                    }
                }
            }

            if (check)
            {
                data.status = AchievementAwardStatus.EMComplete;
            }

            return check;
        }

        private bool CheckFullLevelVictory(BattleInfo battleInfo)
        {
            var data = GetAchievementData(Achievement.EMFullLevelVictory);
            if (data.status == AchievementAwardStatus.EMRecv)
            {
                return false;
            }

            var check = true;
            if (battleInfo.RoleList == null)
            {
                check = false;
            }
            else
            {
                foreach (var r in battleInfo.RoleList)
                {
                    if (r != null && r.Level < 3)
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check)
            {
                data.status = AchievementAwardStatus.EMComplete;
            }

            return check;
        }

        private bool CheckMachinistlVictory(BattleInfo battleInfo)
        {
            var data = GetAchievementData(Achievement.EMMachinistlVictory);
            if (data.status == AchievementAwardStatus.EMRecv)
            {
                return false;
            }

            var check = true;
            if (battleInfo.RoleList == null)
            {
                check = false;
            }
            else
            {
                foreach (var r in battleInfo.RoleList)
                {
                    if (r != null && r.FettersSkillID.fetters_id != 6)
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check)
            {
                data.status = AchievementAwardStatus.EMComplete;
            }

            return check;
        }

        private bool CheckFullAttributesVictory(BattleInfo battleInfo)
        {
            var data = GetAchievementData(Achievement.EMFullAttributesVictory);
            if (data.status == AchievementAwardStatus.EMRecv)
            {
                return false;
            }

            var check = true;
            if (battleInfo.RoleList == null)
            {
                check = false;
            }
            else
            {
                foreach (var r in battleInfo.RoleList)
                {
                    if (r != null && r.Attack >= 50 && r.HP >= 50)
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check)
            {
                data.status = AchievementAwardStatus.EMComplete;
            }

            return check;
        }

        private bool CheckNoneEquipmentVictory(BattleInfo battleInfo)
        {
            var data = GetAchievementData(Achievement.EMNoneEquipmentVictory);
            if (data.status == AchievementAwardStatus.EMRecv)
            {
                return false;
            }

            var check = true;
            if (battleInfo.RoleList == null)
            {
                check = false;
            }
            else
            {
                foreach (var r in battleInfo.RoleList)
                {
                    if (r != null && r.equipID != 0)
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (check)
            {
                data.status = AchievementAwardStatus.EMComplete;
            }

            return check;
        }

        public bool CheckAchievement(BattleInfo battleInfo)
        {
            bool check = false;
            if (CheckSuccessiveFiveVictory())
            {
                check = true;
            }
            if (CheckFullLevelVictory(battleInfo))
            {
                check = true;
            }
            if (battleInfo.isStreakVictory)
            {
                var data = GetAchievementData(Achievement.EMStreakVictory);
                if (data.status != AchievementAwardStatus.EMRecv)
                {
                    data.status = AchievementAwardStatus.EMComplete;
                    check = true;
                }
            }
            if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMFiveHundredGame), out var task))
            {
                if (info.Achiev.battleInfo.Count >= task.Value)
                {
                    var data = GetAchievementData(Achievement.EMFiveHundredGame);
                    if (data.status != AchievementAwardStatus.EMRecv)
                    {
                        data.status = AchievementAwardStatus.EMComplete;
                        check = true;
                    }
                }
            }
            if (CheckMachinistlVictory(battleInfo))
            {
                check = true;
            }
            if (CheckFullAttributesVictory(battleInfo))
            {
                check = true;
            }
            if (CheckNoneEquipmentVictory(battleInfo))
            {
                check = true;
            }
            if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMNotGivenAllYet), out var task1))
            {
                if (battleInfo.RoleList == null)
                {
                    check = false;
                }
                else
                {
                    var count = 0;
                    foreach(var r in battleInfo.RoleList)
                    {
                        if (r != null)
                        {
                            count++;
                        }
                    }
                    if (count < task1.Value)
                    {
                        var data = GetAchievementData(Achievement.EMNotGivenAllYet);
                        if (data.status != AchievementAwardStatus.EMRecv)
                        {
                            data.status = AchievementAwardStatus.EMComplete;
                            check = true;
                        }
                    }
                }
            }

            return check;
        }

        public bool AddCheckAchievement(BattleInfo battleInfo)
        {
            info.Achiev.battleInfo.Add(battleInfo);

            return CheckAchievement(battleInfo);
        }

        public AchievementData GetWeekAchievementData(Achievement enumAchieve)
        {
            foreach (var a in info.wAchiev.wAchievData)
            {
                if (a.emAchievement == enumAchieve)
                {
                    return a;
                }
            }

            var data = new AchievementData()
            {
                emAchievement = enumAchieve,
                status = AchievementAwardStatus.EMNotComplete,
                count = 0,
            };
            info.wAchiev.wAchievData.Add(data);

            return data;
        }

        private bool CheckFullLevelRole(BattleInfo battleInfo)
        {
            if (battleInfo.RoleList != null)
            {
                foreach (var role in battleInfo.RoleList)
                {
                    if (role != null && role.Level >= 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool CheckKillRole(Role roleInfo)
        {
            RefreshWeekAchiev();

            var check = false;
            var a = GetWeekAchievementData(Achievement.EMWeekTotalAnnihilation);
            if (a.status != AchievementAwardStatus.EMRecv)
            {
                a.count++;
                if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekTotalAnnihilation), out var task))
                {
                    if (a.count >= task.Value)
                    {
                        a.status = AchievementAwardStatus.EMComplete;
                        check = true;
                    }
                }
            }

            if (roleInfo.FettersSkillID != null)
            {
                switch (roleInfo.FettersSkillID.fetters_id)
                {
                    case 9:
                        {
                            a = GetWeekAchievementData(Achievement.EMWeekWizardAnnihilation);
                            if (a.status != AchievementAwardStatus.EMRecv)
                            {
                                a.count++;
                                if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekWizardAnnihilation), out var task))
                                {
                                    if (a.count >= task.Value)
                                    {
                                        a.status = AchievementAwardStatus.EMComplete;
                                        check = true;
                                    }
                                }
                            }
                        }
                        break;

                    case 2:
                        {
                            a = GetWeekAchievementData(Achievement.EMWeekBerserkerAnnihilation);
                            if (a.status != AchievementAwardStatus.EMRecv)
                            {
                                a.count++;
                                if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekBerserkerAnnihilation), out var task))
                                {
                                    if (a.count >= task.Value)
                                    {
                                        a.status = AchievementAwardStatus.EMComplete;
                                        check = true;
                                    }
                                }
                            }
                        }
                        break;

                    case 3:
                        {
                            a = GetWeekAchievementData(Achievement.EMWeekCorsairAnnihilation);
                            if (a.status != AchievementAwardStatus.EMRecv)
                            {
                                if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekCorsairAnnihilation), out var task))
                                {
                                    a.count++;
                                    if (a.count >= task.Value)
                                    {
                                        a.status = AchievementAwardStatus.EMComplete;
                                        check = true;
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            
            return check;
        }

        public bool CheckBuyRole(Role roleInfo)
        {
            RefreshWeekAchiev();

            var check = false;
            if (config.Config.SkillConfigs.TryGetValue(roleInfo.SkillID, out var skill))
            {
                if (skill.EffectTime == 3)
                {
                    var a = GetWeekAchievementData(Achievement.EMWeekBuyTenBeforeRound);
                    if (a.status != AchievementAwardStatus.EMRecv)
                    {
                        a.count++;
                        if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekBuyTenBeforeRound), out var task))
                        {
                            if (a.count >= task.Value)
                            {
                                a.status = AchievementAwardStatus.EMComplete;
                                check = true;
                            }
                        }
                    }
                }
                else if (skill.EffectTime == 18)
                {
                    var a = GetWeekAchievementData(Achievement.EMWeekBuyBeHurted);
                    if (a.status != AchievementAwardStatus.EMRecv)
                    {
                        a.count++;
                        if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekBuyBeHurted), out var task))
                        {
                            if (a.count >= task.Value)
                            {
                                a.status = AchievementAwardStatus.EMComplete;
                                check = true;
                            }
                        }
                    }
                }
                else if (skill.EffectTime == 8)
                {
                    var a = GetWeekAchievementData(Achievement.EMWeekBuyBeDead);
                    if (a.status != AchievementAwardStatus.EMRecv)
                    {
                        a.count++;
                        if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekBuyBeDead), out var task))
                        {
                            if (a.count >= task.Value)
                            {
                                a.status = AchievementAwardStatus.EMComplete;
                                check = true;
                            }
                        }
                    }
                }
            }

            return check;
        }

        public bool CheckBuyEquip(int equipID)
        {
            RefreshWeekAchiev();

            var a = GetWeekAchievementData(Achievement.EMWeekBuyTenEquip);
            if (a.status != AchievementAwardStatus.EMRecv)
            {
                a.count++;
                if (config.Config.TaskConfigs.TryGetValue(Enum.GetName(typeof(Achievement), Achievement.EMWeekBuyTenEquip), out var task))
                {
                    if (a.count >= task.Value)
                    {
                        a.status = AchievementAwardStatus.EMComplete;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool CheckWeekAchievement(BattleInfo battleInfo)
        {
            var check = false;
            if (battleInfo.isVictory == BattleVictory.victory)
            {
                var a = GetWeekAchievementData(Achievement.EMWeekOneGameVictory);
                if (a.status != AchievementAwardStatus.EMRecv)
                {
                    a.status = AchievementAwardStatus.EMComplete;
                    check = true;
                }
            }
            if (CheckFullLevelRole(battleInfo))
            {
                var a = GetWeekAchievementData(Achievement.EMWeekOneFullLevelRole);
                if (a.status != AchievementAwardStatus.EMRecv)
                {
                    a.status = AchievementAwardStatus.EMComplete;
                    check = true;
                }
            }

            return check;
        }

        public bool AddCheckWeekAchievement(BattleInfo battleInfo)
        {
            RefreshWeekAchiev();
            info.wAchiev.battleInfo.Add(battleInfo);

            return CheckWeekAchievement(battleInfo);
        }

        private void RefreshWeekAchiev()
        {
            if (info.wAchiev != null && info.wAchiev.timeout < Timerservice.Tick)
            {
                info.wAchiev = NewUserWeekAchievement();
            }
        }

        public bool CheckGetAchievementReward(Achievement achievement)
        {
            RefreshWeekAchiev();

            switch (achievement)
            {
                case Achievement.EMSuccessiveFiveVictory:
                case Achievement.EMFullLevelVictory:
                case Achievement.EMStreakVictory:
                case Achievement.EMFiveHundredGame:
                case Achievement.EMMachinistlVictory:
                case Achievement.EMPeakStrengthVictory:
                case Achievement.EMGold25:
                case Achievement.EMFullAttributesVictory:
                case Achievement.EMNoneEquipmentVictory:
                    {
                        var a = GetAchievementData(achievement);
                        if (a.status == AchievementAwardStatus.EMComplete)
                        {
                            a.status = AchievementAwardStatus.EMRecv;
                            return true;
                        }
                    }
                    break;

                case Achievement.EMWeekOneGameVictory:
                case Achievement.EMWeekOpenCardPack:
                case Achievement.EMWeekTotalAnnihilation:
                case Achievement.EMWeekWizardAnnihilation:
                case Achievement.EMWeekBerserkerAnnihilation:
                case Achievement.EMWeekCorsairAnnihilation:
                case Achievement.EMWeekBuyTenBeforeRound:
                case Achievement.EMWeekBuyBeHurted:
                case Achievement.EMWeekBuyBeDead:
                case Achievement.EMWeekBuyTenEquip:
                case Achievement.EMWeekOneFullLevelRole:
                    {
                        var a = GetWeekAchievementData(achievement);
                        if (a.status == AchievementAwardStatus.EMComplete)
                        {
                            a.status = AchievementAwardStatus.EMRecv;
                            return true;
                        }
                    }
                    break;
            }

            return false;
        }

        public bool refresh(int stage)
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
            }

            if (BattleShopPlayerImpl.BattleData.coin <= 0)
            {
                Log.Log.trace("refresh coin is not enough coin:{0}", BattleShopPlayerImpl.BattleData.coin);
                return false;
            }

            BattleShopPlayerImpl.BattleData.coin--;
            BattleShopPlayerImpl.ShopData = BattleShopPlayerImpl.refresh(stage);
            Log.Log.trace("refresh coin:{0} stage:{1} ShopData:{2}", BattleShopPlayerImpl.BattleData.coin, stage, BattleShopPlayerImpl.ShopData.ToJson());
            return true;
        }

        public int GetStage()
        {
            var PVELevelCfgImpl = pveLevelCfg;
            var PVELevelIndex = info.PVELevelIndex;
            if (tmpPVEData != null && tmpPVEData.TmpPVELevelCfg != null)
            {
                PVELevelCfgImpl = tmpPVEData.TmpPVELevelCfg;
                PVELevelIndex = tmpPVEData.TmpPVELevelIndex;
            }

            Log.Log.trace("GetStage PVELevelCfg:{0} PVELevelIndex:{1}", PVELevelCfgImpl.ToJson(), PVELevelIndex);
            if (config.Config.PVERoundConfigs.TryGetValue(PVELevelCfgImpl.Level[PVELevelIndex], out var cfg))
            {
                return cfg.Stage;
            }

            return 1;
        }

        public void ClearCurrPVEState()
        {
            Log.Log.trace("ClearPVEState");
            battleShopPlayer = null;
            pveLevelCfg = null;
            info.PVELevelIndex = 0;
        }

        public void ClearTmpPVEState()
        {
            Log.Log.trace("ClearTmpPVEState");
            tmpPVEData = null;
        }

        public battle_shop_player battle_failed_back()
        {
            if (isTmp)
            {
                if (tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
                {
                    tmpPVEData.TmpBattleShopPlayer.BattleData.faild = 1;
                    StartPVERound(0);
                }

                return tmpPVEData.TmpBattleShopPlayer;
            }
            else
            {
                battleShopPlayer.BattleData.faild = 1;
                StartPVERound(0);

                return battleShopPlayer;
            }
        }

        public void StartPVERound(int addCoin)
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            var PVELevelCfg = pveLevelCfg;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
                PVELevelCfg = tmpPVEData.TmpPVELevelCfg;
            }

            if (info.PVELevelIndex >= PVELevelCfg.Level.Count)
            {
                info.PVELevelIndex = 0;
            }

            foreach (var r in BattleShopPlayerImpl.BattleData.RoleList)
            {
                if (r != null)
                {
                    r.TempHP = 0;
                    r.TempAttack = 0;
                    r.TempAdditionBuffer.Clear();
                }
            }

            if (BattleShopPlayerImpl.ShopData.SaleRoleList[5] == null)
            {
                BattleShopPlayerImpl.ShopData = BattleShopPlayerImpl.refresh(GetStage());
            }

            if (config.Config.PVERoundConfigs.TryGetValue(PVELevelCfg.Level[info.PVELevelIndex], out var cfg))
            {
                BattleShopPlayerImpl.BattleData.coin = cfg.Gold + BattleShopPlayerImpl.bankCoin + addCoin;
                BattleShopPlayerImpl.bankCoin = 0;
                BattleShopPlayerImpl.Coin = BattleShopPlayerImpl.BattleData.coin;

                Log.Log.trace("StartPVERound PVELevelCfg:{0} PVELevelIndex:{1} cfg:{2} coin:{3}", PVELevelCfg.ToJson(), info.PVELevelIndex, cfg.ToJson(), BattleShopPlayerImpl.BattleData.coin);
            }

            BattleShopPlayerImpl.evs.Add(new shop_event()
            {
                ev = EMRoleShopEvent.start_round
            });

            BattleShopPlayerImpl.do_skill(GetStage());
        }

        public void EndPVERound()
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            if (tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
            }

            BattleShopPlayerImpl.evs.Add(new shop_event()
            {
                ev = EMRoleShopEvent.end_round
            });

            BattleShopPlayerImpl.do_skill(GetStage());
        }

        private void ResetTmpBattleShop()
        {
            isTmp = false;
        }

        public Tuple<em_error, List<QuestRogueRole>> StartQuestReady(string _clientUUID, battle_client_caller battleClientCaller)
        {
            ResetTmpBattleShop();

            if (battleShopPlayer == null)
            {
                battleShopPlayer = new battle_shop_player(_clientUUID, battleClientCaller, BattleRoleGroup(), info.User);
            }
            else
            {
                battleShopPlayer.ClientUUID = _clientUUID;
                battleShopPlayer.BattleClientCaller = battleClientCaller;
            }

            if (config.Config.PVELevelConfigs.TryGetValue(info.quest, out var cfg))
            {
                Log.Log.trace("StartQuestReady PVELevelConfigs TryGetValue quest:{0}", info.quest);

                if (cfg.Level.Count <= 0)
                {
                    return Tuple.Create(em_error.last_quest, new List<QuestRogueRole>());
                }

                pveLevelCfg = cfg;
                if (!isInitRound)
                {
                    battleShopPlayer.BattleData.totalFaild = 0;
                    battleShopPlayer.BattleData.faild = pveLevelCfg.Hp;
                    StartPVERound(0);
                    isInitRound = true;
                }
                if ((battleShopPlayer.ShopData.SaleRoleList.Count == 0 || 
                     battleShopPlayer.ShopData.SalePropList.Count == 0) && 
                    battleShopPlayer.ShopData.SaleRoleList[5] == null)
                {
                    battleShopPlayer.refresh(GetStage());
                }

                Log.Log.trace("PVELevelConfigs TryGetValue isQuestEvent:{0}", isPVEQuestEvent);
                if (!isPVEQuestEvent)
                {
                    Log.Log.trace("PVELevelConfigs TryGetValue info.PVELevelIndex:{0}", info.PVELevelIndex);
                    if (pveLevelCfg.Level.Count <= info.PVELevelIndex)
                    {
                        return Tuple.Create(em_error.last_quest, new List<QuestRogueRole>());
                    }

                    if (!config.Config.PVERoundConfigs.TryGetValue(pveLevelCfg.Level[info.PVELevelIndex], out var roundcfg))
                    {
                        return Tuple.Create(em_error.success, new List<QuestRogueRole>());
                    }

                    var roles = new List<QuestRogueRole>();
                    for (int index = 0; index < roundcfg.EventID.Count; index++)
                    {
                        var eventId = roundcfg.EventID[index];
                        if (config.Config.PVEEventConfigs.TryGetValue(eventId, out var ecfg))
                        {
                            if (config.Config.RoleConfigs.TryGetValue(ecfg.RoleID, out RoleConfig rcfg))
                            {
                                var r = new Role();
                                r.RoleID = ecfg.RoleID;
                                r.Level = ecfg.RoleLevel;
                                r.SkillID = rcfg.SkillID;
                                r.HP = ecfg.RoleHP;
                                r.Attack = ecfg.RoleAttack;
                                r.equipID = ecfg.RoleEquip;

                                var _roleRougue = new QuestRogueRole()
                                {
                                    r = r,
                                    index = index
                                };
                                roles.Add(_roleRougue);
                            }
                        }
                    }

                    return Tuple.Create(em_error.success, roles);
                }
                return Tuple.Create(em_error.success, new List<QuestRogueRole>());
            }

            return Tuple.Create(em_error.not_exist_quest, new List<QuestRogueRole>());
        }

        public Tuple<em_error, List<QuestRogueRole>> StartQuestReady1(int quest, string _clientUUID, battle_client_caller battleClientCaller)
        {
            if (tmpPVEData == null)
            {
                tmpPVEData = new TmpPVEData();
            }
            if (tmpPVEData.TmpBattleShopPlayer == null || tmpPVEData.TmpPVELevelCfg == null || quest != tmpPVEData.TmpPVELevelCfg.ID)
            {
                tmpPVEData.TmpBattleShopPlayer = new battle_shop_player(_clientUUID, battleClientCaller, BattleRoleGroup(), info.User);
            }
            else
            {
                tmpPVEData.TmpBattleShopPlayer.ClientUUID = _clientUUID;
                tmpPVEData.TmpBattleShopPlayer.BattleClientCaller = battleClientCaller;
            }
            isTmp = true;

            if (config.Config.PVELevelConfigs.TryGetValue(quest, out var cfg))
            {
                Log.Log.trace("StartQuestReady1 PVELevelConfigs TryGetValue quest:{0}", quest);

                if (cfg.Level.Count <= 0)
                {
                    return Tuple.Create(em_error.last_quest, new List<QuestRogueRole>());
                }

                tmpPVEData.TmpPVELevelCfg = cfg;
                if (!tmpPVEData.isInitRound)
                {
                    tmpPVEData.TmpBattleShopPlayer.BattleData.totalFaild = 0;
                    tmpPVEData.TmpBattleShopPlayer.BattleData.faild = tmpPVEData.TmpPVELevelCfg.Hp;
                    StartPVERound(0);
                    tmpPVEData.isInitRound = true;
                }
                if ((tmpPVEData.TmpBattleShopPlayer.ShopData.SaleRoleList.Count == 0 || 
                     tmpPVEData.TmpBattleShopPlayer.ShopData.SalePropList.Count == 0) && 
                    tmpPVEData.TmpBattleShopPlayer.ShopData.SaleRoleList[5] == null)
                {
                    tmpPVEData.TmpBattleShopPlayer.refresh(GetStage());
                }

                Log.Log.trace("PVELevelConfigs TryGetValue isQuestEvent:{0}", tmpPVEData.isQuestEvent);
                if (!tmpPVEData.isQuestEvent)
                {
                    Log.Log.trace("PVELevelConfigs TryGetValue info.PVELevelIndex:{0}", tmpPVEData.TmpPVELevelIndex);
                    if (tmpPVEData.TmpPVELevelCfg.Level.Count <= tmpPVEData.TmpPVELevelIndex)
                    {
                        return Tuple.Create(em_error.last_quest, new List<QuestRogueRole>());
                    }

                    if (!config.Config.PVERoundConfigs.TryGetValue(tmpPVEData.TmpPVELevelCfg.Level[tmpPVEData.TmpPVELevelIndex], out var roundcfg))
                    {
                        return Tuple.Create(em_error.success, new List<QuestRogueRole>());
                    }

                    var roles = new List<QuestRogueRole>();
                    for (int index = 0; index < roundcfg.EventID.Count; index++)
                    {
                        var eventId = roundcfg.EventID[index];
                        if (config.Config.PVEEventConfigs.TryGetValue(eventId, out var ecfg))
                        {
                            if (config.Config.RoleConfigs.TryGetValue(ecfg.RoleID, out RoleConfig rcfg))
                            {
                                var r = new Role();
                                r.RoleID = ecfg.RoleID;
                                r.Level = ecfg.RoleLevel;
                                r.SkillID = rcfg.SkillID;
                                r.HP = ecfg.RoleHP;
                                r.Attack = ecfg.RoleAttack;
                                r.equipID = ecfg.RoleEquip;

                                var _roleRougue = new QuestRogueRole()
                                {
                                    r = r,
                                    index = index
                                };
                                roles.Add(_roleRougue);
                            }
                        }
                    }
                    return Tuple.Create(em_error.success, roles);
                }
                return Tuple.Create(em_error.success, new List<QuestRogueRole>());
            }

            return Tuple.Create(em_error.not_exist_quest, new List<QuestRogueRole>());
        }

        public void StartQuestShop(int index)
        {
            Log.Log.trace("StartQuestShop index:{0}", index);
            if (index < 0)
            {
                return;
            }

            var r = new ShopRole();
            var PVELevelCfgImpl = pveLevelCfg;
            var PVELevelIndex = info.PVELevelIndex;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                PVELevelCfgImpl = tmpPVEData.TmpPVELevelCfg;
                PVELevelIndex = tmpPVEData.TmpPVELevelIndex;
            }

            if (config.Config.PVERoundConfigs.TryGetValue(PVELevelCfgImpl.Level[PVELevelIndex], out var roundcfg))
            {
                var eventId = roundcfg.EventID[index];
                if (config.Config.PVEEventConfigs.TryGetValue(eventId, out var ecfg))
                {
                    if (config.Config.RoleConfigs.TryGetValue(ecfg.RoleID, out RoleConfig rcfg))
                    {
                        r.RoleID = ecfg.RoleID;
                        r.Level = ecfg.RoleLevel;
                        r.SkillID = rcfg.SkillID;
                        r.HP = ecfg.RoleHP;
                        r.Attack = ecfg.RoleAttack;
                        r.equipID = ecfg.RoleEquip;
                    }
                }
            }

            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                tmpPVEData.TmpBattleShopPlayer.ShopData.SaleRoleList[5] = r;
                tmpPVEData.isQuestEvent = true;
            }
            else
            {
                battleShopPlayer.ShopData.SaleRoleList[5] = r;
                isPVEQuestEvent = true;
            }
        }
        

        public UserBattleData StartQuestBattle()
        {
            var PVELevelCfgImpl = pveLevelCfg;
            var PVELevelIndex = info.PVELevelIndex;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                PVELevelCfgImpl = tmpPVEData.TmpPVELevelCfg;
                PVELevelIndex = tmpPVEData.TmpPVELevelIndex;
            }

            var target = new UserBattleData();
            target.RoleList = new List<Role>();

            if (config.Config.PVERoundConfigs.TryGetValue(PVELevelCfgImpl.Level[PVELevelIndex], out var cfg))
            {
                foreach(var rInfo in cfg.Enemys)
                {
                    if (config.Config.RoleConfigs.TryGetValue(rInfo.RoleID, out RoleConfig rcfg))
                    {
                        var r = new Role();

                        r.RoleID = rInfo.RoleID;
                        r.Level = rInfo.RoleLevel;
                        r.SkillID = rcfg.SkillID;
                        r.HP = rInfo.RoleHP;
                        r.Attack = rInfo.RoleAttack;
                        r.TempHP = 0;
                        r.TempAttack = 0;
                        r.TempAdditionBuffer = new();
                        r.FettersSkillID = new Fetters()
                        {
                            fetters_id = rcfg.Fetters,
                            fetters_level = 0,
                            number = 1
                        };
                        r.equipID = rInfo.RoleEquip;

                        target.RoleList.Add(r);
                    }
                }

                target.FettersList = battle_shop_player.check_fetters(target.RoleList);
            }

            return target;
        }

        private bool add_role(string ClientUUID, int role_index, int index)
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
            }

            var r = BattleShopPlayerImpl.add_role(role_index, index);
            if (r != null)
            {
                if (CheckBuyRole(r))
                {
                    client_mng.PlayerClientCaller.get_client(ClientUUID).achievement_complete(info.Achiev, info.wAchiev);
                }

                return true;
            }

            return false;
        }

        private em_error buy_role(bool isTmp, string ClientUUID, int index, int role_index)
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
            }

            var r = BattleShopPlayerImpl.BattleData.RoleList[role_index];
            var s = BattleShopPlayerImpl.ShopData.SaleRoleList[index];

            if (s == null)
            {
                return em_error.db_error;
            }

            if (BattleShopPlayerImpl.BattleData.coin < s.Price)
            {
                return em_error.no_enough_coin;
            }
            BattleShopPlayerImpl.BattleData.coin -= s.Price;

            if (r == null)
            {
                if (!add_role(ClientUUID, role_index, index))
                {
                    return em_error.db_error;
                }
            }
            else
            {
                var err = BattleShopPlayerImpl.merge_role(index, role_index);
                if (err != em_error.success)
                {
                    return err;
                }

                if (CheckBuyRole(r))
                {
                    client_mng.PlayerClientCaller.get_client(ClientUUID).achievement_complete(info.Achiev, info.wAchiev);
                }
            }

            return em_error.success;
        }

        private em_error buy_equip(string ClientUUID, ShopProp p, int index, int role_index)
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
            }

            var err = BattleShopPlayerImpl.buy_equip(p, index, role_index);
            if (err != em_error.success)
            {
                return err;
            }

            if (CheckBuyEquip(p.PropID))
            {
                client_mng.PlayerClientCaller.get_client(ClientUUID).achievement_complete(info.Achiev, info.wAchiev);
            }

            return em_error.success;
        }

        public em_error buy(string ClientUUID, ShopIndex shop_index, int index, int role_index)
        {
            var BattleShopPlayerImpl = battleShopPlayer;
            if (isTmp && tmpPVEData != null && tmpPVEData.TmpBattleShopPlayer != null)
            {
                BattleShopPlayerImpl = tmpPVEData.TmpBattleShopPlayer;
            }

            if (shop_index == ShopIndex.Role)
            {
                var result = buy_role(isTmp, ClientUUID, index, role_index);
                if (result != em_error.success)
                {
                    return result;
                }
            }
            else if (shop_index == ShopIndex.Prop)
            {
                var p = BattleShopPlayerImpl.ShopData.SalePropList[index];
                if (p == null)
                {
                    return em_error.db_error;
                }

                if (BattleShopPlayerImpl.BattleData.coin < p.Price)
                {
                    return em_error.no_enough_coin;
                }
                BattleShopPlayerImpl.BattleData.coin -= p.Price;

                if (p.PropID >= config.Config.FoodIDMin && p.PropID <= config.Config.FoodIDMax)
                {
                    var result = BattleShopPlayerImpl.buy_food(p, index, role_index, GetStage());
                    if (result != em_error.success)
                    {
                        return result;
                    }
                }
                else if (p.PropID >= config.Config.EquipIDMin && p.PropID <= config.Config.EquipIDMax)
                {
                    var result = buy_equip(ClientUUID, p, index, role_index);
                    if (result != em_error.success)
                    {
                        return result;
                    }
                }
                else
                {
                    return em_error.db_error;
                }

                BattleShopPlayerImpl.ShopData.SalePropList[index] = null;
            }
            else
            {
                return em_error.db_error;
            }

            return em_error.success;
        }
    }

    public static class AvatarExtensions
    {
        public static PlayerInfo PlayerInfo(this Avatar avatar)
        {
            var _data = avatar.get_real_hosting_data<PlayerInfo>();
            return _data.Data;
        }
    }

    public class client_mng
    {
        private readonly Dictionary<string, avatar.Avatar> client_token_dict = new ();
        private readonly Dictionary<string, string> client_sdk_uuid = new ();

        private static readonly player_client_caller player_Client_Caller = new();
        public static player_client_caller PlayerClientCaller
        {
            get
            {
                return player_Client_Caller;
            }
        }

        private static readonly battle_client_caller battleClientCaller = new();
        public static battle_client_caller BattleClientCaller
        {
            get
            {
                return battleClientCaller;
            }
        }

        private readonly AvatarMgr avatarMgr;

        public int Count
        {
            get
            {
                return avatarMgr.Count;
            }
        }

        public static async void forward_offline_msg(OfflineMsgMgr.offline_msg _offline_msg)
        {
            if (await Player.offline_Msg_Mng.send_offline_msg(_offline_msg))
            {
                var player_guid = long.Parse(_offline_msg.player_guid);
                var player_svr_key = RedisHelp.BuildPlayerGuidCacheKey(player_guid);
                string player_hub_name = await Player._redis_handle.GetStrData(player_svr_key);
                if (string.IsNullOrEmpty(player_hub_name))
                {
                    return;
                }
                if (player_hub_name != Hub.Hub.name)
                {
                    var player_proxy = Player.player_Proxy_Mng.get_player_proxy(player_hub_name);
                    player_proxy.player_have_offline_msg(player_guid);
                }
                else
                {
                    var target_role = Player.client_Mng.guid_get_client_proxy(player_guid);
                    if (target_role != null)
                    {
                        await Player.offline_Msg_Mng.process_offline_msg(_offline_msg.player_guid);
                    }
                }
            }
        }

        public client_mng()
        {
            avatarMgr = new();
            avatarMgr.init();
            avatarMgr.init_data_db_opt((opt) =>
            {
                opt.DBName = Constant.Constant.player_db_name;
                opt.DBCollection = Constant.Constant.player_db_collection;
                opt.GuidCollection = Constant.Constant.player_db_guid_collection;
                opt.StoreDelayTime = 10 * 60 * 1000;
            });
            avatarMgr.add_hosting<PlayerInfo>();

            bag.BagModdule.init(30, avatarMgr);
        }

        public async Task<Tuple<string, bool>> token_player_login(string sdk_uuid, string anonymous_sdk_uuid = "")
        {
            var token = Guid.NewGuid().ToString();

            avatar.Avatar _avatar = null;
            if (!string.IsNullOrEmpty(anonymous_sdk_uuid))
            {
                _avatar = await avatarMgr.load(anonymous_sdk_uuid);
                if (!string.IsNullOrEmpty(sdk_uuid))
                {
                    if (!await _avatar.transfer(sdk_uuid))
                    {
                        Log.Log.err("_avatar.transfer faild anonymous_sdk_uuid:{0, }sdk_uuid:{1}", anonymous_sdk_uuid, sdk_uuid);
                    }
                }
            }
            else
            {
                _avatar = await avatarMgr.load(sdk_uuid);
            }

            if (_avatar != null)
            {

                var player_svr_key = RedisHelp.BuildPlayerGuidCacheKey(_avatar.Guid);
                await Player._redis_handle.SetStrData(player_svr_key, Hub.Hub.name, RedisHelp.PlayerSvrInfoCacheTimeout);

                _avatar.onDestory += () =>
                {
                    var uuid_key = RedisHelp.BuildPlayerSDKUUIDCacheKey(_avatar.ClientUUID);
                    Player._redis_handle.DelData(uuid_key);

                    var player_key = RedisHelp.BuildPlayerSvrCacheKey(_avatar.SDKUUID);
                    Player._redis_handle.DelData(player_key);

                    var player_guid_key = RedisHelp.BuildPlayerGuidCacheKey(_avatar.Guid);
                    Player._redis_handle.DelData(player_guid_key);

                    var gate_key = RedisHelp.BuildPlayerGateCacheKey(_avatar.SDKUUID);
                    Player._redis_handle.DelData(gate_key);
                };

                client_token_dict[token] = _avatar;

                return Tuple.Create(token, false);
            }
            else
            {
                client_sdk_uuid[token] = sdk_uuid;
            }

            return Tuple.Create(token, true);
        }

        public avatar.Avatar token_get_client_proxy(string uuid, string token)
        {
            if (!client_token_dict.Remove(token, out avatar.Avatar _avatar))
            {
                throw new LoginException($"invaild token:{token}");
            }

            if (!string.IsNullOrEmpty(_avatar.ClientUUID))
            {
                Hub.Hub._gates.disconnect_client(_avatar.ClientUUID, "others login!");
            }

            avatarMgr.bind_avatar(_avatar, uuid);

            var uuid_key = RedisHelp.BuildPlayerSDKUUIDCacheKey(uuid);
            Player._redis_handle.SetStrData(uuid_key, _avatar.SDKUUID, RedisHelp.PlayerSvrInfoCacheTimeout);

            return _avatar;
        }

        public async Task<avatar.Avatar> create_player(string uuid, string token, string name, string nick_name, string avatar)
        {
            if (client_sdk_uuid.Remove(token, out var sdk_uuid))
            {
                var _avatar = await avatarMgr.create_avatar(sdk_uuid);
                avatarMgr.bind_avatar(_avatar, uuid);
                var info = _avatar.get_real_hosting_data<PlayerInfo>();
                info.Data.Info().User.UserName = nick_name;
                info.Data.Info().User.UserGuid = _avatar.Guid;
                info.Data.Info().User.Avatar = avatar;
                _avatar.set_dirty();

                return _avatar;
            }
            return null;
        }

        public async Task<Avatar> uuid_get_client_proxy(string uuid)
        {
            var _avatar = avatarMgr.get_avatar(uuid);
            if (_avatar != null)
            {
                return _avatar;
            }


            var uuid_key = RedisHelp.BuildPlayerSDKUUIDCacheKey(uuid);
            var sdk_uuid = await Player._redis_handle.GetStrData(uuid_key);
            await Player._redis_handle.Expire(uuid_key, RedisHelp.PlayerSvrInfoCacheTimeout);

            if (!string.IsNullOrEmpty(sdk_uuid))
            {
                var gate_key = RedisHelp.BuildPlayerGateCacheKey(sdk_uuid);
                var gate_name = await Player._redis_handle.GetStrData(gate_key);
                await Player._redis_handle.Expire(gate_key, RedisHelp.PlayerSvrInfoCacheTimeout);

                _avatar = await avatarMgr.load_or_create(sdk_uuid, uuid);
                Hub.Hub._gates.client_seep(uuid, gate_name);

                return _avatar;
            }

            return null;
        }

        public Avatar sdk_uuid_get_client_proxy(string sdk_uuid)
        {
            var _avatar = avatarMgr.get_target_avatar(sdk_uuid);
            return _avatar;
        }

        public Avatar guid_get_client_proxy(long guid)
        {
            var _avatar = avatarMgr.get_target_avatar(guid);
            return _avatar;
        }
    }
}
