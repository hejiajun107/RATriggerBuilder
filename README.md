# RATriggerBuilder
## A library to write triggers in RA2 with fluent api
### For Example
```
  Context.CreateTrigger().Name("AutoWin").Owner(Country.YuriCountry).OnEnter(Country.YuriCountry).DoDisableSelf()
            .Next(
                Context.CreateTrigger().Owner(Country.YuriCountry).OnTimeElapse(100).DoCheer(Country.YuriCountry).DoDisableSelf().Next
                (
                    Context.CreateTrigger().Owner(Country.YuriCountry).OnTimeElapse(100).DoDeclareWinner(Country.YuriCountry).DoDisableSelf()
                )
            );
```

### what to do next
* Fullfill the actions and events
* Provide tools to generate code from ini
* Support writing AI teams/scripts with fluent api

### Credits
All annotions is from FA2SP