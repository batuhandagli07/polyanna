using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeOfPlayer : MonoBehaviour
{
    private Scene scene;

    [SerializeField] GameObject player;
    [SerializeField] GameObject GameOver;
    [SerializeField] private float health = 100f;
    private Animator anim;
    [SerializeField]  AudioSource source;
    [SerializeField]  AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        scene=SceneManager.GetActiveScene();
        anim=GetComponent<Animator>();

        if(health <= 0 ){ //if Player dies
        DiePlayer();
        return;
        
    }
       
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0 ){ //if player dies
        DiePlayer();
        return;
    }

    }
    public void ZombieCollision(int damage){
    health -= damage;
    if(health==0){
        source.PlayOneShot(clip); // Sound of Player's death
    }
    
    }


    public void DiePlayer(){
    anim.SetTrigger("PlayerDie");
    Cursor.lockState = CursorLockMode.None;
    GameOver.SetActive(true);
    Time.timeScale=0f;
    //SceneManager.LoadScene(scene.name);

}
}

