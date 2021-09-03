using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class UnitRangedAttack : NetworkBehaviour
{
    [SerializeField] private Targeter targeter = null;
    [SerializeField] private GameObject projectilePrefab = null;
    [SerializeField] private Transform projectileSpawnpoint = null;
    [SerializeField] private float fireRange = 5f;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float rotationSpeed = 20f;

    private float lastShotFired;


    [ServerCallback]
    private void Update()
    {

        Targetable target = targeter.GetTarget();
        if (target == null) { return; }
        if (!CanFire()) { return; }

        Quaternion targetRotation =
            Quaternion.LookRotation(target.transform.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (Time.time > (1 / fireRate) + lastShotFired)
        {
            Quaternion projectileRotation = Quaternion.LookRotation(
                target.GetAimAtPoint().position - projectileSpawnpoint.position);

            GameObject projectileInstance = Instantiate(
                projectilePrefab, projectileSpawnpoint.position, projectileRotation);


            NetworkServer.Spawn(projectileInstance, connectionToClient);
            lastShotFired = Time.time;
        }
    }

    [Server]
    private bool CanFire()
    {
        return (targeter.GetTarget().transform.position - transform.position).sqrMagnitude
            <= fireRange * fireRange;

    }
}
