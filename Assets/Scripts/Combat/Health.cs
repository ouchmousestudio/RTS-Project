using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;

    [SyncVar(hook = nameof(UpdateHealth))]
    private int currentHealth;

    public event Action ServerOnDie;

    public event Action<int, int> ClientOnHealthUpdated;

    #region Server

    public override void OnStartServer()
    {
        currentHealth = maxHealth;

        UnitBase.ServerOnPlayerDie += ServerHandlePlayerDie;
    }

    public override void OnStopServer()
    {
        UnitBase.ServerOnPlayerDie -= ServerHandlePlayerDie;
    }

    [Server]
    private void ServerHandlePlayerDie(int connectionId)
    {
        if (connectionToClient.connectionId != connectionId) { return; }
        
        //Destroy Units on Base Death
        DealDamage(currentHealth);
    }

    [Server]
    public void DealDamage(int damageAmount)
    {
        if (currentHealth == 0) { return; }

        //If health drops below 0  it becomes 0
        currentHealth = Mathf.Max(currentHealth - damageAmount, 0);

        if (currentHealth != 0 ) { return; }

        ServerOnDie?.Invoke();
    }

    #endregion

    #region Client

    private void UpdateHealth(int oldHealth, int newHealth)
    {
        ClientOnHealthUpdated?.Invoke(newHealth, maxHealth);
    }

    #endregion
    
}
