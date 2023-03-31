using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject _connectBTNS;
    [SerializeField] private GameObject _disconnectBTN;


//#if UNITY_SERVER && !UNITY_EDITOR
    public void Start()
    {
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
        NetworkUIElementsVisibility(false, true);
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        PlayerCountServerRpc();

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

    [ServerRpc(RequireOwnership = false)]
    private bool PlayerCountServerRpc() 
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
    }
    //#endif
}
