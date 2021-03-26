using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject objectToSpawn;
    public Transform targetLocation;
    public float spawnDelay = 2f;
    public float spawnAngle = 45f;

    private float currentTimer;

    private void FixedUpdate()
    {
        currentTimer += Time.fixedDeltaTime;
        if (currentTimer >= spawnDelay)
        {
            currentTimer = 0;
            SpawnObject(objectToSpawn, targetLocation, spawnAngle);
        }
    }

    private void SpawnObject(GameObject objectToSpawn, Transform target, float angle)
    {
        var spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        var spawnedRigidBody = spawnedObject.GetComponent<Rigidbody>();
        var randomTorque = Utility.GetRandomTorque(180f);
        var force = Utility.GetBallisticForce(transform.position, target.position, angle, Physics.gravity);

        spawnedRigidBody.AddForce(force, ForceMode.VelocityChange);
        spawnedRigidBody.AddTorque(randomTorque, ForceMode.VelocityChange);
    }
}
