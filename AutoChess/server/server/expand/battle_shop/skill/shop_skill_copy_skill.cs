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
        private void CopySkill(ShopSkillConfig skill, battle_shop_player _player)
        {
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.effect = SkillEffectEM.CopySkill;
            skilleffect.value = new List<int>() { };

            var r = _player.BattleData.RoleList[index];
            var front = index - 3;
            if (front >= 0)
            {
                var copy = _player.BattleData.RoleList[front];

                var level = 1;
                switch (r.Level)
                {
                    case 1:
                    {
                        level = skill.Level1Value_1;
                    }
                    break;

                    case 2:
                    {
                        level = skill.Level2Value_1;
                    }
                    break;

                    case 3:
                    {
                        level = skill.Level3Value_1;
                    }
                    break;
                }

                r.TempSkillID = copy.SkillID;
                r.TempSkillLevel = level;

                skilleffect.value.Add(r.TempSkillID);
                skilleffect.value.Add(r.TempSkillLevel);
            }

            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
        }
    }
}
