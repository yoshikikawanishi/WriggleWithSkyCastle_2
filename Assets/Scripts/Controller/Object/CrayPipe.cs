using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrayPipe : MonoBehaviour {

    [SerializeField] private GameObject beetle_Power_Prefab;
    private GameObject beetle_Power = null;
    

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void LateUpdate () {
        //緑パワーの生成
        if (beetle_Power == null) {
            beetle_Power = Instantiate(beetle_Power_Prefab);
            beetle_Power.transform.position = transform.position + new Vector3(0, -64f);            
            beetle_Power.GetComponent<Animator>().SetBool("ActiveBool", false);
            beetle_Power.GetComponent<CircleCollider2D>().enabled = false;
        }
        //緑パワーの上昇
        else {
            if (beetle_Power.transform.position.y < transform.position.y + 64f) {
                beetle_Power.transform.position += new Vector3(0, 1f) * Time.timeScale;             
            }
            else {
                beetle_Power.GetComponent<Animator>().SetBool("ActiveBool", true);
                beetle_Power.GetComponent<CircleCollider2D>().enabled = true;
            }
        }
	}


}
