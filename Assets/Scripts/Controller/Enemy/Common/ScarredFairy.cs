using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarredFairy : FairyEnemy {

    [Space]
    [SerializeField] private ShootSystem distante_Shoot;
    [SerializeField] private ShootSystem close_Shoot;
    [SerializeField] private ChildColliderTrigger side_Collision;

    private Renderer _renderer;
    private GameObject player;

    private float player_Close_Border = 64f;
    private bool is_Close_Player = false;

    private float MOVE_SPEED = 0.5f;
    private float move_Speed;


    void Start() {
        //取得
        _renderer = GetComponent<Renderer>();
        player = GameObject.FindWithTag("PlayerTag");
    }


    void Update() {
        if (!_renderer.isVisible) {
            return;
        }

        //自機が近付いた時
        if (Is_Close_Player()) {
            
        }
        //自機が遠のいた時
        else {
            
        }       
    }


    //自機が近くにいる時true
    private bool Is_Close_Player() {
        if(Mathf.Abs(transform.position.x - player.transform.position.x) < player_Close_Border) {
            return true;
        }
        return false;
    }

    
    //遠くにいる自機に対するショット
    private IEnumerator Start_Distante_Shoot_Cor() {
        yield return null;
    }


    //近くにいる自機に対するショット
    private IEnumerator Start_Close_Shoot_Cor() {
        yield return null;
    }

}
