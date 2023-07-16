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


}
