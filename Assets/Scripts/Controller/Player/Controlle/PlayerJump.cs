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

   
    private void Awake() {
        _rigid      = GetComponent<Rigidbody2D>();
        _controller = GetComponent<PlayerController>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
    }

    
    public void Jump() {
        _rigid.velocity = new Vector2(_rigid.velocity.x, jump_Power);
        player_SE.Play_Jump_Sound();
        player_Effect.Play_Jump_Effect();
        _controller.Change_Animation("JumpBool");
        _controller.is_Landing = false;
    }


    //減速
    public void Slow_Down() {
        if (_rigid.velocity.y > 0) {
            _rigid.velocity *= new Vector2(1, 0.3f);
        }
    }

	
	
}
