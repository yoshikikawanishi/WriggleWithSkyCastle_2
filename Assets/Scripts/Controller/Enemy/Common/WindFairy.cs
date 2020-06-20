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

    private float UNDER_SHOOT_SPAN = 1.5f;
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
            time_Count = STRONG_SHOOT_SPAN;
        }
	}


    //自機が飛行時
    private void Update_In_Player_Flying() {
        if(time_Count < STRONG_SHOOT_SPAN) {
            time_Count += Time.deltaTime;            
        }
        else {
            time_Count = 0;
            StartCoroutine("Strong_Shoot_Cor");
        }
    }


    //自機が通常時
    private void Update_In_Player_Normal() {
        if(time_Count < UNDER_SHOOT_SPAN) {
            time_Count += Time.deltaTime;            
        }
        else {
            time_Count = 0;
            StartCoroutine("Under_Shoot_Cor");
        }
    }


    private IEnumerator Under_Shoot_Cor() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        for(int i = 0; i < 3; i++) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(0.5f);
        under_Shoot.Shoot();
    }


    private IEnumerator Strong_Shoot_Cor() {
        GetComponentInChildren<ParticleSystem>().Play();
        yield return new WaitForSeconds(1.0f);
        strong_Shoot.center_Angle_Deg = Random.Range(0, 90f);
        strong_Shoot.Shoot();
    }
}
