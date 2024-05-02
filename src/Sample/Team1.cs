using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriggerUtil;
using TriggerUtil.AI;

namespace Sample
{
    public class Team1 : TriggerTemplate
    {
        protected override void Process()
        {
            var attackFrom70Script = Context.CreateScript().MoveToWayPoint(70).AttackNearestTarget(AttackTargetType.Anything);
            var attackFrom71Script = Context.CreateScript().MoveToWayPoint(71).AttackNearestTarget(AttackTargetType.Anything);

            var team = Context.CreateTeam().WithScript(attackFrom70Script).WithTaskForce(x=>x.Add("HTNK",5));



        }
    }
}
