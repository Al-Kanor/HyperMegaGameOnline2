using UnityEngine;
using System.Collections;

namespace DiosesModernos {
    public class GameManager : Singleton<GameManager> {
        #region Properties
        [Header ("Links")]
        [SerializeField]
        [Tooltip ("Player")]
        Player _player;
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
        public DifficultyLevel difficulty {
            get { return difficulty; }
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