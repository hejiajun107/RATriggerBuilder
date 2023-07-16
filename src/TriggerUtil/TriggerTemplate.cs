using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil
{
    public class TriggerTemplate
    {

        public TriggerTemplate() {
            Context = new TriggerContext();
        }

        protected TriggerContext Context { get; set; }

        protected virtual void Process()
        {
            
        }

        private bool _hasBuilt = false;

        /// <summary>
        /// 生成INI
        /// </summary>
        /// <param name="path">新的ini路径</param>
        /// <returns></returns>
        public bool Build(string path)
        {
            if (!_hasBuilt) 
            {
                _hasBuilt = true;
                Process();
            }
            
            return Context.Compile(path);
        }

        /// <summary>
        /// 附加模式，将触发附加到原有的地图上
        /// </summary>
        /// <param name="path">地图的路径</param>
        /// <returns></returns>
        public bool Append(string path)
        {
            if (!_hasBuilt)
            {
                _hasBuilt = true;
                Process();
            }

            return Context.Append(path);
        }
    }


}
