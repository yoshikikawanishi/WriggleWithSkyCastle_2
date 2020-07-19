using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Resources/UIにShopCanvas入れておくこと
/// </summary>
public class Shop : MonoBehaviour {    

    private GameObject shop_Canvas_Prefab;
    private GameObject shop_Canvas_Obj;
    private ChildColliderTrigger hit_Player_Trigger;

    private bool is_Shopping = false;


    void Start () {
        shop_Canvas_Prefab = Resources.Load("UI/ShopCanvas") as GameObject;
        hit_Player_Trigger = transform.Find("Collision").GetComponent<ChildColliderTrigger>();
	}
	
	
    void Update() {
        if (is_Shopping) {
            return;
        }

        if (hit_Player_Trigger.Hit_Trigger()) {
            if (Input.GetAxisRaw("Vertical") > 0) {
                is_Shopping = true;
                StartCoroutine("Start_Shopping_Cor");
            }
        }

    }


    private IEnumerator Start_Shopping_Cor() {
        yield return new WaitForSeconds(0.5f);
        //キャンバス生成、キャンバスの方でショップ開始・終了の処理
        if (shop_Canvas_Obj == null)
            shop_Canvas_Obj = Instantiate(shop_Canvas_Prefab);
        else
            shop_Canvas_Obj.SetActive(true);

        //買い物中は時間が止まる
        yield return new WaitForSeconds(1.0f);
        is_Shopping = false;
    }    

}
