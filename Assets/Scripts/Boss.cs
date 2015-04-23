using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class Boss : MonoBehaviour {
        #region Properties
        [Header ("Configuration")]
        [SerializeField]
        [Tooltip ("Target")]
        Transform[] _targets;
        #endregion

        #region Unity
        void FixedUpdate () {
            GetComponent<NavMeshAgent> ().destination = _target.position;
        }

        void Start () {
            _target = _targets[Random.Range (0, _targets.Length)];
        }
        #endregion

        #region Private properties
        Transform _target;
        #endregion
    }
}