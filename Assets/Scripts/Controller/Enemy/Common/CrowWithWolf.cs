using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowWithWolf : MonoBehaviour {


	// Use this for initialization
	void Start () {
        StartCoroutine("Rush_Cor");
	}
		

    private IEnumerator Rush_Cor() {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        GameObject player = GameObject.FindWithTag("PlayerTag");
        MoveTwoPoints _move = GetComponent<MoveTwoPoints>();

        //初期位置                
        transform.position = new Vector3(camera.transform.position.x + 240f, 170f);

        //突進
        float height = player.transform.position.y - transform.position.y;
        _move.Change_Paramter(0.007f, height, 0);
        Vector2 aim_Pos = new Vector2(transform.position.x - 600f, 170f);
        _move.Start_Move(aim_Pos);

        yield return new WaitUntil(_move.End_Move);
        Destroy(gameObject);
    }
}
