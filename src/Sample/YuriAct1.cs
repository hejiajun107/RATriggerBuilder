using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggerUtil;

namespace Sample
{
    public class YuriAct1 : TriggerTemplate
    {
        protected override void Process()
        {
            Context.CreateTrigger().Name("AutoWin").Owner(Country.YuriCountry).OnEnter(Country.YuriCountry)
            .Next(
                Context.CreateTrigger().Owner(Country.YuriCountry).OnTimeElapse(100).Next
                (
                    Context.CreateTrigger().Owner(Country.YuriCountry).OnTimeElapse(100).DoDeclareWinner(Country.YuriCountry)
                )
            );
        }


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
    }
}
