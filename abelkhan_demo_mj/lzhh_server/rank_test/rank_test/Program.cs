using System;
using System.Collections.Generic;
using System.Collections;

namespace rank_test
{
    class Program
    {
        class rankComparer : IComparer<Hashtable>
        {
            public int Compare(Hashtable x, Hashtable y)
            {
                if ((Int64)x["rank"] > (Int64)y["rank"])
                {
                    return 1;
                }
                else if((Int64)x["rank"] < (Int64)y["rank"])
                {
                    return -1;
                }

                return 0;
            }
        }

        static void Main(string[] args)
        {
            rank.rank rank = new rank.rank(new rankComparer());

            Hashtable data = new Hashtable();
            data.Add("uuid", Guid.NewGuid().ToString());
            data.Add("rank", 1);
            rank.update_rank(data);
            data = new Hashtable();
            data.Add("uuid", Guid.NewGuid().ToString());
            data.Add("rank", 2);
            rank.update_rank(data);

            Int64 _rank = rank.get_rank((string)data["uuid"]);


        }
    }
}
