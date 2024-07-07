using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TriggerUtil
{
    public class ActionBuilder
    {
        public ActionBuilder(TriggerBuilder trigger)
        {
            _trigger = trigger;
        }

        public delegate void NextTriggerAttachedEventHandler(string uniqueId);

        public event NextTriggerAttachedEventHandler OnAttachNextTrigger;

        private TriggerBuilder _trigger;

        public List<string> Actions { get; private set; } = new List<string>();

        public List<string> TeamNodes { get;private set; } = new List<string>();

        #region 结果
        /// <summary>
        /// 空行为，没作用
        /// </summary>
        /// <returns></returns>
        public ActionBuilder Nothing()
        {
            Actions.Add($"0,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 胜利者是...
        /// 特定的所属方会成为胜利者，游戏会立即结束。通常被指定的是玩家所属方，单人战役若胜利方不是玩家所属方，则玩家失败。多人地图时此结果变为“失败者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public ActionBuilder DeclareWinner(Enum owner)
        {
            return DeclareWinner(owner.GetHashCode());
        }
        /// <summary>
        /// 胜利者是...
        /// 特定的所属方会成为胜利者，游戏会立即结束。通常被指定的是玩家所属方，单人战役若胜利方不是玩家所属方，则玩家失败。多人地图时此结果变为“失败者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public ActionBuilder DeclareWinner(int owner)
        {
            Actions.Add($"1,0,{owner},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 失败者是...
        /// 特定的所属方会成为失败者，游戏会立即结束。通常被指定的是玩家所属方。多人地图时此结果变为“胜利者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public ActionBuilder DeclareLoser(Enum owner)
        {
            return DeclareLoser(owner.GetHashCode());
        }
        /// <summary>
        /// 失败者是...
        /// 特定的所属方会成为失败者，游戏会立即结束。通常被指定的是玩家所属方。多人地图时此结果变为“胜利者是”的效果。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public ActionBuilder DeclareLoser(int owner)
        {
            Actions.Add($"2,0,{owner.GetHashCode()},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 生产开始
        /// 特定的电脑所属方将开始生产单位和建筑。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder StartProduction(Enum house)
        {
            return StartProduction(house.GetHashCode());
        }

        /// <summary>
        /// 生产开始
        /// 特定的电脑所属方将开始生产单位和建筑。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder StartProduction(int house)
        {
            Actions.Add($"3,0,{house},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 建立作战小队
        /// 建立一个指定的作战小队 (归属于触发所属方)。通常会直接从已有单位中招募，但无法招募已经加入其他小队的单位。如果没有足够的单位来组建小队，AI会通过生产补足(自动建造)。当所有成员就位时，小队即自动建立。
        /// </summary>
        /// <param name="team">作战小队Id</param>
        /// <returns></returns>
        public ActionBuilder EstablishTeam(string team)
        {
            Actions.Add($"4,1,{team},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 解散作战小队
        /// 解散当前存在的所有特定类型的作战小队。小队的单位会保留，并可以作为新兵加入其他小队。
        /// </summary>
        /// <param name="team">作战小队Id</param>
        /// <returns></returns>
        public ActionBuilder DissolveTeam(string team)
        {
            Actions.Add($"5,1,{team},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 全部寻敌
        /// 特定的所属方的所有单位进入寻敌(Hunt)模式。他们将搜索并消灭敌人。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder AllHunt(Enum house)
        {
            return AllHunt(house.GetHashCode());
        }

        /// <summary>
        /// 全部寻敌
        /// 特定的所属方的所有单位进入寻敌(Hunt)模式。他们将搜索并消灭敌人。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder AllHunt(int house)
        {
            Actions.Add($"6,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 援军(作战小队)...
        /// 创建由指定作战小队组成的援军。小队成员直接刷出，刷兵路径点为小队设置中的路径点。
        /// </summary>
        /// <param name="team">作战小队Id</param>
        /// <returns></returns>
        public ActionBuilder Reinforcements(string team)
        {
            Actions.Add($"7,1,{team},0,0,0,0,A");
            TeamNodes.Add(team);
            return this;
        }

        /// <summary>
        /// 降落区闪烁(路径点)...
        /// 在指定路径点显示一个空降舱降落区域动画，该动画为循环播放。此区域附近的地图也将会显示。
        /// </summary>
        /// <param name="wapPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder BlinkDropZone(int wapPoint)
        {
            Actions.Add($"8,0,0,0,0,0,0,{wapPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 卖掉全部建筑...
        /// 特定所属方会变卖掉所有建筑(来得到金钱和单位)。常用于电脑发动的最后攻击。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder SellAllBuilding(Enum house)
        {
            return SellAllBuilding(house.GetHashCode());
        }

        /// <summary>
        /// 卖掉全部建筑...
        /// 特定所属方会变卖掉所有建筑(来得到金钱和单位)。常用于电脑发动的最后攻击。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder SellAllBuilding(int house)
        {
            Actions.Add($"9,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 全屏播放影片...
        /// 全屏播放特定影片。游戏在此期间将会暂停，在播放完成后自动恢复正常。
        /// </summary>
        /// <param name="movie">影片</param>
        /// <returns></returns>
        public ActionBuilder PlayMovieFullScreen(string movie)
        {
            Actions.Add($"10,0,{movie},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 文本触发事件...
        /// 显示文本内容，参数为CSF文件内的项目。CSF文件需要用CSF编辑器编辑。
        /// </summary>
        /// <param name="label">CSF中的label</param>
        /// <returns></returns>
        public ActionBuilder TriggerText(string label)
        {
            Actions.Add($"11,4,{label},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 摧毁触发事件...
        /// 摧毁所有特定触发事件的当前实例。但不会阻止已经被触发完成的实例(包括正在建立中的)。
        /// </summary>
        /// <param name="triggerid">触发id</param>
        /// <returns></returns>
        public ActionBuilder DestroyTrigger(string triggerid)
        {
            Actions.Add($"12,2,{triggerid},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 开始自动创建作战小队...
        /// 特定所属方会开始自动创建。这会使得电脑所属方在它认为合适的时机自动建造作战小队。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder StartEstablishTeam(Enum house)
        {
            return StartEstablishTeam(house.GetHashCode());
        }

        /// <summary>
        /// 开始自动创建作战小队...
        /// 特定所属方会开始自动创建。这会使得电脑所属方在它认为合适的时机自动建造作战小队。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder StartEstablishTeam(int house)
        {
            Actions.Add($"13,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 更改所属方...
        /// 更改关联对象到特定的所属方。对象装载的载员不受此触发的作用。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder ChangeHouse(Enum house)
        {
            return ChangeHouse(house.GetHashCode());
        }

        /// <summary>
        /// 更改所属方...
        /// 更改关联对象到特定的所属方。对象装载的载员不受此触发的作用。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder ChangeHouse(int house)
        {
            Actions.Add($"14,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 显示全地图
        /// 为玩家显示全部地图(清除黑幕)。联机使用会导致RE(重新连线错误)。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder RevealWholeMap()
        {
            Actions.Add($"16,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 显示路径点周围区域
        /// 为玩家显示特定路径点周围的一片地图区域。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder RevealWayPoint(int wayPoint)
        {
            Actions.Add($"17,0,{wayPoint},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 显示路径点的单元区域
        /// 为玩家显示由路径点划定的「区域」中所有的格子。「区域」的定义参见事件24。详细解释另见工具教程里的“触发备忘”。。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder RevealWayPointZone(int wayPoint)
        {
            Actions.Add($"18,0,{wayPoint},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 播放音效...
        /// 播放特定音效。在Sound(md).ini中定义。
        /// </summary>
        /// <param name="sound">音效的注册名</param>
        /// <returns></returns>
        public ActionBuilder PlaySound(string sound)
        {
            Actions.Add($"19,7,{sound},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 播放音乐...
        /// 播放特定音乐。在Theme(md).ini中定义。
        /// </summary>
        /// <param name="sound">音乐的注册名</param>
        /// <returns></returns>
        public ActionBuilder PlayMusic(string music)
        {
            Actions.Add($"20,8,{music},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 播放语音...
        /// 播放特定语音。在Eva(md).ini中定义。
        /// </summary>
        /// <param name="eva">eva的注册名</param>
        /// <returns></returns>
        public ActionBuilder PlayEva(string eva)
        {
            Actions.Add($"21,6,{eva},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 强制触发事件...
        /// 强制特定类型的所有触发事件进行触发，不管触发此事件的条件。不要对该触发本身使用！
        /// </summary>
        /// <param name="trigger">触发Id</param>
        /// <returns></returns>
        public ActionBuilder TriggerForce(string trigger)
        {
            if (trigger == _trigger.UniqueId)
                throw new Exception("强制触发不允许触发自身");
            Actions.Add($"22,2,{trigger},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时开始
        /// 启动全局任务计时器。一定时间内可显示的全局计时器有且只有一个。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder StartTimer()
        {
            Actions.Add($"23,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时停止
        /// 停止全局任务计时器。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder StopTimer()
        {
            Actions.Add($"24,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时器计时增加..
        /// 将全局任务计时器增加指定的时间。
        /// </summary>
        /// <param name="count">时间</param>
        /// <returns></returns>
        public ActionBuilder AddTimer(int count)
        {
            Actions.Add($"25,0,{count},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时器计时缩短...
        /// 将全局任务计时器减少指定的时间。时间不会减到 0 以下。
        /// </summary>
        /// <param name="count">时间</param>
        /// <returns></returns>
        public ActionBuilder SubstractTimer(int count)
        {
            Actions.Add($"26,0,{count},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 计时设置...
        /// 将全局任务计时器设置为指定的时间值。
        /// </summary>
        /// <param name="count">时间</param>
        /// <returns></returns>
        public ActionBuilder SetTimer(int count)
        {
            Actions.Add($"27,0,{count},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 设置全局变量...
        /// 设置全局变量(1)。全局变量在Rules(md).ini中的[VariableNames]里定义。
        /// </summary>
        /// <param name="idx">变量序号</param>
        /// <returns></returns>
        public ActionBuilder SetGlobalVariable(int idx)
        {
            Actions.Add($"28,0,{idx},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 清除全局变量
        /// 清除全局变量(0)。全局变量在Rules(md).ini中的[VariableNames]里定义。
        /// </summary>
        /// <param name="idx">变量序号</param>
        /// <returns></returns>
        public ActionBuilder ClearGlobalVariable(int idx)
        {
            Actions.Add($"29,0,{idx},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 自动建设基地...
        /// 初始化电脑遭遇战模式的建设控制，可以是[ON]或[OFF]状态。当设为[ON]且电脑所属方无基地节点时，将会像遭遇战模式那样自动进行建设(要确保有建造场)；如果有基地节点，则仍会建造节点对应的建筑。
        /// </summary>
        /// <param name="allowed">是/否</param>
        /// <returns></returns>
        public ActionBuilder AutoConstract(bool allowed = true)
        {
            Actions.Add($"30,0,{(allowed ? 1 : 0)},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 逐单元延伸黑幕
        /// 增大地图的黑幕(一步一单元)，需要相应的延伸黑幕INI设置才能使用。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder ExtendFog()
        {
            Actions.Add($"31,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 摧毁关联对象
        /// 摧毁该触发关联的任何建筑物、桥梁或者单位。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder Destroy()
        {
            Actions.Add($"32,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 添加一次特定超武
        /// 为触发所属方添加一次(只一次)特定超武。在这里设置的核弹可以发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public ActionBuilder GiveSuperWeaponOnce(Enum superWeaponType)
        {
            return GiveSuperWeaponOnce(superWeaponType.GetHashCode());
        }

        /// <summary>
        /// 添加一次特定超武
        /// 为触发所属方添加一次(只一次)特定超武。在这里设置的核弹可以发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public ActionBuilder GiveSuperWeaponOnce(int superWeaponType)
        {
            Actions.Add($"33,0,{superWeaponType},0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 重复添加特定超武
        /// 为触发所属方添加永久的特定超武。在这里设置的核弹无法正常发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public ActionBuilder GiveSuperWeapon(Enum superWeaponType)
        {
            return GiveSuperWeapon(superWeaponType.GetHashCode());
        }

        /// <summary>
        /// 重复添加特定超武
        /// 为触发所属方添加永久的特定超武。在这里设置的核弹无法正常发射。
        /// </summary>
        /// <param name="superWeaponType">超武序号</param>
        /// <returns></returns>
        public ActionBuilder GiveSuperWeapon(int superWeaponType)
        {
            Actions.Add($"34,0,{superWeaponType},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 超武首选目标为特定建筑...
        /// 指定触发所属方的 电脑玩家 使用超武攻击时的首选目标，参数为[BuildingTypes]里的ID。只对打击性超武（如核弹、闪电，Ares对应SW.AITargeting=Offensive）有效。
        /// </summary>
        /// <param name="buildingType"></param>
        /// <returns></returns>
        public ActionBuilder SetAISuperWeaponTarget(Enum buildingType)
        {
            return SetAISuperWeaponTarget(buildingType.GetHashCode());
        }

        /// <summary>
        /// 超武首选目标为特定建筑...
        /// 指定触发所属方的 电脑玩家 使用超武攻击时的首选目标，参数为[BuildingTypes]里的ID。只对打击性超武（如核弹、闪电，Ares对应SW.AITargeting=Offensive）有效。
        /// </summary>
        /// <param name="buildingType">建筑序号</param>
        /// <returns></returns>
        public ActionBuilder SetAISuperWeaponTarget(int buildingType)
        {
            Actions.Add($"35,0,{buildingType},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 全部更改所属...
        /// 触发所属方的所有对象更改所属到特定所属方。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder ChangeAllHouse(Enum house)
        {
            return ChangeAllHouse(house.GetHashCode());
        }

        /// <summary>
        /// 全部更改所属...
        /// 触发所属方的所有对象更改所属到特定所属方。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder ChangeAllHouse(int house)
        {
            Actions.Add($"36,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 结盟...
        /// 使触发所属方与特定所属方结盟。单人任务中是不完全双向结盟（比如A、B双方互不攻击，B有A视野 而 A无B视野。AB同时与对方结盟时，双方均没有对方视野），多人任务中是单向结盟。若多人任务中有至少两个玩家选与触发所属方相同的国家，则只选一个参与结盟。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder AlliedWith(Enum house)
        {
            return AlliedWith(house.GetHashCode());
        }

        /// <summary>
        /// 结盟...
        /// 使触发所属方与特定所属方结盟。单人任务中是不完全双向结盟（比如A、B双方互不攻击，B有A视野 而 A无B视野。AB同时与对方结盟时，双方均没有对方视野），多人任务中是单向结盟。若多人任务中有至少两个玩家选与触发所属方相同的国家，则只选一个参与结盟。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder AlliedWith(int house)
        {
            Actions.Add($"37,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 成为敌人...
        /// 使触发所属方与特定所属方不结盟(宣战)，这个效果是单向的。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public ActionBuilder BeEnermyWith(Enum house)
        {
            return BeEnermyWith(house.GetHashCode());
        }

        /// <summary>
        /// 成为敌人...
        /// 使触发所属方与特定所属方不结盟(宣战)，这个效果是单向的。
        /// </summary>
        /// <param name="house"></param>
        /// <returns></returns>
        public ActionBuilder BeEnermyWith(int house)
        {
            Actions.Add($"38,0,{house},0,0,0,0,A");
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
        public ActionBuilder ChangePlayerView(int x, int y, int w, int h)
        {
            Actions.Add($"40,0,0,{x},{y},{w},{h},A");
            return this;
        }

        /// <summary>
        /// 播放动画在...
        /// 在特定的路径点播放特定的动画，参数为[Animations]里的动画ID。原版动画注册表存在序号漂移，建议使用脚本或Excel予以修正。
        /// </summary>
        /// <param name="animType">动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder PlayAnimAt(Enum animType, int wayPoint)
        {
            return PlayAnimAt(animType.GetHashCode(), wayPoint);
        }


        /// <summary>
        /// 播放动画在...
        /// 在特定的路径点播放特定的动画，参数为[Animations]里的动画ID。原版动画注册表存在序号漂移，建议使用脚本或Excel予以修正。
        /// </summary>
        /// <param name="animType">动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder PlayAnimAt(int animType, int wayPoint)
        {
            Actions.Add($"41,0,{animType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 武器（弹头）爆炸在...
        /// 使用特定「武器的弹头」在指定路径点产生一次爆炸，但无法触发弹头的特殊效果。参数为[WeaponTypes]里的武器ID（从1开始），若无该注册表（或者注册表里没有所填ID）则取[Warheads]里的弹头ID。
        /// </summary>
        /// <param name="weaponType">武器</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder ExplodeWeaponAt(Enum weaponType, int wayPoint)
        {
            return ExplodeWeaponAt(weaponType.GetHashCode(), wayPoint);
        }

        /// <summary>
        /// 武器（弹头）爆炸在...
        /// 使用特定「武器的弹头」在指定路径点产生一次爆炸，但无法触发弹头的特殊效果。参数为[WeaponTypes]里的武器ID（从1开始），若无该注册表（或者注册表里没有所填ID）则取[Warheads]里的弹头ID。
        /// </summary>
        /// <param name="weaponType">武器</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder ExplodeWeaponAt(int weaponType, int wayPoint)
        {
            Actions.Add($"42,0,{weaponType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 播放 Voxel 动画
        /// 在指定路径点播放VXL动画，参数为Rules(md).ini中[VoxelAnims]里的ID。其中8是陨石。
        /// </summary>
        /// <param name="voxelAnim">vxl动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder PlayVoxelAnimAt(Enum voxelAnim, int wayPoint)
        {
            return PlayVoxelAnimAt(voxelAnim.GetHashCode(), wayPoint);
        }

        /// <summary>
        /// 播放 Voxel 动画
        /// 在指定路径点播放VXL动画，参数为Rules(md).ini中[VoxelAnims]里的ID。其中8是陨石。
        /// </summary>
        /// <param name="voxelAnim">vxl动画</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder PlayVoxelAnimAt(int voxelAnim, int wayPoint)
        {
            Actions.Add($"43,0,{voxelAnim},0,0,0,0,{wayPoint.To26()}");
            return this;
        }


        /// <summary>
        /// 禁止玩家输入
        /// 禁止玩家进行操作，失去鼠标无法控制。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder DisablePlayerControl()
        {
            Actions.Add($"46,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 允许玩家输入
        /// 允许玩家进行操作，取消动作46的效果。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder EnablePlayerControl()
        {
            Actions.Add($"47,0,0,0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 移动并居中视野到路径点...
        /// 将玩家视野移动到特定的路径点。速度取1～4，太大会造成卡屏，无法移动视野。
        /// </summary>
        /// <param name="speed">卷动速度</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder MoveViewTo(ViewMoveSpeed speed, int wayPoint)
        {
            Actions.Add($"48,0,{speed.GetHashCode()},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 放大视野
        /// 放大玩家视野。减少分辨率，同时不能输入。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder ZoomInView()
        {
            Actions.Add($"49,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 缩小视野
        /// 缩小玩家视野。可以取消49放大视野的作用，增加分辨率，恢复输入。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder ZoomOutView()
        {
            Actions.Add($"50,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 重置全地图黑幕
        /// 触发所属方将会被重置整张地图的黑幕。联机使用容易RE(重新连线错误)。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder ResetFog()
        {
            Actions.Add($"51,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 更改照明状态 (Ares平台有效)
        /// 更改与该触发关联的探照灯建筑的照明方式。0：无局部照明、1：照明特定角度、2：圆圈、3：跟随敌对目标
        /// </summary>
        /// <returns></returns>
        public ActionBuilder ChangeSpotLightBehavior(SpotLightBehavior behavior)
        {
            Actions.Add($"52,0,{behavior.GetHashCode()},0,0,0,0,A");
            return this;
        }



        /// <summary>
        /// 允许触发
        /// </summary>
        /// <param name="triggerId">触发Id</param>
        /// <returns></returns>
        public ActionBuilder Enable(string triggerId)
        {
            Actions.Add($"53,2,{triggerId},0,0,0,0,A");
            OnAttachNextTrigger?.Invoke(triggerId);
            return this;
        }

        /// <summary>
        /// 禁止触发
        /// </summary>
        /// <param name="triggerId"></param>
        /// <returns></returns>
        public ActionBuilder Disable(string triggerId)
        {
            Actions.Add($"54,2,{triggerId},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 禁止当前触发
        /// </summary>
        /// <returns></returns>
        public ActionBuilder DisableSelf()
        {
            Actions.Add($"54,2,{this._trigger.UniqueId},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 建立小地图事件
        /// 在特定的路径点建立雷达小地图事件。0、3、4：红框，1、2：黄框，5：蓝框。
        /// </summary>
        /// <param name="radarEvent">雷达事件类型</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder RaiseRadarEvent(RadarEvent radarEvent, int wayPoint)
        {
            Actions.Add($"55,0,{radarEvent.GetHashCode()},0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 设置局部变量
        /// 设置局部变量标记(1)。
        /// </summary>
        /// <param name="variable">局部变量序号</param>
        /// <returns></returns>
        public ActionBuilder SetLocalVariable(int variable)
        {
            Actions.Add($"56,0,{variable},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 清除局部变量
        /// 清除局部变量标记(0)。
        /// </summary>
        /// <param name="variable">局部变量序号</param>
        /// <returns></returns>
        public ActionBuilder ClearLocalVariable(int variable)
        {
            Actions.Add($"57,0,{variable},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 变卖关联建筑
        /// 变卖所有与此触发关联的建筑。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder SellBuilding()
        {
            Actions.Add($"60,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 关闭关联建筑
        /// 关闭与此触发关联的建筑。效果与建筑中耗能=0相同。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder ShutDownBuilding()
        {
            Actions.Add($"61,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 启动关联建筑
        /// 启动与此触发关联的建筑。效果与建筑中耗能=1相同。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder SwitchOnBuilding()
        {
            Actions.Add($"62,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 在...造成100点伤害
        /// 在特定的路径点造成100点爆炸伤害，对于建筑的实际效果约为500点，可以摧毁地面桥梁。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder Take100DamageAt(int wayPoint)
        {
            Actions.Add($"63,0,{wayPoint},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 宣告胜利...
        /// 宣告玩家胜利。但不会显示“任务完成”的图片。。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder DeclarePlayerWin()
        {
            Actions.Add($"67,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 宣告失败...
        /// 宣告玩家失败。但不会显示“任务失败”的图片。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder DeclarePlayerLose()
        {
            Actions.Add($"68,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 强制结束任务
        /// 强制结束游戏任务。会显示“任务完成”的图片。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder TerminateMission()
        {
            Actions.Add($"69,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// AI触发开始...
        /// 启动特定所属方的AI触发。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder StartAi(Enum house)
        {
            return StartAi(house.GetHashCode());
        }


        /// <summary>
        /// AI触发开始...
        /// 启动特定所属方的AI触发。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder StartAi(int house)
        {
            Actions.Add($"74,0,{house},0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// AI触发停止...
        /// 停止特定所属方的AI触发。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder EndAi(Enum house)
        {
            return EndAi(house.GetHashCode());
        }

        /// <summary>
        /// AI触发停止...
        /// 停止特定所属方的AI触发。
        /// </summary>
        /// <param name="house">所属方</param>
        /// <returns></returns>
        public ActionBuilder EndAi(int house)
        {
            Actions.Add($"75,0,{house},0,0,0,0,A");
            return this;
        }




        /// <summary>
        /// 唤醒...
        /// 唤醒「关联」的睡眠(Sleep)或无害(Harmless)的单位，让他们进入固守(Guard)模式。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder WakeTeam()
        {
            Actions.Add($"81,0,0,0,0,0,0,A");
            return this;
        }

        /// <summary>
        /// 唤醒所有睡眠的单位...
        /// 唤醒所有「非」触发所属方 的睡眠(Sleep)单位，让他们进入固守(Guard)模式。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder WakeAllSleep()
        {
            Actions.Add($"82,0,0,0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 83 唤醒所有无害的单位
        /// 唤醒所有 触发所属方 的无害(Harmless)单位，让他们进入固守(Guard)模式。
        /// </summary>
        /// <returns></returns>
        public ActionBuilder WakeAllHarmless()
        {
            Actions.Add($"83,0,0,0,0,0,0,A");
            return this;
        }


        /// <summary>
        /// 援军(作战小队)[在路径点]...
        /// 在特定的路径点创建援军作战小队。小队成员会被直接刷出，刷兵路径点为行为设置中的路径点。
        /// </summary>
        /// <param name="teamType">作战小队</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder ReinforcementsAt(string teamType, int wayPoint)
        {
            Actions.Add($"80,1,{teamType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }







        /// <summary>
        /// 计时器文本
        /// 指定计时器显示的文本，参数为CSF文件内的项目。
        /// </summary>
        /// <param name="label">csf文本</param>
        /// <returns></returns>
        public ActionBuilder SetTimerLabel(string label)
        {
            Actions.Add($"103,4,{label},0,0,0,0,A");
            return this;
        }




        /// <summary>
        /// 超时空传送援军... 
        /// 在特定的路径点创建援军作战小队，并播放传送动画。小队成员会凭空分散刷出。
        /// </summary>
        /// <param name="teamType">作战小队</param>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder ChronoReinforceAt(string teamType, int wayPoint)
        {
            Actions.Add($"107,1,{teamType},0,0,0,0,{wayPoint.To26()}");
            return this;
        }



        /// <summary>
        /// 让特定所属方的所有空闲的步兵单位执行欢呼动作。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public ActionBuilder Cheer(Enum owner)
        {
            return Cheer(owner.GetHashCode());
        }

        /// <summary>
        /// 居中视野到特定路径点...
        /// 立即将指挥视野移动到特定的路径点。
        /// </summary>
        /// <param name="wayPoint">路径点</param>
        /// <returns></returns>
        public ActionBuilder CenterViewToWayPoint(int wayPoint)
        {
            Actions.Add($"112,0,0,0,0,0,0,{wayPoint.To26()}");
            return this;
        }

        /// <summary>
        /// 让特定所属方的所有空闲的步兵单位执行欢呼动作。
        /// </summary>
        /// <param name="owner">所属方</param>
        /// <returns></returns>
        public ActionBuilder Cheer(int owner)
        {
            Actions.Add($"113,0,{owner},0,0,0,0,A");
            return this;
        }


        #endregion
    }
}
