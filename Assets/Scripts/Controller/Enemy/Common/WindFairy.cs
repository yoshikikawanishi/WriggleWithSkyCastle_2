using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFairy : Enemy {

    [Space]
    [SerializeField] private ShootSystem under_Shoot;
    [SerializeField] private ShootSystem strong_Shoot;

    private Renderer _renderer;

    private PlayerController player_Controller;
    private bool is_Player_Flying = false;

    private float UNDER_SHOOT_SPAN = 2.0f;
    private float STRONG_SHOOT_SPAN = 2.5f;

    private float time_Count = 0;

	
	void Start () {
        //取得	
        _renderer = GetComponent<Renderer>();
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        is_Player_Flying = player_Controller.Get_Is_Ride_Beetle();
	}
	
	
	void Update () {
        if (!_renderer.isVisible) {
            return;
        }
        
        //自機が飛行時
        if (player_Controller.Get_Is_Ride_Beetle()) {
            Update_In_Player_Flying();
        }
        //自機が通常時
        else {
            Update_In_Player_Normal();
        }
        //自機の状態切り替え時
        if(is_Player_Flying != player_Controller.Get_Is_Ride_Beetle()) {
            is_Player_Flying = player_Controller.Get_Is_Ride_Beetle();
            time_Count = 0;
        }
	}


    //自機が飛行時
    private void Update_In_Player_Flying() {
        if(time_Count < STRONG_SHOOT_SPAN) {
            time_Count += Time.deltaTime;            
        }
        else {
            time_Count = 0;
            strong_Shoot.Shoot();
        }
    }


    //自機が通常時
    private void Update_In_Player_Normal() {
        if(time_Count < UNDER_SHOOT_SPAN) {
            time_Count += Time.deltaTime;            
        }
        else {
            time_Count = 0;
            under_Shoot.Shoot();
        }
    }
}
