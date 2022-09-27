using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace rank
{
    class server
    {
        static void broadcastScoreRank(Int64 tick)
        {
            ArrayList rank_info = new ArrayList();
            for (int i = 1; i <= 9; i++)
            {
                var entity = score_rank.get_rank(i);
                if (entity == null)
                {
                    break;
                }
                rank_info.Add(entity);
            }
            hub.hub.gates.call_global_client("rank", "ack_get_rank_entity", rank_info);

            hub.hub.timer.addticktime(60 * 1000, broadcastScoreRank);
        }

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                return;
            }

            hub.hub _hub = new hub.hub(args);

            ranks = new Dictionary<string, rank>();

            rank_msg _rank_msg = new rank_msg();
            hub.hub.modules.add_module("rank_msg", _rank_msg);

            score_rank = new rank(new scoreComparer());
            ranks.Add("score", score_rank);

            hub.hub.timer.addticktime(60 * 1000, broadcastScoreRank);

            while (true)
            {
                if (hub.hub.closeHandle.is_close)
                {
                    log.log.operation(new System.Diagnostics.StackFrame(), service.timerservice.Tick, "server closed, hub server " + hub.hub.uuid);
                    break;
                }
                
                if (_hub.poll() < 50)
                {
                    Thread.Sleep(15);
                }
            }
        }

        static public Dictionary<string, rank> ranks;
        static rank score_rank;
    }
}
