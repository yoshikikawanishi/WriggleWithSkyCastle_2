using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSoilBullet : MonoBehaviour {

    private Rigidbody2D player_Rigid;


    private void Start() {
        player_Rigid = GameObject.FindWithTag("PlayerTag").GetComponent<Rigidbody2D>();
    }


    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            player_Rigid.AddForce(transform.right * 1200f + new Vector3(0, 700f));            
        }
    }
}
