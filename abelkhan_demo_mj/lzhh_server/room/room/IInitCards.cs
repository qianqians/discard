using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace room
{
    public interface IInitCards
    {
        void InitCards(List<Int64> cards);
    }

    public class TwoInitCards : IInitCards
    {
        public void InitCards(List<Int64> cards)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin InitCards");

            cards.Clear();
            for (int i = 0; i < 4; i++)
            {
                cards.Add((int)GameCommon.mjCards.B_1);
                cards.Add((int)GameCommon.mjCards.B_2);
                cards.Add((int)GameCommon.mjCards.B_3);
                cards.Add((int)GameCommon.mjCards.B_4);
                cards.Add((int)GameCommon.mjCards.B_5);
                cards.Add((int)GameCommon.mjCards.B_6);
                cards.Add((int)GameCommon.mjCards.B_7);
                cards.Add((int)GameCommon.mjCards.B_8);
                cards.Add((int)GameCommon.mjCards.B_9);

                cards.Add((int)GameCommon.mjCards.W_1);
                cards.Add((int)GameCommon.mjCards.W_2);
                cards.Add((int)GameCommon.mjCards.W_3);
                cards.Add((int)GameCommon.mjCards.W_4);
                cards.Add((int)GameCommon.mjCards.W_5);
                cards.Add((int)GameCommon.mjCards.W_6);
                cards.Add((int)GameCommon.mjCards.W_7);
                cards.Add((int)GameCommon.mjCards.W_8);
                cards.Add((int)GameCommon.mjCards.W_9);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end InitCards");
        }
    }

    public class ThreeInitCards : IInitCards
    {
        public void InitCards(List<Int64> cards)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin InitCards");

            cards.Clear();
            for (int i = 0; i < 4; i++)
            {
                cards.Add((int)GameCommon.mjCards.B_1);
                cards.Add((int)GameCommon.mjCards.B_2);
                cards.Add((int)GameCommon.mjCards.B_3);
                cards.Add((int)GameCommon.mjCards.B_4);
                cards.Add((int)GameCommon.mjCards.B_5);
                cards.Add((int)GameCommon.mjCards.B_6);
                cards.Add((int)GameCommon.mjCards.B_7);
                cards.Add((int)GameCommon.mjCards.B_8);
                cards.Add((int)GameCommon.mjCards.B_9);

                cards.Add((int)GameCommon.mjCards.W_1);
                cards.Add((int)GameCommon.mjCards.W_2);
                cards.Add((int)GameCommon.mjCards.W_3);
                cards.Add((int)GameCommon.mjCards.W_4);
                cards.Add((int)GameCommon.mjCards.W_5);
                cards.Add((int)GameCommon.mjCards.W_6);
                cards.Add((int)GameCommon.mjCards.W_7);
                cards.Add((int)GameCommon.mjCards.W_8);
                cards.Add((int)GameCommon.mjCards.W_9);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end InitCards");
        }
    }

    public class FourInitCards : IInitCards
    {
        public void InitCards(List<Int64> cards)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin InitCards");

            cards.Clear();
            for (int i = 0; i < 4; i++)
            {
                cards.Add((int)GameCommon.mjCards.W_1);
                cards.Add((int)GameCommon.mjCards.W_2);
                cards.Add((int)GameCommon.mjCards.W_3);
                cards.Add((int)GameCommon.mjCards.W_4);
                cards.Add((int)GameCommon.mjCards.W_5);
                cards.Add((int)GameCommon.mjCards.W_6);
                cards.Add((int)GameCommon.mjCards.W_7);
                cards.Add((int)GameCommon.mjCards.W_8);
                cards.Add((int)GameCommon.mjCards.W_9);

                cards.Add((int)GameCommon.mjCards.B_1);
                cards.Add((int)GameCommon.mjCards.B_2);
                cards.Add((int)GameCommon.mjCards.B_3);
                cards.Add((int)GameCommon.mjCards.B_4);
                cards.Add((int)GameCommon.mjCards.B_5);
                cards.Add((int)GameCommon.mjCards.B_6);
                cards.Add((int)GameCommon.mjCards.B_7);
                cards.Add((int)GameCommon.mjCards.B_8);
                cards.Add((int)GameCommon.mjCards.B_9);

                cards.Add((int)GameCommon.mjCards.T_1);
                cards.Add((int)GameCommon.mjCards.T_2);
                cards.Add((int)GameCommon.mjCards.T_3);
                cards.Add((int)GameCommon.mjCards.T_4);
                cards.Add((int)GameCommon.mjCards.T_5);
                cards.Add((int)GameCommon.mjCards.T_6);
                cards.Add((int)GameCommon.mjCards.T_7);
                cards.Add((int)GameCommon.mjCards.T_8);
                cards.Add((int)GameCommon.mjCards.T_9);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end InitCards");
        }
    }

    public class InitCards
    {
        static public IInitCards GetInst(GameCommon.PeopleNum num)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "call GetInst");

            switch (num)
            {
            case GameCommon.PeopleNum.TwoPeople:
                return new TwoInitCards();
            case GameCommon.PeopleNum.ThreePeople:
                return new ThreeInitCards();
            case GameCommon.PeopleNum.FourPeople:
                return new FourInitCards();
            default:
                return null; 
            }
        }
    }
}
