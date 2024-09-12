using SRAMBase;
using System.ComponentModel;
using System.Text;
using UserFuncTest;

namespace SRAM
{
    internal class BinningUserfunc
    {
        /* The numbering is in the following order: */
        /* 0,1,2,......,24,25,26,  */
        /* 27,28,29,...,53,54,55,  */
        /* 56,57,58,...,89,90,91,  */
        /* 92,93,94,...,97,98,99 } */

        /* Modified by Ashraf to merge both sort and class */
        /* 10/20/2005 */

        /*sort*/
        private static uint[] failbinpriority = {
            0,60,59,58,57,56,55,0,6,16,4,3,11,0,54,5,15,0,6,14,24,30,0,25,50,18,26,
            34,0,7,0,0,37,0,27,20,41,0,0,3,22,21,35,0,0,0,23,39,13,26,50,0,0,0,0,0,
             0,0,0,0,0,8,9,10,30,30,0,54,0,1,0,0,0,0,0,0,0,0,0,0,0,36,36,39,26,36,36,26,26,2,0,1,
            1,1,1,1,1,1,1,1 
        };

        /* class */
        private static uint[] failbinpriorityclass = {
            0,60,59,58,57,56,55,0,6,16,4,3,11,0,54,5,15,29,17,14,24,30,0,25,50,0,26,
            34,0,7,0,0,37,0,27,20,41,0,0,3,22,21,35,0,0,0,7,39,13,0,0,0,0,1,0,0,
             0,0,0,0,0,8,9,10,30,30,0,54,0,1,0,0,0,0,0,0,0,0,0,0,0,36,36,39,26,36,36,26,26,2,0,1,
            1,1,1,1,1,1,1,1 
        };

        private static uint[] ibinclass = {
            8,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99
         };

        private static uint[] softbin_cth_handler = {
            9999,101,201,301,401,501,601,701,801,901,1001,1101,1201,1301,1401,1501,1601,1701,1801,1901,2001,2101,2201,2301,2401,2501,2601,
            2701,2801,2901,3001,3101,3201,3301,3401,3501,3601,3701,3801,3901,4001,4101,4201,4301,4401,4501,4601,4701,4801,4901,5001,5101,
            5201,5301,5401,5501,5601,5701,5801,5901,6001,6101,6201,6301,6401,6501,6601,6701,6801,6901,7001,7101,7201,7301,7401,7501,7601,
            7701,7801,7901,8001,8101,8201,8301,8401,8501,8601,8701,8801,8901,9001,9101,9201,9301,9401,9501,9601,9701,9801,9901
        };

        /*   Repair Binning for X5 with Vccmin and Vccmax repair   2-26-04  */
        /*   RISO bin is IB, ISO bin is DB at sort for X5. opposite of historical */
        /*             4701 test added for NR vccmin */
        /*              Bin 5,6,7 moved to 61,62,63 */
        /* BFNRR = NR raster = TC6101,6201,6301 or 1201 on fail. */
        /* BFNRT = BFNR test = TC201 on fail. */
        /* VMINNR = NR save_fails at vccmin value = TC4701 */
        /* VMAXNR = NR save_fails at vccmax value = TC4801 */
        /* PSBPS2 = redundant testing = TC1902 on fail. */
        /* Redundant fusing is between PSBPS1 and PSBPS2. */

        /* DB61 or IB61 means 61,62,63,or 12. */
        /* FB6101 means 6101,6201,6301,or 1201 */

        /*    6101   201     4901    5001    1902 */
        /*    BFNRR  BFNRT  VMINNR  VMAXNR  PSBPS2   FB  DB  IB */
        /*    ------------------------------------------------- */
        /*     P      P       P       P       P     100   1   1 */
        /*   + P      P       P       P       F    1902  19  19 */
        /*     P      P       P       F       P     200  50   2 */
        /*     P      P       P       F       F    1902  50  19 */
        /*     P      P       F       P       P     200  49   2 */
        /*     P      P       F       P       F    1902  49  19 */
        /*     P      P       F       F       P     200  49   2 */
        /*     P      P       F       F       F    1902  49  19 */
        /*     P      F       P       P       P     201  19   2 */
        /*     P      F       P       P       F    1902  19  19 */
        /*     P      F       P       F       P     201  50   2 */
        /*     P      F       P       F       F    1902  50  19 */
        /*     P      F       F       P       P     201  49   2 */
        /*     P      F       F       P       F    1902  49  19 */
        /*     P      F       F       F       P     201  49   2 */
        /*     P      F       F       F       F    1902  49  19 */

        /*   + F      P       P       P       P     200  61   2 */
        /*   + F      P       P       P       F    1902  61  19 */
        /*     F      P       P       F       P     200  61   2 */
        /*     F      P       P       F       F    1902  61  19 */
        /*     F      P       F       P       P     200  61   2 */
        /*     F      P       F       P       F    1902  61  19 */
        /*     F      P       F       F       P     200  61   2 */
        /*     F      P       F       F       F    1902  61  19 */
        /*     F      F       P       P       P     201  61   2 */
        /*     F      F       P       P       F    6101  61  61 */
        /*     F      F       P       F       P     201  61   2 */
        /*     F      F       P       F       F    6101  61  61 */
        /*     F      F       F       P       P     201  61   2 */
        /*     F      F       F       P       F    6101  61  61 */
        /*     F      F       F       F       P     201  61   2 */
        /*     F      F       F       F       F    6101  61  61 */
        /* + means it shouldn't happen but needs be accounted for */

        private static uint[] fbrepairbin = {0,1902,200,1902,200,1902,200,1902,201,1902,201,1902,201,1902,201,1902,
                       200,1902,200,1902,200,1902,200,1902,201,6101,201,6101,201,6101,201,6101};

        private static uint[] dbrepairbin = {1,19,50,50,49,49,49,49,19,19,50,50,49,49,49,49,
                       61,61,61,61,61,61,61,61,61,61,61,61,61,61,61,61};

       private static uint[] ibrepairbin = {1,19,2,19,2,19,2,19,2,19,2,19,2,19,2,19,
                       2,19,2,19,2,19,2,19,2,61,2,61,2,61,2,61};

        /*    class hot */

        /*    6101   201   1902 */
        /*    BFNRR  BFNRT SBPS2   FB  DB  IB */
        /*    ------------------------------- */
        /*     P      P     P     100   1   1 --> class hot*/
        /*   + P      P     F    1902  19  19 --> class hot*/
        /*     P      F     P     201  19   2 -->class hot*/
        /*     P      F     F    1902  19  19 --> class hot*/

        /*   + F      P     P     200  61   2 --> class hot*/
        /*   + F      P     F    1902  61  19 --> class hot*/
        /*     F      F     P     201  61   2 -->class hot*/
        /*     F      F     F    6101  61  61 -->class hot*/
        /* + means it shouldn't happen but needs be accounted for */
        private static uint[] fbrepairbin_classhot = { 0, 1902, 201, 1902, 200, 1902, 201, 6101 };

        private static uint[] dbrepairbin_classhot = { 1, 19, 2, 19, 2, 19, 2, 61 };

        private static uint[] commonbins = { 30, 31, 46, 41, 18, 20, 27, 29, 35, 6, 8, 10, 11, 9, 15, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98, 99 };

        public static uint doBinning(List<string> passcounters, List<string> failcounters, StringBuilder sb)
        {
            if(TestFlow.isSort()) 
            {
                return doSortBinning(passcounters, failcounters, sb);
            }
            return doClassBinning(passcounters, failcounters, sb);
        }

        private static uint doClassBinning(List<string> passcounters, List<string> failcounters, StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        private static uint doSortBinning(List<string> passcounters, List<string> failcounters, StringBuilder sb)
        {
            uint priority = 100;
            uint ib = 1;
            uint fb = 100;
            uint bfnrr_bin = 6101;
            uint repair_index = 0;

            foreach (string failcounter in failcounters) /* find fails and set possible bins  */
            {
                uint fcounter = uint.Parse(failcounter);
                uint fcounterb = fcounter / 100;

                if (fcounter > 9999 || fcounter < 0) continue;  /* skip pattern counters above 10000 */
                //if(failcounter > 9999) continue;  /* skip pattern counters above 10000 */
                if (fcounterb == 49) { repair_index |= (1 << 2); continue; } /* set ks vhi stm pbist flag */
                if (fcounterb == 50) { repair_index |= (1 << 1); continue; } /* set hvqk pre repair flag  */
                if ((fcounterb == 61) || (fcounterb == 62) || (fcounterb == 63) || (fcounterb == 12)) /*  raster bins non-redundant */
                {
                    repair_index |= (1 << 4);
                    bfnrr_bin = fcounter;
                    continue;
                }
                if (fcounterb == 2) { repair_index |= (1 << 3); continue; }   /* pre fuse prog non redundant fail */
                if (fcounterb == 19) { repair_index |= 1; continue; }   /* post fuse prog redundant test fail */

                uint j = fcounterb;

                if ((failbinpriority[j] < priority) && (failbinpriority[j] != 0))
                {
                    ib = j;
                    fb = fcounter;
                    priority = failbinpriority[j];
                }
            }

            uint prior_fb = 0;
            uint prior_db = 0;
            uint prior_ib = 0;

            if (repair_index != 0) /* if there was any repair section binning */
            {
                prior_fb = (fbrepairbin[repair_index] == 6101) ? bfnrr_bin : fbrepairbin[repair_index]; /* if 61,62,63 or 12 set func bin right */
                prior_db = (dbrepairbin[repair_index] == 61) ? (bfnrr_bin / 100) : dbrepairbin[repair_index]; /* if 61,62,63,or 12 set to that */
                prior_ib = (ibrepairbin[repair_index] == 61) ? (bfnrr_bin / 100) : ibrepairbin[repair_index]; /* same for ib if 61,62,63 or 12 */
            }

            uint db = (prior_db != 0) ? prior_db : ib;  /* set ISO bin - now db */

            if ((failbinpriority[prior_ib] < priority) && (failbinpriority[prior_ib] != 0))
            {
                ib = prior_ib;
                fb = prior_fb;
            }

            //New IB implementation for class
            bool p2893 = false;
            bool p2894 = false;
            bool p2895 = false;
            bool p2896 = false;

            foreach (string passcounterstr in passcounters) /* find passing summary counters and set possible bins  */
            {
                uint passcounter = uint.Parse(passcounterstr);
                if (passcounter == 2893)
                {
                    p2893 = true; 
                }
                if (passcounter == 2894)
                {
                    p2894 = true;
                }
                if (passcounter == 2895)
                {
                    p2895 = true;
                }
                if (passcounter == 2896)
                {
                    p2896 = true;
                }
            }

            if (ib > 2 && ib < 90 && ib != 8 && ib != 18 && ib != 11)
            {
                if (p2893)
                {
                    ib = 3;
                }
                else if (p2894)
                {
                    ib = 4;
                }
                else if (p2895)
                {
                    ib = 5;
                }
                else if (p2896)
                {
                    ib = 6;
                }
            }

            setBins(sb, ib, db, fb);
            return (db * 100) + 1;
        }

        private static void setBins(StringBuilder sb, uint ib, uint db, uint fb)
        {
            sb.AppendLine("3_curfbin_" + fb);
            sb.AppendLine("3_binn_" + db);
            sb.AppendLine("3_curibin_" + ib);
            BinningAndLotDataStore.setBin(db, fb, ib);

            if (db <= 7)
            {
                sb.AppendLine("3_trslt_pass");
            }
            else
            {
                sb.AppendLine("3_trslt_fail");
            }
        }
    }
}
