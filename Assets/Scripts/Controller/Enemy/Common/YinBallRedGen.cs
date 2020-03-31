using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YinBallRedGen : MonoBehaviour {

    private GameObject main_Camera;
    private bool is_Generated = false;


	// Use this for initialization
	void Start () {
        //取得
        main_Camera = GameObject.FindWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
        //すでに生成済み
        if (is_Generated)
            return;
        //カメラが右側にいったら生成なくす
        if (main_Camera.transform.position.x > transform.position.x)
            return;

        //カメラが160左についたら生成開始
		if(main_Camera.transform.position.x > transform.position.x - 120f) {
            is_Generated = true;
            StartCoroutine("Generate_Cor");
        }
	}


    private IEnumerator Generate_Cor() {
        //溜め
        GetComponent<ParticleSystem>().Play();
        transform.SetParent(main_Camera.transform);
        yield return new WaitForSeconds(0.5f);
        //生成開始
        GetComponent<SpreadBombController>().enabled = true;
        transform.GetChild(0).gameObject.SetActive(true);        
    }
}
