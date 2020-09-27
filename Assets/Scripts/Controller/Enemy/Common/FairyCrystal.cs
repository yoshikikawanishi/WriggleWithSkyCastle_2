using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyCrystal : MonoBehaviour {

    private enum State {
        appear,
        idle,
        converge,
        disable
    }
    private State state = State.appear;

    [SerializeField] private Enemy generate_Enemy_Prefab;
    [SerializeField] private ParticleSystem converge_Effect;
    [SerializeField] private ParticleSystem burst_Effect;

    private SpriteRenderer _sprite;

    private float time = 0;

	
	void Awake () {
        _sprite = GetComponent<SpriteRenderer>();
	}


    void OnEnable() {
        time = 0;
        state = State.appear;
        converge_Effect.Play();
        _sprite.color = new Color(1, 1, 1, 0f);
    }
	
	
	void Update () {
        time += Time.deltaTime;
        //出現中
        if(state == State.appear) {
            //出現
            if(time > 1.0f) {
                state = State.idle;
                _sprite.color = new Color(1, 1, 1, 0.7f);
            }
        }
        //待つ
        else if(state == State.idle) {
            //敵生成開始
            if(time > 1.2f) {
                state = State.converge;
                converge_Effect.Play();
            }            
        }
        else if(state == State.converge) {
            //敵生成、消滅
            if(time > 2.0f) {
                state = State.disable;
                Generate_Enemy();
                Play_Burst_Effect();
                gameObject.SetActive(false);
            }
        }
	}
    

    private void Play_Burst_Effect() {
        var obj = Instantiate(burst_Effect.gameObject);
        obj.transform.position = transform.position;
        obj.GetComponent<ParticleSystem>().Play();
        UsualSoundManager.Instance.Play_Shoot_Sound();
        Destroy(obj, 1.5f);
    }

    private void Generate_Enemy() {
        var obj = Instantiate(generate_Enemy_Prefab);
        obj.transform.position = transform.position;
    }

    
}
