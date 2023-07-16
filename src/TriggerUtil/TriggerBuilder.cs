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
            events.Add($"1,0,{owner.GetHashCode()}");
            return this;
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
        /// 当流逝的时间达到特定值时触发此事件。当触发被允许时计时器初始化；若触发是重复的，时间达到指定值时，计时器复位。
        /// </summary>
        /// <param name="timer">计时器值</param>
        /// <returns></returns>
        public TriggerBuilder OnTimeElapse(int timer)
        {
            events.Add($"13,0,{timer}");
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
            actions.Add($"1,0,{owner.GetHashCode()},0,0,0,0,A");
            return this;
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
            actions.Add($"2,0,{owner.GetHashCode()},0,0,0,0,A");
            return this;
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
            actions.Add($"113,0,{owner.GetHashCode()},0,0,0,0,A");
            return this;
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