using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiosesModernos {
    public class Vector3 {
        #region Getters
        public float x {
            get { return _x; }
            set { _x = value; }
        }

        public float y {
            get { return _y; }
            set { _y = value; }
        }

        public float z {
            get { return _z; }
            set { _z = value; }
        }
        #endregion

        #region Private properties
        float _x = 0;
        float _y = 0;
        float _z = 0;
        #endregion
    }
}
