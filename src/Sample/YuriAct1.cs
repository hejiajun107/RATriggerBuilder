using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggerUtil;

namespace Sample
{
    public partial class YuriAct1 : TriggerTemplate
    {
        protected override void Process()
        {
            Context.CreateTrigger().Name("Start").Owner(Country.YuriCountry).OnAnything().DoDisablePlayerControl().DoDisableSelf()
               .Next(x => x.Owner(Country.YuriCountry).OnAnything().DoDisableSelf().DoPlayEva(Eva.EVA_EstablishBattlefieldControl.ToString()))
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(5).DoTriggerText("Mission:yr01umd1").DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoRevealWayPoint(365).DoMoveViewTo(ViewMoveSpeed.Normal, 365).DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoTriggerText("Mission:yr01umd2").DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(20).DoRevealWayPoint(307).DoMoveViewTo(ViewMoveSpeed.Normal, 307).DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoTriggerText("Mission:yr01umd3").DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoRevealWayPoint(62).DoMoveViewTo(ViewMoveSpeed.Normal, 59).DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoReinforcements("01000032").DoReinforcements("01000033").DoPlayEva(Eva.EVA_ReinforcementsHaveArrived.ToString()).DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoTriggerText("Mission:yr01umd4").DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoTriggerText("Mission:yr01umd5").DoDisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).OnTimeElapse(10).DoPlayEva(Eva.EVA_BattleControlTerminated.ToString()).DoDisableSelf().DoEnablePlayerControl())
               ;
        }
    }
}
