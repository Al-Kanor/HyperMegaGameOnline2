using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace HMGO {
    public class AnimatorControllerLog : Singleton<AnimatorControllerLog> {
        #region Properties
        
        #endregion

        #region Unity
        void FixedUpdate () {
            Text t = GetComponent<Text> ();
            if (GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).IsName ("Player Idle")) {
                t.text = "Idle";
            }
            else t.text = "Walk";
        }
        #endregion
    }
}