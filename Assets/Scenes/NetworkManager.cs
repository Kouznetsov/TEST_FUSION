using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Fusion.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        public NetworkRunner runner { get; private set; }
        public static NetworkManager Instance { get; private set; }

        public bool IsMaster => runner.GameMode == GameMode.Host;
        public PlayerRef playerRef => runner.LocalPlayer;
        public bool logEnabled = true;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            runner = gameObject.AddComponent<NetworkRunner>();
            Instance = this;
        }

        public void StartGame()
        {
            var sceneObjectProvider = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneObjectProvider>().FirstOrDefault();
            if (sceneObjectProvider == null) {
                Debug.Log($"NetworkRunner does not have any component implementing {nameof(INetworkSceneObjectProvider)} interface, adding {nameof(NetworkSceneManagerDefault)}.", runner);
                sceneObjectProvider = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
            }
            runner.ProvideInput = true;
            text.text = "Connecting...";

            runner.StartGame(new StartGameArgs()
            {
                GameMode = GameMode.AutoHostOrClient,
                PlayerCount = 2,
                SessionName = "AAA",
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneObjectProvider = sceneObjectProvider
            });
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnPlayerJoined " + Time.time);
            Debug.Log("Player Joined");
            SceneManager.LoadSceneAsync(1);
            text.text = "Player joined";
        }

        public void SetActiveScene(SceneRef sceneRef)
        {
            runner.SetActiveScene(sceneRef);
        }

        public void Disconnect()
        {
            runner.Shutdown(false);
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnPlayerLeft " + Time.time);
            // if (this.runner.CurrentScene == (int) ProjectScene.GameBase)
                // SceneLoader.Instance.LoadScene((int) ProjectScene.Home, LoadSceneMode.Single);
            // NetworkEventsManager.OnPlayerLeft();
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnShutdown " + Time.time);
            // SceneLoader.Instance.LoadScene((int) ProjectScene.Home, LoadSceneMode.Single);
        }

        public TextMeshProUGUI text;
        
        public void OnConnectedToServer(NetworkRunner runner)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnConnectedToServer " + Time.time);

            text.text = "Connected";
            // NetworkEventsManager.OnConnected();
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnDisconnectedFromServer " + Time.time);
            // NetworkEventsManager.OnDisconnected();
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnConnectRequest " + Time.time);
            if (runner.SessionInfo.PlayerCount >= 2)
                request.Refuse();
            else
                request.Accept();
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnConnectFailed " + Time.time);
            var str = reason switch
            {
                NetConnectFailedReason.Timeout => "Connection timed out",
                NetConnectFailedReason.ServerFull => "Server is full",
                NetConnectFailedReason.ServerRefused => "Server refused the connection",
                _ => "Unknown error"
            };
            // NetworkEventsManager.OnConnectFailed(str);
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnUserSimulationMessage " + Time.time);
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnSessionListUpdated " + Time.time);
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnCustomAuthenticationResponse " + Time.time);
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnHostMigration " + Time.time);
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnReliableDataReceived " + Time.time);
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnSceneLoadDone " + Time.time);
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
            if (logEnabled)
                Debug.Log("FusionManager.OnSceneLoadStart " + Time.time);
        }
    }
}