/*
 * hub
 * qianqians
 * 2020/6/4
 */
using System;
using System.Collections.Generic;
using System.Net;

namespace abelkhan
{
    public class outAddr
    {
        public string host;
        public ushort port;

        public outAddr(string _host, ushort _port)
        {
            host = _host;
            port = _port;
        }
    }

    public class hub
    {
        public byte gen_xor_byte()
        {
            Random ra = new Random();
            return (byte)ra.Next(1, 255);
        }

        public uint gen_xor_key()
        {
            var xor_key0 = (uint)gen_xor_byte();
            var xor_key1 = (uint)gen_xor_byte();
            var xor_key2 = (uint)gen_xor_byte();
            var xor_key3 = (uint)gen_xor_byte();

            return (xor_key0 << 24 | xor_key1 << 16 | xor_key2 << 8 | xor_key3);
        }

        public modulemng modules;
        public timerservice _timer;
        public closehandle _closehandle;
        public config _config; 
        public dbproxyproxy _dbproxyproxy;
        public string name;
        public string hub_type;

        public outAddr _outAddr = null;
        public uint xor_key = 0;

        private List<rawchannel> raw_chs_add;
        private List<rawchannel> raw_chs;
        private List<enetchannel> enet_chs;
        private List<cryptchannel> crypt_chs_add;
        private List<cryptchannel> crypt_chs_remove;
        private List<cryptchannel> crypt_chs;

        private center_msg_handle _center_msg_handle;
        private centerproxy _centerproxy;
        private dbproxy_msg_handle _dbproxy_msg_handle;
        private enetservice _enetservice;
        public hubmanager _hubmanager;
        private hub_msg_handle _hub_msg_handle;
        private cryptacceptservice _cryptacceptservice;
        private Int64 _timetmp;

        public hub_cmd_dispatcher _cmd_dispatcher;

        public event Action<hubproxy> on_hubproxy;
        public event Action on_connect_db;
        public event Action on_reload;
        public event Action on_close;
        public event Action<cryptchannel> on_client_connect;
        public event Action<cryptchannel> on_client_disconnect;
        public event Action<cryptchannel> on_client_exception;

        public hub(string cfg_file, string cfg_name)
        {
            var _root_cfg = new config(cfg_file);
            _config = _root_cfg.get_value_dict(cfg_name);
            var _center_config = _root_cfg.get_value_dict("center");

            var log_level = _config.get_value_string("log_level");
            if (log_level == "debug")
            {
                log.logMode = log.enLogMode.Debug;
            }
            else if (log_level == "release")
            {
                log.logMode = log.enLogMode.Release;
            }
            var log_file = _config.get_value_string("log_file");
            log.logFile = log_file;
            var log_dir = _config.get_value_string("log_dir");
            log.logPath = log_dir;
            {
                if (!System.IO.Directory.Exists(log_dir))
                {
                    System.IO.Directory.CreateDirectory(log_dir);
                }
            }

            name = _config.get_value_string("name");
            hub_type = _config.get_value_string("hub_type");

            modules = new abelkhan.modulemng();
            _closehandle = new closehandle();
            _timer = new timerservice();

            raw_chs_add = new List<rawchannel>();
            raw_chs = new List<rawchannel>();

            enet_chs = new List<enetchannel>();

            crypt_chs_add = new List<cryptchannel>();
            crypt_chs_remove = new List<cryptchannel>();
            crypt_chs = new List<cryptchannel>();

            ENet.Library.Initialize();
            var lan_ip = _config.get_value_string("ip");
            var lan_port = (ushort)_config.get_value_int("port");
            _enetservice = new enetservice(lan_ip, lan_port);
            _enetservice.on_connect += (enetchannel ch) => {
                enet_chs.Add(ch);
            };

            if (!string.IsNullOrEmpty(hub_type))
            {
                _cmd_dispatcher = new hub_cmd_dispatcher(this);
                _cmd_dispatcher.StartUp(new List<string>(new string[] { "hub", hub_type }));
            }

            _hubmanager = new hubmanager(modules);
            _hub_msg_handle = new hub_msg_handle(modules, _hubmanager, _cmd_dispatcher);
            _hub_msg_handle.on_hubproxy += (hubproxy _hubproxy) =>
            {
                if (on_hubproxy != null)
                {
                    on_hubproxy(_hubproxy);
                }
            };

            var center_ip = _center_config.get_value_string("ip");
            var center_port = (short)_center_config.get_value_int("port");
            var s_center = connectservice.connect(IPAddress.Parse(center_ip), center_port);
            var ch_center = new rawchannel(s_center);
            raw_chs.Add(ch_center);
            _centerproxy = new centerproxy(ch_center, modules);
            _center_msg_handle = new center_msg_handle(modules, _closehandle, _centerproxy);
            _center_msg_handle.on_svr += (server_info svr_info) =>
            {
                if (svr_info.type == "dbproxy")
                {
                    if (!_config.has_key("dbproxy"))
                    {
                        return;
                    }

                    if (svr_info.name != _config.get_value_string("dbproxy"))
                    {
                        return;
                    }

                    var s_db = connectservice.connect(IPAddress.Parse(svr_info.ip), (short)svr_info.port);
                    var ch_db = new rawchannel(s_db);
                    raw_chs_add.Add(ch_db);
                    if (_dbproxyproxy == null)
                    {
                        _dbproxyproxy = new dbproxyproxy(ch_db, modules);
                    }
                    else
                    {
                        _dbproxyproxy.reset(ch_db, modules);
                    }
                    _dbproxy_msg_handle = new dbproxy_msg_handle(modules, _dbproxyproxy);
                    _dbproxyproxy.reg_hub(name);
                    _dbproxy_msg_handle.on_connect_db += () =>
                    {
                        if (on_connect_db != null)
                        {
                            on_connect_db();
                        }
                    };
                    ch_db.onDisconnect += (ch) => {
                        _dbproxyproxy.on_closed();
                    };
                }
                else if (svr_info.type == "hub")
                {
                    _enetservice.connect(svr_info.ip, svr_info.port, (enetchannel ch) =>
                    {
                        var _caller = new hub_call_hub_caller(ch, modules);
                        _caller.reg_hub(name, hub_type).callBack(() =>
                        {
                            log.operation(new System.Diagnostics.StackFrame(true), timerservice.Tick, "reg hub:{0} suces", svr_info.name);
                        }, () =>
                        {
                            log.operation(new System.Diagnostics.StackFrame(true), timerservice.Tick, "reg hub:{0} faild", svr_info.name);
                        });
                    });
                }
            };
            _center_msg_handle.on_close += () =>
            {
                if (on_close != null)
                {
                    on_close();
                }
            };
            _center_msg_handle.on_reload += () =>
            {
                if (on_reload != null)
                {
                    on_reload();
                }
            };
            _center_msg_handle.on_svr_closed += (string type, string name) =>
            {
                log.trace(new System.Diagnostics.StackFrame(true), timerservice.Tick, "on_svr_closed type:{0}, name:{1}", type, name);
                if (type == "dbproxy")
                {
                    if (name == _config.get_value_string("dbproxy"))
                    {
                        _dbproxyproxy.on_closed();
                    }
                }
                else if (type == "hub")
                {
                    _hubmanager.hub_be_closed(name);
                }
            };
            _centerproxy.reg_hub(name, hub_type, lan_ip, lan_port);

            if (_config.has_key("out_host") && _config.has_key("out_port"))
            {
                var out_host = _config.get_value_string("out_host");
                var out_port = (ushort)_config.get_value_int("out_port");

                _outAddr = new outAddr(out_host, out_port);
                xor_key = _root_cfg.get_value_uint("default_key");

                _cryptacceptservice = new cryptacceptservice(xor_key, out_port);
                _cryptacceptservice.on_connect += (cryptchannel ch) =>
                {
                    lock (crypt_chs_add)
                    {
                        crypt_chs_add.Add(ch);
                    }

                    var new_key = gen_xor_key();
                    var xor_key_caller = new xor_key_caller(ch, modules);
                    xor_key_caller.refresh_xor_key(new_key);
                    ch.set_xor_key(new_key);

                    if (on_client_connect != null)
                    {
                        on_client_connect(ch);
                    }
                };
                _cryptacceptservice.on_disconnect += (cryptchannel ch) =>
                {
                    remove_client_channel(ch);
                    if (on_client_disconnect != null)
                    {
                        on_client_disconnect(ch);
                    }
                };
                _cryptacceptservice.on_channel_exception += (cryptchannel ch) =>
                {
                    remove_client_channel(ch);
                    if (on_client_exception != null)
                    {
                        on_client_exception(ch);
                    }
                };
                _cryptacceptservice.start();
            }

            _timetmp = _timer.refresh();
        }

        private void remove_client_channel(cryptchannel ch)
        {
            lock(crypt_chs_remove)
            {
                crypt_chs_remove.Add(ch);
            }
        }

        public void disconnect_client(cryptchannel ch)
        {
            ch.disconnect();
            remove_client_channel(ch);
        }

        public void close()
        {
            ENet.Library.Deinitialize();
            _timer.addticktime(1000, (Int64 tick) => {     
                _closehandle.is_close = true;
                _centerproxy.closed();
            });
        }


        public Int64 poll()
        {
            var tick_begin = _timer.refresh();
            try
            {
                _timer.poll();
                _enetservice.poll();

                foreach (var ch in raw_chs_add)
                {
                    raw_chs.Add(ch);
                }
                raw_chs_add.Clear();
                foreach (var ch in raw_chs)
                {
                    while (true)
                    {
                        var ev = ch._channel_onrecv.pop();
                        if (ev == null)
                        {
                            break;
                        }

                        modules.process_event(ch, ev);
                    }
                }

                foreach(var ch in enet_chs)
                {
                    while (true)
                    {
                        var ev = ch._channel_onrecv.pop();
                        if (ev == null)
                        {
                            break;
                        }

                        modules.process_event(ch, ev);
                    }
                }

                lock(crypt_chs_add)
                {
                    foreach (var ch in crypt_chs_add)
                    {
                        crypt_chs.Add(ch);
                    }
                    crypt_chs_add.Clear();
                }
                foreach (var ch in crypt_chs)
                {
                    while (true)
                    {
                        var ev = ch._channel_onrecv.pop();
                        if (ev == null)
                        {
                            break;
                        }

                        modules.process_event(ch, ev);
                    }
                }
                lock(crypt_chs_remove)
                {
                    foreach (var ch in crypt_chs_remove)
                    {
                        crypt_chs.Remove(ch);
                    }
                    crypt_chs_remove.Clear();
                }
            }
            catch (AbelkhanException e)
            {
                log.error(new System.Diagnostics.StackFrame(true), tick_begin, e.Message);
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(true), tick_begin, "{0}", e);
            }

            Int64 tick_end = _timer.refresh();
            _timetmp = tick_end;
            return tick_end - tick_begin;
        }
    }
}