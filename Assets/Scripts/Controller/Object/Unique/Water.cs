using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {
    
    [SerializeField] private GameObject water_Screen_Effect_Prefab;

    private ObjectPool surface_Effect_Pool;
    

	// Use this for initialization
	void Start () {
        //水面エフェクトのオブジェクトプール
        var effect = Resources.Load("Effect/WaterSurfaceEffect") as GameObject;
        ObjectPoolManager.Instance.Create_New_Pool(effect, 2);
        surface_Effect_Pool = ObjectPoolManager.Instance.Get_Pool(effect);        
    }
	

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            Play_Surface_Effect(collision.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            Play_Surface_Effect(collision.transform.position);
        }
    }


    //水面の出入りでエフェクトを出す
    private void Play_Surface_Effect(Vector2 pos) {
        var effect = surface_Effect_Pool.GetObject();
        effect.transform.position = new Vector3(pos.x, Surface_Height(pos.y));        
    }


    //当たり判定から一番近いマス目の高さを返す
    private float Surface_Height(float collision_Height) {
        int index = (int)(collision_Height + 16) / 32;
        if (collision_Height < 0)
            index--;

        return index * 32f;
    }
}
