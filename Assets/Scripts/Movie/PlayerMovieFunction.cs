﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovieFunction : MonoBehaviour {

    private GameObject player;
    private PlayerController player_Controller;
    private Rigidbody2D player_Rigid;


    void Awake() {
        player = GameObject.FindWithTag("PlayerTag");
        player_Controller = player.GetComponent<PlayerController>();
        player_Rigid = player.GetComponent<Rigidbody2D>();
    }


    //自機の操作無効化、停止
    public void Disable_Controlle_Player() {
        if (player == null)
            return;

        player_Controller.Set_Is_Playable(false);
        player_Controller.To_Disable_Ride_Beetle();
        player_Controller.Change_Animation("IdleBool");
        player_Rigid.velocity = Vector2.zero;
    }


    //自機の操作無効化、飛行
    public void Disable_Controlle_Player_Flying() {
        if (player == null)
            return;

        player_Controller.Set_Is_Playable(false);        
        player_Controller.Change_Animation("RideBeetleBool");
        player_Rigid.velocity = Vector2.zero;
    }


    //自機の操作有効化
    public void Enable_Controlle_Player() {
        if (player == null)
            return;

        player_Controller.Set_Is_Playable(true);
        player_Controller.To_Enable_Ride_Beetle();
    }
}