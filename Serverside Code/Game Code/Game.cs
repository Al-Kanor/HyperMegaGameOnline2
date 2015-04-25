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
            SendPlayersPositions ();
            // Reset game every 30 seconds
            AddTimer (Reset, 30000);
        }

        // This method is called when a player sends a message into the server code
        public override void GotMessage (Player sender, Message message) {
            switch (message.Type) {
                // The new position / rotation of a player
                case "Player Position":
                    PlayerPosition (sender.ConnectUserId, message.GetFloat (0), message.GetFloat (1), message.GetFloat (2));
                    break;
                /*case "Cube Destroyed":
                    // called when the player has destroyed a cube
                    int destroyedCubeId = int.Parse (message.GetString (0).Replace ("Cube", ""));

                    // Find a cube by this id
                    Cube result = cubes.Find (delegate (Cube cube) {
                        return cube.id == destroyedCubeId;
                    });

                    if (null != result) {
                        // Sends everyone information that a cube has been destroyed
                        // Increases player score
                        Broadcast ("Cube Destroyed", result.id);
                        cubes.Remove (result);
                        sender.cubesDestroyed++;
                        sender.Send ("Score", sender.cubesDestroyed);
                    }
                    break;*/
                // A player sends a message to the other players
                case "Chat":
                    foreach (Player player in Players) {
                        if (player.ConnectUserId != sender.ConnectUserId) {
                            player.Send ("Chat", sender.ConnectUserId, message.GetString (0));
                        }
                    }
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
            
            // Send current cubes infos to the player
            /*foreach (Cube cube in cubes) {
                newPlayer.Send ("Cube Spawn", cube.id, cube.x, cube.z);
            }*/
        }

        // This method is called when a player leaves the game
        public override void UserLeft (Player player) {
            Broadcast ("Player Left", player.ConnectUserId);
        }
        #endregion

        #region Private properties
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
            ScheduleCallback (SendPlayersPositions, 50);    // Envoi toutes les 50 millisecondes pour un rendu fluide
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
        #endregion

        #region Client requests treatment
        void PlayerPosition (string playerId, float px, float pz, float ry) {
            foreach (Player player in Players) {
                if (player.ConnectUserId == playerId) {
                    player.avatar.x = px;
                    player.avatar.z = pz;
                    player.avatar.rotation.y = ry;
                }
            }
        }
        #endregion
    }
}