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
        private void NextRoundCoin(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("NextRoundCoin begin");

            var addCoin = 0;
            var r = _player.BattleData.RoleList[index];
            switch (r.Level)
            {
                case 1:
                    {
                        addCoin = skill.Level1Value_1;
                    }
                    break;

                case 2:
                    {
                        addCoin = skill.Level2Value_1;
                    }
                    break;

                case 3:
                    {
                        addCoin = skill.Level3Value_1;
                    }
                    break;
            }
            _player.bankCoin += addCoin;

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.NextRoundCoin;
            skilleffect.value = new List<int>() { addCoin };
            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);

            Log.Log.trace("NextRoundCoin end");
        }
    }
}
