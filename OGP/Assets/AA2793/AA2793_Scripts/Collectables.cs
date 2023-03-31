using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectables : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent.tag != "Basket")
        {
            if (transform.parent != other.transform && other.tag == "Player")
            {
                transform.parent = other.transform;            }

        }

        if (transform.parent.tag == ("Player") && other.transform.tag == "Basket")
        {
            Debug.Log("Hei hei hei");

            transform.parent = other.transform;
        }
    }

    
}
