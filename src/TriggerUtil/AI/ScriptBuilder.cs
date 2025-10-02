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
        /// 移动到坐标
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
        /// 跳转到当前Script的第n(从1开始)行。
        /// </summary>
        /// <param name="row">行数(从1开始)</param>
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
        /// 所有可部署单位进行部署，会尝试驱散阻挡的友军单位。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Deploy()
        {
            Scripts.Add((9, 0));
            return this;
        }

        /// <summary>
        /// 令小队跟随最近的友军单位。移动到友军单位附近后完成脚本。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder FollowFriendly()
        {
            Scripts.Add((10, 0));
            return this;
        }

        /// <summary>
        /// 小队所有成员执行指定任务。成员会一直执行这个脚本。
        /// </summary>
        /// <param name="mission">状态</param>
        /// <returns></returns>
        public ScriptBuilder ComeTo(Mission mission)
        {
            Scripts.Add((11, mission.GetHashCode()));
            return this;
        }

        /// <summary>
        /// 如果存在载员和未装满载具，则要求载员进入载具，但不考虑载具SizeLimit，也不考虑剩余位置能否装下载员，可能会因装载而卡住。在装载完所有成员、载具被装满、不存在载具时结束该动作。如果条件不能被满足，脚本会卡在这里。若存在多个载具，会将特遣中行数最靠下的载具视为载具，无视其他条件。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder EnterTransport()
        {
            Scripts.Add((14, 0));
            return this;
        }

        /// <summary>
        /// 小队成员移动攻击到指定路径点，会积极攻击射程内的敌人。其余表现同脚本3 - 移动到路径点。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ScriptBuilder PatrolAround(int wayPoint)
        {
            Scripts.Add((16, wayPoint));
            return this;
        }
        /// <summary>
        /// Obsolete
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeToScript(int index)
        {
            Scripts.Add((17, index));
            return this;
        }
        /// <summary>
        /// 让小队更改小队类型，如果新小队的特遣与本小队成员有重叠，则会依据招募逻辑招募这些成员，并且改变成员的分组(即使新小队没有勾选忽视分组)，其余成员会被解散。参数为包含ai(md)中小队的从0开始的索引。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeToTeam(int index)
        {
            Scripts.Add((18, index));
            return this;
        }

        /// <summary>
        /// 让小队所有步兵惊慌一次，Fraidycat=yes的步兵会使用Panic序列，其余步兵会卧倒。
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
        /// 小队成员全部更改为特定所属方(参数为国家编号)。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public ScriptBuilder ChangeHouse(int house)
        {
            Scripts.Add((20, house));
            return this;
        }

        /// <summary>
        /// 让所有单位分散，类似于玩家按X。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Scatter()
        {
            Scripts.Add((21, 0));
            return this;
        }

        /// <summary>
        /// 让所有单位逃到有黑幕的地方。车辆成员会在执行中获得移动攻击能力。
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
        /// 让所属方开始建造建筑物，效果同触发行为3。IQ>Production的所属方始终会自动建造，不需要本脚本。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder StartProduce()
        {
            Scripts.Add((29, 0));
            return this;
        }

        /// <summary>
        /// 让AI所属方变卖所有建筑并让所有单位进入寻敌(Hunt)状态，效果同触发行为9。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder SellAndWula()
        {
            Scripts.Add((30, 0));
            return this;
        }

        /// <summary>
        /// 让该小队所有成员自毁，会触发死亡武器，会播放Unit Lost语音。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Sucide()
        {
            Scripts.Add((31, 0));
            return this;
        }

        /// <summary>
        /// 删除该小队所有成员，成员凭空消失，不会触发Unit Lost语音。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder DeleteTeam() 
        {
            Scripts.Add((37, 0));
            return this;
        }



        /// <summary>
        /// 使所有小队成员停止惊慌。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Calm()
        {
            Scripts.Add((41, 0));
            return this;
        }

        /// <summary>
        /// 强制小队成员面向一个特定的方向，执行后立即跳转至下一条，不会等待转向完成。
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
        /// 小队会侦察玩家未探索的区域。如果要持续侦查，应当循环执行此脚本。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Detect()
        {
            Scripts.Add((48, 0));
            return this;
        }


        /// <summary>
        /// 闪烁所有小队成员一段时间(参数为闪烁的帧数)。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Flash(int frames)
        {
            Scripts.Add((50, frames));
            return this;
        }




        /// <summary>
        /// 在每个小队单位上播放动画，会跟随成员运动。成员死亡动画随即消失。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder PlayAnim(int animIndex)
        {
            Scripts.Add((51, animIndex));
            return this;
        }

        /// <summary>
        /// 在小队第一个单位上显示对话气泡。原版中需要添加素材才能生效
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder Dialog(DialogType dialogType)
        {
            Scripts.Add((52, dialogType.GetHashCode()));
            return this;
        }




        /// <summary>
        ///  攻占路径点上的建筑
        /// 攻击占据该路径点的建筑物。AI间谍类单位(渗透、占领、C4、驻扎、突击者)能够正常攻击、驻扎建筑，人类玩家除C4外无法正常使用。如果路径点上没有建筑，则会跳过该脚本。如果该建筑是友军，则会在原地锁定直至对象消失，人类玩家单位会攻击友军。
        /// </summary>
        /// <returns></returns>
        public ScriptBuilder AttackBuildingOnWaypoint(int waypoint)
        {
            Scripts.Add((59,waypoint));
            return this;
        }










        /// <summary>
        /// 区域警戒（时间计数(Phobos B31+)）
        /// 原地区域警戒(Area Guard) X 秒。区域警戒的单位将更加激进的处理附近敌军单位。(Phobos B31+)
        /// </summary>
        /// <param name="seconds">秒</param>
        /// <returns></returns>
        public ScriptBuilder AreaGuard(int seconds)
        {
            Scripts.Add((10100, seconds));
            return this;
        }





    }
}
