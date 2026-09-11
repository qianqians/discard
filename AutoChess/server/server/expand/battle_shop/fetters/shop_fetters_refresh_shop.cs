using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void RefreshShop(FettersConfig fetters, battle_shop_player _player, int stage)
        {
            if (fetters.RefreshItemID[0] != 0 && fetters.RefreshItemNum != 0)
            {
                _player.add_shop_item(fetters.RefreshItemID[0], fetters.RefreshItemNum);
            }
            else
            {
                _player.refresh(stage);
            }
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = fetters.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.RefreshShop;
            skilleffect.value = new List<int>();
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_fetters_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
        }
    }
}
