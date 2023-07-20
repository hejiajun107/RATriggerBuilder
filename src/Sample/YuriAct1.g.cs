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
