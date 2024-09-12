using SRAMBase;
using System.Text;

namespace SRAM
{
    internal class TestFlowUserFuncs
    {
        private static UserVariable? visualid;

        public static void PrintSiteID()
        {
            UserVariable xcoorduv = new UserVariable("SCVars", "SC_SITEID");
            string val = xcoorduv.GetStringValue();   
            StringBuilder sb = new StringBuilder();
            ItuffUtilities.mrsltToStringBuilder("siteid", val, sb);
        }

        public static string getVisualID()
        {
            if (visualid == null)
            {
                UserVariable uv = new UserVariable("CTSCVars", "EI_VISUALID");
                if (uv.Isset)
                {
                    visualid = uv;
                }
                else
                {
                    uv = new UserVariable("NTSCVars", "SC_VISUALID");
                    if (uv.Isset)
                    {
                        visualid = uv;
                    }
                    else
                    {
                        uv = new UserVariable("SCVars", "SC_VISUALID");
                        if (uv.Isset)
                        {
                            visualid = uv;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            return visualid.GetStringValue();
        }

        public static int hasVisualID()
        {
            string visid = getVisualID();
            if (visid.Length == 13 || visid.Length == 20)
            {
                return 1;
            }
            return 0;
        }
    }
}
