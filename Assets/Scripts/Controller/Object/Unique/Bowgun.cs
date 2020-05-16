using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bowgun : MonoBehaviour {

    [SerializeField] private GameObject bow_Bullet;

    private Animator _anim;

    private bool is_Active = false;
    private float time = 2.0f;
    private float SHOOT_SPAN = 1.2f;


	// Use this for initialization
	void Start () {
        //取得
        _anim = GetComponent<Animator>();
        //弾のオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(bow_Bullet, 3);
	}
	
	// Update is called once per frame
	void Update () {
        if (!is_Active) 
            return;
        
	    if(time < SHOOT_SPAN) {
            time += Time.deltaTime;
        }
        else {
            time = 0;
            StartCoroutine("Shoot_Cor");
        }
	}

    private void OnBecameVisible() {
        is_Active = true;
    }

    private void OnBecameInvisible() {
        is_Active = false;
    }


    private IEnumerator Shoot_Cor() {
        //溜め
        _anim.SetTrigger("ShootTrigger");
        yield return new WaitForSeconds(0.2f);
        //弾の生成、回転、発射
        var bullet = ObjectPoolManager.Instance.Get_Pool(bow_Bullet).GetObject();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = transform.rotation;
        bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * 150f;        
    }
}
