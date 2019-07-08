using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShrinelandsTactics.World;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class SelectedCharacterState : UIState
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            throw new NotImplementedException();
        }
    }
}
