using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    bool running = false;
    bool game_started = false;
    bool game_over = false; 

    public GameObject player;
    public Animator player_animator;

    public GameObject toy;

    public GameObject laser;
    public Animator toy_animator;

    public GameObject camera;

    public ParticleSystem blood_splash;
    public GameObject blood;

    AudioSource source;

    public AudioClip step;
    public AudioClip shooting;
    public AudioClip hit;
    public AudioClip fall;

    float steps_counter;

    public GameObject ui_start;
    public GameObject ui_gameover;
    public GameObject ui_win;
    public Text ui_guide;

    KeyCode key1 = 0,key2=0,key3=0;
    void Start()
    {
        source = GetComponent<AudioSource>();
        ui_start.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(running){
            player.transform.position -= new Vector3(0,0,0.5f* Time.deltaTime);
            camera.transform.position -= new Vector3(0,0,0.5f* Time.deltaTime);
            steps_counter += Time.deltaTime;
            if(steps_counter > 0.25){
                steps_counter = 0;
                source.PlayOneShot(step);
            }
        }

        if(Input.GetKeyDown(KeyCode.Space) && !game_started){
            running = true;
            game_started = true;
            ui_start.SetActive(false);
            player_animator.SetTrigger("run");
            StartCoroutine(Sing());
        }
        if(Input.GetKeyDown(KeyCode.Space) && game_over){
            SceneManager.LoadScene("game");

        }
        //if(Input.GetKey(key1) && Input.GetKey(key2) && Input.GetKey(key3) && !game_over )
        if(Input.GetKey(key1) && Input.GetKey(key2) && !game_over ){
            running = false;
            player_animator.speed = 0;
        }
        else if((Input.GetKeyUp(key1) || Input.GetKeyUp(key2) || Input.GetKeyUp(key3)) && !game_over){
            running = true;
            player_animator.speed = 1;
        }
        
    }

    IEnumerator Sing(){
        //sing
        toy.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(5);

        key1 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));
        key2 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));
        key3 = (KeyCode)System.Enum.Parse(typeof(KeyCode), (System.Char.ConvertFromUtf32('A' + Random.Range(0, 25)).ToString()));

        ui_guide.text = "PRESS " + key1 + " + " + key2 + " + " + key3 + " TO STOP" ;

        toy_animator.SetTrigger("look");
        yield return new WaitForSeconds(2);
        //check player movment
        if(running){
            Debug.Log("shoot the player");
            GameObject new_laser = Instantiate(laser);
            new_laser.transform.position = toy.transform.GetChild(0).transform.position;
            game_over = true;
            source.PlayOneShot(shooting);
        }
        ui_guide.text="";
        yield return new WaitForSeconds(2);
        toy_animator.SetTrigger("idle");
        yield return new WaitForSeconds(1);
        toy.GetComponent<AudioSource>().Stop();
       if(!game_over) StartCoroutine(Sing());
    }
    public void HitPlayer(){
        Debug.Log("lol");
        running = false;
        player_animator.SetTrigger("idle");
        player.GetComponent<Rigidbody>().velocity = new Vector3(0,2,2);
        player.GetComponent<Rigidbody>().angularVelocity = new Vector3(3,0,0);
        camera.GetComponent<Animator>().Play("camera_lose");
        blood_splash.Play();
        StartCoroutine(ShowBlood());
        source.PlayOneShot(hit);
    }
    IEnumerator ShowBlood(){
        yield return new WaitForSeconds(1f);
        ui_gameover.SetActive(true);
        source.PlayOneShot(fall);
        blood.SetActive(true);
        blood.transform.position = new Vector3(player.transform.position.x , 0.001f ,player.transform.position.z+0.15f);
    }
    public IEnumerator Playerwin(){
        game_over = true;
        yield return new WaitForSeconds(1f);
        running = false;
        player_animator.SetTrigger("idle");
        ui_win.SetActive(true);
        
    
    }
}
