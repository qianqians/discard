using System;
using System.Collections.Generic;
using System.Text;

namespace abelkhan
{
    public class CloseCmd : BaseCmd
    {
        public override string GetName() {
            return "close";
        }

        public override string DoCmd(gm _gm, string name) {
            _gm._center_proxy.close_clutter(_gm._gm_name);
            return string.Empty;
        }
    }
}
