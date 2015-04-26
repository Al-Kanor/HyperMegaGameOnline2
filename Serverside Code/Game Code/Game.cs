using System;
using System.Collections.Generic;
using System.Collections;
using PlayerIO.GameLibrary;

namespace DiosesModernos {
    [RoomType ("DiosesModernos")]
    public class GameCode : Game<Player> {
        #region Override
        // This method is called when an instance of your the game is created
        public override void GameStarted () {
            Update ();
            UpdateBoss ();
        }

        // This method is called when a player sends a message into the server code
        public override void GotMessage (Player sender, Message message) {
            switch (message.Type) {
                // The boss has taken damage
                case "Boss Damage":
                    BossDamage (message.GetInt (0));
                    break;
                // A player sends a message to the other players
                case "Chat":
                    Chat (sender.ConnectUserId, message.GetString (0));
                    break;
                // The new position / rotation of a player
                case "Player Position":
                    PlayerPosition (sender.ConnectUserId, message.GetFloat (0), message.GetFloat (1), message.GetFloat (2));
                    break;
                // A player shoots
                case "Player Shoot":
                    PlayerShoot (sender.ConnectUserId, message.GetFloat (0), message.GetFloat (1), message.GetFloat (2));
                    break;
                
            }
        }

        // This method is called whenever a player joins the game
        public override void UserJoined (Player newPlayer) {
            foreach (Player player in Players) {
                if (player.ConnectUserId != newPlayer.ConnectUserId) {
                    player.Send ("Player Joined", newPlayer.ConnectUserId);
                    newPlayer.Send ("Player Joined", player.ConnectUserId, player.avatar.x, player.avatar.z);
                }
            }

            boss.AddTarget (newPlayer.avatar);

            // Send current cubes infos to the player
            /*foreach (Cube cube in cubes) {
                newPlayer.Send ("Cube Spawn", cube.id, cube.x, cube.z);
            }*/
        }

        // This method is called when a player leaves the game
        public override void UserLeft (Player player) {
            Broadcast ("Player Left", player.ConnectUserId);
            boss.RemoveTarget (player.avatar);
        }
        #endregion

        #region Private properties
        int updateRate = 50;
        Boss boss = new Boss ();
        //const int MAX_CUBES = 20;

        //List<Cube> cubes = new List<Cube> ();
        //int lastCubeId = 0;
        #endregion

        #region Private methods
        // Reset the game
        void Reset () {
            /*// Score
            Player winner = new Player ();
            int scoreMax = -1;
            foreach (Player player in Players) {
                if (player.cubesDestroyed > scoreMax) {
                    winner = player;
                    scoreMax = player.cubesDestroyed;
                }
            }

            // Broadcast who won the round
            if (winner.cubesDestroyed > 0) {
                Broadcast ("Chat", "Server", "The round is won by " + winner.ConnectUserId + " ! (" + winner.cubesDestroyed + " cubes destroyed)");
            }
            else {
                Broadcast ("Chat", "Server", "No one won this round.");
            }

            // Reset all scores
            foreach (Player player in Players) {
                player.cubesDestroyed = 0;
            }
            Broadcast ("Score", 0);*/
        }

        // Regularly send position and rotation of the players to all players
        void SendPlayersPositions () {
            foreach (Player player in Players) {
                Broadcast ("Player Position", player.ConnectUserId, player.avatar.x, player.avatar.z, player.avatar.rotation.y);
            }
        }

        /*void SpawnCube () {
            if (MAX_CUBES <= cubes.Count) return;
            System.Random rand = new System.Random ();
            int x = rand.Next (-20, 20);
            int z = rand.Next (-20, 20);
            Cube tmpCube = new Cube ();
            tmpCube.id = lastCubeId;
            tmpCube.x = x;
            tmpCube.z = z;
            cubes.Add (tmpCube);
            ++lastCubeId;
            // Broadcast new cube information to all players
            Broadcast ("Cube Spawn", tmpCube.id, tmpCube.x, tmpCube.z);
        }*/

        void Update () {
            SendPlayersPositions ();
            ScheduleCallback (Update, updateRate);
        }

        void UpdateBoss () {
            if (boss.UpdateTarget ()) {
                // The boss has a new target
                foreach (Player player in Players) {
                    if (player.avatar == boss.target) {
                        Broadcast ("Boss Target", player.ConnectUserId);
                        break;
                    }
                }
            }
            ScheduleCallback (UpdateBoss, boss.targetSwitchDelay);
        }
        #endregion

        #region Client requests treatment
        void BossDamage (int damage) {
            // If the boss just died don't reapply damage
            if (0 == boss.health) return;
            if (boss.TakeDamage (damage)) {
                // The boss is dead
            }
            Broadcast ("Boss Health", boss.health);
        }

        void Chat (string playerId, string message) {
            foreach (Player player in Players) {
                if (player.ConnectUserId != playerId) {
                    player.Send ("Chat", playerId, message);
                }
            }
        }

        void PlayerPosition (string playerId, float px, float pz, float ry) {
            foreach (Player player in Players) {
                if (player.ConnectUserId == playerId) {
                    player.avatar.x = px;
                    player.avatar.z = pz;
                    player.avatar.rotation.y = ry;
                    return;
                }
            }
        }

        void PlayerShoot (string playerId, float px, float pz, float ry) {
            foreach (Player player in Players) {
                if (player.ConnectUserId == playerId) {
                    // Clamps the avatar to the right position and rotation
                    player.avatar.x = px;
                    player.avatar.z = pz;
                    player.avatar.rotation.y = ry;
                    Broadcast ("Player Shoot", player.ConnectUserId, px, pz, ry);
                    return;
                }
            }
        }
        #endregion
    }
}