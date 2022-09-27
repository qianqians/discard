using System;
using System.Collections;
using System.Collections.Generic;
using MessagePack;

namespace abelkhan
{
    public class AbelkhanException : System.Exception
    {
        public AbelkhanException(string _err) : base(_err)
        {
        }
    }

    public interface Ichannel
    {
        void disconnect();
        void push(ProtoRoot ev);
    }

    public class Icaller
    {
        public Icaller(string _module_name, Ichannel _ch)
        {
            module_name = _module_name;
            ch = _ch;
        }

        public void call_module_method(string methodname, byte[] argvs)
        {
            ProtoRoot _event = new ProtoRoot();
            _event.module_name = module_name;
            _event.method_name = methodname;
            _event.argvs = argvs;

            try
            {
                ch.push(_event);
            }
            catch (System.Exception)
            {
                throw new abelkhan.AbelkhanException("error argvs");
            }
        }

        protected string module_name;
        private readonly Ichannel ch;
    }

    public class Response : Icaller
    {
        public Response(string _module_name, Ichannel _ch) : base(_module_name, _ch)
        {
        }
    }

    public class Imodule
    {
        public delegate void on_event(byte[] _event);
        protected Dictionary<string, on_event> events;

        public Imodule(string _module_name)
        {
            module_name = _module_name;
            events = new Dictionary<string, on_event>();
            current_ch = null;
            rsp = null;
        }

        public void reg_method(string method_name, on_event method)
        {
            events.Add(method_name, method);
        }

        public void process_event(Ichannel _ch, ProtoRoot _event)
        {
            current_ch = _ch;
            if (events.TryGetValue(_event.method_name, out on_event method))
            {
                try
                {
                    method(_event.argvs);
                    current_ch = null;
                }
                catch (System.Exception e)
                {
                    throw new abelkhan.AbelkhanException(string.Format("function name:{0} System.Exception:{1}", _event.method_name, e));
                }
            }
            else
            {
                throw new abelkhan.AbelkhanException(string.Format("do not have a function named:{0}", _event.method_name));
            }
        }

        public Ichannel current_ch;
        public Response rsp;
        public string module_name;
    }

    public class modulemng
    {
        public modulemng()
        {
            module_set = new Dictionary<string, Imodule>();
        }

        public void reg_module(Imodule module)
        {
            module_set.Add(module.module_name, module);
        }

        public void unreg_module(Imodule module)
        {
            module_set.Remove(module.module_name);
        }

        public void process_event(Ichannel _ch, ProtoRoot _event)
        {
            if (module_set.TryGetValue(_event.module_name, out Imodule _module))
            {
                try
                {
                    _module.process_event(_ch, _event);
                }
                catch (System.Exception e)
                {
                    throw new abelkhan.AbelkhanException(string.Format("System.Exception:{0}", e));
                }
            }
            else
            {
                throw new abelkhan.AbelkhanException(string.Format("do not have a module named::{0}", _event.module_name));
            }
        }

        private readonly Dictionary<string, Imodule> module_set;
    }
}