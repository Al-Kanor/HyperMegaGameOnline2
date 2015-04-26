using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiosesModernos {
    class Boss : Character {
        #region API
        public Boss () {
            _health = 100;
            _target = this;
        }
        #endregion

        #region Private properties
        // Delay between two eventual target switches (ms)
        int _targetSwitchDelay = 1000;
        // Probability of target switch (%)
        float _targetSwitchProba = 30;
        // List of possible targets
        List<Character> _targets = new List<Character> ();
        // Target to follow
        Character _target;
        // Random generator
        Random rnd = new Random ();
        #endregion

        #region Getters
        public Character target {
            get { return _target; }
        }

        public int targetSwitchDelay {
            get { return _targetSwitchDelay; }
        }
        #endregion

        #region API
        public void AddTarget (Character target) {
            _targets.Add (target);
            if (1 == _targets.Count) {
                // First target
                _target = target;
            }
        }

        public void RemoveTarget (Character target) {
            _targets.Remove (target);
            if (0 == _targets.Count) {
                // No more target !
                _target = this;
            }
            else if (_target == target) {
                // The removed target was the current target
                // The boss follows a new target
                SelectRandomTarget ();
            }
        }

        // Hurt the boss then return true if the boss is dead, false otherwise
        public bool TakeDamage (int damage) {
            _health = Math.Max (_health - damage, 0);
            return 0 == _health;
        }

        // Is true if a new target is selected, false otherwise
        public bool UpdateTarget () {
            if (_targets.Count > 1) {
                // Eventual target switch
                if (rnd.Next (0, 100) < _targetSwitchProba) {
                    Character oldTarget = _target;
                    do {
                        SelectRandomTarget ();
                    } while (_target == oldTarget);
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region Private methods
        void SelectRandomTarget () {
            _target = _targets[rnd.Next (0, _targets.Count)];
        }
        #endregion
    }
}
