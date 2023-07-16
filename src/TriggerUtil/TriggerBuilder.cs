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
            var str = $"{actions.Count},{string.Join(",", events)}";
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
        public TriggerBuilder OnVariableSet(int idx)
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
        public TriggerBuilder OnVariableClear(int idx)
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