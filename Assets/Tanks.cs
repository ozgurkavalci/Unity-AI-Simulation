using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public  class Tanks : MonoBehaviour
{
    public static Tanks staticTank;
     private void Awake() 
    {
        if(staticTank==null)
        {
            staticTank=this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
           // Destroy(gameObject);
           return;
        }

       
    }

    
    [SerializeField] private float wayPointApproachDistance = 2f;

    [SerializeField] private GameObject[] patrolPoints;

    [SerializeField] private GameObject chasingPoint;

    private GameObject currentPoint;

    private NavMeshAgent tank;

    [SerializeField] private GameObject tower;

    private  States _currentStateTank;
    public  States currentStateTank
    {
        get { return _currentStateTank; }
        set {

            StopAllCoroutines();
            _currentStateTank = value;
            switch (_currentStateTank)
            {
                case States.Patrol:
                    StartCoroutine(Patrolling());
                    break;
                case States.Chase:
                    StartCoroutine(Chasing());
                    break;
                case States.Attack:
                    StartCoroutine(Attacking());
                    break;
            }     
        
        }
        
    }
    

   

    private void Start() 
    {
        tank=GetComponent<NavMeshAgent>();
        currentStateTank=States.Patrol;
    }

    public IEnumerator Patrolling()
    {
        currentPoint=patrolPoints[Random.Range(0,patrolPoints.Length)];

         while(_currentStateTank==States.Patrol)
        {
            tank.SetDestination(currentPoint.transform.position);

            if(Vector3.Distance(transform.position,currentPoint.transform.position)<wayPointApproachDistance)
            {
                 currentPoint=patrolPoints[Random.Range(0,patrolPoints.Length)];
            }
             yield return new WaitForSecondsRealtime(.1f);

        }

       
    }
    public IEnumerator Chasing()
    {

        
        Debug.Log("Tank is chasing");
        while(_currentStateTank==States.Chase)
        {
            

            tank.SetDestination(chasingPoint.transform.position);
          
            yield return new WaitForSecondsRealtime(.1f);
        }
        
    }
    public IEnumerator Attacking()
    {
        
        Debug.Log("Tank is Attacking");
        while(_currentStateTank==States.Attack)
        {
            

             tank.SetDestination(tower.transform.position);

             yield return new WaitForSecondsRealtime(.1f);
        } 
        
    }


    

    



}
