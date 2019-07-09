using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShrinelandsTactics.World;
using UnityEngine;

namespace Assets.Scripts.FSM
{
    public class ActivatedCharacterState : UIState
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            base.OnStateEnter(animator, animatorStateInfo, layerIndex);
            cm.Nameplate.ShowCharacter(cm.SelectedCharacter);
            cm.AbilityPanel.ShowAbilities(cm, cm.SelectedCharacter);
            cm.OverlayClicked += OverlayClicked;

            //get movement
            ShowMovementOptions();
        }

        private void OverlayClicked(object sender, Vector3 e)
        {
            var outcome = cm.DM.MoveCharacter(cm.SelectedCharacter, Map.Direction.N);
            Debug.Log(outcome.Message.ToString());
            ShowMovementOptions();
        }

        public void ShowMovementOptions()
        {
            List<Vector3Int> movePlaces = new List<Vector3Int>();
            movePlaces.Add(new Vector3Int(cm.SelectedCharacter.Pos.x, cm.DM.map.Height - 1 - cm.SelectedCharacter.Pos.y, 0));
            cm.PutOverlayTilesAt(movePlaces);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            cm.overlayMap.ClearAllTiles();
            cm.Nameplate.StopShowing();
            cm.AbilityPanel.ClearButtons();
            cm.OverlayClicked -= OverlayClicked;
        }
    }
}
