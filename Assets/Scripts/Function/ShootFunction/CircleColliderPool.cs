using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleColliderPool : MonoBehaviour {

    List<CircleCollider2D> pool;

    public CircleColliderPool() {
        pool = new List<CircleCollider2D>();
    }

    public CircleCollider2D Get_Collider() {
        //使用していないものを探す
        foreach (CircleCollider2D c in pool) {
            if (!c.enabled) {
                c.enabled = true;
                return c;
            }
        }
        //すべて使用していたら新しく作る
        CircleCollider2D cc = gameObject.AddComponent<CircleCollider2D>();
        cc.isTrigger = true;
        cc.radius = GetComponent<Laser>().laserWidth / 2 - 1;
        pool.Add(cc);
        return cc;
    }


    public void Set_Inactive_All() {
        foreach(CircleCollider2D c in pool) {
            c.enabled = false;
        }
    }
}
