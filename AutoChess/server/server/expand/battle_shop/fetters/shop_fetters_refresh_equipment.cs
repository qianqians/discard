using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void RefreshEquipment(FettersConfig fetters, battle_shop_player _player)
        {
            var PropID = 0;
            if (fetters.RefreshItemID.Count > 0)
            {
                var index = RandomHelper.RandomInt(fetters.RefreshItemID.Count);
                PropID = fetters.RefreshItemID[index];
                foreach (var prop in _player.ShopData.SalePropList)
                {
                    if (prop.PropID > 3001 && prop.PropID < 3999)
                    {
                        _player.ShopData.SalePropList.Remove(prop);
                        break;
                    }
                }
                _player.ShopData.SalePropList.Add(new ShopProp()
                {
                    PropID = PropID,
                    IsFreeze = false,
                });
            }
            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = fetters.Id;
            skilleffect.spellcaster = index;
            skilleffect.effect = SkillEffectEM.RefreshEquipment;
            skilleffect.value = new List<int>() { PropID };
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_fetters_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);
        }
    }
}
