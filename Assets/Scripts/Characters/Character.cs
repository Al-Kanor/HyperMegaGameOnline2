using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class Character : MonoBehaviour {
        #region Properties
        [Header ("General")]
        [SerializeField]
        [Tooltip ("Life points of the character")]
        [Range (0.0f, 1000.0f)]
        float _health = 100;
        [SerializeField]
        [Tooltip ("Speed of the character")]
        [Range (0.0f, 100.0f)]
        protected float _speed = 6;

        [Header ("Links")]
        [SerializeField]
        [Tooltip ("Animation of the character (drag & drop the character model in child)")]
        protected Animation _animation;
        #endregion

        #region Getters
        /*public User user {
            get { return _user; }
        }*/
        #endregion

        #region API
        public void TakeDamage (int damage) {
            _health = Mathf.Clamp (_health, 0, _health - damage);
            if (0 == _health) {
                SoundManager.instance.StopMusic ();
            }
        }
        #endregion

        #region Unity
        /*void Awake () {
            _user = GetComponent<User> ();
        }*/
        #endregion

        #region Private properties
        //User _user;
        #endregion
    }
}