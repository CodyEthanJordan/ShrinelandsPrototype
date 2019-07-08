using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShrinelandsTactics.World;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class TargetingActionState : UIState
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
            cm.Nameplate.ShowCharacter(cm.SelectedCharacter);
            cm.AbilityPanel.ShowAbilities(cm, cm.SelectedCharacter);

            cm.overlayMap.ClearAllTiles();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
        }
    }
}
