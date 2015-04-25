using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DiosesModernos {
    public class Boss : Enemy {
        #region Properties
        [Header ("Configuration")]
        [SerializeField]
        [Tooltip ("Seconds between two eventual target switches")]
        [Range (0, 3)]
        float targetSwitchDelay = 1;
        [SerializeField]
        [Tooltip ("Probability of target switch (%)")]
        [Range (0, 100)]
        float targetSwitchProba = 10;
        #endregion

        #region API
        public void AddTarget (Transform target) {
            _targets.Add (target);
            if (1 == _targets.Count) {
                // First target
                _target = target;
            }
        }

        public void RemoveTarget (Transform target) {
            _targets.Remove (target);
            if (0 == _targets.Count) {
                // No more target !
                _target = transform;
            }
            else if (_target == target) {
                // The removed target was the current target
                // The boss follows a new target
                SelectRandomTarget ();
            }
        }
        #endregion

        #region Unity
        void FixedUpdate () {
            FollowTarget ();
        }

        void Start () {
            //GetComponent<NavMeshAgent> ().destination = transform.position;
            _target = transform;
            StartCoroutine ("UpdateTarget");
        }
        #endregion

        #region Private properties
        List<Transform> _targets = new List<Transform> ();
        Transform _target;
        #endregion

        #region Private methods
        void FollowTarget () {
            GetComponent<NavMeshAgent> ().destination = _target.position;
        }

        void SelectRandomTarget () {
            _target = _targets[Random.Range (0, _targets.Count)];
        }

        IEnumerator UpdateTarget () {
            do {
                if (_targets.Count > 0) {
                    // Evenual target switch
                    if (Random.Range (0, 100) < targetSwitchProba) {
                        Debug.Log ("switch");
                        SelectRandomTarget ();
                    }
                }
                yield return new WaitForSeconds (targetSwitchDelay);
            } while (true);
        }
        #endregion
    }
}