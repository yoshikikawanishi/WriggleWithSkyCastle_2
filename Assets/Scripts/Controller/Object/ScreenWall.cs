using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機が地上時当たり判定をなくす
/// </summary>
public class ScreenWall : MonoBehaviour {

    private PlayerController player_Controller;

    private bool is_Player_Flying = false;


	// Use this for initialization
	void Start () {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player != null) {
            player_Controller = player.GetComponent<PlayerController>();            
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (player_Controller == null)
            return;

        if (player_Controller.Get_Is_Ride_Beetle()) {
            if (!is_Player_Flying) {
                is_Player_Flying = true;
                gameObject.layer = LayerMask.NameToLayer("ScreenWallLayer");
            }
        }
        else {
            if (is_Player_Flying) {
                is_Player_Flying = false;
                gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
            }
        }
	}
}
