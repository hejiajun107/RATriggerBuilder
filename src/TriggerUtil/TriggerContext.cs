using IniParser;
using IniParser.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TriggerUtil.Chart;

namespace TriggerUtil
{
    public class TriggerContext
    {
        private List<TriggerBuilder> triggers = new List<TriggerBuilder>();

        public TriggerBuilder CreateTrigger()
        {
            var trigger = new TriggerBuilder(this);
            triggers.Add(trigger);
            return trigger;
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
            foreach(var trigger in triggers)
            {
                data["Tags"][trigger.UniqueId] = trigger.BuildTagString();
                data["Triggers"][trigger.UniqueId] = trigger.BuildTriggerString();
                data["Events"][trigger.UniqueId] = trigger.BuildEventsString();
                data["Actions"][trigger.UniqueId] = trigger.BuildActionString();
            }

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

            IniData data = new IniData();
            foreach (var trigger in triggers)
            {
                data["Tags"][trigger.Tag] = trigger.BuildTagString();
                data["Triggers"][trigger.UniqueId] = trigger.BuildTriggerString();
                data["Events"][trigger.UniqueId] = trigger.BuildEventsString();
                data["Actions"][trigger.UniqueId] = trigger.BuildActionString();
            }

            var parser = new FileIniDataParser();

            var map = parser.ReadFile(path);
            map.Merge(data);
            parser.WriteFile(path, map);

            return true;
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
    }
}
