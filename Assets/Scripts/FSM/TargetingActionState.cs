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
            cm.Deselect += Deselect;
            ShowValidTargets();
            cm.OverlayClicked += OverlayClicked;
        }

        private void OverlayClicked(object sender, Vector3 e)
        {
            var name = cm.SelectedAction.Name;
            var pos = cm.UnityToShrinelandsPosition(e);
            var target = cm.DM.Characters.FirstOrDefault(c => c.Pos == pos);
            List<string> targetString = new List<string>();
            targetString.Add(target.Name);
            var outcome = cm.DM.UseAbility(name, targetString); //TOOD: this is horrible
            Debug.Log(outcome.Message.ToString());
            cm.anim.SetTrigger("Deselect");
        }

        private void ShowValidTargets()
        {
            cm.overlayMap.ClearAllTiles();
            var places = cm.DM.GetValidTargetsFor(cm.SelectedCharacter, cm.SelectedAction);
            foreach (var pos in places)
            {
                cm.overlayMap.SetTile(cm.ShrinelandsToUnityVector(pos), cm.overlayTile);
            }
        }

        private void Deselect(object sender, EventArgs e)
        {
            cm.anim.SetTrigger("Deselect");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            cm.Deselect -= Deselect;
            cm.OverlayClicked -= OverlayClicked;
        }
    }
}
