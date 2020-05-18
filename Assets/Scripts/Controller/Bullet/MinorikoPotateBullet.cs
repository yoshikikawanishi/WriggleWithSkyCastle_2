using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 画面内で地面に当たったときエフェクト出して消滅
/// </summary>
public class MinorikoPotateBullet : Bullet {

    private Renderer _renderer;

    private List<string> crash_Obj_Tags = new List<string>() {
        "GroundTag",
        "PlayerTag",
        "PlayerAttackTag",
        "PlayerChargeAttackTag",        
        "PlayerKickTag",
    };


    private void Awake() {
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!_renderer.isVisible) {
            return;
        }
        foreach(string  tag in crash_Obj_Tags) {
            if(collision.tag == tag) {
                Play_Crash_Effect();
                gameObject.SetActive(false);
            }
        }    
    }


    private void Play_Crash_Effect() {
        GameObject effect = Instantiate(transform.GetChild(2).gameObject);
        effect.transform.position = transform.position;
        effect.SetActive(true);
        Destroy(effect, 1.0f);
    }
}
