using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil
{
    public enum Difficulty
    {
        Easy,
        Middle,
        Hard
    }

    public enum IdType
    {
        Trigger,
        Event,
        Action
    }

    public enum RepeatType
    {
        OneTimeOr,
        OneTimeAnd,
        RepeatingOr
    }

    public enum Player
    {
        PlayerA = 4475,
        PlayerB = 4476,
        PlayerC = 4477,
        PlayerD = 4478,
        PlayerE = 4479,
        PlayerF = 4480,
        PlayerG = 4481,
        PlayerH = 4482
    }

    public enum ViewMoveSpeed
    {
        VerySlow,
        Slow,
        Normal,
        Fast,
        VeryFast
    }

    public enum SpotLightBehavior
    {
        /// <summary>
        /// 无局部照明
        /// </summary>
        None,
        /// <summary>
        /// 照明特定角度
        /// </summary>
        SpeficAnlge,
        /// <summary>
        /// 圆圈
        /// </summary>
        Circle,
        /// <summary>
        /// 跟随敌对目标
        /// </summary>
        FocusOnEnermy,
    }

    public enum RadarEvent
    {
        Combat,
        NonCombat,
        DropZone,
        BaseAttack,
        HarvestAttack,
        EnemySensed,
        UnitReady,
        UnitLost,
        UnitRepaired,
        BuildingInfiltrated,
        BuildingCaptured,
        BeaconPlaced,
        SWDetected,
        SWActivated,
        BridgeRepaired,
        GarrisonAbandoned,
        AllyAttack,

    }






    public enum AttackTargetType
    {
        /// <summary>
        /// 任何目标
        /// </summary>
        NotSpecified,
        /// <summary>
        /// 任何目标 Use Auto Targeting
        /// </summary>
        Anything,
        /// <summary>
        /// 建筑物
        /// </summary>
        BuildingTypes,
        /// <summary>
        /// 资源类
        /// </summary>
        Storage,
        /// <summary>
        /// 步兵类
        /// </summary>
        InfantryTypes,
        /// <summary>
        /// 载具类
        /// </summary>
        VehicleTypes,
        /// <summary>
        /// 工厂
        /// </summary>
        Factory,
        /// <summary>
        /// 防御建筑
        /// </summary>
        BaseDefense,
        Anything2,
        /// <summary>
        /// 电厂
        /// </summary>
        PowerPlant,
        /// <summary>
        /// 可驻军建筑
        /// </summary>
        OccupyBuilding,
        /// <summary>
        /// 中立科技建筑
        /// </summary>
        SpecialBuilding
    }

    public enum UnloadResult
    {
        /// <summary>
        /// 保留运输工具和乘客执行接下来的指令
        /// </summary>
        KeepAll,
        /// <summary>
        /// 只保留运输工具
        /// </summary>
        TransportOnly,
        /// <summary>
        /// 只保留乘客
        /// </summary>
        PassengerOnly,
        /// <summary>
        /// 全解散
        /// </summary>
        ReleaseAll
    }

    /// <summary>
    /// ★11,n - 进入第n种状态(通常能在Rules里找到并修改相应状态)，无法执行该Script内接下来的任何指令。(可以通过重组至其他小队来终止任务)
    //  n =	0	Sleep - 休眠。
    //	1	Attack - 根据威胁等级进行TeamType中的攻击任务。
    //	2	Move - 移动。
    //	3	QMove - 在其他部队移动结束后移动到目的地。(遭遇战无效)
    //	4	Retreat - 撤退(可能会逃出地图)。
    //	☆5	Guard - 防御，只攻击进入射程内的敌人。
    //	6	Sticky - 与防御类似，会与敌人交战但不会移动和追击。(默认无法重组，但是可以改)
    //	☆7	Enter - 进入建筑或运输工具。AI会自动找寻附近的建筑物进驻。
    //	8	Capture - 用于MultiEngineer模式的工程师抢建筑。
    //	9	Eaten - 修理时卖掉(疑似Ra95残留)。
    //	10	Harvest - 采矿。
    //	★11	Area Guard - 区域防御，会主动迎击附近的敌人。
    //	12	Return - 子机回归。
    //	13	Stop - 停止当前动作。
    //	14	Ambush - 无效。
    //	★15	Hunt - 猎杀敌人。(伞兵默认执行此状态。【海军、空军均有效】。默认无法重组，但是可以改)
    //	16	Unload - 运输单位卸货。
    //	17	Sabotage - 放置C4或炸药。
    //	18	Construction - 在建筑建立的位置造建筑。(车辆部署建筑同样有效)
    //	19	Selling - 变卖建筑。
    //	20	Repair - 修理。
    //	21	Rescue - 救援(意味不明)。
    //	22	Missile - 种蘑菇。
    //	23	Harmless - 进入人畜无害状态。
    //	24	Open - 开门开门开门呐！
    //	25	Patrol - 巡逻。(遭遇战无效)
    //	26	Paradrop Approach - 空投靠近。
    //	27	Paradrop Overfly - 空投。
    //	28	Wait - 等待。
    //	29	Move - (特殊) Chrono类单位移动到目的地。
    //	30	Attack - (特殊) 使用AreaFire武器开火。
    //	31	Spyplane Approach - 侦察机靠近。
    //	32	Spyplane Overfly - 侦察机侦查。
    /// </summary>
    public enum Mission
    {
        Sleep,
        Attack,
        Move,
        QMove,
        Retreat,
        Guard,
        Sticky,
        Enter,
        Capture,
        Eaten,
        Harvest,
        AreaGuard,
        Return,
        Stop,
        Ambush,
        Hunt,
        Unload,
        Sabotage,
        Construction,
        Selling,
        Repair,
        Rescue,
        Missile,
        Harmless,
        Open,
        Patrol,
        ParadropApproach,
        ParadropOverfly,
        Wait,
        MoveSpecial,
        AttackSpecial,
        SpyplaneApproach,
        SpyplaneOverfly
    }


    public enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }

    public enum Threat
    {
        Min = 0,
        Max = 65536,
        Nearest = 131072,
        Fatest = 196608
    }


    public enum DialogType
    {
        /// <summary>
        /// 无 
        /// </summary>
        None = 0,
        /// <summary>
        /// *
        /// </summary>
        Asterisk = 1,
        /// <summary>
        /// ?
        /// </summary>
        QuestionMark = 2,
        /// <summary>
        /// !
        /// </summary>
        ExclamationMark = 3
    }
}
