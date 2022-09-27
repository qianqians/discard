using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan
{
    public class ReloadCmd : BaseCmd
    {
        public override string GetName() {
            return "reload";
        }

        public override string DoCmd(gm _gm, string name) {
            _gm._center_proxy.reload(_gm._gm_name);
            return string.Empty;
        }
    }
}
