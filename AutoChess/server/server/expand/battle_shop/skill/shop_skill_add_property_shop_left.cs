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
        private void AddPropertyShopLeft(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("AddPropertyShopLeft begin");

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.AddPropertyShopLeft;

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
            skilleffect.value = new List<int>() { v1, v2 };

            var _s_r = _player.ShopData.SaleRoleList[0];
            _s_r.HP += v1;
            _s_r.Attack += v2;
            _s_r = _player.ShopData.SaleRoleList[2];
            _s_r.HP += v1;
            _s_r.Attack += v2;

            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);

            Log.Log.trace("AddPropertyShopLeft end");
        }
    }
}
