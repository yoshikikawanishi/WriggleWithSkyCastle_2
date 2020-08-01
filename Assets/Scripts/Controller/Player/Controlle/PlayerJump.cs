using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

    //コンポーネント
    private Rigidbody2D _rigid;
    private PlayerController _controller;
    private PlayerSoundEffect player_SE;
    private PlayerEffect player_Effect;

    private float jump_Power = 300f;

    //大ジャンプ、中ジャンプ、小ジャンプ区別用
    private bool is_Jumping = false;
    private float jumping_Time = 0;    
    private float slow_Down_Time = 0;

   
    void Awake() {
        _rigid      = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
    }


    void Update() {
        //大ジャンプ、中ジャンプ、小ジャンプ区別用
        if (is_Jumping) {
            jumping_Time += Time.deltaTime;
            if(jumping_Time > slow_Down_Time) {
                is_Jumping = false;
                jumping_Time = 0;
                Slow_Down();
            }
        }                
    }

    
    public void Jump() {
        _rigid.velocity = new Vector2(_rigid.velocity.x, jump_Power);
        player_SE.Play_Jump_Sound();
        player_Effect.Play_Jump_Effect();
        _controller.Change_Animation("JumpBool");
        _controller.is_Landing = false;
        //大ジャンプ、中ジャンプ、小ジャンプ区別用
        is_Jumping = true;
        jumping_Time = 0;
        slow_Down_Time = 100f;
    }


    //ジャンプボタンが離されたときの時間を保存
    public void Release_Jumping() {
        if (!is_Jumping)
            return;

        if(jumping_Time < 0.05f) {
            slow_Down_Time = 0.05f;
        }        
        else {
            slow_Down_Time = 0;
        }        
    }


    //減速
    private void Slow_Down() {
        if (_rigid.velocity.y > 0) {
            _rigid.velocity *= new Vector2(1, 0.3f);
        }        
    }

	
	
}
