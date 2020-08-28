using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunFlowerFairy2 : Enemy {


    private MoveMotion _move;
    private ShootSystem _shoot;


	void Start() {
        _move = GetComponent<MoveMotion>();
        _shoot = GetComponentInChildren<ShootSystem>();       

        StartCoroutine("Action_Cor");
    }


    private IEnumerator Action_Cor() {
        _move.Start_Move(0);
        yield return new WaitForSeconds(1.0f);
        _shoot.Shoot();
    }
}
