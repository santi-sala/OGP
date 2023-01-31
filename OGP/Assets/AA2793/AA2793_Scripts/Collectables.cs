using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent != other.transform && other.tag == "Player")
        {
            transform.parent = other.transform;
        }
    }
}
