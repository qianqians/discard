using Abelkhan;
using config;
using System.Collections.Generic;
using System.Linq;

namespace battle_shop
{
    public partial class shop_skill_role
    {
        private void UseFettersSkill(battle_shop_player _player, shop_event trigger_ev, int stage)
        {
            if (fettersLevel <= 0)
            {
                return;
            }

            FettersConfig fetters;
            if (!config.Config.FettersConfigs.TryGetValue(fettersSkillID, out fetters))
            {
                return;
            }

            switch (fetters.Effect)
            {
                case SkillEffectEM.AddProperty:
                {
                    AddProperty(fetters, _player);
                }
                break;

                case SkillEffectEM.AddCoin:
                {
                    AddCoin(fetters, _player);
                }
                break;

                case SkillEffectEM.RefreshShop:
                {
                    RefreshShop(fetters, _player, stage);
                }
                break;

                case SkillEffectEM.UpdateLevel:
                {
                    UpdateLevel(_player);
                }
                break;

                case SkillEffectEM.SummonShop:
                {
                    FettersSummonShop(_player, trigger_ev);
                }
                break;

                case SkillEffectEM.AddBuffer:
                {
                    AddBuffer(fetters, _player);
                }
                break;

                case SkillEffectEM.AddBuildValue:
                {
                    AddBuildValue(fetters, _player);
                }
                break;

                case SkillEffectEM.RefreshEquipment:
                {
                    RefreshEquipment(fetters, _player);
                }
                break;
            }
        }
    }
}
