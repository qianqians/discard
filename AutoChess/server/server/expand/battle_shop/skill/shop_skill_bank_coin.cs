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
        private void BankCoin(ShopSkillConfig skill, battle_shop_player _player)
        {
            Log.Log.trace("BankCoin begin");

            var skilleffect = new ShopSkillEffect();
            skilleffect.skill_id = skill.Id;
            skilleffect.spellcaster = index;
            skilleffect.recipient = new List<int>();
            skilleffect.effect = SkillEffectEM.BankCoin;

            var v1 = skill.Level1Value_1;
            var r = _player.BattleData.RoleList[index];
            switch (r.Level)
            {
                case 1:
                    {
                        v1 = skill.Level1Value_1;
                    }
                    break;

                case 2:
                    {
                        v1 = skill.Level2Value_1;
                    }
                    break;

                case 3:
                    {
                        v1 = skill.Level2Value_1;
                    }
                    break;
            }
            skilleffect.value = new List<int>() { v1 };

            var bankCoin = _player.BattleData.coin < v1 ? _player.BattleData.coin : v1;
            _player.bankCoin += bankCoin;

            _player.BattleClientCaller.get_client(_player.ClientUUID).shop_skill_effect(skilleffect);
            _player.BattleClientCaller.get_client(_player.ClientUUID).refresh(_player.BattleData, _player.ShopData);

            Log.Log.trace("BankCoin end");
        }
    }
}
