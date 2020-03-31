using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderFootingEnemy : MonoBehaviour {

    private GameObject spider_Footing;

	// Use this for initialization
	void Start () {
        spider_Footing = Resources.Load("Object/SpiderFooting") as GameObject;
	}
	
	//足場を生成する
    public void Generate_Footing_Vanish() {
        var footing = ObjectPoolManager.Instance.Get_Pool(spider_Footing).GetObject();
        footing.transform.position = transform.position;
        GetComponent<Enemy>().Vanish();
    }
}
