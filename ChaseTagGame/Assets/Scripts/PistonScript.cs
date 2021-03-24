using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using TMPro;
using GameLibrary;

public class PistonScript : MonoBehaviour
{
    public GameObject target;

    [Header("Expansion")]
    [SerializeField] private float expandSpeed = 5f;
    [SerializeField] private float maxExpansion = 2f;
    [Header("Retraction")]
    [SerializeField] private float retractSpeed = 0.2f;
    [SerializeField] private float delayBeforeRetract = 10f;


    private bool isCoolingDown;
    private Rigidbody piston;
    private Vector3 initialPosition;
    private Vector3 expandedPosition;
    private PlayerMovement playerMovementScript;

    void Start()
    {
        piston = GetComponent<Rigidbody>();
        initialPosition = piston.position;
        expandedPosition = initialPosition + piston.transform.forward * maxExpansion;
        isCoolingDown = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == GameTags.Player && !isCoolingDown)
        {
            playerMovementScript = other.gameObject.GetComponent<PlayerMovement>();
            var rigidbody = other.gameObject.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.drag = 2;

            var force = GetBallisticForce(rigidbody.position, target.transform.position, 50f);
            var multiplier = GetBestFitForce(rigidbody.position, target.transform.position, force, rigidbody.mass, rigidbody.drag);

            StartCoroutine(DeactivateCollider(other));
            StartCoroutine(Expand(initialPosition, expandedPosition));

            rigidbody.velocity = force * multiplier;
        }
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
        var maxIterations = 10000;
        var maxSegmentCount = 300;
        var segmentStepModulo = 10f;
        var numSegments = 0;
        var segments = new Vector3[numSegments];

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


    private IEnumerator DeactivateCollider(Collider c)
    {
        c.enabled = false;
        playerMovementScript.IsBumped = true;
        yield return new WaitForSeconds(0.2f);
        playerMovementScript.IsBumped = false;
        c.enabled = true;
    }

    private IEnumerator Expand(Vector3 start, Vector3 target)
    {
        isCoolingDown = true;

        float t = 0;
        while (t <= 1)
        {
            t += Time.fixedDeltaTime * expandSpeed;
            piston.MovePosition(Vector3.Lerp(start, target, t));

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(delayBeforeRetract);

        t = 0;
        while (t <= 1)
        {
            t += Time.fixedDeltaTime * retractSpeed;
            piston.MovePosition(Vector3.Lerp(target, start, t));

            yield return new WaitForEndOfFrame();
        }

        isCoolingDown = false;
    }
}
