using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShrinelandsTactics.World;

namespace Assets.Scripts.Events
{
    public class CharacterClickedEventArgs : EventArgs
    {
        public CharacterClickedEventArgs(Character guy)
        {
            this.guy = guy;
        }
        private Character guy;
        public Character Guy
        {
            get { return guy; }
        }
    }
}
