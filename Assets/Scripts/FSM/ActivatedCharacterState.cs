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
            var pos = cm.UnityToShrinelandsPosition(e);
            var dir = pos - cm.SelectedCharacter.Pos;
            var direction = Map.DirectionToPosition.FirstOrDefault(x => x.Value == dir).Key;
            var outcome = cm.DM.MoveCharacter(cm.SelectedCharacter, direction);
            Debug.Log(outcome.Message.ToString());
            ShowMovementOptions();
        }

        public void ShowMovementOptions()
        {
            var adjacent = cm.DM.GetEmptyAdjacentSquares(cm.SelectedCharacter.Pos);
            List<Vector3Int> movePlaces = new List<Vector3Int>();
            foreach (var pos in adjacent)
            {
                movePlaces.Add(new Vector3Int(pos.x, cm.DM.map.Height - pos.y, 0));
            }
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
