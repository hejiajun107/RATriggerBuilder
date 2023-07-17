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
            _context = context;
            UniqueId = IdGenerator.NextId();
            TriggerName = UniqueId;
            Tag = IdGenerator.NextId();
        }

        private TriggerContext _context;

        private List<string> actions = new List<string>();
        private List<string> events = new List<string>();

        public string Tag { get; private set; }

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
            var str = $"{RepeatType.GetHashCode()},{TriggerName},{UniqueId}";
            if (str.Length >= 512)
                throw new ArgumentOutOfRangeException($"{str} of {TriggerName} is too long");
            return str;
        }

        public string BuildTriggerString()
        {
            var str = $"{_owner},{(string.IsNullOrWhiteSpace(RelateTriggerId) ? "<none>" : RelateTriggerId)},{TriggerName},{(Disabled ? 1 : 0)},{(EasyEnabled ? 1 : 0)},{(NormalEnabled ? 1 : 0)},{(HardEnabled ? 1 : 0)},0";
            if (str.Length >= 512)
                throw new ArgumentOutOfRangeException($"{str} of {TriggerName} is too long");
            return str;
        }

        public string BuildActionString()
        {
            var str = $"{actions.Count},{string.Join(",", actions)}";
            if (str.Length >= 512)
                throw new ArgumentOutOfRangeException($"{str} of {TriggerName} is too long");
            return str;
        }

        public string BuildEventsString()
        {
            var str = $"{events.Count},{string.Join(",", events)}";
            if (str.Length >= 512)
                throw new ArgumentOutOfRangeException($"{str} of {TriggerName} is too long");
            return str;
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

        public TriggerBuilder SetDisabled(bool disabled = true)
        {
            Disabled = disabled;
            return this;
        }


        /// <summary>
        /// 包含触发，在动作中会开启下一个触发，并返回当前触发
        /// </summary>
        /// <param name="nextTrigger"></param>
        /// <returns></returns>
        public TriggerBuilder Contain(TriggerBuilder nextTrigger)
        {
            nextTrigger.Disabled = true;
            DoEnable(nextTrigger.UniqueId);
            return this;
        }

        /// <summary>
        /// 下一个触发，在动作中会开启下一个触发，并返回下一个触发
        /// </summary>
        /// <param name="nextTrigger"></param>
        /// <returns></returns>
        public TriggerBuilder Next(TriggerBuilder nextTrigger)
        {
            nextTrigger.Disabled = true;
            DoEnable(nextTrigger.UniqueId);
            return nextTrigger;
        }

        /// <summary>
        /// 下一个触发，在动作中会开启下一个触发，并返回下一个触发
        /// </summary>
        /// <param name="nextTrigger"></param>
        /// <returns></returns>
        public TriggerBuilder Next(Action<TriggerBuilder> next)
        {
            var trigger = _context.CreateTrigger();
            trigger.Disabled = true;
            next(trigger);
            DoEnable(trigger.UniqueId);
            return trigger;
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
        /// 进入事件
        /// 当步兵或载具进入关联的对象时，触发此事件。此触发的标签可关联到一个建筑、运输载具或单元标记上。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnter(Enum owner)
        {
            return OnEnter(owner.GetHashCode());
        }

        /// <summary>
        /// 进入事件
        /// 当步兵或载具进入关联的对象时，触发此事件。此触发的标签可关联到一个建筑、运输载具或单元标记上。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnter(int owner)
        {
            events.Add($"1,0,{owner}");
            return this;
        }


        /// <summary>
        /// 关联对象被玩家发现
        /// 当关联的对象被玩家发现时触发此事件。被发现意味着从黑幕中暴露。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnDiscoveredByPlayer()
        {
            events.Add("4,0,0");
            return this;
        }

        /// <summary>
        /// 所属方被玩家发现
        /// 当特定所属方的任一单位或建筑被玩家发现时触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnHouseDiscoveredByPlayer(Enum house)
        {
            return OnHouseDiscoveredByPlayer(house.GetHashCode());
        }

        /// <summary>
        /// 所属方被玩家发现
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
        /// 被任一所属方攻击
        /// 当关联的对象受到一些方式的攻击时，触发此事件。间接伤害或友军开火不包括在内，若物体直接被该攻击摧毁则无法触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnAttacked()
        {
            events.Add($"6,0,0");
            return this;
        }

        /// <summary>
        /// 被任一所属方摧毁
        /// 当关联的对象被摧毁时，触发此事件。间接伤害或友军开火造成的摧毁不包括在内。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnDestroyed()
        {
            events.Add($"6,0,0");
            return this;
        }

        /// <summary>
        /// 任何事件
        /// 这个条件永远满足，单独使用时必定会触发事件。不要将其用于重复触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnAnything()
        {
            events.Add($"8,0,0");
            return this;
        }

        /// <summary>
        /// 单位全部被摧毁
        /// 当特定所属方的所有单位被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如平民不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnUnitAllDestroyed(Enum house)
        {
            return OnUnitAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 单位全部被摧毁
        /// 当特定所属方的所有单位被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如平民不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnUnitAllDestroyed(int house)
        {
            events.Add($"9,0,{house}");
            return this;
        }


        /// <summary>
        /// 建筑全部被摧毁
        /// 当特定所属方的所有建筑物被摧毁时触发此事件。常使用在游戏的结束条件中。中立对象如基地摆件不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingAllDestroyed(Enum house)
        {
            return OnBuildingAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 建筑全部被摧毁
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
        /// 所有对象全部被摧毁
        /// 当特定所属方的所有对象被摧毁时触发此事件。这是常规的游戏结束触发事件(全部摧毁)。中立对象如科技建筑不会被算入判定条件中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnObjectAllDestroyed(Enum house)
        {
            return OnObjectAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 所有对象全部被摧毁
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
        /// 金钱超过...
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
        /// 流逝时间...
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
        /// 计时器时间已到
        /// 当全局的任务计时器(显示在屏幕右下角)倒计时为零时触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnTimerEnded()
        {
            events.Add($"14,0,0");
            return this;
        }

        /// <summary>
        /// 建筑被摧毁X个
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
        /// 16 单位被摧毁X个
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
        /// 不再有工厂
        /// 当触发所属方没有生产建筑(如基地、兵营)时触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnNoMoreFactory()
        {
            events.Add($"17,0,0");
            return this;
        }

        /// <summary>
        /// 建造特定类型的建筑...
        /// 当触发所属方建造指定类型的「建筑」时触发此事件。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceBuilding(Enum buildingType)
        {
            return OnProduceBuilding(buildingType.GetHashCode());
        }

        /// <summary>
        /// 建造特定类型的建筑...
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
        /// 生产特定类型的载具...
        /// 当触发所属方生产指定类型的「载具(包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">载具序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceUnit(Enum unitType)
        {
            return OnProduceUnit(unitType.GetHashCode());
        }

        /// <summary>
        /// 生产特定类型的载具...
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
        /// 训练特定类型的步兵...
        /// 当触发所属方训练指定类型的「步兵」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceInfantry(Enum infantryType)
        {
            return OnProduceInfantry(infantryType.GetHashCode());
        }

        /// <summary>
        /// 训练特定类型的步兵...
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
        /// 生产特定类型的飞机...
        /// 当触发所属方生产指定类型的「飞行器(不包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号</param>
        /// <returns></returns>
        public TriggerBuilder OnProduceAircraft(Enum aircraftType)
        {
            return OnProduceAircraft(aircraftType.GetHashCode());
        }

        /// <summary>
        /// 生产特定类型的飞机...
        /// 当触发所属方生产指定类型的「飞行器(不包括直升机)」时触发此事件。
        /// </summary>
        /// <param name="unitType">步兵序号/param>
        /// <returns></returns>
        public TriggerBuilder OnProduceAircraft(int aircraftType)
        {
            events.Add($"22,0,{aircraftType}");
            return this;
        }

        /// <summary>
        ///  作战小队离开地图
        /// 【不确定】当特定的作战小队离开地图时触发此事件，若作战小队被摧毁则不会触发。就算作战小队只剩一个成员但离开了地图，事件也将被触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnTeamLeave()
        {
            events.Add($"23,1,0");
            return this;
        }

        /// <summary>
        /// 进入某区域
        /// 当特定所属方的一个单位进入关联的「仅一个」单元标记划定的「区域」时，触发此事件。「区域」指的是一个单位能过来的地区，或者是一个地面单位能通过寻路逻辑抵达的地区，且必须是「孤立」的。详细解释另见工具教程里的“触发备忘”。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnterArea(Enum house)
        {
            return OnEnterArea(house.GetHashCode());
        }

        /// <summary>
        /// 进入某区域
        /// 当特定所属方的一个单位进入关联的「仅一个」单元标记划定的「区域」时，触发此事件。「区域」指的是一个单位能过来的地区，或者是一个地面单位能通过寻路逻辑抵达的地区，且必须是「孤立」的。详细解释另见工具教程里的“触发备忘”。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnterArea(int house)
        {
            events.Add($"24,0,{house}");
            return this;
        }

        /// <summary>
        /// 越过水平线
        /// 当特定所属方的单位越过触发放置单元所在的水平线时，触发此事件。此触发事件必须放置于一个单元中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnCrossHorizontal(Enum house)
        {
            return OnCrossHorizontal(house.GetHashCode());
        }

        /// <summary>
        /// 越过水平线
        /// 当特定所属方的单位越过触发放置单元所在的水平线时，触发此事件。此触发事件必须放置于一个单元中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnCrossHorizontal(int house)
        {
            events.Add($"25,0,{house}");
            return this;
        }

        /// <summary>
        /// 越过垂直线
        /// 当特定所属方的单位越过触发放置单元所在的竖直线时，触发此事件。此触发事件必须放置于一个单元中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnCrossVerticle(Enum house)
        {
            return OnCrossVerticle(house.GetHashCode());
        }

        /// <summary>
        /// 越过垂直线
        /// 当特定所属方的单位越过触发放置单元所在的竖直线时，触发此事件。此触发事件必须放置于一个单元中。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnCrossVerticle(int house)
        {
            events.Add($"26,0,{house}");
            return this;
        }

        /// <summary>
        /// 全局变量被设置
        /// 当指定的全局变量被设置(1)时，触发此事件。参数为Rules(md)中[VariableNames]里的ID。
        /// </summary>
        /// <param name="idx">全局变量序号</param>
        /// <returns></returns>
        public TriggerBuilder OnGlobalVariableSet(int idx)
        {
            events.Add($"27,0,{idx}");
            return this;
        }

        /// <summary>
        /// 全局变量被清除
        /// 当指定的全局变量被清除(0)时，触发此事件。参数为Rules(md)中[VariableNames]里的ID。
        /// </summary>
        /// <param name="idx">全局变量序号</param>
        /// <returns></returns>
        public TriggerBuilder OnGlobalVariableClear(int idx)
        {
            events.Add($"28,0,{idx}");
            return this;
        }


        /// <summary>
        /// 被任何事物摧毁(不包括渗透)
        /// 当关联的对象被摧毁时，触发此事件。不包括该对象渗透入建筑或单位而消失的情况。
        /// </summary>
        /// <param name="idx">全局变量序号</param>
        /// <returns></returns>
        public TriggerBuilder OnDestroyedByAnything()
        {
            events.Add($"29,0,0");
            return this;
        }



        /// <summary>
        /// 电力不足...
        /// 当特定所属方电力低于100%时，触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnLowPower(Enum house)
        {
            return OnLowPower(house.GetHashCode());
        }

        /// <summary>
        /// 电力不足...
        /// 当特定所属方电力低于100%时，触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnLowPower(int house)
        {
            events.Add($"30,0,{house}");
            return this;
        }

        /// <summary>
        /// 桥梁被摧毁
        /// 当特定的桥梁被摧毁(桥梁出现无法通过的缺口)时，触发此事件。此触发须用单元标记放置在目标桥梁的下方(只能放置一个单元标记)。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnBridgeDestroyed()
        {
            events.Add($"31,0,0");
            return this;
        }



        /// <summary>
        /// 建筑存在...
        /// 当特定的建筑(属于触发所属方)出现于地图上时，触发此事件。此建筑可以是以前就有的或由基地建造的，参数为[BuildingTypes]里的ID。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingExist(Enum buildingType)
        {
            return OnBuildingExist(buildingType.GetHashCode());
        }


        /// <summary>
        /// 建筑存在...
        /// 当特定的建筑(属于触发所属方)出现于地图上时，触发此事件。此建筑可以是以前就有的或由基地建造的，参数为[BuildingTypes]里的ID。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingExist(int buildingType)
        {
            events.Add($"32,0,{buildingType}");
            return this;
        }

        /// <summary>
        /// 被玩家选中
        /// 当关联对象被玩家选中时，触发此事件。仅使用于单人任务中。不要将其用于重复触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnSelectedByPlayer()
        {
            events.Add($"31,0,0");
            return this;
        }


        /// <summary>
        /// 特定对象到达路径点附近
        /// 当关联对象到达特定的路径点时，触发此事件。
        /// </summary>
        /// <param name="wayPoint">路径点编号</param>
        /// <returns></returns>
        public TriggerBuilder OnApproachWayPoint(int wayPoint)
        {
            events.Add($"34,0,{wayPoint}");
            return this;
        }

        /// <summary>
        /// 敌人进入局部照明区
        /// 当一个敌单位进入关联建筑投射的局部照明区时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnEnermyEnterLightning()
        {
            events.Add($"35,0,0");
            return this;
        }


        /// <summary>
        /// 局部变量被设置
        /// 当指定的局部变量被设置(1)时，触发此事件。
        /// </summary>
        /// <param name="idx">局部变量序号</param>
        /// <returns></returns>
        public TriggerBuilder OnLocalVariableSet(int idx)
        {
            events.Add($"36,0,{idx}");
            return this;
        }

        /// <summary>
        /// 局部变量被清除
        /// 当指定的局部变量被清除(0)时，触发此事件。
        /// </summary>
        /// <param name="idx">局部变量序号</param>
        /// <returns></returns>
        public TriggerBuilder OnLocalVariableClear(int idx)
        {
            events.Add($"37,0,{idx}");
            return this;
        }

        /// <summary>
        /// 首次受损(仅战斗伤害)
        /// 关联对象初次受到「战斗」造成的伤害时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnFirstRecieveDamage()
        {
            events.Add($"38,0,0");
            return this;
        }

        /// <summary>
        /// 一半生命值(仅战斗伤害)
        /// 关联对象受到「战斗」伤害而只剩一半生命值时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnFirstHalfLife()
        {
            events.Add($"39,0,0");
            return this;
        }

        /// <summary>
        /// 红色生命值(仅战斗伤害)
        /// 关联对象受到「战斗」伤害使得生命值变为红色时，触发此事件。。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnFirstRedLife()
        {
            events.Add($"40,0,0");
            return this;
        }

        /// <summary>
        /// 首次受损(任何伤害来源)
        /// 关联对象初次受到「任何来源」造成的伤害时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnFirstRecieveDamage2()
        {
            events.Add($"41,0,0");
            return this;
        }

        /// <summary>
        ///  一半生命值(任何伤害来源)
        /// 关联对象受到「任何来源」的伤害而只剩一半生命值时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnFirstHalfLife2()
        {
            events.Add($"42,0,0");
            return this;
        }

        /// <summary>
        /// 红色生命值(任何伤害来源)
        /// 关联对象受到「任何来源」的伤害使得生命值变为红色时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnFirstRedLife2()
        {
            events.Add($"43,0,0");
            return this;
        }

        /// <summary>
        /// 被特定所属方攻击
        /// 当受到特定所属方某些单位的攻击时，触发此事件。若物体直接被该攻击摧毁则无法触发。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnAttackedByHouse(Enum house)
        {
            return OnAttackedByHouse(house);
        }

        /// <summary>
        /// 被特定所属方攻击
        /// 当受到特定所属方某些单位的攻击时，触发此事件。若物体直接被该攻击摧毁则无法触发。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnAttackedByHouse(int house)
        {
            events.Add($"44,0,{house}");
            return this;
        }

        /// <summary>
        /// 环境照明 <=...
        /// 当环境照明(亮度)低于<=某一指定值时，触发此事件。可用值介于 0 到 100 之间。
        /// </summary>
        /// <param name="brightness">亮度</param>
        /// <returns></returns>
        public TriggerBuilder OnBrightnessLessThan(int brightness)
        {
            events.Add($"45,0,{brightness}");
            return this;
        }

        /// <summary>
        /// 环境照明 >=...
        /// 当环境照明(亮度)高于>=某一指定值时，触发此事件。可用值介于 0 到 100 之间。。
        /// </summary>
        /// <param name="brightness">亮度</param>
        /// <returns></returns>
        public TriggerBuilder OnBrightnessMoreThan(int brightness)
        {
            events.Add($"46,0,{brightness}");
            return this;
        }

        /// <summary>
        /// 流逝游戏时间
        /// 在游戏开始后特定的时间（秒），触发此事件。
        /// </summary>
        /// <param name="timer">计时器值</param>
        /// <returns></returns>
        public TriggerBuilder OnGameTimeElapse(int timer)
        {
            events.Add($"47,0,{timer}");
            return this;
        }

        /// <summary>
        /// 被任何事物摧毁
        /// 当关联对象被任何事物摧毁、占领或渗透时，触发此事件。此触发不能关联到有子机的单位如驱逐、航母、V3上，否则其子机被摧毁后再摧毁该单位则不触发。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnDestroyedByAnything(int timer)
        {
            events.Add($"48,0,0");
            return this;
        }

        /// <summary>
        /// 关联对象拾得工具箱
        /// 当关联对象拾得工具箱时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnObjectGetCrate()
        {
            events.Add($"49,0,0");
            return this;
        }

        /// <summary>
        /// 任何单位拾得工具箱
        /// 当任何单位拾得工具箱时，触发此事件。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnGetCrate()
        {
            events.Add($"50,0,0");
            return this;
        }

        /// <summary>
        /// 随机延时...
        /// 进行随机延时，时间值（帧）介于指定值的 50% 到 150%。联机使用容易RE(重新连线错误)。
        /// </summary>
        /// <param name="delay">延迟</param>
        /// <returns></returns>
        public TriggerBuilder RandomDelay(int delay)
        {
            events.Add($"51,0,{delay}");
            return this;
        }

        /// <summary>
        /// 金钱低于...
        /// 当触发所属方的金钱低于指定值时触发此事件。
        /// </summary>
        /// <param name="cash">金额</param>
        /// <returns></returns>
        public TriggerBuilder OnCashLessThan(int cash)
        {
            events.Add($"52,0,{cash}");
            return this;
        }

        /// <summary>
        /// 海军单位全部被摧毁
        /// 当特定所属方的所有海军单位被摧毁时，触发此事件。常用于游戏结束条件中。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public TriggerBuilder OnNavalAllDestroyed(Enum house)
        {
            return OnNavalAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 海军单位全部被摧毁
        /// 当特定所属方的所有海军单位被摧毁时，触发此事件。常用于游戏结束条件中。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public TriggerBuilder OnNavalAllDestroyed(int house)
        {
            events.Add($"55,0,{house}");
            return this;
        }

        /// <summary>
        /// 陆军单位全部被摧毁
        /// 当特定所属方的所有陆军单位(包括飞行器)被摧毁时，触发此事件。常用于游戏结束条件中。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public TriggerBuilder OnGroundAllDestroyed(Enum house)
        {
            return OnGroundAllDestroyed(house.GetHashCode());
        }

        /// <summary>
        /// 陆军单位全部被摧毁
        /// 当特定所属方的所有陆军单位(包括飞行器)被摧毁时，触发此事件。常用于游戏结束条件中。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public TriggerBuilder OnGroundAllDestroyed(int house)
        {
            events.Add($"56,0,{house}");
            return this;
        }

        /// <summary>
        /// 建筑不再存在
        /// 当触发所属方的特定的建筑不存在于地图上时，触发此事件。参数为[BuildingTypes]里的ID。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingNotExist(Enum buildingType)
        {
            return OnBuildingNotExist(buildingType.GetHashCode());
        }

        /// <summary>
        /// 建筑不再存在
        /// 当触发所属方的特定的建筑不存在于地图上时，触发此事件。参数为[BuildingTypes]里的ID。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder OnBuildingNotExist(int buildingType)
        {
            events.Add($"57,0,{buildingType}");
            return this;
        }


        /// <summary>
        /// 电力充足
        /// 当特定所属方的电力达到或超过100%时，触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnHighPower(Enum house)
        {
            return OnHighPower(house.GetHashCode());
        }

        /// <summary>
        /// 电力充足
        /// 当特定所属方的电力达到或超过100%时，触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnHighPower(int house)
        {
            events.Add($"58,0,{house}");
            return this;
        }

        /// <summary>
        /// 进入或飞越...
        /// 当「任何物体」进入或飞越此触发对应标记关联的单元标记时，触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnterOrFlyOver(Enum house)
        {
            return OnEnterOrFlyOver(house);
        }

        /// <summary>
        /// 进入或飞越...
        /// 当「任何物体」进入或飞越此触发对应标记关联的单元标记时，触发此事件。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnterOrFlyOver(int house)
        {
            events.Add($"59,0,{house}");
            return this;
        }


        /// <summary>
        /// 科技类型存在
        /// 当该科技类型存在于任一所属方时，触发此事件。此触发计算正在建造中的物体，数值决定存在的数目。
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="technoType">Techno注册名</param>
        /// <returns></returns>
        public TriggerBuilder OnTechnoExist(int count,string technoType)
        {
            events.Add($"60,2,{count},{technoType}");
            return this;
        }

        /// <summary>
        /// 科技类型不存在
        /// 当该科技类型不存在于任一所属方时，触发此事件。此触发计算正在建造中的物体，数值不决定什么，属无关参数。
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="technoType">Techno注册名</param>
        /// <returns></returns>
        public TriggerBuilder OnTechnoNotExist(int count, string technoType)
        {
            events.Add($"61,2,{count},{technoType}");
            return this;
        }


        /// <summary>
        ///【Ares】对象被EMP... 
        /// 当关联对象被EMP黑入时，触发此事件。
        /// </summary>
        /// <param name="count">数量</param>
        /// <param name="technoType">Techno注册名</param>
        /// <returns></returns>
        public TriggerBuilder OnUnderEmp(int count, string technoType)
        {
            events.Add($"62,0,0");
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
        /// 胜利者是...
        /// 特定的所属方会成为胜利者，游戏会立即结束。通常被指定的是玩家所属方，单人战役若胜利方不是玩家所属方，则玩家失败。多人地图时此结果变为“失败者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareWinner(Enum owner)
        {
            return DoDeclareWinner(owner.GetHashCode());
        }
        /// <summary>
        /// 胜利者是...
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
        /// 失败者是...
        /// 特定的所属方会成为失败者，游戏会立即结束。通常被指定的是玩家所属方。多人地图时此结果变为“胜利者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareLoser(Enum owner)
        {
            return DoDeclareLoser(owner.GetHashCode());
        }
        /// <summary>
        /// 失败者是...
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
        /// 生产开始
        /// 特定的电脑所属方将开始生产单位和建筑。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoStartProduction(Enum house)
        {
            return DoStartProduction(house.GetHashCode());
        }

        /// <summary>
        /// 生产开始
        /// 特定的电脑所属方将开始生产单位和建筑。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoStartProduction(int house)
        {
            actions.Add($"3,0,{house},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 建立作战小队
        /// 建立一个指定的作战小队 (归属于触发所属方)。通常会直接从已有单位中招募，但无法招募已经加入其他小队的单位。如果没有足够的单位来组建小队，AI会通过生产补足(自动建造)。当所有成员就位时，小队即自动建立。
        /// </summary>
        /// <param name="team">作战小队Id</param>
        /// <returns></returns>
        public TriggerBuilder DoEstablishTeam(string team)
        {
            actions.Add($"4,1,{team},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 解散作战小队
        /// 解散当前存在的所有特定类型的作战小队。小队的单位会保留，并可以作为新兵加入其他小队。
        /// </summary>
        /// <param name="team">作战小队Id</param>
        /// <returns></returns>
        public TriggerBuilder DoDissolveTeam(string team)
        {
            actions.Add($"5,1,{team},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 全部寻敌
        /// 特定的所属方的所有单位进入寻敌(Hunt)模式。他们将搜索并消灭敌人。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoAllHunt(Enum house)
        {
            return DoAllHunt(house.GetHashCode());
        }

        /// <summary>
        /// 全部寻敌
        /// 特定的所属方的所有单位进入寻敌(Hunt)模式。他们将搜索并消灭敌人。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoAllHunt(int house)
        {
            actions.Add($"6,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 援军(作战小队)...
        /// 创建由指定作战小队组成的援军。小队成员直接刷出，刷兵路径点为小队设置中的路径点。
        /// </summary>
        /// <param name="team">作战小队Id</param>
        /// <returns></returns>
        public TriggerBuilder DoReinforcements(string team)
        {
            actions.Add($"7,1,{team},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 降落区闪烁(路径点)...
        /// 在指定路径点显示一个空降舱降落区域动画，该动画为循环播放。此区域附近的地图也将会显示。
        /// </summary>
        /// <param name="wapPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoBlinkDropZone(int wapPoint)
        {
            actions.Add($"8,0,0,0,0,0,0,{wapPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 卖掉全部建筑...
        /// 特定所属方会变卖掉所有建筑(来得到金钱和单位)。常用于电脑发动的最后攻击。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoSellAllBuilding(Enum house)
        {
            return DoSellAllBuilding(house.GetHashCode());
        }

        /// <summary>
        /// 卖掉全部建筑...
        /// 特定所属方会变卖掉所有建筑(来得到金钱和单位)。常用于电脑发动的最后攻击。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoSellAllBuilding(int house)
        {
            actions.Add($"9,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 全屏播放影片...
        /// 全屏播放特定影片。游戏在此期间将会暂停，在播放完成后自动恢复正常。
        /// </summary>
        /// <param name="movie">影片</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayMovieFullScreen(string movie)
        {
            actions.Add($"10,0,{movie},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 文本触发事件...
        /// 显示文本内容，参数为CSF文件内的项目。CSF文件需要用CSF编辑器编辑。
        /// </summary>
        /// <param name="label">CSF中的label</param>
        /// <returns></returns>
        public TriggerBuilder DoTriggerText(string label)
        {
            actions.Add($"11,4,{label},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 摧毁触发事件...
        /// 摧毁所有特定触发事件的当前实例。但不会阻止已经被触发完成的实例(包括正在建立中的)。
        /// </summary>
        /// <param name="triggerid">触发id</param>
        /// <returns></returns>
        public TriggerBuilder DoDestroyTrigger(string triggerid)
        {
            actions.Add($"12,2,{triggerid},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 开始自动创建作战小队...
        /// 特定所属方会开始自动创建。这会使得电脑所属方在它认为合适的时机自动建造作战小队。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoStartEstablishTeam(Enum house)
        {
            return DoStartEstablishTeam(house.GetHashCode());
        }

        /// <summary>
        /// 开始自动创建作战小队...
        /// 特定所属方会开始自动创建。这会使得电脑所属方在它认为合适的时机自动建造作战小队。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoStartEstablishTeam(int house)
        {
            actions.Add($"13,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 更改所属方...
        /// 更改关联对象到特定的所属方。对象装载的载员不受此触发的作用。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoChangeHouse(Enum house)
        {
            return DoChangeHouse(house.GetHashCode());
        }

        /// <summary>
        /// 更改所属方...
        /// 更改关联对象到特定的所属方。对象装载的载员不受此触发的作用。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoChangeHouse(int house)
        {
            actions.Add($"14,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 显示全地图
        /// 为玩家显示全部地图(清除黑幕)。联机使用会导致RE(重新连线错误)。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoRevealWholeMap()
        {
            actions.Add($"16,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 显示路径点周围区域
        /// 为玩家显示特定路径点周围的一片地图区域。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoRevealWayPoint(int wayPoint)
        {
            actions.Add($"17,0,{wayPoint},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 显示路径点的单元区域
        /// 为玩家显示由路径点划定的「区域」中所有的格子。「区域」的定义参见事件24。详细解释另见工具教程里的“触发备忘”。。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoRevealWayPointZone(int wayPoint)
        {
            actions.Add($"18,0,{wayPoint},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 播放音效...
        /// 播放特定音效。在Sound(md).ini中定义。
        /// </summary>
        /// <param name="sound">音效的注册名</param>
        /// <returns></returns>
        public TriggerBuilder DoPlaySound(string sound)
        {
            actions.Add($"19,7,{sound},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 播放音乐...
        /// 播放特定音乐。在Theme(md).ini中定义。
        /// </summary>
        /// <param name="sound">音乐的注册名</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayMusic(string music)
        {
            actions.Add($"20,8,{music},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 播放语音...
        /// 播放特定语音。在Eva(md).ini中定义。
        /// </summary>
        /// <param name="eva">eva的注册名</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayEva(string eva)
        {
            actions.Add($"21,6,{eva},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 强制触发事件...
        /// 强制特定类型的所有触发事件进行触发，不管触发此事件的条件。不要对该触发本身使用！
        /// </summary>
        /// <param name="trigger">触发Id</param>
        /// <returns></returns>
        public TriggerBuilder DoTriggerForce(string trigger)
        {
            if (trigger == UniqueId)
                throw new Exception("强制触发不允许触发自身");
            actions.Add($"22,2,{trigger},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时开始
        /// 启动全局任务计时器。一定时间内可显示的全局计时器有且只有一个。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoStartTimer()
        {
            actions.Add($"23,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时停止
        /// 停止全局任务计时器。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoStopTimer()
        {
            actions.Add($"24,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时器计时增加..
        /// 将全局任务计时器增加指定的时间。
        /// </summary>
        /// <param name="count">时间</param>
        /// <returns></returns>
        public TriggerBuilder DoAddTimer(int count)
        {
            actions.Add($"25,0,{count},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时器计时缩短...
        /// 将全局任务计时器减少指定的时间。时间不会减到 0 以下。
        /// </summary>
        /// <param name="count">时间</param>
        /// <returns></returns>
        public TriggerBuilder DoSubstractTimer(int count)
        {
            actions.Add($"26,0,{count},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时设置...
        /// 将全局任务计时器设置为指定的时间值。
        /// </summary>
        /// <param name="count">时间</param>
        /// <returns></returns>
        public TriggerBuilder DoSetTimer(int count)
        {
            actions.Add($"27,0,{count},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 设置全局变量...
        /// 设置全局变量(1)。全局变量在Rules(md).ini中的[VariableNames]里定义。
        /// </summary>
        /// <param name="idx">变量序号</param>
        /// <returns></returns>
        public TriggerBuilder DoSetGlobalVariable(int idx)
        {
            actions.Add($"28,0,{idx},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 清除全局变量
        /// 清除全局变量(0)。全局变量在Rules(md).ini中的[VariableNames]里定义。
        /// </summary>
        /// <param name="idx">变量序号</param>
        /// <returns></returns>
        public TriggerBuilder DoClearGlobalVariable(int idx)
        {
            actions.Add($"29,0,{idx},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 自动建设基地...
        /// 初始化电脑遭遇战模式的建设控制，可以是[ON]或[OFF]状态。当设为[ON]且电脑所属方无基地节点时，将会像遭遇战模式那样自动进行建设(要确保有建造场)；如果有基地节点，则仍会建造节点对应的建筑。
        /// </summary>
        /// <param name="allowed">是/否</param>
        /// <returns></returns>
        public TriggerBuilder DoAutoConstract(bool allowed = true)
        {
            actions.Add($"30,0,{(allowed ? 1 : 0)},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 逐单元延伸黑幕
        /// 增大地图的黑幕(一步一单元)，需要相应的延伸黑幕INI设置才能使用。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoExtendFog()
        {
            actions.Add($"31,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 摧毁关联对象
        /// 摧毁该触发关联的任何建筑物、桥梁或者单位。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoDestroy()
        {
            actions.Add($"32,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 添加一次特定超武
        /// 为触发所属方添加一次(只一次)特定超武。在这里设置的核弹可以发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public TriggerBuilder DoGiveSuperWeaponOnce(Enum superWeaponType)
        {
            return DoGiveSuperWeaponOnce(superWeaponType.GetHashCode());
        }

        /// <summary>
        /// 添加一次特定超武
        /// 为触发所属方添加一次(只一次)特定超武。在这里设置的核弹可以发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public TriggerBuilder DoGiveSuperWeaponOnce(int superWeaponType)
        {
            actions.Add($"33,0,{superWeaponType},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 重复添加特定超武
        /// 为触发所属方添加永久的特定超武。在这里设置的核弹无法正常发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public TriggerBuilder DoGiveSuperWeapon(Enum superWeaponType)
        {
            return DoGiveSuperWeapon(superWeaponType.GetHashCode());
        }

        /// <summary>
        /// 重复添加特定超武
        /// 为触发所属方添加永久的特定超武。在这里设置的核弹无法正常发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public TriggerBuilder DoGiveSuperWeapon(int superWeaponType)
        {
            actions.Add($"34,0,{superWeaponType},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 超武首选目标为特定建筑...
        /// 指定触发所属方的 电脑玩家 使用超武攻击时的首选目标，参数为[BuildingTypes]里的ID。只对打击性超武（如核弹、闪电，Ares对应SW.AITargeting=Offensive）有效。
        /// </summary>
        /// <param name="buildingType"></param>
        /// <returns></returns>
        public TriggerBuilder DoSetAISuperWeaponTarget(Enum buildingType)
        {
            return DoSetAISuperWeaponTarget(buildingType.GetHashCode());
        }

        /// <summary>
        /// 超武首选目标为特定建筑...
        /// 指定触发所属方的 电脑玩家 使用超武攻击时的首选目标，参数为[BuildingTypes]里的ID。只对打击性超武（如核弹、闪电，Ares对应SW.AITargeting=Offensive）有效。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public TriggerBuilder DoSetAISuperWeaponTarget(int buildingType)
        {
            actions.Add($"35,0,{buildingType},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 全部更改所属...
        /// 触发所属方的所有对象更改所属到特定所属方。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoChangeAllHouse(Enum house)
        {
            return DoChangeAllHouse(house);
        }

        /// <summary>
        /// 全部更改所属...
        /// 触发所属方的所有对象更改所属到特定所属方。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoChangeAllHouse(int house)
        {
            actions.Add($"36,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 结盟...
        /// 使触发所属方与特定所属方结盟。单人任务中是不完全双向结盟（比如A、B双方互不攻击，B有A视野 而 A无B视野。AB同时与对方结盟时，双方均没有对方视野），多人任务中是单向结盟。若多人任务中有至少两个玩家选与触发所属方相同的国家，则只选一个参与结盟。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoAlliedWith(Enum house)
        {
            return DoAlliedWith(house.GetHashCode());
        }

        /// <summary>
        /// 结盟...
        /// 使触发所属方与特定所属方结盟。单人任务中是不完全双向结盟（比如A、B双方互不攻击，B有A视野 而 A无B视野。AB同时与对方结盟时，双方均没有对方视野），多人任务中是单向结盟。若多人任务中有至少两个玩家选与触发所属方相同的国家，则只选一个参与结盟。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoAlliedWith(int house)
        {
            actions.Add($"37,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 成为敌人...
        /// 使触发所属方与特定所属方不结盟(宣战)，这个效果是单向的。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public TriggerBuilder DoBeEnermyWith(Enum house)
        {
            return DoBeEnermyWith(house.GetHashCode());
        }

        /// <summary>
        /// 成为敌人...
        /// 使触发所属方与特定所属方不结盟(宣战)，这个效果是单向的。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public TriggerBuilder DoBeEnermyWith(int house)
        {
            actions.Add($"38,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 调整玩家视野大小
        /// 调整玩家在地图内的矩形视野。格式：左,上,宽,高，其中左、上决定左上角位置，宽、高决定宽度和高度。可先用编辑→地图参数里预设以确定范围
        /// </summary>
        /// <param name="x">左</param>
        /// <param name="y">上</param>
        /// <param name="w">宽</param>
        /// <param name="h">高</param>
        /// <returns></returns>
        public TriggerBuilder DoChangePlayerView(int x,int y,int w,int h)
        {
            actions.Add($"40,0,0,{x},{y},{w},{h},A");
            return this;
        }

        /// <summary>
        /// 播放动画在...
        /// 在特定的路径点播放特定的动画，参数为[Animations]里的动画ID。原版动画注册表存在序号漂移，建议使用脚本或Excel予以修正。
        /// </summary>
        /// <param name="animType">动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayAnimAt(Enum animType, int wayPoint)
        {
            return DoPlayAnimAt(animType.GetHashCode(),wayPoint);
        }


        /// <summary>
        /// 播放动画在...
        /// 在特定的路径点播放特定的动画，参数为[Animations]里的动画ID。原版动画注册表存在序号漂移，建议使用脚本或Excel予以修正。
        /// </summary>
        /// <param name="animType">动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayAnimAt(int animType,int wayPoint)
        {
            actions.Add($"41,0,{animType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 武器（弹头）爆炸在...
        /// 使用特定「武器的弹头」在指定路径点产生一次爆炸，但无法触发弹头的特殊效果。参数为[WeaponTypes]里的武器ID（从1开始），若无该注册表（或者注册表里没有所填ID）则取[Warheads]里的弹头ID。
        /// </summary>
        /// <param name="weaponType">武器</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoExplodeWeaponAt(Enum weaponType, int wayPoint)
        {
            return DoExplodeWeaponAt(weaponType.GetHashCode(), wayPoint);
        }

        /// <summary>
        /// 武器（弹头）爆炸在...
        /// 使用特定「武器的弹头」在指定路径点产生一次爆炸，但无法触发弹头的特殊效果。参数为[WeaponTypes]里的武器ID（从1开始），若无该注册表（或者注册表里没有所填ID）则取[Warheads]里的弹头ID。
        /// </summary>
        /// <param name="weaponType">武器</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoExplodeWeaponAt(int weaponType, int wayPoint)
        {
            actions.Add($"42,0,{weaponType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 播放 Voxel 动画
        /// 在指定路径点播放VXL动画，参数为Rules(md).ini中[VoxelAnims]里的ID。其中8是陨石。
        /// </summary>
        /// <param name="voxelAnim">vxl动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayVoxelAnimAt(Enum voxelAnim, int wayPoint)
        {
            return DoPlayVoxelAnimAt(voxelAnim.GetHashCode(),wayPoint);
        }

        /// <summary>
        /// 播放 Voxel 动画
        /// 在指定路径点播放VXL动画，参数为Rules(md).ini中[VoxelAnims]里的ID。其中8是陨石。
        /// </summary>
        /// <param name="voxelAnim">vxl动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoPlayVoxelAnimAt(int voxelAnim, int wayPoint)
        {
            actions.Add($"43,0,{voxelAnim},0,0,0,0,{wayPoint.To26()}");
            return this;
        }


        /// <summary>
        /// 禁止玩家输入
        /// 禁止玩家进行操作，失去鼠标无法控制。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoDisablePlayerControl()
        {
            actions.Add($"46,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 允许玩家输入
        /// 允许玩家进行操作，取消动作46的效果。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoEnablePlayerControl()
        {
            actions.Add($"47,0,0,0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 移动并居中视野到路径点...
        /// 将玩家视野移动到特定的路径点。速度取1～4，太大会造成卡屏，无法移动视野。
        /// </summary>
        /// <param name="speed">卷动速度</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoMoveViewTo(ViewMoveSpeed speed,int wayPoint)
        {
            actions.Add($"48,0,{speed.GetHashCode()},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 放大视野
        /// 放大玩家视野。减少分辨率，同时不能输入。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoZoomInView()
        {
            actions.Add($"49,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 缩小视野
        /// 缩小玩家视野。可以取消49放大视野的作用，增加分辨率，恢复输入。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoZoomOutView()
        {
            actions.Add($"50,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 重置全地图黑幕
        /// 触发所属方将会被重置整张地图的黑幕。联机使用容易RE(重新连线错误)。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoResetFog()
        {
            actions.Add($"51,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 更改照明状态 (Ares平台有效)
        /// 更改与该触发关联的探照灯建筑的照明方式。0：无局部照明、1：照明特定角度、2：圆圈、3：跟随敌对目标
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoChangeSpotLightBehavior(SpotLightBehavior behavior)
        {
            actions.Add($"52,0,{behavior.GetHashCode()},0,0,0,0,A");
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
        /// 建立小地图事件
        /// 在特定的路径点建立雷达小地图事件。0、3、4：红框，1、2：黄框，5：蓝框。
        /// </summary>
        /// <param name="radarEvent">雷达事件类型</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoRaiseRadarEvent(RadarEvent radarEvent,int wayPoint)
        {
            actions.Add($"55,0,{radarEvent.GetHashCode()},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 设置局部变量
        /// 设置局部变量标记(1)。
        /// </summary>
        /// <param name="variable">局部变量序号</param>
        /// <returns></returns>
        public TriggerBuilder DoSetLocalVariable(int variable)
        {
            actions.Add($"56,0,{variable},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 清除局部变量
        /// 清除局部变量标记(0)。
        /// </summary>
        /// <param name="variable">局部变量序号</param>
        /// <returns></returns>
        public TriggerBuilder DoClearLocalVariable(int variable)
        {
            actions.Add($"57,0,{variable},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 变卖关联建筑
        /// 变卖所有与此触发关联的建筑。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoSellBuilding()
        {
            actions.Add($"60,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 关闭关联建筑
        /// 关闭与此触发关联的建筑。效果与建筑中耗能=0相同。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoShutDownBuilding()
        {
            actions.Add($"61,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 启动关联建筑
        /// 启动与此触发关联的建筑。效果与建筑中耗能=1相同。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoSwitchOnBuilding()
        {
            actions.Add($"62,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 在...造成100点伤害
        /// 在特定的路径点造成100点爆炸伤害，对于建筑的实际效果约为500点，可以摧毁地面桥梁。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder Do100DamageAt(int wayPoint)
        {
            actions.Add($"63,0,{wayPoint},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 宣告胜利...
        /// 宣告玩家胜利。但不会显示“任务完成”的图片。。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoDeclarePlayerWin()
        {
            actions.Add($"67,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 宣告失败...
        /// 宣告玩家失败。但不会显示“任务失败”的图片。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoDeclarePlayerLose()
        {
            actions.Add($"68,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 强制结束任务
        /// 强制结束游戏任务。会显示“任务完成”的图片。
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder DoTerminateMission()
        {
            actions.Add($"69,0,0,0,0,0,0,A");
            return this;
        }






        /// <summary>
        /// 援军(作战小队)[在路径点]...
        /// 在特定的路径点创建援军作战小队。小队成员会被直接刷出，刷兵路径点为行为设置中的路径点。
        /// </summary>
        /// <param name="teamType">作战小队</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoReinforcementsAt(string teamType,int wayPoint)
        {
            actions.Add($"80,1,{teamType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }




        /// <summary>
        /// 计时器文本
        /// 指定计时器显示的文本，参数为CSF文件内的项目。
        /// </summary>
        /// <param name="label">csf文本</param>
        /// <returns></returns>
        public TriggerBuilder DoSetTimerLabel(string label)
        {
            actions.Add($"103,4,css,0,0,0,0,A");
            return this;
        }




        /// <summary>
        /// 超时空传送援军... 
        /// 在特定的路径点创建援军作战小队，并播放传送动画。小队成员会凭空分散刷出。
        /// </summary>
        /// <param name="teamType">作战小队</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public TriggerBuilder DoChronoReinforceAt(string teamType, int wayPoint)
        {
            actions.Add($"107,1,{teamType},0,0,0,0,{wayPoint.To26()}");
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