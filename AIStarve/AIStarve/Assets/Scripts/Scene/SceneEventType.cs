using UnityEngine;

namespace SceneManagement
{
    public enum SceneEvent
    {
        CharacterCreated,
        CharacterRemoved,
        ObjectCreated,
        ObjectRemoved,
        ObjectPlaced,
        CharacterMoved,//角色对象移动
        SelfMove,//自己移动
        SendCreateObject,//
        CreateObject,//��������
        Working, //工作键只有一个F
        DBInitFinish,//�������ݿ��ʼ��
        SendCreateGrowUpObject, //发送生长植物网络请求
        CreateGrowUpObject, //创建生长植物
        CreateCultivatedLand //创建耕地
    }

}