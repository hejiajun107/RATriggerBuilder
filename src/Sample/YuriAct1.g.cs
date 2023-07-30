using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggerUtil;

namespace Sample
{
    public partial class YuriAct1 
    {

        internal class BuildingTypes
        {
           public const int NAHAND = 100;
           public const int GAWEAP = 200;
        }

        internal enum Country
        {
           American,
           Russian,
           YuriCountry = 9
        }

        internal enum Eva
        {
            //如果注册表中不符合变量名的命名规则（如出现不允许的字符，或者以数字符号靠头等）可以写这个Attribute，具体可参考自动生成的代码
            [EnumKey(Name = "EVA_MissionAccomplished")]
            EVA_MissionAccomplished,
            EVA_BattleControlTerminated,
            EVA_PrimaryObjectiveAchieved,
            EVA_EstablishBattlefieldControl,
            EVA_IncomingTransmission,
            EVA_ReinforcementsHaveArrived,
            EVA_BattleControlOnline,
            EVA_BattlefieldControlOnline,
            EVA_MissionFailed
        }
    }
}
