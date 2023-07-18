using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil
{
    public class IdGenerator
    {
        private static int TriggerCounter = 0;

        public static (string trigger,string tag) NextId()
        {
            TriggerCounter++;
            return ("03A" + TriggerCounter.ToString().PadLeft(5, '0'), ("03B" + TriggerCounter.ToString().PadLeft(5, '0')));
            //switch (idType)
            //{
            //    case IdType.Trigger:
            //        return "01" + TriggerCounter++.ToString().PadLeft(6, '0');
            //    case IdType.Event:
            //        return "02" + EventCounter++.ToString().PadLeft(6, '0');
            //    case IdType.Action:
            //        return "03" + ActionCounter++.ToString().PadLeft(6, '0');
            //    default:
            //        throw new Exception();
            //}
        }

        private static HashSet<int> fiexedNums = new HashSet<int>();

        /// <summary>
        /// 获取固定Id
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string FixedId(int num)
        {
            if(fiexedNums.Add(num))
            {
                return "04" + num.ToString().PadLeft(6, '0');
            }
            else
            {
                throw new Exception("固定Id已被使用");
            }
        }

        
    }   
}
