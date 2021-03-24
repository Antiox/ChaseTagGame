using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using TMPro;

public class PistonScript : MonoBehaviour
{
    public GameObject target;
    private float speed = 10f;
    private float maxDistance = 2f;
    private Rigidbody piston;
    private Vector3 initialPosition;
    public LineRenderer lineRenderer;
    public int maxIterations = 10000;
    public int maxSegmentCount = 300;
    public float segmentStepModulo = 10f;
    private Vector3[] segments;
    private int numSegments = 0;

    void Start()
    {
        piston = GetComponent<Rigidbody>();
        initialPosition = piston.position;
    }

    // Update is called once per frame
    void Update()
    {
        float newForward = Mathf.Sin(Time.time * speed);
        piston.MovePosition(initialPosition + piston.transform.forward * newForward * maxDistance);
    }

    private void OnTriggerEnter(Collider other)
    {
        var rigidbody = other.gameObject.GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.drag = 2;
        rigidbody.MovePosition(rigidbody.position + piston.transform.forward + Vector3.up);
        var force = GetBallisticForce(rigidbody.position, target.transform.position, 50f);
        var multiplier = GetBestFitForce(rigidbody.position, target.transform.position, force, rigidbody.mass, rigidbody.drag);
        SimulatePath(rigidbody.position, force * multiplier, rigidbody.mass, rigidbody.drag);
        Draw();

        rigidbody.velocity = force * multiplier;
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

        var velocity = Mathf.Sqrt(distance * 40f /*gravity*/ / Mathf.Sin(2 * a));
        return velocity * direction.normalized;
    }


    public float GetBestFitForce(Vector3 initialPosition, Vector3 target, Vector3 forceDirection, float mass, float drag)
    {
        var paths = new List<Vector3>();
        var results = new Dictionary<int, float>();

        for (float i = 0f; i < 200f; i++)
            paths.Add(SimulatePath(initialPosition, forceDirection * (1f+i/25f), mass, drag));

        for (int i = 0; i < paths.Count; i++)
            results.Add(i, Vector3.Distance(target, paths[i]));

        return 1f + results.Aggregate((l, r) => l.Value < r.Value ? l : r).Key/25f;
    }

    public Vector3 SimulatePath(Vector3 initialPosition, Vector3 forceDirection, float mass, float drag)
    {
        var timestep = Time.fixedDeltaTime;
        var stepDrag = 1 - drag * timestep;
        var velocity = forceDirection / mass * timestep;
        var gravity = Vector3.down * 40f * timestep * timestep;
        var position = initialPosition;

        if (segments == null || segments.Length != maxSegmentCount)
            segments = new Vector3[maxSegmentCount];

        segments[0] = position;
        numSegments = 1;

        for (int i = 0; i < maxIterations && numSegments < maxSegmentCount && position.y > 0f; i++)
        {
            velocity += gravity;
            velocity *= stepDrag;
            position += velocity;

            if (i % segmentStepModulo == 0)
            {
                segments[numSegments] = position;
                numSegments++;
            }
        }

        return segments[numSegments - 1];
    }

    private void Draw()
    {
        Color startColor = Color.magenta;
        Color endColor = Color.magenta;
        startColor.a = 1f;
        endColor.a = 1f;

        lineRenderer.transform.position = segments[0];

        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;

        lineRenderer.positionCount = numSegments;
        for (int i = 0; i < numSegments; i++)
        {
            lineRenderer.SetPosition(i, lineRenderer.transform.InverseTransformPoint(segments[i]));
        }
    }
}
