using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracePlayer : MonoBehaviour {

    enum Kind {
        onlyX,
        onlyY,
        both
    }
    [SerializeField] private Kind kind;
    [SerializeField] private float speed = 0.2f;    

    private GameObject player;

	
	void Awake () {
        player = GameObject.FindWithTag("PlayerTag");
	}
	
	
	void FixedUpdate () {
        if (!enabled)
            return;

		if(kind == Kind.onlyX || kind == Kind.both) {
            Trase_Player_X();            
        }
        if(kind == Kind.onlyY || kind == Kind.both) {
            Trase_Player_Y();
        }
	}


    private void Trase_Player_X() {
        //自機が右にいるとき
        if (player.transform.position.x - transform.position.x > 2f) {
            transform.position += new Vector3(speed, 0);
        }
        //左にいるとき
        else if(player.transform.position.x - transform.position.x < -2f) {
            transform.position -= new Vector3(speed, 0);
        }
        //重なっているとき
        else {
            transform.position = new Vector3(player.transform.position.x, transform.position.y);
        }
    }


    private void Trase_Player_Y() {
        //自機が上にいるとき
        if (player.transform.position.y - transform.position.y > 2f) {
            transform.position += new Vector3(0, speed);
        }
        //下にいるとき
        else if (player.transform.position.y - transform.position.y < -2f) {
            transform.position -= new Vector3(0, speed);
        }
        //重なっているとき
        else {
            transform.position = new Vector3(transform.position.x, player.transform.position.y);
        }
    }
}
