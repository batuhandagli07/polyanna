using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIExample : MonoBehaviour
{

public enum WanderType{Random, Waypoint};  // Waypoint follow numbers


public PlayerController fpsc;   // Our main player

public int Zhealth = 100;
public WanderType wanderType= WanderType.Random;   // Zombie can run randomly but didn't use

private Rigidbody rb;

public Transform[] waypoints; // This will be used when waypoint wandering is selected


public float fov=120f;  // 45 angles
public float viewDistance=10f;   // range of zombie's view
private bool isAware= false;  // When it is true, zombie are starting to follow Player
private bool isDetecting=false;
private NavMeshAgent agent;
public float wanderspeed= 1f; // Dolaþma Hýzý
public float chasespeed = 3f; // Kovalama Hýzý
public float loseThreshold=10f; //lose time ( Player'i ne kadar süre sonra takibi býrakýyor)
public float loseTimer=0;

public float WanderRadius =3f;
private Vector3 wanderPoint;
private int waypointIndex=0;  // Waypointleri dolaþmasý için, loop gibi


private Animator animator;


public void Start(){
    agent=GetComponent<NavMeshAgent>();
    wanderPoint = RandomWanderPoint();
    animator = GetComponentInChildren<Animator>();
    
}
public void Update(){

    
    if(Zhealth <= 0 ){ //if zombie dies
        Die();
        return;
        
    }

    if (isAware){
        agent.SetDestination(fpsc.transform.position);
        animator.SetBool("Aware", true);  // Calling animation for zombie. Follow the Player animation
        agent.speed = chasespeed;  // We are increasing walking speed
            if(!isDetecting){  // If we getting away from zombie's view, losetimer starts to increase
                loseTimer  += Time.deltaTime; // If losetimer higher than the losethreshold, zombie is turning to own waypoint.
                if(loseTimer>=loseThreshold){
                    isAware=false;
                    loseTimer=0;
                }
            }
    }
    else{
        
        Wander();
        animator.SetBool("Aware", false);  // We are turning back to the zombie's animation
        agent.speed=wanderspeed;  // Zombie speed decreased
    }
    SearchforPlayer();  // Always running, because it can detect the player any time

}


public void SearchforPlayer(){
    if(Vector3.Angle(Vector3.forward, transform.InverseTransformPoint(fpsc.transform.position)) < fov /2f){
            if(Vector3.Distance(fpsc.transform.position, transform.position)< viewDistance){
                RaycastHit hitzombie;
                if(Physics.Linecast(transform.position, fpsc.transform.position, out hitzombie, -1)){
                    if(hitzombie.transform.CompareTag("Player")){
                OnAware();
                    }
                    else{
                        isDetecting=false;
                    }
                }
                else{
                    isDetecting=false;
                }
            }

            else{
                isDetecting=false;
            }
    
    }
    else{
        isDetecting=false;
    }
}

public void OnAware(){

    isAware=true;
    isDetecting=true;
    loseTimer=0;
}

public void Wander(){
    if(wanderType == WanderType.Random){
    if(Vector3.Distance(transform.position, wanderPoint) < 2f){
        
        wanderPoint = RandomWanderPoint();
    }
    else{
        agent.SetDestination(wanderPoint);
    }
    }

    else{
        //Waypoint wandering
        
        if(Vector3.Distance(waypoints[waypointIndex].position, transform.position) <2f){
            if(waypointIndex==waypoints.Length -1){
                    waypointIndex=0;           
            }
            else{
                waypointIndex++;
            }
            
        }
        else{
            agent.SetDestination(waypoints[waypointIndex].position);
        }
    }
}

//Wanderin Zombies
public Vector3 RandomWanderPoint(){

    Vector3 randomPoint = (Random.insideUnitSphere * WanderRadius) + transform.position;
    NavMeshHit navHit;
    NavMesh.SamplePosition(randomPoint, out navHit, WanderRadius, -1);
    return new Vector3(navHit.position.x, transform.position.y, navHit.position.z);
}


public void OnHit(int damage){
Zhealth -= damage;
}

public void Die(){
    agent.speed=0;
    //rb.isKinematic=true;
    animator.SetTrigger("Die");
    //gameObject.SetActive(false);

}


public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            other.gameObject.GetComponent<LifeOfPlayer>().ZombieCollision(5);
        }
    }
    

}