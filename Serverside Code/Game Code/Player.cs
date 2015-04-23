using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayerIO.GameLibrary;

namespace DiosesModernos {
    public class Player : BasePlayer {
        #region Getters
        public Character avatar {
            get { return _avatar; }
        }
        #endregion

        #region Private properties
        Character _avatar;
        #endregion
    }
}