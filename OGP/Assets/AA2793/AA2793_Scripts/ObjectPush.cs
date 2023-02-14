using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPush : MonoBehaviour
{
    [SerializeField]
    private float _forceAmount;
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rigidbody = hit.collider.attachedRigidbody;

        if (rigidbody != null)
        {
            Vector3 direction = hit.gameObject.transform.position - transform.position;
            direction.y = 0f;
            direction.Normalize();

            rigidbody.AddForceAtPosition(direction * _forceAmount, transform.position, ForceMode.Impulse);
        }
    }
}
