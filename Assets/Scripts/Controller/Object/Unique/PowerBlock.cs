using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerBlock : MonoBehaviour {

    private GameObject power_Prefab;

    private List<string> damaged_Tag_List = new List<string> {
        "PlayerKickTag",
        "PlayerAttackTag",
        "PlayerChargeAttackTag",
    };

    private Vector2 default_Pos;

    private SpriteRenderer _sprite;

    private int damaged_Count = 0;


    void Start () {
        power_Prefab = Resources.Load("Object/Power") as GameObject;
        ObjectPoolManager.Instance.Create_New_Pool(power_Prefab, 50);
        default_Pos = transform.position;
        _sprite = GetComponent<SpriteRenderer>();
	}


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (var tag in damaged_Tag_List) {
            if (tag == collision.tag) {
                Damaged();
            }
        }
    }

    private void Damaged() {
        //パワー出す
        int num = 100 - damaged_Count * 10;
        float angle;
        float power;
        for(int i = 0; i < num; i++) {            
            //生成
            var obj = ObjectPoolManager.Instance.Get_Pool(power_Prefab).GetObject();
            obj.transform.position = transform.position;
            //初速
            angle = Random.Range(0.3f * Mathf.PI, 0.7f * Mathf.PI);
            power = Random.Range(200f, 300f);
            obj.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * power;
        }
        //揺らす
        StartCoroutine(Shake_Cor());
        //色を下げる
        _sprite.color -= new Color(0.05f, 0.05f, 0.05f, 0);
        damaged_Count++;
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
