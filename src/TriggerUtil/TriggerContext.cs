using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil
{
    public class TriggerContext
    {
        public TriggerBuilder CreateTrigger()
        {
            return new TriggerBuilder(this);
        }

        public string Compile()
        {
            return null;
        }
    }
}
