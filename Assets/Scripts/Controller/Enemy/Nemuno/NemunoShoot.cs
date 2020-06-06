using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NemunoShoot : MonoBehaviour {

    [SerializeField] private GameObject yellow_Rice_Bullet;
    [Space]
    [SerializeField] private ShootSystem slash_Shotgun_Odd;
    [SerializeField] private ShootSystem slash_Shootgun_Forward;
    [SerializeField] private GameObject knife_Shoot_Obj;
    [SerializeField] private ShootSystem kunai_Shoot;
    [Space]  
    [SerializeField] private GameObject phase2_Kunai_Shoot_Obj;
    [SerializeField] private GameObject Phase2_Square_Blocks;

    //コンポーネント
    private BulletAccelerator _accelerator;
    private Nemuno nemuno_Controller;


	// Use this for initialization
	void Start () {
        //取得
        _accelerator = GetComponent<BulletAccelerator>();
        nemuno_Controller = transform.parent.GetComponent<Nemuno>();
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(yellow_Rice_Bullet, 48);
	}
	
	
    //遠距離斬撃用のショットガン
    public void Shoot_Shotgun(int num) {       
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            return;

        slash_Shotgun_Odd.connect_Num = num;
        //自機が前方にいたら自機狙い 
        if ((player.transform.position.x - transform.position.x) * transform.parent.localScale.x < 0)
            slash_Shotgun_Odd.Shoot();
        //それ以外は前方に出す
        else {
            if (transform.parent.localScale.x > 0) {
                slash_Shootgun_Forward.center_Angle_Deg = 180f;
            }
            else {
                slash_Shootgun_Forward.center_Angle_Deg = 0;
            }
            slash_Shootgun_Forward.Shoot();
        }
    }


    //空中斬撃用
    public IEnumerator Shoot_Jump_Slash_Cor(int num) {
        List<GameObject> bullet_List = new List<GameObject>();
        Vector3 pos = transform.position + new Vector3(transform.parent.localScale.x * -24f, 0);

        //弾の生成
        for (int i = 0; i < num; i++) {
            var bullet = ObjectPoolManager.Instance.Get_Pool(yellow_Rice_Bullet).GetObject();
            bullet.transform.position = pos + new Vector3(0, -21f + i * 1.7f);
            bullet.transform.rotation = new Quaternion(0, 0, 0, 0);
            bullet_List.Add(bullet);
            yield return new WaitForSeconds(0.16f / num);
        }
        yield return new WaitForSeconds(0.3f);

        //回転と加速
        float angle = 0, speed = 0;
        for (int i = 0; i < num; i++) {
            if (bullet_List[i].activeSelf == false)
                continue;
            angle = Random.Range(-180f, 180f);
            bullet_List[i].transform.Rotate(new Vector3(0, 0, angle));
            speed = Random.Range(0.6f, 1.5f);
            bullet_List[i].GetComponent<Rigidbody2D>().velocity = bullet_List[i].transform.right * speed;
        }
        _accelerator.Accelerat_Bullet(bullet_List, 1.05f, 1.5f);
        UsualSoundManager.Instance.Play_Shoot_Sound();
        nemuno_Controller.Play_Burst_Effect();
    }


    //ナイフ弾開始
    public void Start_Knife_Shoot() {
        StartCoroutine("Knife_Shoot_Cor");
    }

    private IEnumerator Knife_Shoot_Cor() {
        ShootSystem[] _shoots = knife_Shoot_Obj.GetComponents<ShootSystem>();
        _shoots[1].Shoot();
        yield return new WaitForSeconds(0.4f);
        _shoots[0].Shoot();
    }

    //ナイフ弾終了
    public void Stop_Knife_Shoot() {
        ShootSystem[] _shoots = knife_Shoot_Obj.GetComponents<ShootSystem>();
        _shoots[0].Stop_Shoot();
        _shoots[1].Stop_Shoot();        
    }



    //フェーズ２移行時のブロック弾
    public IEnumerator Play_Square_Blocks_Attack() {
        MoveMotion[] moves = Phase2_Square_Blocks.GetComponentsInChildren<MoveMotion>();
        for(int i = 0; i < moves.Length; i++) {
            moves[i].Start_Move();
            yield return new WaitForSeconds(1.5f);
        }
    }


    //フェーズ２クナイ弾開始
    public void Start_Kunai_Shoot() {
        ShootSystem[] shoots = phase2_Kunai_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        for(int i = 0; i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }

    public void Stop_Kunai_Shoot() {
        ShootSystem[] shoots = phase2_Kunai_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].Stop_Shoot();
        }
    }

    

}
