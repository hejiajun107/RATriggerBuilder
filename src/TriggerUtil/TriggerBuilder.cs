namespace TriggerUtil
{
    public class TriggerBuilder
    {

        public static TriggerBuilder CreateBuilder()
        {
            return new TriggerBuilder();
        }

        /// <summary>
        /// 触发名称
        /// </summary>
        private string? _triggerName;

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
            _triggerName = name;
            return this;
        }

        /// <summary>
        /// 设置触发所属
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public TriggerBuilder Owner(string owner)
        {
            _owner = owner;
            return this;
        }


        public TriggerBuilder Next(TriggerBuilder nextTrigger)
        {
            return this;
        }


        public string Build()
        {
            return string.Empty;
        }



        #region 条件
        /// <summary>
        /// 无
        /// </summary>
        /// <returns></returns>
        public TriggerBuilder OnNothing()
        {
            return this;
        }

        /// <summary>
        /// 当流逝的时间达到特定值时触发此事件。当触发被允许时计时器初始化；若触发是重复的，时间达到指定值时，计时器复位。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder OnEnter(string owner)
        {
            return this;
        }


        /// <summary>
        /// 当流逝的时间达到特定值时触发此事件。当触发被允许时计时器初始化；若触发是重复的，时间达到指定值时，计时器复位。
        /// </summary>
        /// <param name="timer">计时器值</param>
        /// <returns></returns>
        public TriggerBuilder OnTimeElapse(int timer)
        {
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
            return this;
        }

        /// <summary>
        /// 特定的所属方会成为胜利者，游戏会立即结束。通常被指定的是玩家所属方，单人战役若胜利方不是玩家所属方，则玩家失败。多人地图时此结果变为“失败者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareWinner(string owner)
        {
            return this;
        }


        /// <summary>
        /// 特定的所属方会成为失败者，游戏会立即结束。通常被指定的是玩家所属方。多人地图时此结果变为“胜利者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoDeclareLoser(string owner)
        {
            return this;
        }


        /// <summary>
        /// 让特定所属方的所有空闲的步兵单位执行欢呼动作。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public TriggerBuilder DoCheer(string owner)
        {
            return this;
        }

        #endregion



    }
}