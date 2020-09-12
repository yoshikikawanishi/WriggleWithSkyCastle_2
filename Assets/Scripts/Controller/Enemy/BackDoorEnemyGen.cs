using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDoorEnemyGen : MonoBehaviour {

    [SerializeField] private GameObject generate_Enemy_Prefab;
    [SerializeField] private float start_Generate_Distance;

    private enum State {
        wait,
        appear,
        generate,
        disapper
    }
    private State now_State = State.wait;

    private SpriteRenderer _sprite;
    private SpriteRenderer enemy_Sprite;
    private GameObject player;


    void Start () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("PlayerTag");
	}
	
	
	void Update () {
        //自機が近付いたら現れる
        if(now_State == State.wait) {
            if(Mathf.Abs(player.transform.position.x - transform.position.x) < start_Generate_Distance) {
                now_State = State.appear;
            }
        }
        //現れる
        else if(now_State == State.appear) {
            Appear();            
        }
        //生成
        else if(now_State == State.generate) {

        }
        //消える
        else if(now_State == State.disapper) {
            Disappear();
        }
	}


    private void Appear() {
        if (_sprite.color.a < 1.0f)
            _sprite.color += new Color(0, 0, 0, 0.02f);
        else
            now_State = State.generate;
    }


    private void Generate() {

    }


    private void Disappear() {
        if (_sprite.color.a > 0)
            _sprite.color += new Color(0, 0, 0, -0.02f);
        else
            Destroy(gameObject);
    }
}
