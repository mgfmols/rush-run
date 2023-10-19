using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class MovementAI : MonoBehaviour
{
    [SerializeField] public float Range = 10f;
    [SerializeField] private NavMeshAgent aiAgent;
    private float timer;
    public float wanderTimer;

    void Start()
    {
        aiAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        Move();
        //LookWhereYoureGoing();
        Debug.DrawRay(SelectRandomPointWithinRange(), Vector3.up, Color.blue, 1.0f);
    }
    public Vector3 SelectRandomPointWithinRange()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * Range;
        if (NavMesh.SamplePosition(randomPoint, out var hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    void Move()
    {
        if (timer >= wanderTimer && !aiAgent.hasPath)
        {
            aiAgent.SetDestination(SelectRandomPointWithinRange());
            timer = 0;
        }

        timer += Time.deltaTime;
    }


    //void LookWhereYoureGoing()
    //{
    //    Vector3 targetDirection = moveLocation - transform.position;
    //    Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0);
    //    transform.rotation = Quaternion.LookRotation(newDirection);
    //}
}