using UnityEngine;
using Unity.Netcode.Components;

namespace Unity.Multiplayer.Samples.Utilities.ClentAuthority
{
    [DisallowMultipleComponent]
    public class ClientNetworkTransform : NetworkTransform
    {
        protected override bool OnIsServerAuthoritative()
        {
            return false;
        }
    }

}
