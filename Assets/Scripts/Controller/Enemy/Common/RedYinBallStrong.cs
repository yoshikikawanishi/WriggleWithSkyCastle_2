using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedYinBallStrong : Enemy {

    private GameObject player;
    private GravitatePlayer _gravitate;

    private const float initial_Radius = 32f;

    private bool is_Enabled = false;

	
	void Start () {
        //取得
        _gravitate = GetComponent<GravitatePlayer>();
        player = GameObject.FindWithTag("PlayerTag");

        _gravitate.enabled = false;
	}
	

    void OnBecameVisible() {
        if (!is_Enabled) {
            is_Enabled = true;
            StartCoroutine("Action_Cor");
        }
    }


    private IEnumerator Action_Cor() {
        //自機追従
        _gravitate.enabled = true;
        yield return new WaitForSeconds(1.0f);
        _gravitate.enabled = false;

        //移動
        //中心座標計算


        is_Enabled = false;
    }


    //円弧移動の中心座標を計算
    private Vector2 Cal_Center_Pos() {
        Vector2 to_Center_Vec = transform.position - player.transform.position;
        to_Center_Vec = new Vector2(1f / to_Center_Vec.x, 1f / to_Center_Vec.y).normalized;
        Vector2 center = (Vector2)transform.position + to_Center_Vec * initial_Radius;
        return center;
    }
	
}
