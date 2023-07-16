using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriggerUtil
{
    public enum Difficulty
    {
        Easy,
        Middle,
        Hard
    }

    public enum IdType
    {
        Trigger,
        Event,
        Action
    }

    public enum RepeatType
    {
        OneTimeOr,
        OneTimeAnd,
        RepeatingOr
    }
}
