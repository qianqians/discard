/*
 * dbproxy
 * 2020/6/3
 * qianqians
 */
using System;
using System.Collections.Generic;
using System.Net;

namespace abelkhan
{
	public class dbproxy
	{
		private modulemng modules;
		private dbevent _dbevent;
		private mongodbproxy _mongodbproxy;
		private centerproxy _centerproxy;
		private center_msg_handle _center_msg_handle;
		private hubmanager _hubmanager;
		private hub_msg_handle _hub_msg_handle;
		private acceptservice _hub_acceptservice;
		private List<channel> chs;
		private rawchannel ch_center;
		private Int64 _timetmp;

		public closeHandle _closeHandle;
		public timerservice _timer;

		public dbproxy(string cfg_file, string cfg_name)
		{
			var _root_cfg = new config(cfg_file);
			var _config = _root_cfg.get_value_dict(cfg_name);
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

			modules = new modulemng();
			_closeHandle = new closeHandle();
			_timer = new timerservice();
			_dbevent = new dbevent(_closeHandle);
			_mongodbproxy = new mongodbproxy(_config.get_value_string("db_url"));
			chs = new List<channel>();

			_dbevent.start();
			if (_config.has_key("index"))
			{
				var _index_cfg = _config.get_value_list("index");
				for (var i = 0; i < _index_cfg.get_list_size(); ++i)
				{
					var _index_cfg_i = _index_cfg.get_list_dict(i);
					var db = _index_cfg_i.get_value_string("db");
					var collection = _index_cfg_i.get_value_string("collection");
					var key = _index_cfg_i.get_value_string("key");
					var is_unique = _index_cfg_i.get_value_bool("is_unique");
					_mongodbproxy.create_index(db, collection, key, is_unique);
				}
			}

			_hubmanager = new hubmanager(modules);
			_hub_msg_handle = new hub_msg_handle(modules, _dbevent, _hubmanager, _mongodbproxy);
			var ip = _config.get_value_string("ip");
			ushort port = (ushort)_config.get_value_int("port");
			_hub_acceptservice = new acceptservice(port);
			_hub_acceptservice.on_connect += (channel ch) => {
				chs.Add(ch);
			};
			_hub_acceptservice.on_disconnect += (channel ch) =>
			{
				chs.Remove(ch);
			};
			_hub_acceptservice.start();

			var center_ip = _center_config.get_value_string("ip");
			var center_port = (short)_center_config.get_value_int("port");
			var s = connectservice.connect(IPAddress.Parse(center_ip), center_port);
			ch_center = new rawchannel(s);
			_centerproxy = new centerproxy(ch_center, modules);
			_center_msg_handle = new center_msg_handle(modules, _hubmanager, _closeHandle, _centerproxy);
			var name = _config.get_value_string("name");
			_centerproxy.reg_dbproxy(name, ip, port);

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
				while (true)
				{
					var ev = ch_center._channel_onrecv.pop();
					if (ev == null)
					{
						break;
					}

					modules.process_event(ch_center, ev);
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