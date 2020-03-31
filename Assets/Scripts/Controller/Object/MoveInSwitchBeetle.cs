using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自機の飛行状態切り替え時に動く地面にアタッチする
/// MoveMotionのElement0に飛行開始時、Element1に飛行解除時の動きを入れること
/// </summary>
[RequireComponent(typeof(MoveMotion))]
public class MoveInSwitchBeetle : MonoBehaviour {

    private PlayerController player_Controller;
    private MoveMotion _move;

    private enum PlayerState {
        normal,
        fly,
    }
    PlayerState player_State = PlayerState.normal;

    private bool is_Moving = false;


    private void Awake() {
        _move = GetComponent<MoveMotion>();
        
    }

    // Use this for initialization
    void Start () {
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        if (player_Controller.Get_Is_Ride_Beetle()) {
            player_State = PlayerState.fly;
            is_Moving = true;
            _move.Start_Move(0);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (_move.Is_End_Move()) {
            is_Moving = false;
        }
        if (is_Moving) {
            return;
        }
        //飛行開始時
		if(player_State == PlayerState.normal && player_Controller.Get_Is_Ride_Beetle()) {
            player_State = PlayerState.fly;
            is_Moving = true;
            _move.Start_Move(0);
        }
        //飛行終了時
        if(player_State == PlayerState.fly && !player_Controller.Get_Is_Ride_Beetle()) {
            player_State = PlayerState.normal;
            is_Moving = true;
            _move.Start_Move(1);
        }
	}
}
