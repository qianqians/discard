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
        private void UseSkill(battle_shop_player _player, shop_event trigger_ev, int stage)
        {
            Log.Log.trace("UseSkill skillID:{0} begin!", skillID);

            ShopSkillConfig skill;
            if (!config.Config.ShopSkillConfigs.TryGetValue(skillID, out skill))
            {
                return;
            }
            Log.Log.trace("UseSkill skillID:{0} start!", skillID);

            switch (skill.Effect)
            {
                case SkillEffectEM.AddProperty:
                {
                    AddProperty(skill, _player, trigger_ev);
                }
                break;

                case SkillEffectEM.AddPropertyCoin5:
                {
                    AddPropertyCoin5(skill, _player);
                }
                break;

                case SkillEffectEM.AddAllPropertyByBattleRound:
                {
                    AddAllPropertyByBattleRound(skill, _player);
                }
                break;

                case SkillEffectEM.AddPropertyMoreCoin:
                {
                    AddPropertyMoreCoin(skill, _player);
                }
                break;

                case SkillEffectEM.AddPropertyLevel3:
                {
                    AddPropertyLevel3(skill, _player);
                }
                break;

                case SkillEffectEM.AddPropertyShopLeft:
                {
                    AddPropertyShopLeft(skill, _player);
                }
                break;

                case SkillEffectEM.AddPropertyHasEquip:
                {
                    AddPropertyHasEquip(skill, _player);
                }
                break;

                case SkillEffectEM.BankCoin:
                {
                    BankCoin(skill, _player);
                }
                break;

                case SkillEffectEM.AddCoin:
                {
                    AddCoin(skill, _player);
                }
                break;

                case SkillEffectEM.NextRoundCoin:
                {
                    NextRoundCoin(skill, _player);
                }
                break;

                case SkillEffectEM.RefreshShop:
                {
                    RefreshShop(skill, _player, stage);
                }
                break;

                case SkillEffectEM.UpdateLevel:
                {
                    UpdateLevel(skill, _player);
                }
                break;

                case SkillEffectEM.SummonBattle:
                {
                    SummonShop(skill, _player, trigger_ev);
                }
                break;

                case SkillEffectEM.ItemReduced:
                {
                    ItemReduced(skill, _player);
                }
                break;

                case SkillEffectEM.AddBuffer:
                {
                    AddBuffer(skill, _player);
                }
                break;

                case SkillEffectEM.AddEquipment:
                {
                    AddEquipment(skill, _player);
                }
                break;

                case SkillEffectEM.CopySkill:
                {
                    CopySkill(skill, _player);
                }
                break;
            }

            Log.Log.trace("UseSkill skillID:{0} end!", skillID);
        }
    }
}
