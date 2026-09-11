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
        private void ItemReduced(ShopSkillConfig skill, battle_shop_player _player)
        {
            foreach (var prop in _player.ShopData.SalePropList)
            {
                if (prop.PropID > 1001 && prop.PropID < 1999)
                {
                    prop.Price -= 1;
                }
            }

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.effect = SkillEffectEM.ItemReduced;
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
        }
    }
}
