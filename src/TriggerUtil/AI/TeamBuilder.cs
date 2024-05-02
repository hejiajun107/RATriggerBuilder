using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil.AI
{
    public class TeamBuilder
    {
        private TriggerContext _context;

        internal TeamBuilder(TriggerContext context) 
        {
            _context = context;
            var tuple = IdGenerator.NextTeam();
            No = tuple.id;
            UniqueId = tuple.name;
            TeamOption = new TeamOption();
        }

        public TeamOption TeamOption { get; private set; }

        /// <summary>
        /// 注册序号
        /// </summary>
        public string No { get; private set; }
        /// <summary>
        /// 注册名
        /// </summary>
        public string UniqueId { get; private set; }

        public string TaskForceKey { get; private set; }

        public string ScriptKey { get; private set; }

        
        public TeamBuilder WithOptions(Action<TeamOption> opt)
        {
            opt(TeamOption);
            return this;
        }

        public TeamBuilder WithScript(ScriptBuilder scriptBuilder)
        {
            ScriptKey = scriptBuilder.UniqueId;
            return this;
        }

        public TeamBuilder WithScript(Action<ScriptBuilder> scriptExpression)
        {
            var script = _context.CreateScript();
            ScriptKey = script.UniqueId;
            scriptExpression(script);
            return this;
        }

        public TeamBuilder WithScript(string scriptKey)
        {
            ScriptKey = scriptKey;
            return this;
        }

        public TeamBuilder WithTaskForce(TaskForceBuilder taskForceBuilder) 
        {
            TaskForceKey = taskForceBuilder.UniqueId;
            return this;
        }
        public TeamBuilder WithTaskForce(Action<TaskForceBuilder> taskForceExpression)
        {
            var taskForce = _context.CreateTaskForce();
            TaskForceKey = taskForce.UniqueId;
            taskForceExpression(taskForce);
            return this;
        }

        public TeamBuilder WithTaskForce(string taskForceKey)
        {
            TaskForceKey = taskForceKey;
            return this;
        }

    }

}
