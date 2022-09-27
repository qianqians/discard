using System;
using System.Collections;
using System.Collections.Generic;

namespace rank
{
    class scoreComparer : IComparer<Hashtable>
    {
        public int Compare(Hashtable x, Hashtable y)
        {
            if (x == null && y != null)
            {
                return -1;
            }
            if (x != null && y == null)
            {
                return 1;
            }
            if (x == null && y == null)
            {
                return 0;
            }

            if ((Int64)x["score"] < (Int64)y["score"])
            {
                return -1;
            }
            else if ((Int64)x["score"] == (Int64)y["score"])
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
    }
}
