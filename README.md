# RATriggerBuilder
## A library to write triggers in RA2 with fluent api
### For Example
```
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
```

### what to do next
* Fullfill the actions and events
* Provide tools to generate code from ini
* Support writing AI teams/scripts with fluent api

### Credits
All annotions is from FA2SP