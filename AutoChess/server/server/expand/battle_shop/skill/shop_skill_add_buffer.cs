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
        private void AddBuffer(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("AddBuffer begin");

            var target_list = GetTargetIndex(_player, skill.ObjectDirection, skill.ObjCount);
            foreach (var target_index in target_list)
            {
                AddBuffer(_player, target_index, skill.EffectScope, skill.AddBufferID);
            }
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = target_list;
            skilleffect.effect = SkillEffectEM.AddBuffer;
            skilleffect.value = new List<int>() { skill.AddBufferID };
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);

            Log.Log.trace("AddBuffer end");
        }
    }
}
