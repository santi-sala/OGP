using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    
    private float _movementSpeed = 10;
    [SerializeField]
    private CharacterController _characterController;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangePlayerMovementSpeed();
        PlayerMovementInput();       

    }

    private void PlayerMovementInput()
    {
        if (IsOwner)
        {
            Vector3 _movementDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                _movementDirection.z++;
            }
            if (Input.GetKey(KeyCode.A))
            {
                _movementDirection.x--;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _movementDirection.z--;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _movementDirection.x++;
            }

            transform.LookAt(transform.position + _movementDirection);

            // transform.localPosition += _movementDirection * Time.deltaTime * _movementSpeed;
            _characterController.Move(_movementDirection * Time.deltaTime * _movementSpeed);
        }
    }

    private void ChangePlayerMovementSpeed()
    {
        foreach (Transform child in transform)
        {
            if (child.tag == "Collectable")
            {
                _movementSpeed = 3;
            }
            else
            {
                _movementSpeed = 20;
            }
        }
    }
}
