using System;
using System.Collections;
using System.Collections.Generic;

namespace rank
{
    //升序排行
    class rank
    {
        // 降序
        // Compare(Hashtable x, Hashtable y)
        // x < y => -1
        // x == y => 0
        // x > y => 1
        public rank(IComparer<Hashtable> _comparer)
        {
            rank_data = new List<sub_rank_list>();
            comparer = _comparer;
        }

        public void clear_rank()
        {
            rank_data = new List<sub_rank_list>();
        }

        public void update_rank(Hashtable data)
        {
            String uuid = (String)data["uuid"];

            if (rank_data.Count <= 0)
            {
                insert_rank(data);
                return;
            }
            
            foreach (var sub_list in rank_data)
            {
                if (!sub_list.in_rank(uuid))
                {
                    continue;
                }

                if (sub_list.in_range(data))
                {
                    sub_list.update_rank(data);
                }
                else
                {
                    sub_list.del_rank(uuid);
                    insert_rank(data);
                }

                break;
            }
        }

        public void insert_rank(Hashtable data)
        {
            sub_rank_list sub_list = null;
            sub_rank_list up_list = null;
            sub_rank_list low_list = null;
            foreach (var _sub_list in rank_data)
            {
                if (up_list == null)
                {
                    up_list = _sub_list;
                }
                else
                {
                    if (comparer.Compare(up_list.up, _sub_list.up) < 0)
                    {
                        up_list = _sub_list;
                    }
                }

                if (low_list == null)
                {
                    low_list = _sub_list;
                }
                else
                {
                    if (comparer.Compare(low_list.low, _sub_list.low) > 0)
                    {
                        low_list = _sub_list;
                    }
                }

                if (!_sub_list.in_range(data))
                {
                    continue;
                }

                sub_list = _sub_list;
                break;
            }
            if (sub_list == null)
            {
                if (low_list == null && up_list == null)
                {
                    sub_list = new sub_rank_list(data, data, comparer);
                    rank_data.Add(sub_list);
                }
                else
                {
                    if (comparer.Compare(low_list.low, data) >= 0)
                    {
                        sub_list = low_list;
                        sub_list.low = data;
                    }
                    if (comparer.Compare(up_list.up, data) <= 0)
                    {
                        sub_list = up_list;
                        sub_list.up = data;
                    }
                }
            }
            sub_list.insert_rank(data);

            if (sub_list.count() > 2000)
            {
                var new_rank = sub_list.split_rank();
                rank_data.Add(new_rank);

                for(int i = 0; i < rank_data.Count - 1; i++)
                {
                    var rank1 = rank_data[i];
                    var rank2 = rank_data[i + 1];

                    if (comparer.Compare(rank1.low, rank2.up) < 0)
                    {
                        rank_data[i] = rank2;
                        rank_data[i + 1] = rank1;
                    }
                }
            }
        }

        public int get_rank(String uuid)
        {
            int rank = 0;
            foreach(var sub_rank_list in rank_data)
            {
                if (!sub_rank_list.in_rank(uuid))
                {
                    rank += sub_rank_list.count();
                    continue;
                }

                rank += sub_rank_list.rank(uuid);

                break;
            }

            return rank;
        }

        public Hashtable get_rank(int rank)
        {
            int rank_low = 0;
            int rank_up = 1;
            foreach(var sub_rank_list in rank_data)
            {
                rank_low = rank_up;
                rank_up += sub_rank_list.count();

                if (rank >= rank_low && rank < rank_up)
                {
                    return sub_rank_list.rank_list[rank - rank_low];
                }
            }

            return null;
        }

        public bool in_rank(string uuid)
        {
            foreach (var item in rank_data)
            {
                if (item.in_rank(uuid) == true)
                {
                    return true;
                }
                
            }
            return false;
        }
        
        public int count()
        {
            int count = 0;
            foreach (var item in rank_data)
            {
                count += item.count();
            }
            return count;
        }

        class sub_rank_list
        {
            public sub_rank_list(Hashtable _low, Hashtable _up, IComparer<Hashtable> _comparer)
            {
                low = _low;
                up = _up;
                mem_set = new HashSet<String>();
                rank_list = new List<Hashtable>();

                comparer = _comparer;
            }

            public bool in_range(Hashtable data)
            {
                int c1 = comparer.Compare(data, up);
                int c2 = comparer.Compare(data, low);

                return c1 <= 0 && c2 > 0;
            }

            public bool in_rank(String uuid)
            {
                return mem_set.Contains(uuid);
            }

            public int count()
            {
                return mem_set.Count;
            }

            public int rank(String uuid)
            {
                int rank = 0;
                foreach(var mem in rank_list)
                {
                    rank++;

                    if (uuid == (String)mem["uuid"])
                    {
                        break;
                    }
                }

                return rank;
            }

            public void update_rank(Hashtable data)
            {
                foreach(var _data in rank_list)
                {
                    if ((String)_data["uuid"] != (String)data["uuid"])
                    {
                        continue;
                    }

                    rank_list.Remove(_data);
                    break;
                }
                rank_list.Add(data);
                rank_list.Sort(comparer);
            }

            public void del_rank(String uuid)
            {
                foreach (var _data in rank_list)
                {
                    if ((String)_data["uuid"] != uuid)
                    {
                        continue;
                    }

                    mem_set.Remove(uuid);
                    rank_list.Remove(_data);
                    break;
                }
            }

            public void insert_rank(Hashtable data)
            {
                mem_set.Add((String)data["uuid"]);
                rank_list.Add(data);
                rank_list.Sort(comparer);
            }

            public sub_rank_list split_rank()
            {
                sub_rank_list new_rank = new sub_rank_list(low, up, comparer);

                int _count = count() / 2;
                Hashtable data = null;
                for (int i = 0; i < _count; i++)
                {
                    data = rank_list[i];
                    del_rank((String)data["uuid"]);
                    new_rank.insert_rank(data);
                }
                new_rank.low = data;
                up = rank_list[0];

                return new_rank;
            }

            public Hashtable low;
            public Hashtable up;

            private HashSet<String> mem_set;
            public List<Hashtable> rank_list;

            private IComparer<Hashtable> comparer;
        }
        private List<sub_rank_list> rank_data;
        private IComparer<Hashtable> comparer;
    }
}
