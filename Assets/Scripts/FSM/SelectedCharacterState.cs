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
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
            cm.Nameplate.ShowCharacter(cm.SelectedCharacter);
            cm.Deselect += Deselect;
            cm.TileClicked += DeselectTileClick;
        }

        private void DeselectTileClick(object sender, Vector3 e)
        {
            Deselect(sender, null);
        }

        private void Deselect(object sender, EventArgs e)
        {
            cm.Nameplate.StopShowing();
            cm.SelectedCharacter = null;
            cm.anim.SetTrigger("Deselect");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            cm.Deselect -= Deselect;
        }
    }
}
