using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFEngine.SDK
{
    public class PinsError
    {
        public static Dictionary<int, string> PinErrorDesc = new Dictionary<int, string>();
        public const int PinNotActive = 0;
        public const int PinIdNotvalid = -1;
        public const int PinException = -2;
        public const int EntryPointNotFound = -3;
        public const int PinOutNotDefined = -4;

        static PinsError()
        {
            PinErrorDesc.Add(0, "PinNotActive");
            PinErrorDesc.Add(-1, "PinIdNotvalid");
            PinErrorDesc.Add(-2, "PinException");
            PinErrorDesc.Add(-3, "EntryPointNotFound");
            PinErrorDesc.Add(-4, "PinOutNotDefined");
        }
    }
}