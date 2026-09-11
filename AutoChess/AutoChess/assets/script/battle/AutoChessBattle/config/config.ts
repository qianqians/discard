/*
 * config.ts
 * author: qianqians
 * 2023/10/2
 */
import * as skill_config from './skill_config'
import * as buffer_config from './buffer_config'
import * as role_config from './role_config'
import * as Food_config from './Food_config';
import * as fetters from './fetters_config'
import * as Equip_Config from './Equip_config';
import * as task_config from './task_config';
import * as SkillIntroduce_config from './SkillIntroduce_config'
import * as  FetterIntroduce_config from './FetterIntroduce_config';
import * as Plot_Config from "./Plot_config";
import * as pvevent_config from './pvevent_config';
import * as RoleSp_config from './RoleSp_config';
import * as SpList_config from './SpList_config';
import * as HotFixed_config from './HotFixed_config';
import * as language_config from './language_config';
import * as SkillSound_config from './SkillSound_config';

export class config {
    public static MechanicFetters = 6;

    public static SkillConfig : Map<number, skill_config.SkillConfig>;
    public static FettersConfig:Map<number,fetters.FettersConfig>
    public static BufferConfig : Map<number, buffer_config.BufferConfig>;
    public static RoleConfig : Map<number, role_config.RoleConfig>;
    public static FoodConfig:Map<number,Food_config.FoodConfig>;
    public static EquipConfig:Map<number,Equip_Config.EquipConfig>;
    public static TaskConfig:Map<number,task_config.TaskConfig>;
    public static SkillIntroduceConfig:Map<number,SkillIntroduce_config.SkillIntroduceConfig>;
    public static FetterIntroduceConfig:Map<number,FetterIntroduce_config.FetterIntroduceConfig>;
    public static PlotConfig:Map<number,Plot_Config.PlotConfig>;
    public static PVEventConfig:Map<number,pvevent_config.PVEventConfig>;
    public static RoleSpConfig:Map<number,RoleSp_config.RoleSpConfig>;
    public static SpListConfig:Map<string,SpList_config.SpEffect>;
    public static LanguageConfig:Map<string, language_config.LanguageConfig>;
    public static SkillSoundConfig:Map<number,SkillSound_config.SkillSoundConfig>;
    public static HotFixedConfig:HotFixed_config.HotFixed_config;

    public static load()
    {
        let configList = [];

        let lamdbaSkillConfig = async () => config.SkillConfig = await skill_config.LoadSkillConfig();
        configList.push(lamdbaSkillConfig());
        let lamdbaFettersConfig = async () => config.FettersConfig = await fetters.LoadFettersConfig();
        configList.push(lamdbaFettersConfig());
        let lamdbaBufferConfig = async () => config.BufferConfig = await buffer_config.LoadBufferConfig();
        configList.push(lamdbaBufferConfig());
        let lamdbaRoleConfig = async () => config.RoleConfig = await role_config.LoadRoleConfig();
        configList.push(lamdbaRoleConfig());
        let lamdbaFoodConfig = async () => config.FoodConfig = await Food_config.LoadFoodConfig();
        configList.push(lamdbaFoodConfig());
        let lamdbaEquipConfig = async () => config.EquipConfig = await Equip_Config.LoadEquipConfig();
        configList.push(lamdbaEquipConfig());
        let lamdbaTaskConfig = async () => config.TaskConfig = await task_config.LoadTaskConfig()
        configList.push(lamdbaTaskConfig());
        let lamdbaSkillIntroduceConfig = async () => config.SkillIntroduceConfig = await SkillIntroduce_config.LoadSkillIntroduceConfig();
        configList.push(lamdbaSkillIntroduceConfig());
        let lamdbaFetterIntroduceConfig = async () => config.FetterIntroduceConfig = await FetterIntroduce_config.LoadFetterIntroduceConfig();
        configList.push(lamdbaFetterIntroduceConfig());
        let lamdbaPVEventConfig = async () => config.PVEventConfig=await pvevent_config.LoadPVEventConfig();
        configList.push(lamdbaPVEventConfig());
        let lamdbaRoleSpConfig = async () => config.RoleSpConfig=await RoleSp_config.LoadRoleSpConfig();
        configList.push(lamdbaRoleSpConfig());
        let lamdbaSpListConfig = async () => config.SpListConfig=await SpList_config.LoadSpConfig();
        configList.push(lamdbaSpListConfig());
        let lamdbaLanguageConfig = async () => config.LanguageConfig = await language_config.LoadLanguageConfig();
        configList.push(lamdbaLanguageConfig());
        let lamdbaHotFixedConfig = async () => config.HotFixedConfig = await HotFixed_config.LoadHotFixedConfig();
        configList.push(lamdbaHotFixedConfig());
        let lamdbaSkillSoundConfig = async () => config.SkillSoundConfig=await SkillSound_config.LoadSkillSoundConfig();
        configList.push(lamdbaSkillSoundConfig());
        //config.PlotConfig = await PlotConfig.LoadPlotConfig();

        return Promise.all(configList);
    }
}