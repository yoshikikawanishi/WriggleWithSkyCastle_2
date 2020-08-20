using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulEnemy : Enemy {

    [SerializeField] private Bullet bullet_Prefab;

    private ObjectPool bullet_Pool;

    private MoveMotion _move;

    private readonly float span = 0.2f;

	
	void Start () {
        //カメラの子供に生成されるので解除
        transform.SetParent(null);
        //弾のオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(bullet_Prefab.gameObject, 10);
        bullet_Pool = ObjectPoolManager.Instance.Get_Pool(bullet_Prefab.gameObject);
        //移動
        _move = GetComponent<MoveMotion>();
        _move.Start_Move();
        //ショット開始
        StartCoroutine("Shoot_Cor");
	}


    private IEnumerator Shoot_Cor() {
        yield return new WaitForSeconds(1.0f);
        while(transform.position.y > -200f) {
            yield return new WaitForSeconds(span);
            var bullet = bullet_Pool.GetObject();
            bullet.transform.position = transform.position;
        }
        _move.Stop_Move();
        Destroy(gameObject);
    }
	
}
