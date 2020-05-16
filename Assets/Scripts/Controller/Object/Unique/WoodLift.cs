using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodLift : MonoBehaviour {

    [SerializeField] private bool is_Upper_Lift = true;

    private GameObject player;
    //床、子供に設置すること
    private GameObject lift_Origin;

    private bool start_Generate = false;

    //パラメータ
    private float generate_Span = 3.0f;
    private float lift_Speed = 1f;


	// Use this for initialization
	void Start () {
        //取得
        player = GameObject.FindWithTag("PlayerTag");
        //床のオブジェクトプール
        lift_Origin = transform.GetChild(0).gameObject;
        ObjectPoolManager.Instance.Create_New_Pool(lift_Origin, 4);
	}
	

	// Update is called once per frame
	void Update () {
	    if(player.transform.position.x > transform.position.x - 400f && !start_Generate) {
            start_Generate = true;
            StartCoroutine("Generate_Lift_Cor");
        }	
	}


    private IEnumerator Generate_Lift_Cor() {
        while (true) {
            //リフトの生成
            var lift = ObjectPoolManager.Instance.Get_Pool(lift_Origin).GetObject();
            lift.transform.position = transform.position;
            //移動させる
            StartCoroutine("Move_Lift_Cor", lift);
            yield return new WaitForSeconds(generate_Span);
        }
    }

    private IEnumerator Move_Lift_Cor(GameObject lift) {
        if (is_Upper_Lift) {
            while(lift.transform.position.y < 150f) {
                lift.transform.position += new Vector3(0, lift_Speed);
                yield return new WaitForSeconds(0.016f);
            }            
        }
        else {
            while (lift.transform.position.y > -150f) {
                lift.transform.position += new Vector3(0, -lift_Speed);
                yield return new WaitForSeconds(0.016f);
            }            
        }
        lift.SetActive(false);
    }


}
