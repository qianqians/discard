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

    public enum GameScore
    {
        Two = 2,
        Five = 5,
        Ten = 10,
        Twenty = 20,
        Fifty = 25,
    }

    public enum GameTimes
    {
        unlimited = -1,
        Eight = 8,
        Sixteen = 16,
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
        W_1 = 1,
        W_2 = 2,
        W_3 = 3,
        W_4 = 4,
        W_5 = 5,
        W_6 = 6,
        W_7 = 7,
        W_8 = 8,
        W_9 = 9,

        B_1 = 11,
        B_2 = 12,
        B_3 = 13,
        B_4 = 14,
        B_5 = 15,
        B_6 = 16,
        B_7 = 17,
        B_8 = 18,
        B_9 = 19,

        T_1 = 21,
        T_2 = 22,
        T_3 = 23,
        T_4 = 24,
        T_5 = 25,
        T_6 = 26,
        T_7 = 27,
        T_8 = 28,
        T_9 = 29,
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
        Four = 28,
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

    public enum Box
    {
        One = 5000,
        Two = 8000,
        Three = 2,
        Four = 3,
        Five = 4
    }

    public enum gameCount
    {
        Ten = 10,
        Twenty = 20,
        Thirty = 30
    }

    public enum laiyou
    {
        no,
        ruanlaiyou,
        yinlaiyou,
        ruanyoushangyou,
        yinyoushangyou,
    }

    public enum DiamondInc
    {
        Sign,
        Task,
        GoldFor,
        Gm,
        Pay,
        Agent
    }
}