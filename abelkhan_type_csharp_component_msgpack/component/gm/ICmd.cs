using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using System;
using abelkhan.cmd;

namespace abelkhan
{

    #region gm內部接口
    public interface ICmd<T, P>
    {
        T DoCmd(gm _gm, P p);
    }

    public interface ICmd : ICmd<string, string> {
    }

    interface IParams {
    }

    public abstract class BaseCmd : ICmd, IName
    {
        public abstract string GetName();

        public virtual string DoCmd(gm _gm, string name)
        {
            return string.Empty;
        }
    }
    #endregion
}