# RATriggerBuilder
A library to write triggers in RA2 with fluent api
> 一个使用流畅 API 编写红警2触发器的库

## 🚀 Features / 特性
- **Fluent API Design** - Write trigger logic through intuitive chain calls
  > **流畅的 API 设计** - 通过直观的链式调用编写触发器逻辑
- **Strong Type Support** - Compile-time type checking reduces runtime errors
  > **强类型支持** - 编译时类型检查，减少运行时错误
- **Code as Configuration** - Use C# code instead of traditional INI configuration, enjoy IDE intelligent prompts and auto-completion
  > **代码即配置** - 用 C# 代码代替传统的 INI 配置，享受 IDE 智能提示和自动补全
- **Reusable Components** - Support trigger templates and loop generation, easy to create in batches
  > **可复用组件** - 支持触发器模板和循环生成，便于批量创建
- **AI Script Support** - Also supports writing AI teams and scripts with fluent API
  > **AI 脚本支持** - 同样支持用流畅 API 编写 AI 队伍和脚本

## ✨ Advantages / 优势
- **Development Efficiency Improvement** - Compared to manually writing INI configuration, code writing speed increases by 3-5 times
  > **开发效率提升** - 相比手动编写 INI 配置，代码编写速度提升 3-5 倍
- **Excellent Maintainability** - After modifying global settings like country registry, recompile to synchronize updates to all related triggers
  > **维护性极佳** - 修改国家注册表等全局设置后，重新编译即可同步更新所有相关触发器
- **Visual Preview** - Built-in generator can preview trigger effects in real-time
  > **可视化预览** - 内置生成器可实时预览触发器效果
- **Early Error Detection** - Discover syntax and logic errors at compile time, not runtime
  > **错误早发现** - 编译时即可发现语法和逻辑错误，而非运行时
- **Low Learning Cost** - If you are familiar with C#, you can get started almost immediately
  > **学习成本低** - 如果你熟悉 C#，几乎可以立即上手

## 💡 Benefits / 实际收益
- **Save Significant Time** - Complex trigger chains that originally took hours to manually configure now only require a few minutes of coding
  > **节省大量时间** - 原本需要数小时手动配置的复杂触发器链，现在只需几分钟代码编写
- **Reduce Error Rate** - Strong type system prevents common spelling errors and parameter errors
  > **降低出错率** - 强类型系统防止了常见的拼写错误和参数错误
- **Facilitate Team Collaboration** - Stored in code form, easy for version control and code review
  > **便于团队协作** - 代码形式存储，易于版本控制和代码审查
- **Rapid Iteration** - Modify trigger logic and regenerate immediately without manually finding and modifying multiple INI entries
  > **快速迭代** - 修改触发器逻辑后立即重新生成，无需手动查找和修改多个 INI 条目
- **Long-term Maintainability** - Even after months of review, the clear API allows you to quickly understand the original logic
  > **长期可维护** - 即使数月后回顾代码，清晰的 API 也能让你快速理解原有逻辑

## Usage Example / 使用示例
```c#
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

Generator previews:
![4f526633b8b38a7a250f4cbc662228eb](https://github.com/user-attachments/assets/b7552808-88f7-4100-99ba-909d951c83a2)
![2cb2220cf4f1cc265274fc0614955c59](https://github.com/user-attachments/assets/11890bfb-f2df-46da-82c3-1d073249055b)

## what to do next / 后续计划
- [ ] Fullfill the actions and events
  > 完善所有动作和事件的 API 支持
- [x] Provide tools to generate code from ini
  > 提供从 INI 生成代码的工具
- [x] Support writing AI teams/scripts with fluent api
  > 支持用流畅 API 编写 AI 队伍/脚本

## Credits / 致谢
- All annotions is from FA2SP
  > 所有注释来自 FA2SP
- Annotions of AI is from AI的艺♂术(2019年4月版AI教程) by Madman_M
  > AI 相关注释来自《AI的艺♂术》(2019年4月版AI教程) by Madman_M
