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
        private void UpdateLevel(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("UpdateLevel begin");

            UpdateLevel(_player);

            var r = _player.BattleData.RoleList[index];
            
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.UpdateLevel;
            skilleffect.value = new List<int>() { r.Level };
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);

            Log.Log.trace("UpdateLevel end");
        }
    }
}
