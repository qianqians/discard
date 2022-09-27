using System;
using System.Collections;
using common;

namespace room
{
    class mj_huanghuang : imodule
    {
        public void play(Int64 room_id, Int64 card)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin play");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);
            if (_table == null)
            {
                return;
            }

            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = _table.get_player_proxy(client_uuid);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player proxy is null");
                return;
            }
                
            if ((Int64)_proxy.player_info["site"] != _table.card_righter)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player not card righter");
                return;
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "check player site");

            if (!_table.player_cards[_table.card_righter].Contains(card))
            {
                log.log.error(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "no exist cards:{0}", card);
                return;
            }

            _table.play_card_player = (Int64)_proxy.player_info["site"];

            _table.mopai_state = false;
            _table.play_state = true;

            _table.player_play_cards[_table.card_righter].Add(card);

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "play card_righter cards count:{0}", _table.player_cards[_table.card_righter].Count);

            if (card == _table.laizi)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "laizi draw card");

                _table.player_cards[_table.card_righter].Remove(card);

                _table.broadcast("mj_huanghuang", "play_card", card);

                _proxy.player_info["base"] = (Int64)_proxy.player_info["base"] * 2;
                _table.draw();
            }
            else
            {
                _table.player_cards[_table.card_righter].Remove(card);

                _table.broadcast("mj_huanghuang", "play_card", card);

                _table.processer_card = card;
                _table.play_carder = _table.card_righter;

                _table.processer = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                while (_table.processer != _table.card_righter)
                {

                    if (mj_huanghuang_check.check_peng(_table.player_cards[_table.processer], _table.laizipi, card))
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "peng");

                        _table.next_card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                        _table.card_righter = (Int64)_table.GetEndSite();

                        _table._hh_state = hh_state.peng;

                        if (_table.site[_table.processer].is_robot)
                        {
                            robotUtil.onRobot(_table, _table.processer);
                        }
                        return;
                    }

                    if (mj_huanghuang_check.check_gang(_table.player_cards[_table.processer], _table.player_peng[_table.processer], _table.laizipi, card, false, true))
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gang");

                        _table.next_card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                        _table.card_righter = (Int64)_table.GetEndSite();

                        _table._hh_state = hh_state.gang;

                        if (_table.site[_table.processer].is_robot)
                        {
                            robotUtil.onRobot(_table, _table.processer);
                        }
                        return;
                    }

                    if (mj_huanghuang_check.check_dian_hu(_table.player_cards[_table.processer], _table.laizi, card) != HupaiState.no_hu)
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "hu");

                        _table.next_card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                        _table.card_righter = (Int64)_table.GetEndSite();

                        _table._hh_state = hh_state.dian_hu;

                        if (_table.site[_table.processer].is_robot)
                        {
                            robotUtil.onRobot(_table, _table.processer);
                        }
                        return;
                    }

                    _table.processer = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.processer);
                }


                if (_table.processer == _table.card_righter)
                {
                    _table.card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);

                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "draw card to next card_righter");
                    _table.draw();
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end play");
        }

        public void guo(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin guo");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);

            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = _table.get_player_proxy(client_uuid);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player proxy is null");
                return;
            }
            if ((Int64)_proxy.player_info["site"] != _table.processer)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player not processer");
                return;
            }

            if (_table.processer == _table.card_righter)
            {
                _table.broadcast("mj_huanghuang", "right", _table.card_righter);
            }
            else
            {
                _table.card_righter = _table.next_card_righter;
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "guo");
                _table.draw();
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end guo");
        }

        public void peng(Int64 room_id, Int64 card)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin peng");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);

            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = _table.get_player_proxy(client_uuid);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player proxy is null");
                return;
            }
            if ((Int64)_proxy.player_info["site"] != _table.processer)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player not processer {0},{1}", (Int64)_proxy.player_info["site"], _table.processer);
                return;
            }

            bool ret = mj_huanghuang_check.check_peng(_table.player_cards[_table.processer], _table.laizipi, card);
            if (!ret)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "can not peng {0}, {1}, {2}", ret, _table.processer, card);
                return;
            }

            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "broadcast peng {0}, {1}", ret, _table.processer);

            for (int i = 0; i < 2; i++)
            {
                _table.player_cards[_table.processer].Remove(card);
            }
            _table.player_peng[_table.processer].Add(card);

            _table.broadcast("mj_huanghuang", "pengpai", _table.processer, card);
            _table.card_righter = _table.processer;
            _table.broadcast("mj_huanghuang", "right", _table.card_righter);

            _table.processer_card = 0;
            _table.play_state = false;

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end peng");
        }

        public void gang(Int64 room_id, Int64 card)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin gang");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);

            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = _table.get_player_proxy(client_uuid);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player proxy is null");
                return;
            }
            if ((Int64)_proxy.player_info["site"] != _table.processer)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player not processer :{0}", _table.processer);
                return;
            }

            if (_table.card_righter == _table.processer)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "check zi mo");
                if (mj_huanghuang_check.check_gang(_table.player_cards[_table.processer], _table.player_peng[_table.processer], _table.laizipi, card, true, false))
                {
                    if (_table.player_peng[_table.processer].Contains(card))
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "hui tou xiao");

                        _table.gang_state.Add(card.ToString(), (Int64)GameCommon.mjGangstate.hui_tou_xiao);
                        //回头笑
                        Hashtable score = new Hashtable();
                        score[_table.site[_table.processer].player_info["unionid"]] = (Int64)0;

                        Int64 player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.processer);
                        while (player != _table.processer)
                        {
                            _table.site[player].player_info["score"] = (Int64)_table.site[player].player_info["score"] - 1 * _table.score;
                            _table.site[_table.processer].player_info["score"] = (Int64)_table.site[_table.processer].player_info["score"] + 1 * _table.score;

                            score[_table.site[player].player_info["unionid"]] = (Int64)(0 - 1 * _table.score);
                            if (score[_table.site[_table.processer].player_info["unionid"]] == null)
                            {
                                score[_table.site[_table.processer].player_info["unionid"]] = (Int64)0;
                            }
                            score[_table.site[_table.processer].player_info["unionid"]] = (Int64)score[_table.site[_table.processer].player_info["unionid"]] + 1 * _table.score;

                            player = (Int64)_table.GetNextSite((GameCommon.mjSite)player);
                        }
                        _table.broadcast("mj_huanghuang", "gangscore", score);
                        _table.broadcast("mj_huanghuang", "gangpai", _table.processer, card, (Int64)GameCommon.mjGangstate.hui_tou_xiao);

                        _table.player_peng[_table.processer].Remove(card);
                        _table.player_gang[_table.processer].Add(card);

                        _table.player_cards[_table.processer].Remove(card);

                        _table.card_righter = _table.processer;
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "hui tou xiao");
                        _table.draw();
                    }
                    else
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "zimo gang");

                        _table.gang_state.Add(card.ToString(), (Int64)GameCommon.mjGangstate.zi_xiao);
                        //自摸
                        Hashtable score = new Hashtable();
                        score[_table.site[_table.processer].player_info["unionid"]] = (Int64)0;

                        Int64 player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.processer);
                        while (player != _table.processer)
                        {
                            _table.site[player].player_info["score"] = (Int64)_table.site[player].player_info["score"] - 2 * _table.score;
                            _table.site[_table.processer].player_info["score"] = (Int64)_table.site[_table.processer].player_info["score"] + 2 * _table.score;

                            score[_table.site[player].player_info["unionid"]] = 0 - 2 * _table.score;
                            if (score[_table.site[_table.processer].player_info["unionid"]] == null)
                            {
                                score[_table.site[_table.processer].player_info["unionid"]] = (Int64)0;
                            }
                            score[_table.site[_table.processer].player_info["unionid"]] = (Int64)score[_table.site[_table.processer].player_info["unionid"]] + 2 * _table.score;

                            player = (Int64)_table.GetNextSite((GameCommon.mjSite)player);
                        }

                        _table.broadcast("mj_huanghuang", "gangscore", score);

                        _table.broadcast("mj_huanghuang", "gangpai", _table.processer, card, (Int64)GameCommon.mjGangstate.zi_xiao);

                        _table.player_gang[_table.processer].Add(card);
                       
                        _table.card_righter = _table.processer;
                        if (_table.laizipi != card)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                _table.player_cards[_table.processer].Remove(card);
                            }

                            log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gang");
                            _table.draw();
                        }
                        else
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                _table.player_cards[_table.processer].Remove(card);
                            }

                            _table.broadcast("mj_huanghuang", "right", _table.card_righter);
                        }
                    }
                    
                    _table.processer_card = 0;
                    _table.play_state = false;
                    _table.mopai_state = false;
                }
            }
            else
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "gang");
                if (!mj_huanghuang_check.check_gang(_table.player_cards[_table.processer], _table.player_peng[_table.processer], _table.laizipi, card, false, true))
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "can not gang");
                    return;
                }
                
                if (card != _table.processer_card)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "error input");
                    return;
                }

                _table.gang_state.Add(card.ToString(), (Int64)GameCommon.mjGangstate.gang);

                //别人打的
                Hashtable score = new Hashtable();

                _table.site[_table.processer].player_info["score"] = (Int64)_table.site[_table.processer].player_info["score"] + (_table.peopleNum - 1) * _table.score;
                _table.site[_table.play_carder].player_info["score"] = (Int64)_table.site[_table.play_carder].player_info["score"] - (_table.peopleNum - 1) * _table.score;

                score[_table.site[_table.processer].player_info["unionid"]] = (Int64)((_table.peopleNum - 1) * _table.score);
                score[_table.site[_table.play_carder].player_info["unionid"]] = (Int64)(0 - (_table.peopleNum - 1) * _table.score);

                _table.broadcast("mj_huanghuang", "gangscore", score);

                _table.broadcast("mj_huanghuang", "gangpai", _table.processer, card, (Int64)GameCommon.mjGangstate.gang);
                _table.card_righter = _table.processer;
                _table.player_gang[_table.processer].Add(card);
                if (_table.laizipi != card)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        _table.player_cards[_table.processer].Remove(card);
                    }

                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "gang from other");
                    _table.draw();
                }
                else
                {
                    for (int i = 0; i < 3; i++)
                    {
                        _table.player_cards[_table.processer].Remove(card);
                    }

                    _table.broadcast("mj_huanghuang", "right", _table.card_righter);
                }
                _table.processer_card = 0;
                _table.play_state = false;
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end gang");
        }

        public void hu(Int64 room_id, Int64 pai)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin hu");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);

            var client_uuid = hub.hub.gates.current_client_uuid;
            var _proxy = _table.get_player_proxy(client_uuid);
            if (_proxy == null)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "player proxy is null");
                return;
            }
            if (_table.is_hu)
            {
                return;
            }

            if ((Int64)_proxy.player_info["site"] == _table.card_righter)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick,  "check hu {0}", pai);
                HupaiState state = mj_huanghuang_check.check_hu(_table.player_cards[_table.card_righter], _table.laizi, pai);
                if (state == HupaiState.no_hu)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "can not hu");
                    return;
                }

                _table.last_card();

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "begin score");
                int rate = 1;
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "rate{0}", rate);
                if (state == HupaiState.hard_hu)
                {
                    rate *= 2;
                    log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "hard_hu rate{0}", rate);
                }

                Hashtable score = new Hashtable();
                Int64 player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                while (player != _table.card_righter)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "rate{0} player{1} card_righter{2}", rate, (Int64)_table.site[player].player_info["base"], (Int64)_table.site[_table.card_righter].player_info["base"]);

                    Int64 tmp_score = (Int64)_table.site[player].player_info["base"] * (Int64)_table.site[_table.card_righter].player_info["base"] * _table.score * rate;
                    _table.site[player].player_info["score"] = (Int64)_table.site[player].player_info["score"] - tmp_score;
                    _table.site[_table.card_righter].player_info["score"] = (Int64)_table.site[_table.card_righter].player_info["score"] + tmp_score;

                    score[_table.site[player].player_info["unionid"]] = 0 - tmp_score;
                    if (score[_table.site[_table.card_righter].player_info["unionid"]] == null)
                    {
                        score[_table.site[_table.card_righter].player_info["unionid"]] = (Int64)0;
                    }
                    score[_table.site[_table.card_righter].player_info["unionid"]] = (Int64)score[_table.site[_table.card_righter].player_info["unionid"]] + tmp_score;

                    player = (Int64)_table.GetNextSite((GameCommon.mjSite)player);
                }
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "end score ：{0}", Json.Jsonparser.pack((Hashtable)score));

                if (_table.payRule == (Int64)GameCommon.PayRule.MatchPay)
                {
                    foreach (DictionaryEntry k_v in score)
                    {
                        hub.hub.hubs.call_hub("lobby", "match", "wind_up", hub.hub.name, k_v.Key, k_v.Value);
                    }
                }

                _table.broadcast("mj_huanghuang", "hupai", _table.player_cards[_table.card_righter], pai, _table.card_righter);

                ArrayList other_pai = new ArrayList();
                Int64 _player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                while(_player != _table.card_righter)
                {
                    Hashtable pai_info = new Hashtable();
                    pai_info.Add("player", _player);
                    pai_info.Add("pai", _table.player_cards[_player]);
                    other_pai.Add(pai_info);
                
                    _player = (Int64)_table.GetNextSite((GameCommon.mjSite)_player);
                }
                _table.broadcast("mj_huanghuang", "otherpai", other_pai);

                _table.broadcast("mj_huanghuang", "huscore", score);

                _table.is_hu = true;
                _table.hu_player = _table.card_righter;
                _table.processer_card = 0;
                _table.end_game();
            }
            else if ((Int64)_proxy.player_info["site"] == _table.processer)
            {
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "check hu {0}", pai);
                HupaiState state = mj_huanghuang_check.check_dian_hu(_table.player_cards[_table.processer], _table.laizi, pai);
                if (state == HupaiState.no_hu)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "can not hu");
                    return;
                }

                _table.last_card();

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "begin score");
                int rate = 1;
                log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "rate{0}", rate);
                if (state == HupaiState.hard_hu)
                {
                    rate *= 2;
                    log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "hard_hu rate{0}", rate);
                }

                Hashtable score = new Hashtable();

                Int64 tmp_score = (Int64)_table.site[_table.play_carder].player_info["base"] * (Int64)_table.site[_table.processer].player_info["base"] * _table.score * rate;
                _table.site[_table.play_carder].player_info["score"] = (Int64)_table.site[_table.play_carder].player_info["score"] - tmp_score;
                _table.site[_table.processer].player_info["score"] = (Int64)_table.site[_table.processer].player_info["score"] + tmp_score;

                score[_table.site[_table.play_carder].player_info["unionid"]] = 0 - tmp_score;
                score[_table.site[_table.processer].player_info["unionid"]] = tmp_score;

                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "end score ：{0}", Json.Jsonparser.pack((Hashtable)score));

                if (_table.payRule == (Int64)GameCommon.PayRule.MatchPay)
                {
                    foreach (DictionaryEntry k_v in score)
                    {
                        hub.hub.hubs.call_hub("lobby", "match", "wind_up", hub.hub.name, k_v.Key, k_v.Value);
                    }
                }

                _table.broadcast("mj_huanghuang", "hupai", _table.player_cards[_table.processer], pai, _table.processer);

                ArrayList other_pai = new ArrayList();
                Int64 _player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.processer);
                while (_player != _table.processer)
                {
                    Hashtable pai_info = new Hashtable();
                    pai_info.Add("player", _player);
                    pai_info.Add("pai", _table.player_cards[_player]);
                    other_pai.Add(pai_info);

                    _player = (Int64)_table.GetNextSite((GameCommon.mjSite)_player);
                }
                _table.broadcast("mj_huanghuang", "otherpai", other_pai);

                _table.broadcast("mj_huanghuang", "huscore", score);

                _table.is_hu = true;
                _table.hu_player = _table.processer;
                _table.card_righter = _table.processer;
                _table.processer_card = 0;
                _table.end_game();
            }
            
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end hu");
        }

        public void read(Int64 room_id)
        {
            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "begin read");

            mj_huanghuang_table _table = server.tables.get_mj_huanghuang_table(room_id);

            if (_table.play_count < _table.times || _table.times < 0)
            {
                var client_uuid = hub.hub.gates.current_client_uuid;
                var _proxy = _table.get_player_proxy(client_uuid);
                _proxy.state = GameCommon.mjPlayerstate.read;

                bool is_read = true;
                GameCommon.mjSite site = GameCommon.mjSite.One;
                for ( int i = 0; i < _table.peopleNum; i++ )
                {
                    if (_table.site[(int)site].is_robot)
                    {
                        _table.site[(int)site].state = GameCommon.mjPlayerstate.read;
                    }

                    if (_table.site[(int)site].state != GameCommon.mjPlayerstate.read)
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "site:{0}", site);
                        is_read = false;
                        break;
                    }

                    site = _table.GetNextSite(site);
                }
                if (is_read)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "new game");
                    _table.begin_game();
                    _table.deal();
                }
                else
                {
                    _table.broadcast("mj_huanghuang", "read", _proxy.player_info["site"]);
                }
            }

            log.log.trace(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "end read");
        }
    }
}
