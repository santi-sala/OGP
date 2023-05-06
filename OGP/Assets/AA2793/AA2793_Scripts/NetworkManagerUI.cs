using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Http;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport;
using Unity.Networking.Transport.Relay;
using NetworkEvent = Unity.Networking.Transport.NetworkEvent;

public class NetworkManagerUI : MonoBehaviour
{
    const int MAX_CONNECTIONS = 4;

    //public string RelayJoinCode;

    [SerializeField] private GameObject _connectBTNS;
    [SerializeField] private GameObject _disconnectBTN;


//#if UNITY_SERVER && !UNITY_EDITOR
    public void Start()
    {
        Example_AuthenticatingAPlayer();
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientDisconnectCallback;
    }
//#else

    private void OnClientDisconnectCallback(ulong clinetID)
    {
        if(NetworkManager.Singleton.LocalClientId == clinetID)
        {
            //_disconnectBTN.SetActive(false);
            //_connectBTNS.SetActive(true);
            NetworkUIElementsVisibility(false, true);
        }
    }
    public void StartSever()
    {
        NetworkUIElementsVisibility(false, true);
        NetworkManager.Singleton.StartServer();
    }
   
    public void StartHost()
    {
        Debug.Log("Sup");
        StartCoroutine("StartHostCallback");        
    }

    public void StartClient()
    {
        //PlayerCountServerRpc();

        NetworkUIElementsVisibility(false, true);
        NetworkManager.Singleton.StartClient();

    }

    public void Disconnect()
    {
        PlayerSpawner.Instance.StopListener();
        NetworkUIElementsVisibility(true, false);
        NetworkManager.Singleton.Shutdown();
        //Scene scene = SceneManager.GetActiveScene();
        //SceneManager.LoadScene(scene.name);
    }

    private void NetworkUIElementsVisibility(bool connectBTNS, bool disconnectBTN)
    {
        _connectBTNS.SetActive(connectBTNS);
        _disconnectBTN.SetActive(disconnectBTN);
    }

    async void Example_AuthenticatingAPlayer()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            var playerID = AuthenticationService.Instance.PlayerId;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
    /*
    [ServerRpc(RequireOwnership = false)]
    private void PlayerCountServerRpc() 
    {
        PlayerCountClientRpc();
    }

    [ClientRpc]
    private bool PlayerCountClientRpc()
    {
        int clientCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
        Debug.Log("Im here!!!");

        if (clientCount > 4)
        {
            return false;
        }
        else
        {
            return true;
        }
    }*/

    public static async Task<RelayServerData> AllocateRelayServerAndGetJoinCode(int maxConnections, string region = null)
    {
        Allocation allocation;
        string createJoinCode;
        try
        {
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections, region);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        try
        {
            createJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log(createJoinCode);
        }
        catch
        {
            Debug.LogError("Relay create join code request failed");
            throw;
        }

        return new RelayServerData(allocation, "dtls");
    }

    private IEnumerator StartHostCallback()
    {
        
        Task<RelayServerData> relayTask = AllocateRelayServerAndGetJoinCode(MAX_CONNECTIONS);
        
        while(!relayTask.IsCompleted)
        {
            yield return null;
        }

        RelayServerData serverData = relayTask.Result;

        NetworkUIElementsVisibility(false, true);
        NetworkManager.Singleton.StartHost();

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(serverData);
        NetworkManager.Singleton.StartHost();
    }


//#endif
}
