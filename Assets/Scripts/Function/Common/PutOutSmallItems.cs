using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutOutSmallItems : MonoBehaviour {

    private GameObject power_Prefab;
    private GameObject score_Prefab;

    private ObjectPool power_Pool;
    private ObjectPool score_Pool;
    

    private void Awake() {
        power_Prefab = Resources.Load("Object/Power") as GameObject;
        score_Prefab = Resources.Load("Object/Score") as GameObject;
        power_Pool = ObjectPoolManager.Instance.Get_Pool(power_Prefab);
        score_Pool = ObjectPoolManager.Instance.Get_Pool(score_Prefab);
    }


    public void Put_Out_Item(int power_Num, int score_Num) {
        //パワー
        for(int i = 0; i < power_Num; i++) {
            var item = power_Pool.GetObject();
            item.transform.position = transform.position + new Vector3(0, 16f);
            var velocity = new Vector2(Random.Range(-10f, 10f) * i, Random.Range(300f, 450f));
            item.GetComponent<Rigidbody2D>().velocity = velocity;
        }
        //スコア
        for (int i = 0; i < score_Num; i++) {
            var item = score_Pool.GetObject();
            item.transform.position = transform.position + new Vector3(0, 16f);
            var velocity = new Vector2(Random.Range(-10f, 10f) * i, Random.Range(300f, 450f));
            item.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }
}
