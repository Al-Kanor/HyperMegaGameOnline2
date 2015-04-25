using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DiosesModernos {
    public class GameManager : Singleton<GameManager> {
        #region Properties
        [Header ("Links")]
        [SerializeField]
        [Tooltip ("Player")]
        Player _player;
        [SerializeField]
        [Tooltip ("Boss")]
        Boss _boss;
        [SerializeField]
        [Tooltip ("Base difficulty")]
        DifficultyLevel _difficulty;
        #endregion

        #region Public enums
        public enum DifficultyLevel {
            HUMAN,
            CYBORG,
            GOD
        };
        #endregion

        #region Getters
        public Boss boss {
            get { return _boss; }
        }

        public DifficultyLevel difficulty {
            get { return _difficulty; }
        }

        public Player player {
            get { return _player; }
        }
        #endregion

        #region API
        public void ResetGame () {
            Application.LoadLevel (Application.loadedLevel);
        }
        #endregion
    }
}