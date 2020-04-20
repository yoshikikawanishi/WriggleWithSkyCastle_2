﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBLDefine;


public class ButtonCrashBlock : MonoBehaviour {

    //ボタン種類
    private enum ButtonKind {
        jump = 0,
        attack = 1,
        fly = 2,
    }
    private const int BUTTONNUM = 3;
    
    //アイコンの色
    private readonly Color dark_Color = new Color(0.5f, 0.5f, 0.5f);
    private readonly Color light_Color = new Color(1, 1, 1);
    
    //アイコンスプライト
    [SerializeField] private Sprite icon_Jump;
    [SerializeField] private Sprite icon_Attack;
    [SerializeField] private Sprite icon_Fly;
    [SerializeField] private SpriteRenderer icon_Sprite;
    [Space]
    //要求ボタンの設定用
    [SerializeField] private List<ButtonKind> require_Button_List = new List<ButtonKind> { ButtonKind.jump };
    
    //ボタンアイコン配列化用
    private Sprite[] icon_Textures = new Sprite[BUTTONNUM];
    
    //その他クラス
    private PlayerController player_Controller;
    private ChildColliderTrigger detection;

    private bool is_Player_Nearly = false;


    void Awake() {
        //初期設定
        icon_Textures[(int)ButtonKind.jump] = icon_Jump;
        icon_Textures[(int)ButtonKind.attack] = icon_Attack;
        icon_Textures[(int)ButtonKind.fly] = icon_Fly;

        Display_Required_Button_Sprite();
    }


    void Start() {
        //取得
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        detection = GetComponentInChildren<ChildColliderTrigger>();
    }
    
    
    void Update() {
        //自機が近くにいるとき
        if (detection.Hit_Trigger()) {
            Accept_Input();
            //近づいた瞬間
            if (!is_Player_Nearly) {
                is_Player_Nearly = true;
                Do_Process_Approach_Player();
            }
        }
        //自機が離れているとき
        else {
            //離れた瞬間
            if (is_Player_Nearly) {
                is_Player_Nearly = false;
                Do_Process_Depart_Player();
            }
        }
    }    


    //自機が近付いた瞬間の処理
    private void Do_Process_Approach_Player() {
        icon_Sprite.color = light_Color;            //色
        player_Controller.Set_Can_Action(false);    //自機のアクション無効化
    }


    //自機が離れた瞬間の処理
    private void Do_Process_Depart_Player() {
        icon_Sprite.color = dark_Color;             //色
        player_Controller.Set_Can_Action(true);     //自機のアクション無効化
    }


    //入力受付
    private void Accept_Input() {
        if (require_Button_List.Count == 0)
            return;

        Key require_Key = Get_Key(require_Button_List[0]);
        if (InputManager.Instance.GetKeyDown(require_Key)) {
            Action_In_Input();
        }
    }


    //要求ボタンを入力されたときの処理
    private void Action_In_Input() {
        require_Button_List.RemoveAt(0);
        //ボタン変更
        if (require_Button_List.Count > 0) {
            Display_Required_Button_Sprite();
            GetComponent<ObjectShake>().Shake(0.2f, new Vector2(1, 0), true);
        }
        //消滅
        else {
            Destroy(gameObject);
        }
    }



    //ButtonKindを入力ボタンに変換する
    private Key Get_Key(ButtonKind kind) {
        if (kind == ButtonKind.jump)
            return Key.Jump;
        if (kind == ButtonKind.attack)
            return Key.Attack;
        if (kind == ButtonKind.fly)
            return Key.Fly;
        return null;
    }


    //入力待ちのボタンを表示する
    private void Display_Required_Button_Sprite() {
        for (int i = 0; i < BUTTONNUM; i++) {            
            if (i == (int)require_Button_List[0]) {
                icon_Sprite.sprite = icon_Textures[i];
            }
        }
    }
}