using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetlePowerBlock : MonoBehaviour {


    private List<string> damaged_Tag_List = new List<string> {
        "PlayerKickTag",
        "PlayerAttackTag",
        "PlayerChargeAttackTag",
    };

    private Vector2 default_Pos;


    private void Start() {
        default_Pos = transform.position;    
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(var tag in damaged_Tag_List) {
            if(tag == collision.tag) {
                Damaged();
            }
        }
    }

    private void Damaged() {
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 20);
        //エフェクト
        StartCoroutine(Shake_Cor());
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        ReleaseAndConvergePlayer RC = transform.GetChild(0).GetComponent<ReleaseAndConvergePlayer>();
        RC.Play_Release_And_Converge(12, transform.position, new Vector2(208f, -140f), main_Camera);        
    }

    //揺れる
    private IEnumerator Shake_Cor() {
        for (float t = 0; t < 0.25f; t += 0.016f) {
            transform.position = default_Pos + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * Time.timeScale;
            yield return null;
        }
        transform.position = default_Pos;
    }

}
