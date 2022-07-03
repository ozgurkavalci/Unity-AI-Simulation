using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

  public enum States
{ 
    Patrol,
    Chase,
    Attack,
}


public class Commander : MonoBehaviour
{
    [SerializeField] private float wayPointApproachDistance = 2f;
    [SerializeField] private float attackingDistance = 4f;
    [SerializeField] private GameObject[] patrolPoints;
    DateTime attackingTime;
    private GameObject currentPoint;
    private NavMeshAgent commander;
    private GameObject player;

    private OffMeshLink[] teleports;
    

    public Tanks[] tanks;

    private States _currentState;
    public States currentState
    {
        get { return _currentState; }
        set {

            StopAllCoroutines();
            _currentState = value;
            switch (_currentState)
            {
                case States.Patrol:
                    StartCoroutine(this.Patrolling());
                    break;
                case States.Chase:
                    StartCoroutine(this.Chasing());
                    break;
                case States.Attack:
                    StartCoroutine(this.Attacking());
                    break;
                default:
                    break;
            }

        }

    }
    private void Awake()
    {
        tanks = FindObjectsOfType<Tanks>();
        teleports = FindObjectsOfType<OffMeshLink>();
    }


    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        commander = GetComponent<NavMeshAgent>();
        
        currentState = States.Patrol;

    }

    public IEnumerator Patrolling()
    {
        Debug.Log("Commander is patrolling");
        
        
        currentPoint = patrolPoints[UnityEngine.Random.Range(0, patrolPoints.Length)];
       

        while (_currentState == States.Patrol)
        {
           

            commander.SetDestination(currentPoint.transform.position);

            if (Vector3.Distance(transform.position, currentPoint.transform.position) < wayPointApproachDistance)
            {
                
                currentPoint = patrolPoints[UnityEngine.Random.Range(0, patrolPoints.Length)];
            }

           

            yield return new WaitForSecondsRealtime(.1f);

            foreach (OffMeshLink item in teleports)
            {
                item.activated = false;
            }
            foreach (Tanks item in tanks)
            {
                item.currentStateTank = States.Patrol;
            }

        }

       

    }
    public IEnumerator Chasing()
    {
        Debug.Log("Commander is chasing");
        while (_currentState == States.Chase)
        {
            commander.SetDestination(player.transform.position);

            foreach(Tanks item in tanks)
            {
                //item.Chasing();
                item.currentStateTank=States.Chase;
            }
            if (Vector3.Distance(commander.transform.position, player.transform.position) < attackingDistance)
            {
                currentState = States.Attack;
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
        
    }
    public IEnumerator Attacking()
    {
        Debug.Log("Commander is attacking");
        attackingTime=DateTime.Now;
        float timePassed=2;
        
        while (_currentState == States.Attack)
        {
            
            commander.SetDestination(player.transform.position);
            

            if (Vector3.Distance(transform.position, player.transform.position) > attackingDistance)
            {
                currentState = States.Chase;
                attackingTime=DateTime.MinValue;
                Debug.Log("Tuh be kacti");
            }

            
            if (Vector3.Distance(transform.position, player.transform.position)<=attackingDistance && DateTime.Now.Subtract(attackingTime).TotalSeconds>timePassed)
            {
                foreach(OffMeshLink item in teleports)
                {
                   item.activated=true;
                   Debug.Log("Activated");

                }
                foreach(Tanks item in tanks)
                {
                    
                    item.currentStateTank=States.Attack;
                  
                  Debug.Log("is it happening?");
                }

                

                //Debug.Log(DateTime.Now.Subtract(attackingTime).TotalSeconds);
                //Debug.Log("Activate Bridge,Let the troops pass through");
            }

             yield return new WaitForSecondsRealtime(0.1f);
        }
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
            { currentState = States.Chase;
            Debug.Log("I am chasing");
             
           }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag=="Player")
        {
            currentState = States.Patrol;
            Debug.Log("I am patrolling");

        }
    }



}
