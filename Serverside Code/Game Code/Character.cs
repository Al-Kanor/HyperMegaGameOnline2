using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiosesModernos {
    class Character {
        #region Getters
        public int life {
            get { return _life; }
        }

        public Vector3 position {
            get { return _position; }
        }

        public Vector3 rotation {
            get { return _rotation; }
        }
        #endregion

        #region Private properties
        int _life;
        Vector3 _position;
        Vector3 _rotation;
        #endregion
    }
}
