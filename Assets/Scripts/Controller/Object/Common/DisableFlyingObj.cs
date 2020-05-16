using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableFlyingObj : MonoBehaviour {

    private PlayerController player_Controller;

    private bool is_Hit_Player;


	// Use this for initialization
	void Start () {
        //取得
        GameObject player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (player_Controller == null)
            return;

        if (is_Hit_Player) {            
            player_Controller.To_Disable_Ride_Beetle();
        }	
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            is_Hit_Player = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            player_Controller.To_Enable_Ride_Beetle();
            is_Hit_Player = false;
        }
    }
}
