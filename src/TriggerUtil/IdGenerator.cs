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

        public static string NextId()
        {
            return "03" + TriggerCounter++.ToString().PadLeft(6, '0');
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

        
    }   
}
