using DotNetty.Buffers;
using System;
using System.Collections.Generic;
using System.Reflection;
using abelkhan.cmd;

namespace abelkhan
{
    public class CmdLoader
    {
        private static CmdContainer container = new CmdContainer();

        public static void LoadCmd(string assemblyName)
        {
            container.AddAll(SubClassLoader.LoadClassBasisOfSub<BaseCmd>(assemblyName));
        }

        public static void StartUp(List<string> _assemblyNames) {
            foreach (string name in _assemblyNames) {
                LoadCmd(name);
            }
        }

        public static ICmd CreateCmd(string name) => container.CreateGmCmd<ICmd>(name);
    }
}
