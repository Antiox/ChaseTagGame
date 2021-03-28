using GameLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PowerUpSpawner : MonoBehaviour
{
    public CrateScript crate;
    public Transform targetLocation;
    public float spawnAngle = 45f;


    void Start()
    {
        EventManager.Instance.AddListener<OnPowerUpSpawnRequestedEvent>(OnPowerUpSpawnRequested);
    }

    void OnDestroy()
    {
        EventManager.Instance.RemoveListener<OnPowerUpSpawnRequestedEvent>(OnPowerUpSpawnRequested);
    }


    private void OnPowerUpSpawnRequested(OnPowerUpSpawnRequestedEvent e)
    {
        var randomLocation = GetRandomNavMeshPosition();
        SpawnCrate(crate, randomLocation, spawnAngle, e.Faction);
    }

    private void SpawnCrate(CrateScript crateScript, Vector3 target, float angle, PowerUpFaction faction)
    {
        var spawnedObject = Instantiate(crateScript.gameObject, transform.position, Quaternion.identity);
        var spawnedRigidBody = spawnedObject.GetComponent<Rigidbody>();
        var randomTorque = Utility.GetRandomTorque(180f);
        var force = Utility.GetBallisticForce(transform.position, target, angle, Physics.gravity);

        spawnedObject.GetComponent<CrateScript>().powerUp = PowerUpManager.Instance.GetRandom(faction);

        spawnedRigidBody.AddForce(force, ForceMode.VelocityChange);
        spawnedRigidBody.AddTorque(randomTorque, ForceMode.VelocityChange);
    }

    private Vector3 GetRandomNavMeshPosition()
    {
        var navMeshData = NavMesh.CalculateTriangulation();
        var maxIndices = navMeshData.indices.Length - 3;
        var firstVertexSelected = Random.Range(0, maxIndices);
        var secondVertexSelected = Random.Range(0, maxIndices);
        var firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        var secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];

        if ((int)firstVertexPosition.x == (int)secondVertexPosition.x || (int)firstVertexPosition.z == (int)secondVertexPosition.z)
            return GetRandomNavMeshPosition();
        else
            return Vector3.Lerp(firstVertexPosition, secondVertexPosition, Random.Range(0.05f, 0.95f));
    }
}
