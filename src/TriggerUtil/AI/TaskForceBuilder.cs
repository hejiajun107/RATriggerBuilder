using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil.AI
{
    public class TaskForceBuilder
    {
        private TriggerContext _context;

        internal TaskForceBuilder(TriggerContext context)
        {
            _context = context;
            var tuple = IdGenerator.NextTaskForce();
            No = tuple.id;
            UniqueId = tuple.name;
        }


        /// <summary>
        /// 注册序号
        /// </summary>
        public string No { get; private set; }
        /// <summary>
        /// 注册名
        /// </summary>
        public string UniqueId { get; private set; }

        public List<(string,int)> Forces { get;private set; }

        public TaskForceBuilder Add(string techno,int count)
        {
            Forces.Add((techno, count));
            return this;
        }

        
    }
}
