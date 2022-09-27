using System;
using System.Collections;
using System.Collections.Generic;

namespace room
{
    enum hh_state
    {
        none,
        peng,
        gang,
        hu,
        dian_hu,
    }

    class mj_huanghuang_table : table
    {
        public mj_huanghuang_table()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin mj_huanghuang_table");

            player_cards = new Dictionary<Int64, ArrayList>();
            for(int i = (int)GameCommon.mjSite.One ; i <= (int)GameCommon.mjSite.Four; i++)
            {
                player_cards.Add(i, new ArrayList() );
            }
            player_play_cards = new Dictionary<long, ArrayList>();
            for (int i = (int)GameCommon.mjSite.One; i <= (int)GameCommon.mjSite.Four; i++)
            {
                player_play_cards.Add(i, new ArrayList());
            }
            player_peng = new Dictionary<long, ArrayList>();
            for (int i = (int)GameCommon.mjSite.One; i <= (int)GameCommon.mjSite.Four; i++)
            {
                player_peng.Add(i, new ArrayList());
            }
            player_gang = new Dictionary<long, ArrayList>();
            for (int i = (int)GameCommon.mjSite.One; i <= (int)GameCommon.mjSite.Four; i++)
            {
                player_gang.Add(i, new ArrayList());
            }

            gang_state = new Hashtable();
            cards = new List<Int64>();

            config_draw_index = 0;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end mj_huanghuang_table");
        }

        public new void clean()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin clean");

            ((table)this).clean();

            for (int i = (int)GameCommon.mjSite.One; i <= (int)GameCommon.mjSite.Four; i++)
            {
                player_cards[i].Clear();
                player_play_cards[i].Clear();
                player_peng[i].Clear();
                player_gang[i].Clear();
            }
            cards.Clear();
            gang_state.Clear();

            play_count = 0;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end clean");
        }

        public void frist_begin_game()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin frist_begin_game");

            card_righter = (Int64)GameCommon.mjSite.One;
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "2:start game !");
            foreach (var item in site)
            {
                item.Value.state = GameCommon.mjPlayerstate.in_game;

                item.Value.player_info["base"] = (Int64)1;
                item.Value.player_info["score"] = (Int64)0;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end frist_begin_game");
        }

        public void begin_game()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin begin_game");

            zhuang = card_righter;
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "3:start game !");
            foreach (var item in site)
            {
                item.Value.state = GameCommon.mjPlayerstate.in_game;

                item.Value.player_info["base"] = (Int64)1;
            }

            for (int i = (int)GameCommon.mjSite.One; i <= (int)GameCommon.mjSite.Four; i++)
            {
                player_cards[i].Clear();
                player_play_cards[i].Clear();
                player_peng[i].Clear();
                player_gang[i].Clear();
            }
            gang_state.Clear();
            cards.Clear();
            play_count++;
            in_game = true;
            config_draw_index = 0;

            is_hu = false;

            mopai_state = false;
            play_state = false;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end begin_game");
        }

        public void end_game()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin end_game");

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "play_count:{0}", play_count);
            if (play_count == 1)
            {
                //扣费 
                if (payRule == (Int64)GameCommon.PayRule.AAPay)
                {
                    foreach (var item in players)
                    {
                        hub.hub.hubs.call_hub("lobby", "lobby", "on_pay_diamond", item.Key, times, payRule, peopleNum);
                    }
                }
                else if (payRule == (Int64)GameCommon.PayRule.OnePay)
                {
                    hub.hub.hubs.call_hub("lobby", "lobby", "on_pay_diamond", owner, times, payRule, peopleNum);
                }
            }

            if (payRule == (Int64)GameCommon.PayRule.MatchPay)
            {
                foreach (var item in players)
                {
                    if (item.Value.frist_join_match && !item.Value.is_robot)
                    {
                        hub.hub.hubs.call_hub("lobby", "lobby", "on_begin_match_game", item.Key);

                        item.Value.frist_join_match = false;
                    }
                }
            }

            foreach (var item in site)
            {
                item.Value.state = GameCommon.mjPlayerstate.none;
            }

            do
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "play_count:{0}, times:{1}", play_count, times);
                if (times < 0)
                {
                    break;
                }
                if (play_count >= times)
                {
                    //解散房间表明一轮结束，计算输赢,桌上玩家游戏任务加一
                    foreach (var item in site)
                    {
                        if ((Int64)item.Value.player_info["score"] > 0 && item.Value.is_robot != true)
                        {
                            hub.hub.hubs.call_hub("lobby", "lobby", "get_victory_count", item.Value.player_info["unionid"]);
                        }
                        if (item.Value.is_robot != true)
                        {
                            hub.hub.hubs.call_hub("lobby", "lobby", "get_game_count", item.Value.player_info["unionid"]);
                        }
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "mark");
                    }
                    broadcast("mj_huanghuang", "end_game");
                    end_game_disband();
                }
            } while (false);

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end end_game");
        }

        public void deal_laiyou()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin deal_laiyou");

            if (hub.hub.config.has_key("robot_deal_list"))
            {
                InitCards.GetInst((GameCommon.PeopleNum)peopleNum).InitCards(cards);

                var robot_deal_list = hub.hub.config.get_value_list("robot_deal_list");
                Int64 card_people = card_righter;
                for (int i = 0; i < robot_deal_list.get_list_size(); i++)
                {
                    var pai_list = robot_deal_list.get_list_list(i);
                    for (int j = 0; j < pai_list.get_list_size(); j++)
                    {
                        Int64 c = pai_list.get_list_int(j);
                        player_cards[card_people].Add(c);
                        cards.Remove(c);
                    }

                    card_people = (Int64)GetNextSite((GameCommon.mjSite)card_people);
                }
            }
            else
            {
                deal_normal();
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end deal_laiyou");
        }

        public void deal_normal()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin deal_normal");

            InitCards.GetInst((GameCommon.PeopleNum)peopleNum).InitCards(cards);

            Random ra = new Random();
            int r = ra.Next();
            int index = 0;

            for (int i = 0; i < 13; i++)
            {
                Int64 card_people = card_righter;
                for (int n = 0; n < peopleNum; n++)
                {
                    index = r % cards.Count;
                    player_cards[card_people].Add(cards[index]);
                    cards.RemoveAt(index);

                    card_people = (Int64)GetNextSite((GameCommon.mjSite)card_people);
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end deal_normal");
        }

        public void laizi_laiyou()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin laizi_laiyou");

            if (hub.hub.config.has_key("laizi"))
            {
                laizi = hub.hub.config.get_value_int("laizi");
                laizipi = laizi - 1;
                if (laizipi == 0)
                {
                    laizipi = (int)GameCommon.mjCards.W_9;
                }
                else if (laizipi == 10)
                {
                    laizipi = (int)GameCommon.mjCards.B_9;
                }
                else if (laizipi == 20)
                {
                    laizipi = (int)GameCommon.mjCards.T_9;
                }

                cards.Remove(laizipi);
            }
            else
            {
                laizi_normal();
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end laizi_laiyou");
        }

        public void laizi_normal()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin laizi_laiyou");

            //癞子
            Random ra = new Random();
            int r = ra.Next();
            int index = r % cards.Count;
            laizipi = cards[index];
            cards.RemoveAt(index);

            laizi = laizipi + 1;
            if (laizi == 10)
            {
                laizi = (int)GameCommon.mjCards.W_1;
            }
            else if (laizi == 20)
            {
                laizi = (int)GameCommon.mjCards.B_1;
            }
            else if (laizi == 30)
            {
                laizi = (int)GameCommon.mjCards.T_1;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end laizi_laiyou");
        }

        //发13张牌
        public void deal()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin deal");

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "3:start game !");
            if (!is_robot_room)
            {
                deal_normal();
            }
            else
            {
                deal_laiyou();
            }

            //第一个玩家的第14张牌
            Random ra = new Random();
            int r = ra.Next();
            int index = r % cards.Count;
            Int64 card = cards[index];
            cards.RemoveAt(index);

            if (!is_robot_room)
            {
                laizi_normal();
            }
            else
            {
                laizi_laiyou();
            }

            int numa = ra.Next(1, 7);
            int numb = ra.Next(1, 7);
            r_saizi = numa*10+numb;
            
            foreach(var player in site)
            {
                if (!player.Value.is_robot)
                {
                    hub.hub.gates.call_client(player.Value.uuid, "mj_huanghuang", "deal", zhuang, r_saizi, player_cards[player.Key]);
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "draw");

            mopai = card;
            mopai_state = true;

            player_cards[card_righter].Add(card);
            if (!site[card_righter].is_robot)
            {
                hub.hub.gates.call_client(site[card_righter].uuid, "mj_huanghuang", "draw", card);
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "draw_pai");
            broadcast("mj_huanghuang", "lazi", laizipi, laizi);
            broadcast("mj_huanghuang", "draw_pai", card_righter);
            
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "right");
            if (site[card_righter].is_robot)
            {
                _hh_state = hh_state.none;
                robotUtil.onRobot(this, card_righter);
            }
            else
            {
                foreach(Int64 c in player_cards[card_righter])
                {
                    processer = (int)GameCommon.mjSite.End;
                    if (mj_huanghuang_check.check_gang(player_cards[card_righter], player_peng[card_righter], laizipi, c, true, false))
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gang");
                        processer_card = c;
                        processer = card_righter;
                        _hh_state = hh_state.gang;
                        break;
                    }
                }

                if (processer == card_righter)
                {
                    //broadcast("mj_huanghuang", "processer", processer);
                }
                else
                {
                    broadcast("mj_huanghuang", "right", card_righter);
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end deal");
        }

        public Int64 draw_laiyou()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin draw_laiyou");

            if (hub.hub.config.has_key("robot_draw_list"))
            {
                if (!site[card_righter].is_robot)
                {
                    var draw_list = hub.hub.config.get_value_list("robot_draw_list");

                    if (config_draw_index < draw_list.get_list_size())
                    {
                        Int64 c = draw_list.get_list_int(config_draw_index++);

                        if (cards.Contains(c))
                        {
                            cards.Remove(c);
                            return c;
                        }
                    }
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end draw_laiyou");

            return draw_normal();
        }

        public Int64 draw_normal()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin draw_normal");

            Random ra = new Random();
            int r = ra.Next();
            if (cards.Count != 0)
            {
                int index = r % cards.Count;
                Int64 c = cards[index];
                cards.RemoveAt(index);
                return c;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end draw_normal");

            return 0;
        }

        public void draw()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin draw");

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "card_righter cards count:{0}", player_cards[card_righter].Count);

            if (cards.Count == 0)
            {
                broadcast("mj_huanghuang", "liu_ju");

                ArrayList other_pai = new ArrayList();
                Int64 _player = card_righter;
                do
                {
                    Hashtable pai_info = new Hashtable();
                    pai_info.Add("player", _player);
                    pai_info.Add("pai", player_cards[_player]);
                    other_pai.Add(pai_info);

                    _player = (Int64)GetNextSite((GameCommon.mjSite)_player);

                } while (_player != card_righter);

                broadcast("mj_huanghuang", "otherpai", other_pai);

                end_game();

                return;
            }

            foreach (Int64 card in player_cards[card_righter])
            {
                processer = (Int64)GetEndSite();
                if (mj_huanghuang_check.check_gang(player_cards[card_righter], player_peng[card_righter], laizipi, card, false, false))
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gang");
                    processer_card = card;
                    processer = card_righter;
                    _hh_state = hh_state.gang;
                    break;
                }
            }
            
            Int64 c = 0;
            if (!is_robot_room)
            {
                c = draw_normal();
            }
            else
            {
                c = draw_laiyou();
            }
            player_cards[card_righter].Add(c);
            mopai = c;
            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "摸牌：{0}",mopai);

            if (mj_huanghuang_check.check_gang(player_cards[card_righter], player_peng[card_righter], laizipi, c, true, false))
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "gang");
                processer_card = c;
                processer = card_righter;
                _hh_state = hh_state.gang;
            }

            HupaiState state = mj_huanghuang_check.check_hu(player_cards[card_righter], laizi, mopai);
            if (state != HupaiState.no_hu)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "hu");
                processer_card = c;
                processer = card_righter;
                _hh_state = hh_state.hu;
            }

            if (!site[card_righter].is_robot)
            {
                hub.hub.gates.call_client(site[card_righter].uuid, "mj_huanghuang", "draw", c);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "draw_pai:{0}, cards count:{1}", card_righter, player_cards[card_righter].Count);
            broadcast("mj_huanghuang", "draw_pai", card_righter);

            mopai_state = true;
            play_state = false;

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "right:{0}", card_righter);
            if (site[card_righter].is_robot)
            {
                if (_hh_state != hh_state.hu)
                {
                    _hh_state = hh_state.none;
                }
                robotUtil.onRobot(this, card_righter);
            }
            else
            {
                if (processer == card_righter)
                {
                    //broadcast("mj_huanghuang", "processer", processer);
                }
                broadcast("mj_huanghuang", "right", card_righter);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end draw");
        }

        public void last_card()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin last_card");

            var c = draw_normal();
            if (c != 0)
            {
                broadcast("mj_huanghuang", "last_card", c);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end last_card");
        }
        
        public new Hashtable get_room_info()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin get_room_info");

            Hashtable info = ((table)this).get_room_info();
            Hashtable playerHandCardCount = new Hashtable();
            if (card_righter != (Int64)GetEndSite())
            {
                info.Add("card_righter", card_righter);
            }
            if (play_state)
            {
                info.Add("play_card", processer_card);
                info.Add("play_card_player", play_card_player);
            }

            for (int i = 1; i < peopleNum+1; i++)
            {
                playerHandCardCount.Add(i.ToString(), player_cards[i].Count);
            }
            info.Add("playerCardNum", playerHandCardCount);
            info.Add("laizi", laizi);
            info.Add("r_saizi", r_saizi);
            Hashtable _player_play_cards = new Hashtable();
            foreach(var item in player_play_cards)
            {
                _player_play_cards.Add(item.Key.ToString(), item.Value);
            }
            info.Add("player_play_cards", _player_play_cards);

            Hashtable _player_peng = new Hashtable();
            foreach(var item in player_peng)
            {
                _player_peng.Add(item.Key.ToString(), item.Value);
            }
            info.Add("player_peng", _player_peng);
            info.Add("gang_state", gang_state);

            Hashtable _player_gang = new Hashtable();
            foreach(var item in player_gang)
            {
                _player_gang.Add(item.Key.ToString(), item.Value);
            }
            info.Add("player_gang", _player_gang);
            
            info.Add("cards_count", cards.Count);

            info.Add("zhuang", zhuang);

            if (is_hu)
            {
                info.Add("is_hu", is_hu);

                info.Add("hu_player", hu_player);
                info.Add("hu_card", player_cards[hu_player]);

                ArrayList other_pai = new ArrayList();
                Int64 _player = (Int64)GetNextSite((GameCommon.mjSite)hu_player);     
                while (_player != hu_player)
                {
                    Hashtable pai_info = new Hashtable();
                    pai_info.Add("player", _player);
                    pai_info.Add("pai", player_cards[_player]);
                    other_pai.Add(pai_info);

                    _player = (Int64)GetNextSite((GameCommon.mjSite)_player);
                }
                info.Add("other_card", other_pai);
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end get_room_info");

            return info;
        }

        public GameCommon.mjSite GetNextSite(GameCommon.mjSite site)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin GetNextSite");

            site++;
            if (peopleNum == (Int64)GameCommon.PeopleNum.TwoPeople)
            {
                if (site == GameCommon.mjSite.Three)
                {
                    site = GameCommon.mjSite.One;
                }
            }
            else if (peopleNum == (Int64)GameCommon.PeopleNum.ThreePeople)
            {
                if (site == GameCommon.mjSite.Four)
                {
                    site = GameCommon.mjSite.One;
                }
            }
            else if (peopleNum == (Int64)GameCommon.PeopleNum.FourPeople)
            {
                if (site == GameCommon.mjSite.End)
                {
                    site = GameCommon.mjSite.One;
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end GetNextSite");

            return site;
        }

        public GameCommon.mjSite GetEndSite()
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin GetEndSite");

            if (peopleNum == (Int64)GameCommon.PeopleNum.TwoPeople)
            {
                return GameCommon.mjSite.Three;
            }
            else if (peopleNum == (Int64)GameCommon.PeopleNum.ThreePeople)
            {
                return GameCommon.mjSite.Four;
            }
            else if (peopleNum == (Int64)GameCommon.PeopleNum.TwoPeople)
            {
                return GameCommon.mjSite.Three;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end GetEndSite");

            return GameCommon.mjSite.End;
        }

        public bool voting = false;
        public bool is_hu;
        public Int64 hu_player;

        public Int64 zhuang;

        public Int64 card_righter;
        public Int64 next_card_righter;

        public Int64 play_carder;
        public hh_state _hh_state;//保留
        public Int64 processer;
        public Int64 processer_card;

        public Int64 play_card_player;
        public Int64 r_saizi;
        public Int64 laizipi;
        public Int64 laizi;
        public Int64 mopai;
        public Dictionary<Int64, ArrayList > player_cards;//玩家手牌
        public Dictionary<Int64, ArrayList> player_play_cards;
        public Dictionary<Int64, ArrayList> player_peng;
        public Dictionary<Int64, ArrayList> player_gang;

        public Hashtable gang_state;

        public List<Int64> cards;

        private int config_draw_index;

        public bool mopai_state;
        public bool play_state;
    }
}
