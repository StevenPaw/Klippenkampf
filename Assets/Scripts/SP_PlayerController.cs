using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SP_PlayerController : NetworkBehaviour
{
    [SerializeField] private Vector3 movement = new Vector3();

    [Client]
    private void Update()
    {
        if (!hasAuthority) { return; }

        if (!Input.GetKeyDown(KeyCode.Space)) { return; }

        transform.Translate(movement);

    }
}
