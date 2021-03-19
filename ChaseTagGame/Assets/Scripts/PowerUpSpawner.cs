using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    public GameObject spawningObject;
    public Transform target;
    public float spawnTimer = 2f;

    private float currentTimer;
    private GameObject spawnedObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;

        if(currentTimer >= spawnTimer)
        {
            currentTimer = 0;
            spawnedObject = Instantiate(spawningObject, transform.position, Quaternion.identity);
            var spawnedRigidBody = spawnedObject.GetComponent<Rigidbody>();
            var randomTorque = GetRandomTorque();
            var force = GetBallisticForce(transform.position, target.position, 60);
            spawnedRigidBody.AddForce(force, ForceMode.VelocityChange);
            spawnedRigidBody.AddTorque(randomTorque, ForceMode.VelocityChange);
        }
    }



    private Vector3 GetBallisticForce(Vector3 source, Vector3 target, float angle)
    {
        Vector3 direction = target - source;
        float h = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += h / Mathf.Tan(a);

        // calculate velocity
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));
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
