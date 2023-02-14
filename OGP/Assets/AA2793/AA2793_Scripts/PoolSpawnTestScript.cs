using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Unity.BossRoom.Infrastructure;

public class PoolSpawnTestScript : NetworkBehaviour
{
    [SerializeField]
    private GameObject prefab;

    public void SpawnPoolObjects()
    {
        if (IsServer)
        {
            NetworkObject no = NetworkObjectPool.Singleton.GetNetworkObject(prefab);
            no.Spawn();
        }
    }
}
