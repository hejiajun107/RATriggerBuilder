// See https://aka.ms/new-console-template for more information
using TriggerUtil;


const string yuri = "YuriCountry";

TriggerBuilder.CreateBuilder().Name("AutoWin").Owner(yuri).OnEnter(yuri)
    .Next(
        TriggerBuilder.CreateBuilder().Owner(yuri).OnTimeElapse(100).Next
        (
            TriggerBuilder.CreateBuilder().Owner(yuri).OnTimeElapse(100).DoDeclareWinner(yuri)
        )
    );
