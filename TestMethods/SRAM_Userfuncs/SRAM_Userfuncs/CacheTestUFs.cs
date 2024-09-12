using DataStore;
using System.Collections.Generic;


namespace SRAM
{
    internal class CacheTestUFs
    {
        static public int Raster_PrintTotalDef_Bin_Fail_Type(string strparam, Dictionary<string, string> parameters)
        {
            string[] stresstypes = parameters["StressType"].Split(',');
            string[] rutypes = parameters["RUTypes"].Split(',');
            //string testname = parameters["TestName"]; we printed earlier, not sure if we need to do this

            List<int> rus = new List<int>();
            foreach(string rutype in rutypes)
            {
                if (CacheTest.iosbyrutype.ContainsKey(rutype))
                {
                    rus.AddRange(CacheTest.iosbyrutype[rutype]);
                }
            }

            int bitcount = 0;
            int colcount = 0;
            int rowcount = 0;
            int othercount = 0;
            //int affectedbits = 0;

            foreach(string stresstype in stresstypes)
            {
                if (CacheTest.binningfailcountbyio.ContainsKey(stresstype))
                {
                    Dictionary<int, CacheRasterFailCountByIO> failsbyio = CacheTest.binningfailcountbyio[stresstype];
                    foreach (int ru in rus)
                    {
                        if(failsbyio.ContainsKey(ru))
                        {
                            CacheRasterFailCountByIO fbyio = failsbyio[ru];
                            bitcount += fbyio.numbitdefects;
                            colcount += fbyio.numcoldefects;
                            rowcount += fbyio.numrowdefects;
                            othercount += fbyio.numotherdefects;
                            //affectedbits += fbyio.numtotalfailingbits;
                        }
                    }
                }
            }

            if ((bitcount == 0) && (colcount == 0) && (rowcount == 0) && (othercount == 0))
            {
                return 1;
            }
            else
            {
                if ((bitcount >= colcount) && (bitcount >= rowcount) && (bitcount >= othercount) && (bitcount != 0))
                {
                    return 2; //Bit
                }
                else
                {
                    if ((colcount >= rowcount) && (colcount >= othercount) && (colcount != 0))
                    {
                        return 3; //Col
                    }
                    else
                    {
                        if ((rowcount >= othercount) && (rowcount != 0))
                        {
                            return 4; //Row
                        }
                    }
                }
            }
            return 5; //Other, probably MAF / TAF
        }
    }
}
