using UnityEngine;
using System.Collections;
using PlayerIOClient;
using System.Collections.Generic;

namespace DiosesModernos {
    public class MultiplayerManager : Singleton<MultiplayerManager> {
        #region Getters
        public bool IsConnected {
            get { return _pioConnection != null ? _pioConnection.Connected : false; }
        }
        #endregion

        #region API
        public void Disconnect () {
            if (!_pioConnection.Connected) return;
            _pioConnection.Disconnect ();
        }

        public void Disconnected (object sender, string error) {
            Debug.LogWarning ("Disconnected !");
        }

        public void SendBossDestroyed () {
            _pioConnection.Send ("Boss Destroyed", GameManager.instance.player.id);
        }

        public void SendStart () {
            Debug.Log ("Sending Start to Server");
            _pioConnection.Send ("Start");
        }

        public void StartConnection () {
            string playerId = GameManager.instance.player.id;

            // User is just using this device with no account
            PlayerIOClient.PlayerIO.Connect (
                "gaminho-test-izo9ok414egosmzcsrd3vw",  // Game id 
                "public",							    // The id of the connection, as given in the settings section of the admin panel. By default, a connection with id='public' is created
                playerId,							    // The id of the user connecting. 
                null,								    // If the connection identified by the connection id only accepts authenticated requests, the auth value generated based on playerId is added here
                null,
                null,
                delegate (Client client) {
                    SuccessfullConnect (client);
                },
                delegate (PlayerIOError error) {
                    Debug.Log ("Error connecting : " + error.ToString ());
                }
            );
        }
        #endregion

        #region Unity
        void FixedUpdate () {
            // Treat the eventual messages from the server
            GotMessages ();
        }

        void Start () {
            StartConnection ();
        }
        #endregion

        #region Handlers
        void HandleMessage (object sender, PlayerIOClient.Message message) {
            _messages.Add (message);
        }
        #endregion

        #region Private properties
        Connection _pioConnection;
        List<PlayerIOClient.Message> _messages = new List<PlayerIOClient.Message> ();
        bool _joinedRoom = false;
        PlayerIOClient.Client _pioClient;
        #endregion

        #region Private methods
        void GotMessages () {
            // Process message queue
            foreach (PlayerIOClient.Message message in _messages) {
                switch (message.Type) {
                    case "Chat":
                        Debug.Log ("Message from " + message.GetString (0) + " : " + message.GetString (1));
                        break;
                    case "Cube Destroyed":
                        GameObject.Find ("Cube" + message.GetInt (0)).Recycle ();
                        break;
                    case "Cube Spawn":
                        break;
                    case "Debug":
                        Debug.Log ("Message from server : " + message.GetString (0));
                        break;
                    case "Player Left":
                        Debug.Log ("PlayerLeft : " + message.GetString (0));
                        break;
                    case "Player Joined":
                        Debug.Log (message.GetString (0) + " has joined !");
                        break;
                    case "Player Position":
                        Debug.Log ("Position of " + message.GetString (0) + " : (" + message.GetFloat (1) + ", " + message.GetFloat (2) + ", " + message.GetFloat (3) + ")");
                        break;
                    case "Score":
                        break;
                }
            }

            // Clear message queue after it's been processed
            _messages.Clear ();
        }

        void JoinGameRoom (string roomId) {
            _pioClient.Multiplayer.CreateJoinRoom (
                roomId,
                "RoomType",			// The room type started on the server
                false,				// Should the room be visible in the lobby?
                null,
                null,
                delegate (Connection connection) {
                    Debug.Log ("Joined Room : " + roomId);
                    // We successfully joined a room so set up the message handler
                    _pioConnection = connection;
                    _pioConnection.OnMessage += HandleMessage;
                    _pioConnection.OnDisconnect += Disconnected;
                    _joinedRoom = true;
                },
                delegate (PlayerIOError error) {
                    Debug.LogError ("Error Joining Room : " + error.ToString ());
                }
            );
        }

        void SuccessfullConnect (Client client) {
            Debug.Log ("Successfully connected to the server");

            // Create or join the room	
            string roomId = "Gaminho";

            client.Multiplayer.CreateJoinRoom (
                roomId,
                "Gaminho",  // The room type started on the server
                false,		// Should the room be visible in the lobby?
                null,
                null,
                delegate (Connection connection) {
                    Debug.Log ("Joined Room : " + roomId);
                    // We successfully joined a room so set up the message handler
                    _pioConnection = connection;
                    _pioConnection.OnMessage += HandleMessage;
                    _pioConnection.OnDisconnect += Disconnected;
                    _joinedRoom = true;
                },
                delegate (PlayerIOError error) {
                    Debug.LogError ("Error Joining Room : " + error.ToString ());
                }
            );

            _pioClient = client;
        }
        #endregion
    }
}