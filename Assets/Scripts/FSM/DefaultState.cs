using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShrinelandsTactics.World;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class DefaultState : UIState
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
            cm.CharacterClicked += OnCharacterClicked;
        }

        private void OnCharacterClicked(object sender, Events.CharacterClickedEventArgs a)
        {
            var guy = a.Guy;
            Debug.Log("Clicked on " + guy.Name);
            cm.anim.SetTrigger("Select");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            cm.CharacterClicked -= OnCharacterClicked;
        }
    }
}
