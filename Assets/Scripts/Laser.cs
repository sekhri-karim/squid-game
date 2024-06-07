using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Laser : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().position = Vector3.MoveTowards(transform.position,player.transform.position, 1000 * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            FindObjectOfType<GameManager>().HitPlayer();
            Destroy(gameObject);
        }

    }
}

