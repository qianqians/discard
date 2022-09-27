using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace abelkhan.cmd
{

    #region 其他服务接受gm命令请求处理接口
    /// <summary>
    /// gm 发送过来的命令接口
    /// </summary>
    public interface IGmCmd<T, P>{
        Task<T> DoCmd(P p);
    }

    public interface IGmCmd : IGmCmd<string, GmParam> {
        
    }

    public interface IGmServer<TServer> {
        void SetServer(TServer _server);
        TServer GetServer();
    }

    public interface IGmParam
    {
        T parse<T>();
    }

    public interface IGmReponse
    {
        string Encode();
    }

    public class GmParam: IGmParam{
        public string svrType { set; get; }
        public string svrName { set; get; }
        public string cmdName { set; get; }
        public string param { set; get; }

        public GmParam(string _param) {
            this.param = _param;
        }

        public T parse<T>() { 
            return JsonConvert.DeserializeObject<T>(param);
        }
    }

    public class GmRespone<T>: IGmReponse {
        public TCode code { set; get; }

        public T data { set; get; }

        public GmRespone(TCode _code)
        {
            this.code = _code;
        }

        public GmRespone(TCode _code, T t)
        {
            this.code = _code;
            this.data = t;
        }

        public static GmRespone<T> Success() 
        {
            return new GmRespone<T>(TCode.OK);
        }

        public static GmRespone<T> Res(T t)
        {
            return new GmRespone<T>(TCode.OK, t);
        }

        public static GmRespone<T> Fail(TCode _code)
        {
            return new GmRespone<T>(_code);
        }

        public static GmRespone<PT> parse<PT>(string param)
        {
             return JsonConvert.DeserializeObject<GmRespone<PT>>(param);
        }

        public string ToSerialize()
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("code", (int)code);
            dict.Add("data", data);
            dict.Add("errMessage", code.Des);
            return JsonConvert.SerializeObject(dict);
        }

        public string Encode() {
            return this.ToSerialize();
        }
    }

    public abstract class GmBaseCmd<TServer> : IGmCmd, IName, IGmServer<TServer>
    {
        public TServer Server { set; get; }
        public abstract string GetName();

        public virtual async Task<string> DoCmd(GmParam param)
        {
            return await Task.FromResult<string>(string.Empty);
        }

        public void SetServer(TServer _server) {
            Server = _server;
        }

        public TServer GetServer() {
            return Server;
        }
    }

    #endregion
}