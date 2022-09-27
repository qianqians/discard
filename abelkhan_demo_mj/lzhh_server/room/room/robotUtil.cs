using System;
using System.Collections;

namespace room
{
    class robotUtil
    {
        public static void onRobot(mj_huanghuang_table _table, Int64 site)
        {
            if (!_table.site[site].is_robot)
            {
                return;
            }

            if (_table._hh_state == hh_state.peng)
            {
                if (site != _table.processer)
                {
                    return;
                }

                for (int i = 0; i < 2; i++)
                {
                    _table.player_cards[_table.processer].Remove(_table.processer_card);
                }
                _table.player_peng[_table.processer].Add(_table.processer_card);

                _table.broadcast("mj_huanghuang", "pengpai", _table.processer, _table.processer_card);
                _table.card_righter = _table.processer;
                _table.broadcast("mj_huanghuang", "right", _table.card_righter);

                _table.processer_card = 0;
                _table.play_state = false;

                robotPlay(_table, site);
            }
            else if (_table._hh_state == hh_state.gang)
            {
                if (site != _table.processer)
                {
                    return;
                }

                if (_table.processer == _table.card_righter)
                {
                    return;
                }

                if (_table.site[_table.processer].is_robot)
                {
                    _table.card_righter = _table.next_card_righter;
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "CheckAndProcessRobotProcess guo");
                    _table.draw();
                }
            }
            else if (_table._hh_state == hh_state.dian_hu)
            {
                if (site != _table.processer)
                {
                    return;
                }

                HupaiState state = mj_huanghuang_check.check_dian_hu(_table.player_cards[_table.processer], _table.laizi, _table.processer_card);
                if (state == HupaiState.no_hu)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "can not hu");
                    return;
                }

                _table.last_card();

                int rate = 1;
                if (state == HupaiState.hard_hu)
                {
                    rate *= 2;
                }

                Hashtable score = new Hashtable();

                Int64 tmp_score = (Int64)_table.site[_table.play_carder].player_info["base"] * (Int64)_table.site[_table.processer].player_info["base"] * _table.score * rate;
                _table.site[_table.play_carder].player_info["score"] = (Int64)_table.site[_table.play_carder].player_info["score"] - tmp_score;
                _table.site[_table.processer].player_info["score"] = (Int64)_table.site[_table.processer].player_info["score"] + tmp_score;

                score[_table.site[_table.play_carder].player_info["unionid"]] = 0 - tmp_score;
                score[_table.site[_table.processer].player_info["unionid"]] = tmp_score;

                if (_table.payRule == (Int64)GameCommon.PayRule.MatchPay)
                {
                    foreach (DictionaryEntry k_v in score)
                    {
                        hub.hub.hubs.call_hub("lobby", "match", "wind_up", hub.hub.name, k_v.Key, k_v.Value);
                    }
                }

                _table.broadcast("mj_huanghuang", "hupai", _table.player_cards[_table.processer], _table.processer_card, _table.processer);

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
            else if (_table._hh_state == hh_state.hu)
            {
                if (site != _table.card_righter)
                {
                    return;
                }

                if (site != _table.processer)
                {
                    return;
                }
                
                HupaiState state = mj_huanghuang_check.check_hu(_table.player_cards[_table.card_righter], _table.laizi, _table.processer_card);
                if (state == HupaiState.no_hu)
                {
                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "can not hu");
                    return;
                }

                _table.last_card();
                
                int rate = 1;
                if (state == HupaiState.hard_hu)
                {
                    rate *= 2;
                }

                Hashtable score = new Hashtable();
                Int64 player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                while (player != _table.card_righter)
                {
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

                _table.broadcast("mj_huanghuang", "hupai", _table.player_cards[_table.card_righter], _table.processer_card, _table.card_righter);

                ArrayList other_pai = new ArrayList();
                Int64 _player = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                while (_player != _table.card_righter)
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
            else
            {
                robotPlay(_table, site);
            }
        }

        static void robotPlay(mj_huanghuang_table _table, Int64 site)
        {
            if (site != _table.card_righter)
            {
                return;
            }

            Random ra = new Random();
            int r = ra.Next();
            int card_index = r % _table.player_cards[_table.card_righter].Count;
            Int64 c = (Int64)_table.player_cards[_table.card_righter][card_index];
            _table.player_cards[_table.card_righter].RemoveAt(card_index);
            _table.player_play_cards[_table.card_righter].Add(c);

            _table.broadcast("mj_huanghuang", "play_card", c);

            if (c == _table.laizi)//出的牌是癞子，则底分乘以2
            {
                _table.site[_table.card_righter].player_info["base"] = (Int64)_table.site[_table.card_righter].player_info["base"] * 2;
                log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "CheckAndProcessRobotRight laizi");
                _table.draw();
            }
            else
            {
                _table.processer_card = c;
                _table.play_carder = _table.card_righter;

                _table.processer = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                while (_table.processer != _table.card_righter)
                {
                    if (mj_huanghuang_check.check_peng(_table.player_cards[_table.processer], _table.laizipi, c))
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "peng");

                        _table.next_card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                        _table.card_righter = (Int64)_table.GetEndSite();

                        _table._hh_state = hh_state.peng;

                        if (_table.site[_table.processer].is_robot)
                        {
                            onRobot(_table, _table.processer);
                        }
                        return;
                    }

                    if (mj_huanghuang_check.check_gang(_table.player_cards[_table.processer], _table.player_peng[_table.processer], _table.laizipi, c, false, true))
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "gang");

                        _table.next_card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                        _table.card_righter = (Int64)_table.GetEndSite();

                        _table._hh_state = hh_state.gang;

                        if (_table.site[_table.processer].is_robot)
                        {
                            onRobot(_table, _table.processer);
                        }
                        return;
                    }

                    if (mj_huanghuang_check.check_dian_hu(_table.player_cards[_table.processer], _table.laizi, c) != HupaiState.no_hu)
                    {
                        log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "hu");

                        _table.next_card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);
                        _table.card_righter = (Int64)_table.GetEndSite();

                        _table._hh_state = hh_state.dian_hu;

                        if (_table.site[_table.processer].is_robot)
                        {
                            onRobot(_table, _table.processer);
                        }
                        return;
                    }

                    _table.processer = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.processer);
                }

                if (_table.processer == _table.card_righter)
                {
                    _table.card_righter = (Int64)_table.GetNextSite((GameCommon.mjSite)_table.card_righter);

                    log.log.trace(new System.Diagnostics.StackFrame(true), service.timerservice.Tick, "CheckAndProcessRobotRight next card_righter");
                    _table.draw();
                }
            }
        }
    }
}
