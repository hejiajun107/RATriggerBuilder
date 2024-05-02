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

        public static Func<int, (string, string)> NextIdAction = count => ("03A" + TriggerCounter.ToString().PadLeft(5, '0'), ("03B" + TriggerCounter.ToString().PadLeft(5, '0')));
        public static Func<int, (string, string)> NextTeamAction = count => ("A" + TeamCounter.ToString().PadLeft(5, '0'), ("03A" + TeamCounter.ToString().PadLeft(5, '0') + "-G"));
        public static Func<int, (string, string)> NextTaskForceAction = count => ("A" + TaskForceCounter.ToString().PadLeft(5, '0'), ("03B" + TaskForceCounter.ToString().PadLeft(5, '0') + "-G"));
        public static Func<int, (string, string)> NextScriptAction = count => ("A" + ScriptCounter.ToString().PadLeft(5, '0'), ("03C" + ScriptCounter.ToString().PadLeft(5, '0') + "-G"));


        public static (string trigger,string tag) NextId()
        {
            TriggerCounter++;
            return NextIdAction(TriggerCounter);
        }

        private static int TeamCounter = 0;

        public static (string id,string name) NextTeam()
        {
            TeamCounter++;
            return NextTeamAction(TeamCounter);
        }

        private static int TaskForceCounter = 0;

        public static (string id, string name) NextTaskForce()
        {
            TaskForceCounter++;
            return NextTaskForceAction(TaskForceCounter);
        }

        private static int ScriptCounter = 0;

        public static (string id, string name) NextScript()
        {
            ScriptCounter++;
            return NextScriptAction(ScriptCounter);
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
                return "03C" + num.ToString().PadLeft(5, '0');
            }
            else
            {
                throw new Exception("固定Id已被使用");
            }
        }

        
    }   
}
