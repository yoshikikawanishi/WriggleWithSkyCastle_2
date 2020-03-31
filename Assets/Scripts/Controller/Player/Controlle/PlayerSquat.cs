using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquat : MonoBehaviour {

    //コンポーネント
    private PlayerController _controller;
    private PlayerBodyCollision player_Body;    


	// Use this for initialization
	void Start () {
        //取得
        _controller = GetComponent<PlayerController>();
        player_Body = GetComponentInChildren<PlayerBodyCollision>();
	}


    //しゃがむ
    public void Squat() {
        if (_controller.is_Squat) 
            return;
        
        _controller.is_Squat = true;
        _controller.Change_Animation("SquatBool");
        player_Body.Change_Collider_Size(new Vector2(10, 12), new Vector2(0, -6));
    }

    //しゃがみ解除
    public void Release_Squat() {
        if (_controller.is_Squat) {
            _controller.is_Squat = false;
            player_Body.Back_Default_Collider();
        }
    }
}
