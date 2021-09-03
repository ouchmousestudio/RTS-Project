using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Building : NetworkBehaviour
{
    [SerializeField] private GameObject buildingPreview = null;
    [SerializeField] private Sprite icon = null;
    [SerializeField] private int id = -1;
    [SerializeField] private int cost = 100;

    public static event Action<Building> ServerOnBuildingSpawned;
    public static event Action<Building> ServerOnBuildingDespawned;

    public static event Action<Building> AuthorityOnBuildingSpawned;
    public static event Action<Building> AuthorityOnBuildingDespawned;

    public GameObject GetBuildingPreview()
    {
        return buildingPreview;
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public int GetId()
    {
        return id;
    }

    public int GetCost()
    {
        return cost;
    }

    #region Server

    public override void OnStartServer()
    {
        ServerOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopServer()
    {
        ServerOnBuildingDespawned?.Invoke(this);
    }

    #endregion

    #region Client
    public override void OnStartClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthorityOnBuildingSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
        if (!isClientOnly || !hasAuthority) { return; }
        AuthorityOnBuildingDespawned?.Invoke(this);
    }

    #endregion
}