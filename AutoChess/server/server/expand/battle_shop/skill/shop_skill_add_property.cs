using Abelkhan;
using config;
using Microsoft.AspNetCore.Routing.Tree;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using static System.Formats.Asn1.AsnWriter;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void AddProperty(ShopSkillConfig skill, battle_shop_player _player, shop_event trigger_ev)
        {
            Log.Log.trace("AddProperty begin");

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.AddProperty;

            var target_list = GetTargetIndex(_player, skill.ObjectDirection, skill.ObjCount);
            if ((int)skill.ObjectDirection == 8)
            {
                target_list = new List<int>() { trigger_ev.index };
            }

            Log.Log.trace("slill  target_list:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(target_list));
            foreach (var target_index in target_list)
            {
                if (target_index >= 0)
                {
                    Log.Log.trace("slill  _player:{0}", Newtonsoft.Json.JsonConvert.SerializeObject(_player));
                    var r = _player.BattleData.RoleList[index];
                    var Level = r.Level;
                    Log.Log.trace("slill r:{0}", r);
                    if (trigger_ev.ev == EMRoleShopEvent.update)
                    {
                        Level -= 1;
                    }

                    switch (Level)
                    {
                        case 1:
                        {
                            AddProperty(_player, target_index, skill.EffectScope, skill.Level1Value_1,
                                skill.Level1Value_2);
                            skilleffect.value = new List<int>() { skill.Level1Value_1, skill.Level1Value_2 };
                        }
                            break;

                        case 2:
                        {
                            AddProperty(_player, target_index, skill.EffectScope, skill.Level2Value_1,
                                skill.Level2Value_2);
                            skilleffect.value = new List<int>() { skill.Level2Value_1, skill.Level2Value_2 };
                        }
                            break;

                        case 3:
                        {
                            AddProperty(_player, target_index, skill.EffectScope, skill.Level3Value_1,
                                skill.Level3Value_2);
                            skilleffect.value = new List<int>() { skill.Level3Value_1, skill.Level3Value_2 };
                        }
                            break;
                    }
                }
            }

            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
            _player.BattleClientCaller.get_client(_player.ClientUUID).role_add_property(_player.BattleData);

            Log.Log.trace("AddProperty end");
        }
    }
}
