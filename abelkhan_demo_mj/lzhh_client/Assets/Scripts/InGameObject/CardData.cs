using GameCommon;
using UnityEngine;
using TinyFrameWork;
public enum CardState
{
    None_State = 0,
    Select_State = 1,//被选中状态
    Play_State = 2,//出牌状态
    Play_Finsh = 3,//出牌结束状态
    Play_Over = 4//展示结束
}
public class CardData : MonoBehaviour
{
    CardState cardState = CardState.None_State;
    public mjCards CardType;
    public int OwnerShip;
    public int interactiveFlag;
    public SelectAnimation cardAnim;
    public PlayAnimation playCardAnim;
    public GameObject gameObjectSelf;
    public Vector3 PlayePosition;
    public Vector3 PositionOnTableShow;
    public Vector3 FinishRotation;
    public delegate void AnimFinish();
    public CharacterType sitType;
  //  public Camera MainCarema;
    //public Camera uiCarema;
    public void AnimFinsh()
    {
        if(cardState == CardState.Play_State)
        {
            gameObjectSelf.transform.Rotate(FinishRotation.x, FinishRotation.y, FinishRotation.z);
            cardState = CardState.Play_Finsh;
            PlayCardFinsh();
        }else if(cardState == CardState.Play_Finsh)
        {
            cardState = CardState.Play_Over;
            PlayCardOver();
        }
    }
    // Use this for initialization
    public void Init(GameObject parmGameObject)
    {
       // gameObjectSelf = parmGameObject;
    }
    void Start () {
        gameObjectSelf = this.gameObject;
        interactiveFlag = 0;
    }

    public void Reset()
    {
        cardState = CardState.None_State;
       // CardType = mjCards.Nodefine;回收只是隐藏
        OwnerShip = 0;
        interactiveFlag = 0;
    }


    public void JumpMovieOver()
    {
        try
        {
            gameObjectSelf.transform.Rotate(FinishRotation.x, FinishRotation.y, FinishRotation.z);
            cardState = CardState.Play_Over;
            PlayCardFinsh();
            PlayCardOver();
            gameObjectSelf.transform.position = PositionOnTableShow;
        }
        catch (System.Exception)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<string>(EventId.UIFrameWork_Bug, "JumpMovieOver");
        }

    }

    public void PlayCardOver()//摆牌结束后 如果自己可以碰牌,则发事件
    {
        GameObject CameraObject = GameObject.Find("Main Camera");
     //   Vector3 pos = gameObjectSelf.transform.position;
        if (interactiveFlag != 0)
        {
            EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<int, int>(EventId.UIFrameWork_Game_Animation_Playover, (int)CardType, interactiveFlag);
        }
        EventDispatcher.GetInstance().UIFrameWorkEventManager.TriggerEvent<Vector3, CharacterType,int>(EventId.UIFrameWork_Game_Current_Card_pos, PositionOnTableShow, sitType, (int)CardType);
    }

    public void PlayCardFinsh()//摆牌
    {
        playCardAnim.handlerAnimFinish = AnimFinsh;
        playCardAnim.init(gameObjectSelf);
        playCardAnim.SetTargetPosition(PositionOnTableShow);
        playCardAnim.BeginAnim();
    }

  //  private Action<RealtivePosition> callFun;
    public void PlayCard()//出牌
    {
        cardState = CardState.Play_State;
        if (playCardAnim == null)
        {
            playCardAnim = new PlayAnimation();
            playCardAnim.handlerAnimFinish = AnimFinsh;
        }
        playCardAnim.init(gameObjectSelf);
        playCardAnim.SetTargetPosition(PlayePosition);
        playCardAnim.BeginAnim();
    } 

	public CardState BeginSelect()
    {
        if (cardState == CardState.None_State)
        {
            if(cardAnim == null)
            {
                cardAnim = new SelectAnimation();
                cardAnim.handlerAnimFinish = AnimFinsh;
            }
            cardAnim.init(gameObjectSelf);
            cardAnim.BeginAnim();
            cardState = CardState.Select_State;
        }
        else if (cardState == CardState.Select_State)
        {
            if (playCardAnim == null)
            {
                playCardAnim = new PlayAnimation();
                playCardAnim.handlerAnimFinish = AnimFinsh;
            }
            cardState = CardState.Play_State;
        }
        return cardState;
    }

    /// <summary>
    /// 没加动画直接归位(选择麻将的时候)
    /// </summary>
    public void ResetObject()
    {
        BoxCollider box = gameObjectSelf.GetComponent("BoxCollider") as BoxCollider;
        float offset = box.bounds.size.y/3;
        gameObjectSelf.transform.position = gameObjectSelf.transform.position - gameObjectSelf.transform.up * offset;
        cardState = CardState.None_State;       
    }
        // Update is called once per frame
    void Update ()
    {
        if (cardAnim != null)
        {
            if(cardState == CardState.Select_State)//可能在选牌动画过程中出牌，要停止当前动画
            {
                cardAnim.Update();
            }
        }
        if(playCardAnim != null)
        {
            if (cardState == CardState.Play_State || cardState == CardState.Play_Finsh)//可能在选牌动画过程中出牌，要停止当前动画
            {
                playCardAnim.Update();
            }
        }
    }
}
