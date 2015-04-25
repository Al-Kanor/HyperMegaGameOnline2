using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiosesModernos {
    public class Character {
        #region Getters
        public int life {
            get { return _life; }
        }

        public Vector3 position {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 rotation {
            get { return _rotation; }
            set { _rotation = value; }
        }

        public float x {
            get { return _position.x; }
            set { _position.x = value; }
        }

        public float y {
            get { return _position.y; }
            set { _position.y = value; }
        }

        public float z {
            get { return _position.z; }
            set { _position.z = value; }
        }
        #endregion

        public Character () {
            _position = new Vector3 ();
            _rotation = new Vector3 ();
        }

        #region Private properties
        int _life;
        Vector3 _position;
        Vector3 _rotation;
        #endregion
    }
}
