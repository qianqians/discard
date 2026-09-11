using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void FettersSummonShop(battle_shop_player _player, shop_event trigger_ev)
        {
            int summon_index = -1;
            for (int i = 0; i < _player.BattleData.RoleList.Count; i++)
            {
                if (_player.BattleData.RoleList[i] == null)
                {
                    summon_index = i;
                    break;
                }
            }
            if (summon_index == -1)
            {
                summon_index = trigger_ev.index;
            }

            FettersConfig fetters;
            if (!config.Config.FettersConfigs.TryGetValue(trigger_ev.fetters_id, out fetters))
            {
                return;
            }
            var v1 = fetters.Stage1value_1;
            var v2 = fetters.Stage1value_2;
            var r = _player.BattleData.RoleList[index];
            switch (r.Level)
            {
                case 1:
                {
                    v1 = fetters.Stage1value_1;
                    v2 = fetters.Stage1value_2;
                }
                break;

                case 2:
                {
                    v1 = fetters.Stage2value_1;
                    v2 = fetters.Stage2value_2;
                }
                break;

                case 3:
                {
                    v1 = fetters.Stage3value_1;
                    v2 = fetters.Stage3value_2;
                }
                break;
            }

            if (_player.summon_role(summon_index, fetters.SummonId, fetters.SummonLevel, v1, v2))
            {
                var skilleffect = new ShopSkillEffect();
                skilleffect.skill_id = fetters.Id;
                skilleffect.spellcaster = summon_index;
                skilleffect.recipient = new List<int>();
                skilleffect.effect = SkillEffectEM.SummonShop;
                skilleffect.value = new List<int>() { _player.BattleData.RoleList[summon_index].RoleID };

                _player.BattleClientCaller.get_client(_player.ClientUUID).shop_fetters_effect(skilleffect);
                _player.BattleClientCaller.get_client(_player.ClientUUID).shop_summon(summon_index, _player.BattleData.RoleList[summon_index]);
            }
        }
    }
}
