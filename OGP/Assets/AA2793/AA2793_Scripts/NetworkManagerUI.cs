using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject _connectBTNS;
    [SerializeField] private GameObject _disconnectBTN;

#if UNITY_SERVER && !UNITY_EDITOR
    public void Start()
    {
        NetworkManager.Singleton.StartServer();
    }
#else
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
        NetworkUIElementsVisibility(false, true);
        NetworkManager.Singleton.StartClient();
    }

    public void Disconnect()
    {
        NetworkUIElementsVisibility(true, false);
        NetworkManager.Singleton.Shutdown();
    }

    private void NetworkUIElementsVisibility(bool connectBTNS, bool disconnectBTN)
    {
        _connectBTNS.SetActive(connectBTNS);
        _disconnectBTN.SetActive(disconnectBTN);
    }
#endif
}
