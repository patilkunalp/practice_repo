using SRAMBase;
using System.Runtime.CompilerServices;
using System.Text;
using UserFuncTest;

namespace SRAM
{
    public class LocalUserfuncs
    {
        //usage: params = "<UserVar1>,<UserVar2>,<Level>"
        public static void update_adaptive_vmin_raster_level_global(string parameters)
        {
            string[] paramsplit = parameters.Split(',');
            if(paramsplit.Length != 3)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Parameters: " + parameters + " should have 3 values.");
                throw new SramException(sb.ToString());
            }

            UserVariable vccminglobal = new UserVariable(paramsplit[0]);
            string global_value1 = vccminglobal.GetStringValue();
            UserVariable rasteroffset = new UserVariable(paramsplit[1]);
            string global_value2 = rasteroffset.GetStringValue();

            SramLibrary.WriteToConsole("VCCMIN: " + global_value1 + " offset " + global_value2 + Environment.NewLine);
            double global_value3 = double.Parse(global_value1) - double.Parse(global_value2);
            SramLibrary.WriteToConsole("Raster voltage: " + global_value3 + Environment.NewLine);

            SramLibrary.setTestConditionVariableValue(paramsplit[2], "vcc_level", global_value3.ToString());
        }

        public static void HelloWorld()
        {
            SramLibrary.WriteToConsole("Hello World");
        }

        public static void WriteToITuff()
        {
            SramLibrary.WriteToITuff("Hello World");
        }

        public static void WriteParameterToConsole(string parameters)
        {
            SramLibrary.WriteToConsole(parameters);
        }

        public static int ExecuteFuncTest()
        {
            string level = "main_level_ltype";
            string timing = "main_timing_tim";
            string plist = "some_plist";

            bool result = SramLibrary.executeFuncTest(level, timing, plist);
            if(result)
            {
                return 1;
            }
            return 0;
        }

        public static int checkReturnValue()
        {
            return 1;
        }

        public static void ReadUserVar()
        {
            UserVariable xcoorduv = new UserVariable("SCVars", "SC_WAFERX");
            SramLibrary.WriteToConsole("X-Coord is: " + xcoorduv.GetStringValue());
        }
    }
}
