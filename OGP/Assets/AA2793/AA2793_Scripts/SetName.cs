using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class SetName : NetworkBehaviour
{
    [SerializeField] private Text _nameText;
    private Button _submitButton;
    private InputField _nameInput;

    private FixedString32Bytes _nameString;
    private NetworkVariable<FixedString32Bytes> _networkName =  new NetworkVariable<FixedString32Bytes>(new FixedString32Bytes(""), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);



    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (IsOwner)
        {
           _submitButton = GameObject.Find("Submit").GetComponent<Button>();
           _nameInput = GameObject.Find("Input").GetComponent<InputField>();
           _submitButton.onClick.AddListener(ChangeNameNetVar);
        }

        _networkName.OnValueChanged += UpdateName;
        _nameText.text = _networkName.Value.ToString();
    }

    public override void OnNetworkDespawn()
    {
        base.OnNetworkDespawn();

        _networkName.OnValueChanged -= UpdateName;
    }

    private void UpdateName(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        _nameText.text = newValue.ToString();
    }

    private void ChangeNameNetVar()
    {
        if (IsOwner)
        {
            _networkName.Value = new FixedString32Bytes(_nameInput.text);
        }
    }
    

    private void ChangeName()
    {
        _nameString = new FixedString32Bytes(_nameInput.text);
        ChangeNameServerRPC(_nameString);
    }
    [ServerRpc]
    private void ChangeNameServerRPC(FixedString32Bytes name)
    {
        ChangeNameClientRPC(name);
    }

    [ClientRpc]
    private void ChangeNameClientRPC(FixedString32Bytes name)
    {
        _nameText.text = name.ToString();
    }
}
