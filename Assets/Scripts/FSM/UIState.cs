using ShrinelandsTactics.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public abstract class UIState : StateMachineBehaviour
    {
        protected CombatManager cm;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            cm = animator.gameObject.GetComponent<CombatManager>();
        }

        public override abstract void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex);

    }
}
