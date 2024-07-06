using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil.AI
{
    public class ScriptBuilder
    {
        private TriggerContext _context;

        internal ScriptBuilder(TriggerContext context)
        {
            _context = context;
            var tuple = IdGenerator.NextScript();
            No = tuple.id;
            UniqueId = tuple.name;
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 注册序号
        /// </summary>
        public string No { get; private set; }
        /// <summary>
        /// 注册名
        /// </summary>
        public string UniqueId { get; private set; }

        public List<(int, object)> Scripts = new List<(int, object)>();

        /// <summary>
        /// 设置名称
        /// </summary>
        /// <param name="name"></param>
        public void WithName(string name)
        {
            Name = name;
        }

        /// <summary>
        /// 攻击
        /// </summary>
        /// <param name="targetType">目标类型</param>
        /// <returns></returns>
        public ScriptBuilder AttackNearestTarget(AttackTargetType targetType)
        {
            Scripts.Add((0,targetType.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 攻击路径点
        /// </summary>
        /// <param name="waypoint">路径点</param>
        /// <returns></returns>
        public ScriptBuilder AttackWayPoint(int waypoint)
        {
            Scripts.Add((1, waypoint));
            return this;
        }

        /// <summary>
        /// 移动到路径点
        /// </summary>
        /// <param name="waypoint">路径点</param>
        /// <returns></returns>
        public ScriptBuilder MoveToWayPoint(int waypoint)
        {
            Scripts.Add((3, waypoint));
            return this;
        }

        /// <summary>
        /// 移动到路径点
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns></returns>
        public ScriptBuilder MoveToCoord(int x,int y)
        {
            Scripts.Add((4, x + y * 128));
            return this;
        }

        /// <summary>
        /// 警戒（时间计数）
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public ScriptBuilder Guard(int seconds)
        {
            Scripts.Add((5,seconds));
            return this;
        }

        /// <summary>
        /// 跳转到当前Script的第n(n=索引+1)行。
        /// </summary>
        /// <param name="row">行数(索引+1)</param>
        /// <returns></returns>
        public ScriptBuilder SkipTo(int row)
        {
            Scripts.Add((6, row));
            return this;
        }

        /// <summary>
        /// 钦点该Team所有者获胜。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder TeamWin()
        {
            Scripts.Add((7, 0));
            return this;
        }


        /// <summary>
        /// 释放乘客
        /// </summary>
        /// <param name="result">释放后的结果</param>
        /// <returns></returns>
        public ScriptBuilder Unload(UnloadResult result)
        {
            Scripts.Add((8, result.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 部署(步兵、基地车、武装直升机皆有效)
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Deploy()
        {
            Scripts.Add((9, 0));
            return this;
        }

        /// <summary>
        /// 跟随最近的友好单位。(如果写在Script的第一行，将会无条件跟随产生的第一个小队)
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder FollowFriendly()
        {
            Scripts.Add((10, 0));
            return this;
        }

        /// <summary>
        /// 进入第n种状态
        /// </summary>
        /// <param name="mission">状态</param>
        /// <returns></returns>
        public ScriptBuilder ComeTo(Mission mission)
        {
            Scripts.Add((11, mission.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 队伍中存在有Passengers位置的运输工具，并且其余单位Size和PhysicalSize满足运输条件时，乘客们进入运输工具。(实际使用时，一个队伍里只能存在1个运输工具)(运输步兵时可以在14,0之后加43,0保证单位装载完毕，运输车辆时不可接43,0)
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder EnterTransport()
        {
            Scripts.Add((14, 0));
            return this;
        }

        /// <summary>
        /// 沿着n号路径点巡逻。巡逻途中会按照PatrolScan搜寻附近敌人并与敌人交战。通常用于任务，多了会卡……
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ScriptBuilder PatrolAround(int wayPoint)
        {
            Scripts.Add((16, wayPoint));
            return this;
        }
        /// <summary>
        /// 变为执行第n号脚本(n为该脚本在注册列表中的【顺序号】而非索引号，从0开始)。理论上可以用来实现超过50行的脚本……
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeToScript(int index)
        {
            Scripts.Add((17, index));
            return this;
        }
        /// <summary>
        /// 改变小队，让当前小队加入第n号小队(n为该小队在注册列表中的【顺序号】而非索引号，从0开始)。理论上可以用来实现超过6行的大队……
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeToTeam(int index)
        {
            Scripts.Add((18, index));
            return this;
        }

        /// <summary>
        /// 恐慌，小队中有Fraidycat标签的成员会乱跑,并且播放Panic序列，没有该标签的会卧倒。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Panic()
        {
            Scripts.Add((19, 0));
            return this;
        }

        /// <summary>
        /// 变更所属方
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeHouse(Enum house)
        {
            ChangeHouse(house.GetHashCode());
            return this;
        }

        /// <summary>
        /// 变更所属方
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeHouse(int house)
        {
            Scripts.Add((20, house));
            return this;
        }

        /// <summary>
        /// 分散部队，某些情况下有奇效。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Scatter()
        {
            Scripts.Add((21, 0));
            return this;
        }

        /// <summary>
        /// 进入周围的黑幕
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder MoveIntoShround()
        {
            Scripts.Add((22, 0));
            return this;
        }

        /// <summary>
        /// 决定该Team所有者失败
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder TeamLost()
        {
            Scripts.Add((23, 0));
            return this;
        }

        /// <summary>
        /// 任务中用于让AI自动建造。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder StartProduce()
        {
            Scripts.Add((29, 0));
            return this;
        }

        /// <summary>
        /// 卖家一波流。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder SellAndWula()
        {
            Scripts.Add((30, 0));
            return this;
        }

        /// <summary>
        /// 小队自毁
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Sucide()
        {
            Scripts.Add((31, 0));
            return this;
        }

        /// <summary>
        /// 镇定，与19相反
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Calm()
        {
            Scripts.Add((41, 0));
            return this;
        }

        /// <summary>
        /// 改变朝向。
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeDirection(Direction direction)
        {
            Scripts.Add((42, direction.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 等待运输单位完全装载完毕。运载步兵时可以用在14,0之后，运载车辆时不要使用！
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder WaitForEntered()
        {
            Scripts.Add((43, 0));
            return this;
        }

        /// <summary>
        /// 攻击N指定建筑。(工程师为占领、间谍为渗透、突击者为清驻兵、海军可用此指令攻击陆地建筑)
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="threat"></param>
        /// <returns></returns>
        public ScriptBuilder AttackBuilding(Enum buildingType, Threat threat)
        {
            AttackBuilding(buildingType.GetHashCode(), threat);
            return this;
        }

        /// <summary>
        /// 攻击N指定建筑。(工程师为占领、间谍为渗透、突击者为清驻兵、海军可用此指令攻击陆地建筑)
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="threat"></param>
        /// <returns></returns>
        public ScriptBuilder AttackBuilding(int buildingType, Threat threat)
        {
            Scripts.Add((46, buildingType + threat.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 移动到N指定敌方或中立建筑附近。N的计算方法同上，"附近"的范围由CloseEnough指定。(海军可用)
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="threat"></param>
        /// <returns></returns>
        public ScriptBuilder MoveToBuilding(Enum buildingType, Threat threat)
        {
            MoveToBuilding(buildingType.GetHashCode(), threat);
            return this;
        }

        /// <summary>
        /// 移动到N指定敌方或中立建筑附近。N的计算方法同上，"附近"的范围由CloseEnough指定。(海军可用)
        /// </summary>
        /// <param name="buildingType"></param>
        /// <param name="threat"></param>
        /// <returns></returns>
        public ScriptBuilder MoveToBuilding(int buildingType, Threat threat)
        {
            Scripts.Add((47, buildingType + threat.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 侦查。(遭遇战无效)
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Detect()
        {
            Scripts.Add((48, 0));
            return this;
        }


        /// <summary>
        /// 闪烁一队一段时间（参数为帧数）
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Flash(int frames)
        {
            Scripts.Add((50, frames));
            return this;
        }

   


        /// <summary>
        /// 播放动画
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder PlayAnim(int animIndex)
        {
            Scripts.Add((51, animIndex));
            return this;
        }

        /// <summary>
        /// 闪烁一队一段时间（参数为帧数）
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Dialog(DialogType dialogType)
        {
            Scripts.Add((52, dialogType.GetHashCode()));
            return this;
        }

    }
}
