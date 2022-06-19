using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExample : MonoBehaviour
{

PlayerController playerController;
[SerializeField] GameObject player;


public float SoundIntensity=20f;  // when we shoot, bullet's sound range increase
public LayerMask zombieLayer;

//public int health = 100;
public AudioClip shootSound;
private AudioSource audioSource;  // Game's main song

private SphereCollider sphereCollider;
//public float walkRadius = 2f;
//public float sprintRadius= 4f;


public void Start(){
    audioSource = GetComponent<AudioSource>();
    
//    sphereCollider = GetComponent<SphereCollider>();
}

public void Update(){

        
        
    //Ateş sesiyle birlikte zombie'yi aktive eder. Uzaklık ve yakınlığa göre.
    if(player.GetComponent<PlayerController>().Shoot){
    audioSource.PlayOneShot(shootSound); // Only once time play
    Collider[] zombies = Physics.OverlapSphere(transform.position, SoundIntensity, zombieLayer); // All zombies getting collide in a layer
    player.GetComponent<PlayerController>().Shoot = false;  // We closed the sound of shoot
    
    for (int i =0; i< zombies.Length; i++){
        zombies[i].GetComponent<AIExample>().OnAware();  // Ateş edildiğinde o alanda kaç tane zombi varsa, hepsini aware yapar
    }
    }

    }


}
