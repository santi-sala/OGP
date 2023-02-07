using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject _spawnPrefab;
    
    private void SpawnObject()
    {
        GameObject go = Instantiate(_spawnPrefab);
        NetworkObject no = go.GetComponent<NetworkObject>();
        no.Spawn();
    }

    public override void OnNetworkSpawn()
    {
        if(IsServer)
        {
            SpawnObject();
        }
    }
}
