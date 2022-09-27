/*
 * center server
 * 2020/6/2
 * qianqians
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;

namespace abelkhan
{
    public class center
    {
        private modulemng modules;
        private acceptservice _accept_svr_service;
        private svr_msg_handle _svr_msg_handle;
        public svrmanager _svrmanager;
        private hub_msg_handle _hub_msg_handle;
        public hubmanager _hubmanager;
        private acceptservice _accept_gm_service;
        private gm_msg_handle _gm_msg_handle;
        private gmmanager _gmmanager;
        private List<channel> chs;
        private Int64 _timetmp;

        public closehandle _closeHandle;
        public timerservice _timer;

        public delegate void cb_svr_disconnect(svrproxy _proxy);
        public event cb_svr_disconnect on_svr_disconnect;

        public center_cmd_dispatcher _cmd_dispatcher;

        public center(string cfg_file, string cfg_name)
        {
            var _root_cfg = new config(cfg_file);
            var _config = _root_cfg.get_value_dict(cfg_name);

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

            chs = new List<channel>();
            _closeHandle = new closehandle();
            modules = new modulemng();
            _timer = new timerservice();

            _svrmanager = new svrmanager(modules);
            _hubmanager = new hubmanager(modules);
            _svr_msg_handle = new svr_msg_handle(modules, _svrmanager, _hubmanager);
            _hub_msg_handle = new hub_msg_handle(modules, _svrmanager, _hubmanager, _closeHandle);
            var ip = _config.get_value_string("ip");
            var port = _config.get_value_int("port");
            _accept_svr_service = new acceptservice((ushort)port);
            _accept_svr_service.on_connect += (channel ch) => {
                chs.Add(ch);
            };
            _accept_svr_service.on_disconnect += (channel ch) => {
                chs.Remove(ch);

                var _proxy = _svrmanager.get_svr(ch);
                if (_proxy != null)
                {
                    if (_proxy.type == "hub")
                    {
                        var _hubproxy = _hubmanager.get_hub(ch);
                        if (_hubproxy != null && _hubproxy.is_closed && _closeHandle.is_closing)
                        {
                            return;
                        }
                        _hubmanager.hub_closed(ch);

                        _svrmanager.on_svr_close(_proxy);
                    }

                    if (on_svr_disconnect != null)
                    {
                        on_svr_disconnect(_proxy);
                    }
                }
            };
            _accept_svr_service.start();

            _cmd_dispatcher = new center_cmd_dispatcher(this);
            _cmd_dispatcher.StartUp(new List<string>(new string[] { "center", "center_server" }));

            _gmmanager = new gmmanager();
            _gm_msg_handle = new gm_msg_handle(modules, _svrmanager, _hubmanager, _gmmanager, _closeHandle, _cmd_dispatcher);
            var gm_ip = _config.get_value_string("gm_ip");
            var gm_port = _config.get_value_int("gm_port");
            _accept_gm_service = new acceptservice((ushort)gm_port);
            _accept_gm_service.on_connect += (channel ch) =>{
                chs.Add(ch);
            };
            _accept_gm_service.on_disconnect += (channel ch) => {
                chs.Remove(ch);
            };
            _accept_gm_service.start();

            _timetmp = _timer.refresh();
            
        }

        public Int64 poll()
        {
            var tick_begin = _timer.refresh();
            try
            {
                _timer.poll();

                foreach (var ch in chs)
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

                _svrmanager.remove_closed_svr();
            }
            catch (AbelkhanException e)
            {
                log.error(new System.Diagnostics.StackFrame(true), tick_begin, "AbelkhanException:{0}", e.Message);
            }
            catch (System.Exception e)
            {
                log.error(new System.Diagnostics.StackFrame(true), tick_begin, "System.Exception:{0}", e);
            }

            Int64 tick_end = _timer.refresh();
            _timetmp = tick_end;
            return tick_end - tick_begin;
        }
    }
}