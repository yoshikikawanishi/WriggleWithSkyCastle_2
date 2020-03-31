using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 自機に触れると透明になる
/// </summary>
public class TileHidden : MonoBehaviour {
    
    private Tilemap map;

    public enum STATE {
        hidden,
        open
    }
    private STATE state = STATE.hidden;


    //Start
    private void Start() {
        //取得
        map = GetComponent<Tilemap>();            
    }


    //Update
    private void Update() {
        if (state == STATE.hidden && map.color.a <= 1) {
            map.color += new Color(0, 0, 0, 0.1f);
        }
        else if (state == STATE.open && map.color.a >= 0) {
            map.color += new Color(0, 0, 0, -0.1f);
        }
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            state = STATE.open;
        }
    }


    //OnTriggerExit
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            state = STATE.hidden;
        }
    }

}
