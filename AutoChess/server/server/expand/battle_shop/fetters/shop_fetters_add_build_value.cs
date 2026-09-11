using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void AddBuildValue(FettersConfig fetters, battle_shop_player _player)
        {
            var buildValue = 0;
            switch (fettersLevel)
            {
                case 1:
                {
                    buildValue = fetters.Stage1value_1;
                }
                break;

                case 2:
                {
                    buildValue = fetters.Stage2value_1;
                }
                break;

                case 3:
                {
                    buildValue = fetters.Stage3value_1;
                }
                break;

                case 4:
                {
                    buildValue = fetters.Stage4value_1;
                }
                break;
            }
            _player.BattleData.buildValue += buildValue;

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = fetters.Id;
            skilleffect.spellcaster = index;
            skilleffect.effect = SkillEffectEM.AddBuildValue;
            skilleffect.value = new List<int>() { buildValue };
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_fetters_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
        }
    }
}
