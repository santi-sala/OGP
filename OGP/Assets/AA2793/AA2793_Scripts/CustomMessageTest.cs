using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CustomMessageTest : NetworkBehaviour
{
    [SerializeField] private Button _sendButton;

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("Hello world", ReadMessage);
        _sendButton.onClick.AddListener(SendMessage);
        Debug.Log("sup");
    }

    private void ReadMessage(ulong clientID, FastBufferReader messageBuffer)
    {
        string message;
        messageBuffer.ReadValue(out message);
        Debug.Log(message);
        Debug.Log("YUP");
    }
    private void SendMessage()
    {
        Debug.Log("K pasa");
        if (NetworkManager.Singleton.IsClient)
        {
            FastBufferWriter writer = new FastBufferWriter(5120, Unity.Collections.Allocator.Temp);
            using (writer)
            {
                writer.WriteValueSafe("Hello from client");
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage(
                    "Hello", 
                    NetworkManager.ServerClientId, 
                    writer);
            }
        }
    }
    
}
