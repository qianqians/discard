using System.Collections.Generic;
using System.Threading.Tasks;

namespace abelkhan.cmd
{
    public class CmdDispatcher<T, TO> where T : IName, IGmCmd, IGmServer<TO>
    {
        protected CmdContainer container;
        protected TO _ower;

        public CmdDispatcher(TO _ower) {
            this._ower = _ower;
        }
        public void StartUp(List<string> _assemblyNames)
        {
            container = new CmdContainer(SubClassLoader.LoadClassBasisOfSub<T>(_assemblyNames));
        }

        public async Task<string> Dispatch(string cmd, string param)
        {
            try
            {
                T instance = container.CreateGmCmd<T>(cmd);
                if (instance != null)
                {
                    instance.SetServer(_ower);
                    return await instance.DoCmd(new GmParam(param));
                }
                else
                {
                    return GmRespone<string>.Fail(TCode.CMD_NOT_EXISTS).Encode();
                }
            }
            catch (System.Exception e) {
                log.error(new System.Diagnostics.StackFrame(true), timerservice.Tick, "Cmd Dispatch fail: {0}", e);
                return GmRespone<string>.Fail(TCode.CMD_DO_EXCEPTION).Encode();
            }
        }
    }
}
