using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
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
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Compile(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new AbandonedMutexException("Invilid Path");

            var fileinfo = new FileInfo(path);

            IniData data = new IniData();
            foreach(var trigger in triggers)
            {
                data["Tags"][trigger.UniqueId] = trigger.BuildTagString();
                data["Triggers"][trigger.UniqueId] = trigger.BuildTriggerString();
                data["Events"][trigger.UniqueId] = trigger.BuildEventsString();
                data["Actions"][trigger.UniqueId] = trigger.BuildActionString();
            }


            if (!Directory.Exists(fileinfo!.Directory!.FullName))
            {
                Directory.CreateDirectory(fileinfo.Directory.FullName);
            }
            var parser = new FileIniDataParser();

            using (StreamWriter sw = new StreamWriter(path, false))
            {
                parser.WriteData(sw, data);
            }

            return true;
        }
    }
}
