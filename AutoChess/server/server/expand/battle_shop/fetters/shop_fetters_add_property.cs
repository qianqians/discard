using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void AddProperty(FettersConfig fetters, battle_shop_player _player)
        {
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = fetters.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.AddProperty;

            var count_index = fetters.ObjCount.Count < fettersLevel ? fetters.ObjCount.Count - 1 : fettersLevel - 1;
            var count = fetters.ObjCount[count_index];
            var target_list = GetTargetIndex(_player, 0, count);
            foreach (var target_index in target_list)
            { 
                if (target_index > 0)
                {
                    switch (fettersLevel)
                    {
                        case 1:
                        {
                            AddProperty(_player, target_index, fetters.EffectScope, fetters.Stage1value_1, fetters.Stage1value_2);
                            skilleffect.value = new List<int>() { fetters.Stage1value_1, fetters.Stage1value_2 };
                        }
                        break;

                        case 2:
                        {
                            AddProperty(_player, target_index, fetters.EffectScope, fetters.Stage2value_1, fetters.Stage2value_2);
                            skilleffect.value = new List<int>() { fetters.Stage2value_1, fetters.Stage2value_2 };
                        }
                        break;

                        case 3:
                        {
                            AddProperty(_player, target_index, fetters.EffectScope, fetters.Stage3value_1, fetters.Stage3value_2);
                            skilleffect.value = new List<int>() { fetters.Stage3value_1, fetters.Stage3value_2 };
                        }
                        break;

                        case 4:
                        {
                            AddProperty(_player, target_index, fetters.EffectScope, fetters.Stage4value_1, fetters.Stage4value_2);
                            skilleffect.value = new List<int>() { fetters.Stage4value_1, fetters.Stage4value_2 };
                        }
                        break;
                    }
                }
            }

            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_fetters_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
            _player.BattleClientCaller.get_client(_player.ClientUUID).role_add_property(_player.BattleData);
        }
    }
}
