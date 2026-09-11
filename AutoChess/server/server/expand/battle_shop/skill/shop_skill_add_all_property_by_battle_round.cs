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
        private void AddAllPropertyByBattleRound(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("AddAllPropertyByBattleRound begin");

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.AddAllPropertyByBattleRound;

            var count = _player.BattleData.round - spawnRound;
            var v1 = skill.Level1Value_1;
            var v2 = skill.Level1Value_2;
            var r = _player.BattleData.RoleList[index];
            switch (r.Level)
            {
                case 1:
                    {
                        v1 = skill.Level1Value_1;
                        v2 = skill.Level1Value_2;
                    }
                    break;

                case 2:
                    {
                        v1 = skill.Level2Value_1;
                        v2 = skill.Level2Value_2;
                    }
                    break;

                case 3:
                    {
                        v1 = skill.Level2Value_1;
                        v2 = skill.Level2Value_2;
                    }
                    break;
            }
            skilleffect.value = new List<int>() { count, v1, v2 };

            while (count > 0)
            {
                foreach (var target_r in _player.BattleData.RoleList)
                {
                    if (target_r != null)
                    {
                        if (skill.EffectScope == EffectScope.SingleBattle)
                        {
                            target_r.TempHP += v1;
                            target_r.TempAttack += v2;
                        }
                        else if (skill.EffectScope == EffectScope.WholeGame)
                        {
                            target_r.HP += v1;
                            target_r.Attack += v2;
                        }
                    }
                }
                count--;
            }

            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
            _player.BattleClientCaller.get_client(_player.ClientUUID).role_add_property(_player.BattleData);

            Log.Log.trace("AddAllPropertyByBattleRound end");
        }
    }
}
