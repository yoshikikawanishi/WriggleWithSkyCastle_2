using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour {

    private enum ColorType {
        red,
        yellow,
        green
    }

    [SerializeField] private ColorType color;
    [SerializeField] private string canvas_Name = "Canvas";
    [SerializeField] private Vector2 save_Point_Offset = new Vector2(0, 0);


    private List<string> collide_Tags = new List<string> {
        "PlayerBodyTag",
        "PlayerAttackTag",
        "PlayerChargeAttackTag",
    };


    void Start() {
        //色変更
        Animator _anim = GetComponent<Animator>();
        switch (color) {
            case ColorType.red: /*アニメーション初期値は赤*/ break;
            case ColorType.yellow: _anim.SetTrigger("YellowTrigger"); break;
            case ColorType.green: _anim.SetTrigger("GreenTrigger"); break;
        }
    }


    void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in collide_Tags) {
            if (collision.tag == tag) {
                //セーブ
                DataManager.Instance.Save_Player_Data(transform.position + (Vector3)save_Point_Offset);
                //エフェクト
                GetComponent<ParticleSystem>().Play();
                GetComponent<AudioSource>().Play();
                //UIの表示
                GameObject.Find(canvas_Name).GetComponent<GameUIController>().Display_Save_Text();
            }
        }
    }
}
