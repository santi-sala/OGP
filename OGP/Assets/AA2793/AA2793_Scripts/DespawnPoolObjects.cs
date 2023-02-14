using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DespawnPoolObjects : NetworkBehaviour
{
    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            StartCoroutine("Despawn");
        }
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(3f);
        NetworkObject no = gameObject.GetComponent<NetworkObject>();
        no.Despawn();
    }

}
