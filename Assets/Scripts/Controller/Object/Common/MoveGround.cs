using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機が踏んでいる間自機を子供にする
/// </summary>
public class MoveGround : MonoBehaviour {

    private GameObject player;
    

	// Use this for initialization
	void Start () {
        player = GameObject.FindWithTag("PlayerTag");	
	}


    private void OnTriggerEnter2D(Collider2D collision) {
        if(!player.activeSelf) {
            return;
        }
        if(collision.tag == "PlayerFootTag") {
            player.transform.SetParent(transform);                
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!player.activeSelf) {
            return;
        }
        if (collision.tag == "PlayerFootTag") {
            player.transform.SetParent(null);
            player.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }


    private void OnBecameInvisible() {
        if (player == null)
            return;
        if (player.transform.parent == this.transform) {
            player.transform.SetParent(null);
        }
    }
}
