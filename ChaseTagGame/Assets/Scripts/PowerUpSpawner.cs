using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject spawningObject;
    public Transform targetTransform;
    public float spawnTimer = 2f;
    public float spawnAngle = 45f;

    private float currentTimer;


    void Update()
    {
        currentTimer += Time.deltaTime;
        if(currentTimer >= spawnTimer)
        {
            currentTimer = 0;
            SpawnWithAngle(spawningObject, targetTransform, spawnAngle);
        }
    }

    private void SpawnWithAngle(GameObject objectToSpawn, Transform target, float angle)
    {
        var spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
        var spawnedRigidBody = spawnedObject.GetComponent<Rigidbody>();
        var randomTorque = GetRandomTorque();
        var force = GetBallisticForce(transform.position, target.position, angle);
        spawnedRigidBody.AddForce(force, ForceMode.VelocityChange);
        spawnedRigidBody.AddTorque(randomTorque, ForceMode.VelocityChange);
    }

    private Vector3 GetBallisticForce(Vector3 source, Vector3 target, float angle)
    {
        var direction = target - source;
        var h = direction.y;
        direction.y = 0;
        var distance = direction.magnitude;
        var a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        var velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return velocity * direction.normalized;
    }
    private Vector3 GetRandomTorque()
    {
        var x = Random.Range(-180, 180);
        var y = Random.Range(-180, 180);
        var z = Random.Range(-180, 180);
        return new Vector3(x, y, z);
    }
}
