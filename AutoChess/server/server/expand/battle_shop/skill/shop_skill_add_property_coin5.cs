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
        private void AddPropertyCoin5(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("AddPropertyCoin5 begin");

            var target_index = 0;
            while (true)
            {
                target_index = RandomHelper.RandomInt(_player.BattleData.RoleList.Count);
                
                if (target_index == index)
                {
                    continue;
                }

                if ((_player.BattleData.RoleList[target_index] != null))
                {
                    break;
                }
            }

            var n = (_player.Coin - _player.BattleData.coin) / 5;
            if (n > 0)
            {
                var skilleffect = new ShopSkillEffect();
                skilleffect.skill_id = skill.Id;
                skilleffect.spellcaster = index;
                skilleffect.recipient = new List<int>();
                skilleffect.effect = SkillEffectEM.AddPropertyCoin5;

                var r = _player.BattleData.RoleList[index];
                var Level = r.Level;
                switch (Level)
                {
                    case 1:
                        {
                            AddProperty(_player, target_index, skill.EffectScope, skill.Level1Value_1 * n, skill.Level1Value_2 * n);
                            skilleffect.value = new List<int>() { skill.Level1Value_1 * n, skill.Level1Value_2 * n };
                        }
                        break;

                    case 2:
                        {
                            AddProperty(_player, target_index, skill.EffectScope, skill.Level2Value_1 * n, skill.Level2Value_2 * n);
                            skilleffect.value = new List<int>() { skill.Level2Value_1 * n, skill.Level2Value_2 * n };
                        }
                        break;

                    case 3:
                        {
                            AddProperty(_player, target_index, skill.EffectScope, skill.Level3Value_1 * n, skill.Level3Value_2 * n);
                            skilleffect.value = new List<int>() { skill.Level3Value_1 * n, skill.Level3Value_2 * n };
                        }
                        break;
                }

                _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
                _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
                _player.BattleClientCaller.get_client(_player.ClientUUID).role_add_property(_player.BattleData);
            }

            Log.Log.trace("AddPropertyCoin5 end");
        }
    }
}
