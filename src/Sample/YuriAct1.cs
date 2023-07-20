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
            Context.CreateTrigger().Name("Start").SetDescription("只是个注释").Owner(Country.YuriCountry).When(e => e.Anything()).Then(a => a.DisablePlayerControl().DisableSelf())
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.Anything()).Then(a => a.DisableSelf().PlayEva(Eva.EVA_EstablishBattlefieldControl.ToString())))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(5)).Then(a => a.TriggerText("Mission:yr01umd1").DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.RevealWayPoint(365).MoveViewTo(ViewMoveSpeed.Normal, 365).DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.TriggerText("Mission:yr01umd2").DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(20)).Then(a => a.RevealWayPoint(307).MoveViewTo(ViewMoveSpeed.Normal, 307)).Then(a => a.DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.TriggerText("Mission:yr01umd3").DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.RevealWayPoint(62).MoveViewTo(ViewMoveSpeed.Normal, 59).DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.Reinforcements("01000032").Reinforcements("01000033").PlayEva(Eva.EVA_ReinforcementsHaveArrived.ToString()).DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.TriggerText("Mission:yr01umd4").DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.TriggerText("Mission:yr01umd5").DisableSelf()))
               .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(10)).Then(a => a.PlayEva(Eva.EVA_BattleControlTerminated.ToString()).DisableSelf().EnablePlayerControl()))
               ;

            Context.CreateTrigger().Name("Game Over").SetDescription("游戏失败").Owner(Country.YuriCountry).When(e => e.ObjectAllDestroyed(Country.YuriCountry).TimeElapse(3)).Then(a => a.DisablePlayerControl().DisableSelf())
                .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(5)).Then(a => a.PlayEva(Eva.EVA_MissionFailed.ToString())))
                .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(5)).Then(a => a.DeclareLoser(Country.YuriCountry)));

            Context.CreateTrigger().Name("Win").SetFixedTag(1).SetDescription("任务完成").Diffulties(true, true, false).Owner(Country.YuriCountry).When(e => e.DestroyedByAnything().TimeElapse(3)).Then(a => a.DisablePlayerControl().DisableSelf())
                 .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(5)).Then(a => a.Cheer(Country.YuriCountry)))
                 .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(5)).Then(a => a.PlayEva(Eva.EVA_MissionAccomplished.ToString())))
                 .Next(x => x.Owner(Country.YuriCountry).When(e => e.TimeElapse(5)).Then(a => a.DeclareWinner(Country.YuriCountry)));

        }
    }
}
