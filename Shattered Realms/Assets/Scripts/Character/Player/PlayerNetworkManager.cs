using Unity.Netcode;
using UnityEngine;

namespace MD
{
    public class PlayerNetworkManager : CharacterNetworkManager
    {
        PlayerManager player;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }
    }
}