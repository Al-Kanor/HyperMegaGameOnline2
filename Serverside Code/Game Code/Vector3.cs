using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiosesModernos {
    class Vector3 {
        #region Getters
        public float x {
            get { return _x; }
        }

        public float y {
            get { return _y; }
        }

        public float z {
            get { return _z; }
        }
        #endregion

        #region Private properties
        float _x = 0;
        float _y = 0;
        float _z = 0;
        #endregion
    }
}
