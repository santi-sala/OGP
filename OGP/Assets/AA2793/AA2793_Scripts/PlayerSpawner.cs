using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance;
    [SerializeField] private GameObject _playerPrefab;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted()
    {
        if(NetworkManager.Singleton.IsServer)
        {
            if (NetworkManager.Singleton.IsHost)
            {
                GameObject go = Instantiate(_playerPrefab);
                NetworkObject no = go.GetComponent<NetworkObject>();
                no.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId);
            }

            NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
        }
    }

    private void OnClientConnectedCallback(ulong clientID)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameObject go = Instantiate(_playerPrefab);
            NetworkObject no = go.GetComponent<NetworkObject>();
            no.SpawnAsPlayerObject(clientID);
        }
    }

    public void StopListener()
    {
        NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
    }
}
