using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
     
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                parser.WriteData(sw, data);
            }

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
                data["Tags"][trigger.UniqueId] = trigger.BuildTagString();
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
    }
}
