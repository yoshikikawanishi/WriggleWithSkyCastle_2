using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// FlexibleLightTextureで管理する光の１マス
/// </summary>
public class SearchLightTile : MonoBehaviour {

    public bool on_Wall = false;
    public bool is_Detect = false;
    private bool is_Exist = true;    
    private SearchLight manager;
    private SpriteRenderer _sprite;    


    public void Setting(SearchLight manager) {
        this.manager = manager;
        _sprite = GetComponent<SpriteRenderer>();
    }


    //壁に遮られて消える
    public void To_Disable() {
        if (is_Exist) {
            is_Exist = false;
            _sprite.enabled = false;
        }
    }


    //壁の遮断が無くなる
    public void To_Enable() {
        if (!is_Exist) {
            is_Exist = true;
            _sprite.enabled = true;
        }
    }


    void OnTriggerEnter2D(Collider2D collision) {        
        //壁と衝突時
        if (!on_Wall) {
            if (manager.wall_Tags.Contains(collision.tag)) {
                on_Wall = true;
            }
        }               
        //検出
        if(is_Exist && !is_Detect) {
            if(collision.tag == manager.detect_Tag) {
                is_Detect = true;
            }
        }
    }


    void OnTriggerExit2D(Collider2D collision) {        
        //壁と離れた時
        if (on_Wall) {
            if (manager.wall_Tags.Contains(collision.tag)) {
                on_Wall = false;
            }
        }
        //検出
        if (is_Exist && is_Detect) {
            if (collision.tag == manager.detect_Tag) {
                is_Detect = false;
            }
        }
    }    
}
