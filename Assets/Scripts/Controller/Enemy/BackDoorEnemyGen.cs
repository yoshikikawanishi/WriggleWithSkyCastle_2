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
    private GameObject main_Camera;

    private float time = 0;


    void Start () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        main_Camera = GameObject.FindWithTag("MainCamera");
        _sprite.color = new Color(1, 1, 1, 0);
	}
	
	
	void Update () {
        //自機が近付いたら現れる
        if(now_State == State.wait) {
            Wait();
        }
        //現れる
        else if(now_State == State.appear) {
            Appear();            
        }
        //生成
        else if(now_State == State.generate) {
            Generate();
        }        
        //消える
        else if(now_State == State.disapper) {
            Disappear();
        }
	}


    private void Wait() {
        float distance = transform.position.x - main_Camera.transform.position.x;
        //カメラの右側に出現する
        if (start_Generate_Distance > 0) {
            if(distance > 0 && distance < start_Generate_Distance) {
                now_State = State.appear;
            }
        }
        //カメラが通過した後左側から出現する
        else {
            if(start_Generate_Distance - 100f < distance && distance < start_Generate_Distance) {
                now_State = State.appear;
            }
        }
        
    }

    private void Appear() {
        if (_sprite.color.a < 1.0f)
            _sprite.color += new Color(0, 0, 0, 0.02f);
        else
            now_State = State.generate;
    }


    private void Generate() {
        GameObject enemy = Instantiate(generate_Enemy_Prefab);
        enemy.transform.position = transform.position;
        now_State = State.disapper;
    }     


    private void Disappear() {
        if (_sprite.color.a > 0)
            _sprite.color += new Color(0, 0, 0, -0.02f);
        else
            Destroy(gameObject);
    }
}
