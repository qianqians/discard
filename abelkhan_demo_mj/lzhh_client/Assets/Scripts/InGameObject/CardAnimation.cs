using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine;
class cardMoveAnim
{
    public float PassTime = 0.1f;
    public float Speed;
    public Vector3 startPosition;
    public Vector3 EndPosition;
    public Vector3 CurrentPosition;
    float CurrentTime = 0.0f;
    public void SetPassTime ( float parmPassTime)
    {
        PassTime = parmPassTime;
    }
    public void SetSpeed(float parmSpeed)
    {
        Speed = parmSpeed;
    }
    public void InitAnim()
    {
        CurrentTime = 0;
    }
    public bool UpdatAnim(float delatTime)
    {
        CurrentTime += delatTime;
        float step = Speed * CurrentTime / PassTime;
        CurrentPosition = new Vector3(
            Mathf.Lerp(startPosition.x, EndPosition.x, step),
            Mathf.Lerp(startPosition.y, EndPosition.y, step),
            Mathf.Lerp(startPosition.z, EndPosition.z, step));
        
        if(CurrentTime > PassTime)
        {
            CurrentTime = PassTime;
         //   EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<int>(EventId.InGame_SelectCardAnimFinsh, 1);
            return true;
        }
       
        return false;
    }
    public virtual void SetStartPostion(Vector3 parmStartPos)
    {
        startPosition = parmStartPos;
    }
    public virtual void SetEndPostion(Vector3 parmStartPos)
    {
        startPosition = parmStartPos;
    }
}
public class BaseCardAnimation
{
    public bool bAnimBegin = false;
    public GameObject CardObject;
    public CardData.AnimFinish handlerAnimFinish;
    // Use this for initialization
    public void BeginAnim()
    {
        bAnimBegin = true;
    }
    public virtual void init(GameObject gameObjectParm)
    {
        CardObject = gameObjectParm;
    }
}
public class PlayAnimation : BaseCardAnimation
{
    cardMoveAnim CardMoveAnim;
    public void SetTargetPosition(Vector3 positionParm)
    {
        CardMoveAnim.EndPosition = positionParm;
    }
    public override void init(GameObject gameObjectParm)
    {
        base.init(gameObjectParm);
        if (CardMoveAnim == null)
        {
            CardMoveAnim = new cardMoveAnim();
        }
        CardMoveAnim.InitAnim();
        CardMoveAnim.startPosition = gameObjectParm.transform.position;
        CardMoveAnim.SetSpeed(4.0f);
        CardMoveAnim.SetPassTime(0.1f);        
    }
    public void Update()
    {
        if (bAnimBegin && CardMoveAnim.UpdatAnim(Time.deltaTime))
        {
            bAnimBegin = false;
            handlerAnimFinish();
        }
        CardObject.transform.position = CardMoveAnim.CurrentPosition;
    }
}
public class SelectAnimation : BaseCardAnimation
{
    cardMoveAnim CardMoveAnim;
    
    public override void init(GameObject gameObjectParm)
    {
        base.init(gameObjectParm);
        CardMoveAnim = new cardMoveAnim();
        BoxCollider box = CardObject.GetComponent("BoxCollider") as BoxCollider;
        float offset = box.bounds.size.y/3;
        CardMoveAnim.startPosition = gameObjectParm.transform.position;
        CardMoveAnim.EndPosition = CardMoveAnim.startPosition + gameObjectParm.transform.up * offset;
        CardMoveAnim.SetSpeed(16.0f);
        CardMoveAnim.SetPassTime(0.001f);
    }

    public void Reset(GameObject gameObjectParm)
    {
        BoxCollider box = CardObject.GetComponent("BoxCollider") as BoxCollider;
        float offset = box.bounds.size.y/3;
        CardMoveAnim.InitAnim();
        CardMoveAnim.startPosition = gameObjectParm.transform.position;
        CardMoveAnim.EndPosition = CardMoveAnim.startPosition - gameObjectParm.transform.up * offset;
        CardMoveAnim.SetSpeed(16.0f);
        CardMoveAnim.SetPassTime(0.001f);
    }

    public void Update()
    {
        if(bAnimBegin && CardMoveAnim.UpdatAnim(Time.deltaTime) )
        {
            handlerAnimFinish();
            bAnimBegin = false;
        }
        CardObject.transform.position = CardMoveAnim.CurrentPosition;//CardMoveAnim.CurrentPosition;
    }
}

