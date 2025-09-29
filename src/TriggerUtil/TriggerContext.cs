﻿using IniParser;
using IniParser.Model;
using IniParser.Parser;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TriggerUtil.AI;
using TriggerUtil.Chart;

namespace TriggerUtil
{
    public class TriggerContext
    {
        public List<TriggerBuilder> triggers = new List<TriggerBuilder>();
        public List<TeamBuilder> teams = new List<TeamBuilder>();
        public List<TaskForceBuilder> forces = new List<TaskForceBuilder>();
        public List<ScriptBuilder> scripts = new List<ScriptBuilder>();


        public TriggerBuilder CreateTrigger()
        {
            var trigger = new TriggerBuilder(this);
            triggers.Add(trigger);
            return trigger;
        }

        public TeamBuilder CreateTeam()
        {
            var team = new TeamBuilder(this);
            teams.Add(team);
            return team;
        }

        public TaskForceBuilder CreateTaskForce()
        {
            var force = new TaskForceBuilder(this);
            forces.Add(force);
            return force;
        }

        public ScriptBuilder CreateScript()
        {
            var script = new ScriptBuilder(this);
            scripts.Add(script);
            return script;
        }

        /// <summary>
        /// 根据名称查找触发，返回满足条件的第一个触发，如果查找的名称不唯或者不存在会抛出异常
        /// </summary>
        /// <param name="name">触发名</param>
        /// <returns>TriggerBuilder</returns>
        public TriggerBuilder FindTriggerByName(string name)
        {
            return triggers!.Single(x => x.TriggerName == name);
        }

        /// <summary>
        /// 编译并生成文件path
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool Compile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Invilid Path");

            var fileinfo = new FileInfo(path);

            IniData data = new IniData();
            SetIniData(data, null);

            var parser = new FileIniDataParser();

          
            if (!Directory.Exists(fileinfo!.Directory!.FullName))
            {
                Directory.CreateDirectory(fileinfo.Directory.FullName);
            }
     
            using var sw = new StreamWriter(path, false);
            parser.WriteData(sw, data);

            return true;
        }

        /// <summary>
        /// 附加模式，将触发附加到原有的地图上
        /// </summary>
        /// <param name="path">地图的路径</param>
        /// <returns></returns>
        public bool Append(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Invilid Path");

            if (!File.Exists(path))
                throw new ArgumentException("File Not Exsist");

            var parser = new FileIniDataParser();
            var map = parser.ReadFile(path);

            IniData data = new IniData();
            SetIniData(data,map);

            map.Merge(data);
            parser.WriteFile(path, map);

            return true;
        }

        private void SetIniData(IniData data,IniData map)
        {
            foreach (var trigger in triggers)
            {
                data["Tags"][trigger.Tag] = trigger.BuildTagString();
                data["Triggers"][trigger.UniqueId] = trigger.BuildTriggerString();
                data["Events"][trigger.UniqueId] = trigger.BuildEventsString();
                data["Actions"][trigger.UniqueId] = trigger.BuildActionString();
            }

            foreach(var taskforce in forces)
            {
                data["TaskForces"][taskforce.UniqueId] = taskforce.UniqueId;
                if(map is not null)
                {
                    if (map[taskforce.UniqueId] is not null)
                    {
                        map[taskforce.UniqueId].RemoveAllKeys();
                    }
                }
                foreach (var force in taskforce.Forces.Select((value, index) => new { value, index }))
                {
                    data[taskforce.UniqueId][force.index.ToString()] = force.value.Item2 + "," + force.value.Item1;
                }

                data[taskforce.UniqueId]["Name"] = taskforce.Name ?? taskforce.UniqueId;
                data[taskforce.UniqueId]["Group"] = "-1";
            }

            foreach(var team in teams)
            {
                data["TeamTypes"][team.UniqueId] = team.UniqueId;

                if (map is not null)
                {
                    if (map[team.UniqueId] is not null)
                    {
                        map[team.UniqueId].RemoveAllKeys();
                    }
                }

                data[team.UniqueId]["Name"] = team.TeamOption.Name ?? team.UniqueId;
                if (!string.IsNullOrWhiteSpace(team.TeamOption.Tag))
                {
                    data[team.UniqueId]["Tag"] = team.TeamOption.Tag;
                }
                data[team.UniqueId]["TaskForce"] = team.TaskForceKey;
                data[team.UniqueId]["Script"] = team.ScriptKey;
                data[team.UniqueId]["Max"] = team.TeamOption.Max.ToString();
                data[team.UniqueId]["Full"] = team.TeamOption.Full ? "yes" : "no";
                data[team.UniqueId]["Group"] = team.TeamOption.Group.ToString();
                data[team.UniqueId]["House"] = team.TeamOption.House;
                data[team.UniqueId]["Whiner"] = team.TeamOption.Whiner ? "yes" : "no";
                data[team.UniqueId]["Droppod"] = team.TeamOption.Droppod ? "yes" : "no";
                data[team.UniqueId]["Suicide"] = team.TeamOption.Suicide ? "yes" : "no";
                data[team.UniqueId]["Loadable"] = team.TeamOption.Loadable ? "yes" : "no";
                data[team.UniqueId]["Prebuild"] = team.TeamOption.Prebuild ? "yes" : "no";
                data[team.UniqueId]["Priority"] = team.TeamOption.Priority.ToString();
                data[team.UniqueId]["Waypoint"] = team.TeamOption.WayPoint is not null ? team.TeamOption.WayPoint.Value.To26() : "" ;

                data[team.UniqueId]["Annoyance"] = team.TeamOption.Annoyance ? "yes" : "no";
                data[team.UniqueId]["IonImmune"] = team.TeamOption.IonImmune ? "yes" : "no";
                data[team.UniqueId]["Recruiter"] = team.TeamOption.Recruiter ? "yes" : "no";
                data[team.UniqueId]["Reinforce"] = team.TeamOption.Reinforce ? "yes" : "no";
                data[team.UniqueId]["TechLevel"] = team.TeamOption.TechLevel.ToString();
                data[team.UniqueId]["Aggressive"] = team.TeamOption.Aggressive ? "yes" : "no";
                data[team.UniqueId]["Autocreate"] = team.TeamOption.Autocreate ? "yes" : "no";
                data[team.UniqueId]["GuardSlower"] = team.TeamOption.GuardSlower ? "yes" : "no";
                data[team.UniqueId]["OnTransOnly"] = team.TeamOption.OnTransOnly ? "yes" : "no";
                data[team.UniqueId]["AvoidThreats"] = team.TeamOption.AvoidThreats ? "yes" : "no";
                data[team.UniqueId]["LooseRecruit"] = team.TeamOption.LooseRecruit ? "yes" : "no";
                data[team.UniqueId]["VeteranLevel"] = team.TeamOption.VeteranLevel.ToString();
                data[team.UniqueId]["IsBaseDefense"] = team.TeamOption.IsBaseDefense ? "yes" : "no";
                data[team.UniqueId]["UseTransportOrigin"] = team.TeamOption.UseTransportOrigin ? "yes" : "no";
                if (team.TeamOption.UseTransportOrigin)
                {
                    data[team.UniqueId]["TransportWaypoint"] = team.TeamOption.TransportWaypoint.To26();
                }
                data[team.UniqueId]["MindControlDecision"] = team.TeamOption.MindControlDecision.ToString();
                data[team.UniqueId]["OnlyTargetHouseEnemy"] = team.TeamOption.OnlyTargetHouseEnemy ? "yes" : "no";
                data[team.UniqueId]["TransportsReturnOnUnload"] = team.TeamOption.TransportsReturnOnUnload ? "yes" : "no";
                data[team.UniqueId]["AreTeamMembersRecruitable"] = team.TeamOption.AreTeamMembersRecruitable ? "yes" : "no";


            }

            foreach (var script in scripts)
            {
                data["ScriptTypes"][script.UniqueId] = script.UniqueId;
                if (map is not null)
                {
                    if (map[script.UniqueId] is not null)
                    {
                        map[script.UniqueId].RemoveAllKeys();
                    }
                }

                data[script.UniqueId]["Name"] = script.Name;
                foreach (var action in script.Scripts.Select((value, index) => new { value, index }))
                {
                    data[script.UniqueId][action.index.ToString()] = action.value.Item1.ToString() + "," + action.value.Item2.ToString();
                }
            }
        }

        /// <summary>
        /// 生成预览图
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Preview(string path)
        {

            var vm = new ChartVM();
            var rd = new Random();

            vm.Categories.Add(new Category()
            {
                Name = "默认",
            });

            foreach (var trigger in triggers) 
            {
                var tnode = new Node()
                {
                    Id = trigger.UniqueId,
                    Name = (trigger.Description ?? "") + "[" + (trigger.TriggerName ?? trigger.UniqueId) + "]",
                    Value = "events:" + trigger.BuildEventsString() + "\n" + "actions:" + trigger.BuildActionString(),
                    Category = 0,
                    X = rd.Next(-500, 500),
                    Y = rd.Next(-500, 500)
                };

                if (!string.IsNullOrEmpty(trigger.TriggerGroup.Name))
                {
                    var idx = vm.Categories.Select(x => x.Name).ToList().IndexOf(trigger.TriggerGroup.Name);
                    if (idx > -1)
                    {
                        tnode.Category = idx;
                    }
                    else
                    {
                        idx = vm.Categories.Count();
                        tnode.Category = idx;
                        vm.Categories.Add(new Category()
                        {
                            Name = trigger.TriggerGroup.Name,
                        });
                    }
                }

                vm.Nodes.Add(tnode) ;

                foreach(var node in trigger.NextNodes)
                {
                    vm.Links.Add(new Link()
                    {
                        Source = trigger.UniqueId,
                        Target = node
                    });
                }
            }

            var template = this.GetType().Assembly.GetManifestResourceStream("TriggerUtil.index.html");
            using StreamReader sr = new StreamReader(template);
            var html = sr.ReadToEnd();

            var setting = new JsonSerializerSettings();
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();
            string json = JsonConvert.SerializeObject(vm, setting);
            html = html.Replace("{{data}}", json);

            using var sw = new StreamWriter(path, false);
            sw.Write(html);

            return true;
        }

        /// <summary>
        /// 从ini中生成模板数据
        /// </summary>
        /// <param name="iniDir">ini路径</param>
        /// <param name="dest">生成的代码文件</param>
        /// <param name="suffix">ini后缀</param>
        /// <returns></returns>
        public bool LoadFromIni(string iniDir,string dest, Type type, string suffix ="md")
        {
            var rulesPath = Path.Combine(iniDir, $"rules{suffix}.ini");
            //var artPath = Path.Combine(dir, $"art{suffix}.ini");
            var evaPath = Path.Combine(iniDir, $"eva{suffix}.ini");
            var soundPath = Path.Combine(iniDir, $"sound{suffix}.ini");

            if (!File.Exists(rulesPath))
                throw new Exception($"cannont find {rulesPath}");

            //if (!File.Exists(artPath))
            //    throw new Exception($"cannont find {artPath}");


            if (!File.Exists(evaPath))
                throw new Exception($"cannont find {evaPath}");

            if (!File.Exists(soundPath))
                throw new Exception($"cannont find {soundPath}");

            IniData rulesdata = new IniData();
            rulesdata = GetIniData(rulesdata, iniDir, rulesPath,0);
            var anims = rulesdata.Sections["Animations"].Select(x => x.Value).Distinct().ToList();
            var vehicles = rulesdata.Sections["VehicleTypes"].Select(x => x.Value).Distinct().ToList();
            var buildings = rulesdata.Sections["BuildingTypes"].Select(x => x.Value).Distinct().ToList();
            var infantries = rulesdata.Sections["InfantryTypes"].Select(x => x.Value).Distinct().ToList();
            var aircrafts = rulesdata.Sections["AircraftTypes"].Select(x => x.Value).Distinct().ToList();
            var superWeapons = rulesdata.Sections["SuperWeaponTypes"].Select(x => x.Value).Distinct().ToList();
            var weapons = rulesdata.Sections["WeaponTypes"].Select(x => x.Value).Distinct().ToList();
            var warheads = rulesdata.Sections["Warheads"].Select(x => x.Value).Distinct().ToList();

            var evadata = new IniData();
            evadata = GetIniData(evadata, iniDir, evaPath, 0);
            var evas = evadata.Sections["DialogList"].Select(x => x.Value).Distinct().ToList();

            var soundData = new IniData();
            soundData = GetIniData(soundData, iniDir, soundPath, 0);
            var sounds = soundData.Sections["SoundList"].Select(x => x.Value).Distinct().ToList();

            var sb = new StringBuilder();
            sb.Append($@"
                using TriggerUtil;
                namespace {type.Namespace}
                {{
                    public partial class {type.Name} 
                    {{
                        internal enum Animations
                        {{
                            {GetEnumCsharpCode(anims)}
                        }}

                        internal enum VehicleTypes
                        {{
                            {GetEnumCsharpCode(vehicles)}
                        }}

                        internal enum BuildingTypes
                        {{
                            {GetEnumCsharpCode(buildings)}
                        }}

                        internal enum InfantryTypes
                        {{
                            {GetEnumCsharpCode(infantries)}
                        }}

                        internal enum AircraftTypes
                        {{
                            {GetEnumCsharpCode(aircrafts)}
                        }}

                        internal enum SuperWeaponTypes
                        {{
                            {GetEnumCsharpCode(superWeapons)}
                        }}

                        internal enum WeaponTypes
                        {{
                            {GetEnumCsharpCode(weapons)}
                        }}

                        internal enum Warheads
                        {{
                            {GetEnumCsharpCode(warheads)}
                        }}

                        internal enum Eva
                        {{
                            {GetEnumCsharpCode(evas)}
                        }}

                        internal enum Sounds
                        {{
                            {GetEnumCsharpCode(sounds)}
                        }}

                    }}
                }}
            ");

            using (StreamWriter sw = new StreamWriter(dest, false))
            {
                sw.Write(sb.ToString());
            }

            return true;
        }

        private IniData GetIniData(IniData data,string dir,string path,int idx)
        {

            using StreamReader iniReader = new StreamReader(path);
            var str = iniReader.ReadToEnd();

            str = ReplaceAutoKey(str, idx, out idx);


            var parser = new IniDataParser();
            parser.Configuration.AllowDuplicateKeys = true;
            parser.Configuration.AllowDuplicateSections = true;
            parser.Configuration.SkipInvalidLines = true;
            var pdata = parser.Parse(str);
            data.Merge(pdata);

            if (pdata.Sections["#include"] is not null)
            {
                foreach(var kv in pdata.Sections["#include"])
                {
                    GetIniData(data, dir, Path.Combine(dir,kv.Value),idx);
                }
            }

            if (pdata.Sections["$include"] is not null)
            {
                foreach (var kv in pdata.Sections["#include"])
                {
                    GetIniData(data, dir, Path.Combine(dir,kv.Value), idx);
                }
            }


            return data;
        }

        private string ReplaceAutoKey(string str,int startIndex,out int endIdx)
        {
            var idx = startIndex;
            var regex = new Regex(@"\+\=");
            str = regex.Replace(str, match =>
            {
                return "AUTO" + idx++.ToString().PadLeft(5, '0') + "=";
            });
            endIdx = idx;

            string pattern = @"^\s*;\s*$"; 
            str = Regex.Replace(str, pattern, "");

            //wwsb
            str = str.Replace(@"// PCG;", ";");
            //wwsb注释没换行
            str = str.Replace(";", "\n;");
            return str;
        }

        private string GetEnumCsharpCode(List<string> li)
        {
            if(li is null)
                return "";

            var sb = new StringBuilder();
            for (var i = 0; i < li.Count; i++)
            {
                var item = li[i];

                var final = item;

                if(item.Contains("-"))
                {
                    final = final.Replace("-", "_");
                }
                if (!Regex.IsMatch(item, @"^[a-zA-Z]"))
                {
                    final = "_" + final;
                }

                final = final.Replace(" ", "");

                sb.Append($"[EnumKey(Name=\"{item}\")]{final}={i},{(final==item?"":$"//notice was {item}")}\n");
            }
            return sb.ToString();
        }


    }
}
