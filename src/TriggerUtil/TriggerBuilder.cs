namespace TriggerUtil
{
    /// <summary>
    /// TiggerBuilder
    /// 所有事件(Event)都用On开头
    /// 所有动作(Action)都用Do开头
    /// 涉及到需要用序号的如所属保留
    /// </summary>
    public class TriggerBuilder
    {

        public TriggerBuilder(TriggerContext context) 
        {
            UniqueId = IdGenerator.NextId();
            TriggerName = UniqueId;
        }

        private List<string> actions = new List<string>();
        private List<string> events = new List<string>();

        public string TriggerName { get; private set; }

        public string UniqueId { get; private set; }

        /// <summary>
        /// 重复类型
        /// </summary>
        public RepeatType RepeatType { get; private set; } = RepeatType.OneTimeOr;

        /// <summary>
        /// 初始启用禁止
        /// </summary>
        public bool Disabled { get; private set; } = false;
        
        /// <summary>
        /// 关联触发
        /// </summary>
        public string RelateTriggerId { get; private set; }

        /// <summary>
        /// 简单难度中可用
        /// </summary>
        public bool EasyEnabled { get; private set; } = true;
        /// <summary>
        /// 普通难度中可用
        /// </summary>
        public bool NormalEnabled { get; private set; } = true;
        /// <summary>
        /// 困难难度中可用
        /// </summary>
        public bool HardEnabled { get; private set; } = true;


        /// <summary>
        /// 触发所属方
        /// </summary>
        private string? _owner;

        /// <summary>
        /// 设置触发的名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TriggerBuilder Name(string name)
        {
            TriggerName = name;
            return this;
        }

        public TriggerBuilder Repeat(RepeatType repeatType)
        {
            RepeatType = repeatType;
            return this;
        }

        public TriggerBuilder Diffulties(bool easy,bool normal,bool hard)
        {
            EasyEnabled = easy;
            NormalEnabled = normal;
            HardEnabled = hard;
            return this;
        }

        public string BuildTagString()
        {
            return $"{RepeatType.GetHashCode()},{TriggerName},{UniqueId}";
        }

        public string BuildTriggerString()
        {
            return $"{_owner},{(string.IsNullOrWhiteSpace(RelateTriggerId) ? "<none>" : RelateTriggerId)},{TriggerName},{(Disabled ? 1 : 0)},{(EasyEnabled ? 1 : 0)},{(NormalEnabled ? 1 : 0)},{(HardEnabled ? 1 : 0)},0";
        }

        public string BuildActionString()
        {
            return $"{actions.Count},{string.Join(",", actions)}";
        }

        public string BuildEventsString()
        {
            return $"{actions.Count},{string.Join(",", events)}";
        }


        /// <summary>
        /// 设置触发所属
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public TriggerBuilder Owner(Enum owner)
        {
            _owner = owner.ToString();
            return this;
        }

        /// <summary>
        /// 下一个触发，在动作中会开启下一个触发
        /// </summary>
        /// <param name="nextTrigger"></param>
        /// <returns></returns>
        public TriggerBuilder Next(TriggerBuilder nextTrigger)
        {
            DoEnable(nextTrigger.UniqueId);
            return this;
        }

        /// <summary>
        /// 设置关联触发
        /// </summary>
        /// <param name="relateTrigger"></param>
        /// <returns></returns>
        public TriggerBuilder Relate(TriggerBuilder relateTrigger)
        {
            Relate(relateTrigger.UniqueId);
            return this;
        }

        /// <summary>
        /// 设置关联触发
        /// </summary>
        /// <param name="relateTriggerId"></param>
        /// <returns></returns>
        public TriggerBuilder Relate(string relateTriggerId)
        {
            RelateTriggerId = relateTriggerId;
            return this;
        }


        #region 条件
        /// <summary>
        /// 无
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnNothing()
        {
            events.Add("0,0,0");
            return this;
        }

        /// <summary>
        /// 当流逝的时间达到特定值时触发此事件。当触发被允许时计时器初始化；若触发是重复的，时间达到指定值时，计时器复位。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnter(Enum owner)
        {
            return OnEnter(owner.GetHashCode());
        }

        /// <summary>
        /// 当流逝的时间达到特定值时触发此事件。当触发被允许时计时器初始化；若触发是重复的，时间达到指定值时，计时器复位。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnter(int owner)
        {
            events.Add($"1,0,{owner}");
            return this;
        }


        /// <summary>
        /// 当关联的对象被玩家发现时触发此事件。被发现意味着从黑幕中暴露。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnDiscoveredByPlayer()
        {
            events.Add("4,0,0");
            return this;
        }

        /// <summary>
        /// 当特定所属方的任一单位或建筑被玩家发现时触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnHouseDiscoveredByPlayer(Enum house)
        {
            return OnHouseDiscoveredByPlayer(house.GetHashCode());
        }

        /// <summary>
        /// 当特定所属方的任一单位或建筑被玩家发现时触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnHouseDiscoveredByPlayer(int house)
        {
            events.Add($"5,0,{house}");
            return this;
        }

        /// <summary>
        /// 当关联的对象受到一些方式的攻击时，触发此事件。间接伤害或友军开火不包括在内，若物体直接被该攻击摧毁则无法触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnAttacked()
        {
            events.Add($"6,0,0");
            return this;
        }

        /// <summary>
        /// 当关联的对象被摧毁时，触发此事件。间接伤害或友军开火造成的摧毁不包括在内。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnDestroyed()
        {
            events.Add($"6,0,0");
            return this;
        }

        /// <summary>
        /// 这个条件永远满足，单独使用时必定会触发事件。不要将其用于重复触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnAnything()
        {
            events.Add($"8,0,0");
            return this;
        }

        /// <summary>
        /// 当特定所属方的所有单位被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如平民不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnUnitAllDestroyed(Enum house)
        {
            return OnUnitAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 当特定所属方的所有单位被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如平民不会被算入判定条件中。
        ///         /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnUnitAllDestroyed(int house)
        {
            events.Add($"9,0,{house}");
            return this;
        }


        /// <summary>
        /// 当特定所属方的所有建筑物被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如基地摆件不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingAllDestroyed(Enum house)
        {
            return OnBuildingAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 当特定所属方的所有建筑物被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如基地摆件不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingAllDestroyed(int house)
        {
            events.Add($"10,0,{house}");
            return this;
        }



        /// <summary>
        /// 当特定所属方的所有对象被摧毁时触发此事件。这是常规的游戏结束触发事件(全部摧毁)。中立对象如科技建筑不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnObjectAllDestroyed(Enum house)
        {
            return OnObjectAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 当特定所属方的所有对象被摧毁时触发此事件。这是常规的游戏结束触发事件(全部摧毁)。中立对象如科技建筑不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnObjectAllDestroyed(int house)
        {
            events.Add($"11,0,{house}");
            return this;
        }

        /// <summary>
        /// 当触发所属方的金钱超过指定值时触发此事件。
        /// </summary>
        /// <param name="cash">金额</param>
        /// <returns></returns>
        public TriggerBuilder OnCashMoreThan(int cash)
        {
            events.Add($"12,0,{cash}");
            return this;
        }

        /// <summary>
        /// 当流逝的时间达到特定值时触发此事件。当触发被允许时计时器初始化；若触发是重复的，时间达到指定值时，计时器复位。
        /// </summary>
        /// <param name="timer">计时器值</param>
        /// <returns></returns>
        public TriggerBuilder OnTimeElapse(int timer)
        {
            events.Add($"13,0,{timer}");
            return this;
        }


        /// <summary>
        /// 当全局的任务计时器(显示在屏幕右下角)倒计时为零时触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnTimerEnded()
        {
            events.Add($"14,0,0");
            return this;
        }

        /// <summary>
        /// 当触发所属方的特定数量的「建筑」被摧毁时触发此事件。
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingDestroyed(int count)
        {
            events.Add($"15,0,{count}");
            return this;
        }

        /// <summary>
        /// 当触发所属方的特定数量的「单位」被摧毁时触发此事件。
        /// </summary>
        /// <param name="count">数量</param>
        /// <returns></returns>
        public TriggerBuilder OnUnitDestroyed(int count)
        {
            events.Add($"16,0,{count}");
            return this;
        }

        /// <summary>
        /// 当触发所属方没有生产建筑(如基地、兵营)时触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnNoMoreFactory()
        {
            events.Add($"17,0,0");
            return this;
        }

        /// <summary>
        /// 当触发所属方建造指定类型的「建筑」时触发此事件。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceBuilding(Enum buildingType)
        {
            return OnProduceBuilding(buildingType.GetHashCode());
        }

        /// <summary>
        /// 当触发所属方建造指定类型的「建筑」时触发此事件。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceBuilding(int buildingType)
        {
            events.Add($"19,0,{buildingType}");
            return this;
        }


        /// <summary>
        /// 当触发所属方生产指定类型的「载具(包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">载具序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceUnit(Enum unitType)
        {
            return OnProduceUnit(unitType.GetHashCode());
        }

        /// <summary>
        /// 当触发所属方生产指定类型的「载具(包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">载具序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceUnit(int unitType)
        {
            events.Add($"20,0,{unitType}");
            return this;
        }


        /// <summary>
        /// 当触发所属方训练指定类型的「步兵」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceInfantry(Enum infantryType)
        {
            return OnProduceInfantry(infantryType.GetHashCode());
        }

        /// <summary>
        /// 当触发所属方训练指定类型的「步兵」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号/param>
        /// <returns></returns>
        public TriggerBuilder OnProduceInfantry(int infantryType)
        {
            events.Add($"21,0,{infantryType}");
            return this;
        }

        /// <summary>
        /// 当触发所属方生产指定类型的「飞行器(不包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceAircraft(Enum aircraftType)
        {
            return OnProduceAircraft(aircraftType.GetHashCode());
        }

        /// <summary>
        /// 当触发所属方生产指定类型的「飞行器(不包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号/param>
        /// <returns></returns>
        public TriggerBuilder OnProduceAircraft(int aircraftType)
        {
            events.Add($"22,0,{aircraftType}");
            return this;
        }







        #endregion


        #region 结果
        /// <summary>
        /// 空行为，没作用
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoNothing()
        {
            actions.Add($"0,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 特定的所属方会成为胜利者，游戏会立即结束。通常被指定的是玩家所属方，单人战役若胜利方不是玩家所属方，则玩家失败。多人地图时此结果变为“失败者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareWinner(Enum owner)
        {
            return DoDeclareWinner(owner.GetHashCode());
        }
        /// <summary>
        /// 特定的所属方会成为胜利者，游戏会立即结束。通常被指定的是玩家所属方，单人战役若胜利方不是玩家所属方，则玩家失败。多人地图时此结果变为“失败者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareWinner(int owner)
        {
            actions.Add($"1,0,{owner},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 特定的所属方会成为失败者，游戏会立即结束。通常被指定的是玩家所属方。多人地图时此结果变为“胜利者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareLoser(Enum owner)
        {
            return DoDeclareLoser(owner.GetHashCode());
        }
        /// <summary>
        /// 特定的所属方会成为失败者，游戏会立即结束。通常被指定的是玩家所属方。多人地图时此结果变为“胜利者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareLoser(int owner)
        {
            actions.Add($"2,0,{owner.GetHashCode()},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 允许触发
        /// </summary>
        /// <param name="triggerId">触发Id</param>
        /// <returns></returns>
        public TriggerBuilder DoEnable(string triggerId)
        {
            actions.Add($"53,2,{triggerId},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 禁止触发
        /// </summary>
        /// <param name="triggerId"></param>
        /// <returns></returns>
        public TriggerBuilder DoDisable(string triggerId)
        {
            actions.Add($"54,2,{triggerId},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 禁止当前触发
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoDisableSelf()
        {
            actions.Add($"54,2,{this.UniqueId},0,0,0,0,A");
            return this;
        }







        /// <summary>
        /// 让特定所属方的所有空闲的步兵单位执行欢呼动作。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoCheer(Enum owner)
        {
            return DoCheer(owner.GetHashCode());
        }
        /// <summary>
        /// 让特定所属方的所有空闲的步兵单位执行欢呼动作。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoCheer(int owner)
        {
            actions.Add($"113,0,{owner},0,0,0,0,A");
            return this;
        }


        #endregion



    }
}