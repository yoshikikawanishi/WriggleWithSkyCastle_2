using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHavingDeleteEffect : Bullet {

    [SerializeField] private GameObject delete_Effect;
    [SerializeField] private int effect_Pool_Num = 1;

    private void Start() {
        if (delete_Effect == null) {
            Debug.Log("Delete Effect not Setting in Bullet");
            return;
        }

        //エフェクトのオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(delete_Effect, effect_Pool_Num);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (delete_Effect == null)
            return;

        //消滅、エフェクト
        foreach (string tag in deleted_Obj_Tag) {
            if (collision.tag == tag) {
                var effect = ObjectPoolManager.Instance.Get_Pool(delete_Effect).GetObject();
                effect.transform.position = transform.position;
                ObjectPoolManager.Instance.Set_Inactive(effect, 2f);
                gameObject.SetActive(false);
            }
        }
    }

}
