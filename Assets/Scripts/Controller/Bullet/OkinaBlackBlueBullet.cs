using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaBlackBlueBullet : MonoBehaviour {

    private SpriteRenderer _sprite;
    private float time = 0;
    private float span = 4.0f;


    void Awake() {
        _sprite = GetComponent<SpriteRenderer>();    
    }


    void OnEnable() {
        time = 0;
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }


    void Update () {
        if (-1.0f < time && time < span) {
            time += Time.deltaTime;
        }
        else {
            _sprite.color = new Color(1, 1, 1, 1);
            gameObject.layer = LayerMask.NameToLayer("EnemyLayer");
            time = -10;
        }
	}
}
