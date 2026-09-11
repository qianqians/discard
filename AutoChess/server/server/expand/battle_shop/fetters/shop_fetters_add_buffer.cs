using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void AddBuffer(FettersConfig fetters, battle_shop_player _player)
        {
            var count_index = fetters.ObjCount.Count < fettersLevel ? fetters.ObjCount.Count - 1 : fettersLevel - 1;
            var count = fetters.ObjCount[count_index];
            var target_list = GetTargetIndex(_player, 0, count);
            foreach (var target_index in target_list)
            {
                AddBuffer(_player, target_index, fetters.EffectScope, fetters.AddBufferID);
            }
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = fetters.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = target_list;
            skilleffect.effect = SkillEffectEM.AddBuffer;
            skilleffect.value = new List<int>() { fetters.AddBufferID };
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_fetters_effect(skilleffect);
        }
    }
}
