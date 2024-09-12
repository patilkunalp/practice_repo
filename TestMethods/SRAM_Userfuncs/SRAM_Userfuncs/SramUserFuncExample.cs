using SRAMBase;
using UserFuncTest;

namespace SRAM
{
    public class SramUserFuncExample
    {
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
