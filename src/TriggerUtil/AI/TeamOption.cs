using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil.AI
{
    public class TeamOption
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        public int VeteranLevel { get; set; } = 1;
        /// <summary>
        /// 被心灵控制后的动作，0=随缘、1=加入控制者作战小队、2=部队回收厂卖钱、3=生化反应炉填电、4=搜索敌人、5=摸鱼
        /// </summary>
        public int MindControlDecision { get; set; } = 0;
        /// <summary>
        ///  ;运载单位写yes会计算ContentScan。
        /// </summary>
        public bool Loadable { get; set; }

        public bool Full { get; set; } = false;

        public bool Annoyance { get; set; } = false;

        public bool GuardSlower { get; set; } = false;

        public string House { get; set; } = "<none>";
        /// <summary>
        /// 可否从低优先级的队伍和闲置单位中直接调用成员。
        /// </summary>
        public bool Recruiter { get; set; } = false;
        /// <summary>
        /// 结合地图预置单位生效。遭遇战中，在条件满足的情况下，平均每隔AutocreateTime规定的时间，制造一队带有Autocreate的小队。
        /// 生效前提是Recruiter=yes。
        ///        地图预置单位都有AUTOCREATE_NO_RECRUITABLE和AUTOCREATE_YES_RECRUITABLE两个标签，Autocreate会结合地图预置单位的这两个标签生效，会优先招募地图预置单位。
        ///预置单位的标签位置根据其类型决定:
        ///        [Infantry]
        ///        INDEX=OWNER,ID,HEALTH,X,Y,SUB_CELL,MISSION,FACING,TAG,VETERANCY,GROUP,HIGH,AUTOCREATE_NO_RECRUITABLE,AUTOCREATE_YES_RECRUITABLE
        ///        [Units]
        ///        INDEX = OWNER, ID, HEALTH, X, Y, FACING, MISSION, TAG, VETERANCY, GROUP, HIGH, FOLLOWS_INDEX, AUTOCREATE_NO_RECRUITABLE, AUTOCREATE_YES_RECRUITABLE
        ///        [Aircraft]
        ///INDEX=OWNER,ID,HEALTH,X,Y,FACING,MISSION,TAG,VETERANCY,GROUP,AUTOCREATE_NO_RECRUITABLE,AUTOCREATE_YES_RECRUITABLE
        ///作用方式:
        ///Autocreate=no时，AUTOCREATE_NO_RECRUITABLE=0的预置单位无法招募，AUTOCREATE_NO_RECRUITABLE=1可以招募，预置单位不够时会通过工厂生产补足队伍，预置单位无法响应招募时无法通过工厂生产替代该单位。
        ///Autocreate=yes时，AUTOCREATE_YES_RECRUITABLE=0的预置单位无法招募，AUTOCREATE_YES_RECRUITABLE=1可以招募。预置单位无法响应招募时会从工厂生产来补足队伍，但是AUTOCREATE_NO_RECRUITABLE=1的单位无法通过工厂生产替代。
        /// </summary>
        public bool Autocreate { get; set; } = false;
        /// <summary>
        /// 可能会在不需要时提前造好该队伍。
        /// </summary>
        public bool Prebuild { get; set; } = false;

        public bool Reinforce { get; set; } = false;
        /// <summary>
        /// 是否空降
        /// </summary>
        public bool Droppod { get; set; } = false;

        public bool UseTransportOrigin { get; set; } = false;

        /// <summary>
        /// 运输机路径点
        /// </summary>
        public int TransportWaypoint { get; set; } 

        public bool Whiner { get; set; } = false;
        /// <summary>
        /// 完成任务后解散。
        /// </summary>
        public bool LooseRecruit { get; set; } = false;
        /// <summary>
        /// 和Suicide共同起作用。
        /// Aggressive和Suicide:
        /// 这两句共同决定AI执行脚本过程中是否主动攻击敌人，以及受到攻击时是否还击。
        /// (1)执行0号，1号，46号，59号指令时：Aggressive和Suicide无论写啥，都不会对射程内的敌人进行攻击，而是继续执行当前脚本。如果受到攻击，Suicide=no会进行还击。
        /// (2)执行3号，47号，53号指令时：如果Aggressive=yes和Suicide=no，则会对进入射程的敌人进行攻击，在攻击结束后继续执行脚本。如果受到攻击，则会进行还击，并在还击结束后才继续执行脚本。其余情况均不会对射程内的敌人进行攻击，也不会还击。
        /// (3)执行16号指令时：只要Suicide=yes，就会攻击进入射程的敌人，并且在攻击结束后继续执行脚本，但是受到攻击不会还击。如果Aggressive=yes和Suicide=no，与(2)相同。如果都是no，那么会变向路径点移动边进行攻击，如果受到攻击，则在射程内进行还击，其余情况继续移动执行脚本。
        /// </summary>
        public bool Aggressive { get; set; } = false;
        /// <summary>
        /// 和Aggressive共同起作用。
        /// </summary>
        public bool Suicide { get; set; } = false;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; } = 5;

        /// <summary>
        /// 该小队最大生成数量
        /// </summary>
        public int Max { get; set; } = 5;

        /// <summary>
        /// 挂载标签
        /// </summary>
        public string Tag { get; set; }

        public int TechLevel { get; set; } = 0;

        /// <summary>
        /// 除非特殊需要。另：-2时会选取任何TaskForce。
        /// </summary>
        public int Group { get; set; } = -1;

        public bool OnTransOnly { get; set; } = false;

        /// <summary>
        /// 是否会按威胁计算方法寻找最小威胁路径。
        /// </summary>
        public bool AvoidThreats { get; set; } = false;

        public bool IonImmune  { get; set; } = false;
        /// <summary>
        /// 运输工具完成任务后是否回家。
        /// </summary>
        public bool TransportsReturnOnUnload { get; set; } = false;
        /// <summary>
        /// 队伍成员可否被调去更高优先级的队伍中。
        /// </summary>
        public bool AreTeamMembersRecruitable { get; set; } = false;
        /// <summary>
        /// 守家部队。
        /// </summary>
        public bool IsBaseDefense { get; set; } = false;

        /// <summary>
        /// 是否只针对敌对方。
        /// </summary>
        public bool OnlyTargetHouseEnemy { get; set; } = false;



        /// <summary>
        /// 路径点 16进制!!!
        /// </summary>
        public int? WayPoint { get; set; }

        //public string Script { get; set; }

        //public string TaskForce { get; set; }
    }

}
