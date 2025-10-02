using System.Linq.Expressions;
using System.Text.RegularExpressions;

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

        internal TriggerBuilder(TriggerContext context,int fixedNum = -1) 
        {
            _context = context;
            if(fixedNum > 0)
            {
                var id = IdGenerator.NextFixedId(fixedNum);
                UniqueId = id.trigger;
                TriggerName = UniqueId;
                Tag = id.tag;
            }
            else
            {
                var id = IdGenerator.NextId();
                UniqueId = id.trigger;
                TriggerName = UniqueId;
                Tag = id.tag;
            }
       

            _actionBuilder = new ActionBuilder(this);
            _eventBuilder = new EventBuilder(this);

            _actionBuilder.OnAttachNextTrigger += (s => NextNodes.Add(s));
        }

        private TriggerContext _context;

        private ActionBuilder _actionBuilder;
        private EventBuilder _eventBuilder;

        public TriggerGroup TriggerGroup { get; private set; } = new TriggerGroup();

        public List<string> NextNodes { get; private set; } = new List<string>();


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

        public string Description { get; private set; }


        /// <summary>
        /// 触发所属方
        /// </summary>
        private string? _owner;

        public List<string> GetTeamIds()
        {
            return _actionBuilder.TeamNodes.Select(x => x).ToList();
        }

        public TriggerBuilder SetGroup(string groupName)
        {
            TriggerGroup = new TriggerGroup()
            {
                Name = groupName,
                Setted = true
            };
            return this;
        }

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

        /// <summary>
        /// 设置为一个固定的Tag，方便在地编里设置标签时保持不变
        /// </summary>
        /// <param name="num">序号，唯一</param>
        /// <returns></returns>
        public TriggerBuilder SetFixedTag(int num)
        {
            Tag = IdGenerator.FixedId(num);
            return this;
        }

        /// <summary>
        /// 设置为一个固定的Tag，方便在地编里设置标签时保持不变
        /// </summary>
        /// <param name="tag">tag名称，唯一</param>
        /// <returns></returns>
        public TriggerBuilder SetCustomTag(string tag)
        {
            Tag = tag;
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
            var str = $"{_actionBuilder.Actions.Count},{string.Join(",", _actionBuilder.Actions)}";
            if (str.Length >= 512)
                throw new ArgumentOutOfRangeException($"{str} of {TriggerName} is too long");
            return str;
        }

        public string BuildEventsString()
        {
            var str = $"{_eventBuilder.Events.Count},{string.Join(",", _eventBuilder.Events)}";
            if (str.Length >= 512)
                throw new ArgumentOutOfRangeException($"{str} of {TriggerName} is too long");
            return str;
        }


        /// <summary>
        /// 触发条件
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TriggerBuilder When(Action<EventBuilder> expression)
        {
            expression(_eventBuilder);
            return this;
        }

        /// <summary>
        /// 触发结果
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TriggerBuilder Then(Action<ActionBuilder> expression)
        {
            expression(_actionBuilder);
            return this;
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

        public TriggerBuilder SetDescription(string desc)
        {
            Description = desc;
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
            if (!nextTrigger.TriggerGroup.Setted)
            {
                nextTrigger.TriggerGroup = TriggerGroup;
            }
            _actionBuilder.Enable(nextTrigger.UniqueId);
            return this;
        }

        /// <summary>
        /// 包含触发，在动作中会开启下一个触发，并返回当前触发
        /// </summary>
        /// <param name="nextTrigger"></param>
        /// <returns></returns>
        public TriggerBuilder Contain(Action<TriggerBuilder> next)
        {
            var trigger = _context.CreateTrigger();
            if (!trigger.TriggerGroup.Setted)
            {
                trigger.TriggerGroup = TriggerGroup;
            }
            trigger.Disabled = true;
            next(trigger);
            _actionBuilder.Enable(trigger.UniqueId);
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
            if (!nextTrigger.TriggerGroup.Setted)
            {
                nextTrigger.TriggerGroup = TriggerGroup;
            }
            _actionBuilder.Enable(nextTrigger.UniqueId);
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
            if (!trigger.TriggerGroup.Setted)
            {
                trigger.TriggerGroup = TriggerGroup;
            }
            next(trigger);
            _actionBuilder.Enable(trigger.UniqueId);
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







    }
}