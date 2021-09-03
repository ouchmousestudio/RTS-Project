using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitBase : NetworkBehaviour
{

    [SerializeField] private Health health = null;

    public static event Action<int> ServerOnPlayerDie;

    public static event Action<UnitBase> ServerOnBaseSpawned;
    public static event Action<UnitBase> ServerOnBaseDespawned;

    #region server
    public override void OnStartServer()
    {
        health.ServerOnDie += HandleServerDeath;

        ServerOnBaseSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= HandleServerDeath;

        ServerOnBaseDespawned?.Invoke(this);
    }

    [Server]
    private void HandleServerDeath()
    {
        ServerOnPlayerDie?.Invoke(connectionToClient.connectionId);
        
        NetworkServer.Destroy(gameObject);
    }

    #endregion

    #region client

    #endregion
}
