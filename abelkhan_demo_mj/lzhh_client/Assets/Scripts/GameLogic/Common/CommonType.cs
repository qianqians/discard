using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameCommon
{
    public enum PeopleNum
    {
        TwoPeople = 2,
        ThreePeople = 3,
        FourPeople = 4
    }

    public enum GameRule
    {
        NoLai = 1,
        HalfLai = 2,
        Laizi = 3,
        LaiShangLai = 4
    }

    public enum GameScore
    {
        Two = 2,
        Five = 5,
        Ten = 10,
        Twenty = 20,
        Fifty = 25,

        Five_hundred = 500,
        Two_thousand = 2000,
        Five_thousand = 5000,
        Twenty_thousand = 20000,
    }

    public enum GameTimes
    {
        Eight = 8,
        Sixteen = 16
    }

    public enum PayRule
    {
        OnePay = 1,
        AAPay = 2,
        MatchPay = 3,
        GlodPay = 4,
        Contest = 5,
    }


    public enum mjSite
    {
        Matcher = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        End
    }

    public enum mjCards
    {
        Nodefine = 0,
        wan_1 = 1,
        wan_2 = 2,
        wan_3 = 3,
        wan_4 = 4,
        wan_5 = 5,
        wan_6 = 6,
        wan_7 = 7,
        wan_8 = 8,
        wan_9 = 9,
        wan_Num = 10,
        tong_1 = 11,
        tong_2 = 12,
        tong_3 = 13,
        tong_4 = 14,
        tong_5 = 15,
        tong_6 = 16,
        tong_7 = 17,
        tong_8 = 18,
        tong_9 = 19,
        tong_Num = 20,
        tiao_1 = 21,
        tiao_2 = 22,
        tiao_3 = 23,
        tiao_4 = 24,
        tiao_5 = 25,
        tiao_6 = 26,
        tiao_7 = 27,
        tiao_8 = 28,
        tiao_9 = 29,
        tiao_Num = 30,
    }

    public enum mjGangstate
    {
        zi_xiao,
        hui_tou_xiao,
        gang,
    }

    public enum mjPlayerstate
    {
        in_game,
        read,
        none,
    }

    public enum roomDisbandVoteState
    {
        unvote = 0,
        agree = 1,
        oppose = 2
    }

    public enum ChatState
    {
        Sentence,
        ChatFace,
        Emoji,
        Voice
    }

    public enum diamondDate
    {
        One = 7,
        Two = 11,
        Three = 20,
        Four = 22,
        Five = 31
    }

    public enum diamondNum
    {
        One = 1,
        Two = 1,
        Three = 1,
        Four = 1,
        Five = 1
    }

    public enum goldOfBox
    {
        One = 5000,
        Two = 8000,
        Three = 0,
        Four = 0,
        Five = 0
    }

    public enum diamondOfBox
    {
        One = 0,
        Two = 0,
        Three = 1,
        Four = 2,
        Five = 3
    }

    public enum gameCount
    {
        Ten = 1,
        Twenty = 2,
        Thirty = 3
    }

    public enum laiyou
    {
        no,
        ruanlaiyou,
        yinlaiyou,
        ruanyoushangyou,
        yinyoushangyou,
    }

    /// <summary>
    /// 软胡，硬胡（没有使用到赖子的配牌功能）
    /// </summary>
    public enum HupaiState
    {
        no_hu,
        soft_hu,
        hard_hu
    }
}
