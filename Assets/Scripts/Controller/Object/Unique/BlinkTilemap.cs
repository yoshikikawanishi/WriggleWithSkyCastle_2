using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 一定時間間隔で半透明化、当たり判定を消す
/// </summary>
[RequireComponent(typeof(Tilemap), typeof(TilemapCollider2D))]
public class BlinkTilemap : MonoBehaviour {

    private enum State {
        tranceparent,
        normal
    }

    [SerializeField] private State initial_State = State.normal;
    private float span = 2.093f;    

    private Tilemap _tilemap;
    private TilemapCollider2D _collider;

    private State now_State = State.normal;
    private float time = 0;

    private Color default_Color;
    private float tranceparency = 0.4f;


    void Start() {
        //取得
        _tilemap = GetComponent<Tilemap>();
        _collider = GetComponent<TilemapCollider2D>();
        //初期値        
        default_Color = _tilemap.color;
        if(initial_State == State.tranceparent) {
            Switch();
        }
    }

	
	void Update () {
	    if(time < span) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            Switch();
        }
	}


    //切り替え
    private void Switch() {
        //消す
        if(now_State == State.normal) {
            now_State = State.tranceparent;
            _tilemap.color = new Color(default_Color.r, default_Color.g, default_Color.b, tranceparency);
            _collider.enabled = false;
        }
        //出す
        else {
            now_State = State.normal;
            _tilemap.color = default_Color;
            _collider.enabled = true;
        }        
    }
}
