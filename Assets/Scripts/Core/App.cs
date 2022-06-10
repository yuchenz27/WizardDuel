using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Fusion;
using Fusion.Sockets;

public enum ConnectionStatus
{
    Disconnected,
    Connecting,
    Connected,
    Failed,
    EnteringLobby,
    InLobby,
    Starting,
    Started
}

public enum WizardDuelGameMode
{
    PvE = 0,
    PvP = 1
}

public class App : MonoBehaviour, INetworkRunnerCallbacks
{
    public static App Instance { get { return _instance; } }

    private static App _instance;

    [SerializeField] private Player _playerPrefab;

    public ConnectionStatus ConnectionStatus { get; private set; }

    public bool IsMaster => _runner != null && (_runner.IsServer || _runner.IsSharedModeMasterClient);

    public ICollection<Player> Players => _players.Values;

    public NetworkRunner Runner => _runner;

    private NetworkRunner _runner;

    private NetworkSceneManagerBase _loader;

    private string _lobbyId;

    private Action<List<SessionInfo>> _onSessionListUpdated;

    private readonly Dictionary<PlayerRef, Player> _players = new Dictionary<PlayerRef, Player>();

    private void Awake()
    {
        Debug.Log("App Awake");
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
        _loader = GetComponent<NetworkSceneManagerBase>();
        DontDestroyOnLoad(gameObject);
    }

    private void Connect()
    {
        if (_runner == null)
        {
            SetConnectionStatus(ConnectionStatus.Connecting);
            GameObject go = new GameObject("Session");
            go.transform.SetParent(transform);

            _runner = go.AddComponent<NetworkRunner>();
            _runner.AddCallbacks(this);
        }
    }

    private void Disconnect()
    {
        if (_runner != null)
        {
            SetConnectionStatus(ConnectionStatus.Disconnected);
            _runner.Shutdown();
        }
    }

    private void SetConnectionStatus(ConnectionStatus status, string reason = "")
    {
        if (ConnectionStatus == status)
            return;
        ConnectionStatus = status;
    }

    public async Task EnterLobby(string lobbyId, Action<List<SessionInfo>> onSessionListUpdated)
    {
        Connect();

        _lobbyId = lobbyId;
        _onSessionListUpdated = onSessionListUpdated;

        SetConnectionStatus(ConnectionStatus.EnteringLobby);
        var result = await _runner.JoinSessionLobby(SessionLobby.Shared);

        if (!result.Ok)
        {
            _onSessionListUpdated = null;
            SetConnectionStatus(ConnectionStatus.Failed);
            onSessionListUpdated(null);
        }
    }

    public void CreateSession()
    {
        StartSession();
    }

    public void JoinSession()
    {
        StartSession();
    }

    private async void StartSession()
    {
        Connect();

        SetConnectionStatus(ConnectionStatus.Starting);

        Dictionary<string, SessionProperty> props = new();
        props.Add("Game Mode", (int)WizardDuelGameMode.PvP);
        props.Add("Enable Spectators", 1);
        props.Add("Host Name", SessionProperty.Convert("Jayce"));
        _runner.ProvideInput = true;
        await _runner.StartGame(new StartGameArgs
        {
            GameMode = Fusion.GameMode.Shared,
            PlayerCount = 4,
            SessionProperties = props,
            DisableClientSessionCreation = false
        });
    }

    public void SetPlayer(PlayerRef playerRef, Player player)
    {
        _players[playerRef] = player;
        player.transform.SetParent(_runner.transform);
    }

    public Player GetPlayer(PlayerRef ply = default)
    {
        if (!_runner)
            return null;
        if (ply == default)
            ply = _runner.LocalPlayer;
        _players.TryGetValue(ply, out Player player);
        return player;
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef playerRef)
    {
        Debug.Log($"Player {playerRef} Joined!");
        if (IsMaster)
        {
            _runner.SetActiveScene("Staging");
            // Should I use a session prefab?
        }

        if (runner.IsServer || runner.Topology == SimulationConfig.Topologies.Shared && playerRef == runner.LocalPlayer)
        {
            Debug.Log("Spawning player");
            runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity, playerRef);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef playerRef)
    {
        
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {

    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {

    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {

    }

    public void OnConnectedToServer(NetworkRunner runner)
    {
        Debug.Log("Connected to server");
        SetConnectionStatus(ConnectionStatus.Connected);
    }

    public void OnDisconnectedFromServer(NetworkRunner runner)
    {
        Debug.Log("Disconnected from server");
        Disconnect();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {

    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        Debug.Log($"Connect failed {reason}");
        Disconnect();
        SetConnectionStatus(ConnectionStatus.Failed, reason.ToString());
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {

    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        SetConnectionStatus(ConnectionStatus.InLobby);
        _onSessionListUpdated?.Invoke(sessionList);
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {

    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {

    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data)
    {

    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {

    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {

    }
}
