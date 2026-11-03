using Abelkhan;
using avatar;
using bag;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Server
{
    class TimeInfoMgr
    {
        private bool save2Redis;
        private TimeInfo _info;

        public async void Init(bool save)
        {
            save2Redis = save;
            if (save2Redis)
            {
                _info = await Server.RedisHandle.GetData<TimeInfo>(RedisHelp.BuildTimeInfoKey());
            }
            if (_info == null)
            {
                _info = new TimeInfo()
                {
                    year = 1,
                    months = 10,
                    day = 1,
                    hour = 10,
                    minute = 12,
                    second = 0
                };
            }
        }

        public async void Update(long _) {
            if (_info == null)
            {
                return;
            }

            bool needSave = false;
            do
            {
                _info.second = 0;
                _info.minute += 6;
                if (_info.minute < 60)
                {
                    break;
                }
                _info.minute = 0;
                needSave = true;
                
                _info.hour++;
                if (_info.hour < 24)
                {
                    break;
                }
                _info.hour = 0;
                
                _info.day++;
                if (_info.day < 30)
                {
                    break;
                }
                _info.day = 0;

                _info.months++;
                if (_info.months < 12)
                {
                    break;
                }
                _info.months = 0;
                _info.year++;

            } while (false);

            if (needSave) {
                if (save2Redis)
                {
                    await Server.RedisHandle.SetData(RedisHelp.BuildTimeInfoKey(), _info);
                }
            }

            Server.SceneMgr?.NtfTimeInfo(_info);
        }

        public TimeInfo TimeInfo()
        {
            var info = new TimeInfo()
            {
                months = _info.months,
                day = _info.day,
                hour = _info.hour,
                minute = _info.minute,
                second = _info.second,
            };
            return info;
        }
    }
}
