using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yuka : BossEnemy {

    //コンポーネント    
    private MoveTwoPoints _move_Two_Points;

    //弾幕用オブジェクト
    [SerializeField] private GameObject cross_Shoot_Obj;
    [SerializeField] private GameObject spiral_Shoot_Obj;
    [SerializeField] private GameObject diffusion_Shoot_Obj;
    [SerializeField] private GameObject flower_Bullet;


    // Use this for initialization
    void Start() {
        //取得
        _move_Two_Points = GetComponent<MoveTwoPoints>();
        //ボス戦開始前無敵化
        Set_Is_Invincible(true);
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(flower_Bullet, 4);
    }   


    //戦闘開始
    public override void Start_Battle() {
        base.Start_Battle();
        //ボス敵にする
        Set_Is_Invincible(false);
        gameObject.tag = "EnemyTag";        
        //攻撃開始
        StartCoroutine("Attack_Cor");
    }


    //クリア後の処理
    protected override void Do_After_Clear_Process() {
        base.Do_After_Clear_Process();
        StartCoroutine("After_Clear_Cor");
    }
    
    private IEnumerator After_Clear_Cor() {        
        //攻撃中止
        StopCoroutine("Attack_Cor");
        Stop_Charge_Effect();
        //ムービー
        YukaMovie.Instance.Start_Clear_Movie();
        //移動
        _move_Two_Points.Start_Move(new Vector3(transform.position.x, -110f), 0);
        yield return new WaitUntil(_move_Two_Points.End_Move);
        GetComponent<Animator>().SetBool("AttackBool", false);
        //アイテム
        transform.GetChild(0).gameObject.SetActive(true);       
    }


    //===================================================攻撃========================================================
    #region Attack
    //攻撃
    private IEnumerator Attack_Cor() {
        GetComponent<Animator>().SetBool("AttackBool", true);
        _move_Two_Points.Start_Move(new Vector3(transform.position.x + 96f, -32f));
        yield return new WaitUntil(_move_Two_Points.End_Move);
        
        int loop_Count = 0;
        while (true) {
            //チャージ
            Play_Charge_Effect();
            yield return new WaitForSeconds(1.5f);
            Stop_Charge_Effect();            

            if (loop_Count % 2 == 0) {
                //花形弾幕と交差弾
                Play_Burst_Effect();
                Shoot_Cross_Bullet(new Vector2(0, 0));
                yield return new WaitForSeconds(1.0f);
                Shoot_Cross_Bullet(new Vector2(0, 0));
                Shoot_Diffusion_Bullet(new Vector2(0, 0), 0);                              
                yield return new WaitForSeconds(8.0f);               
            }
            else {
                //渦巻き弾
                Play_Burst_Effect();
                Start_Spiral_Shoot();
                yield return new WaitForSeconds(7.0f);
            }

            //降りる
            _move_Two_Points.Start_Move(new Vector3(transform.position.x, -80f));
            yield return new WaitForSeconds(1.0f);
            
            //花落とし
            for(int i = 0; i < 1; i++) {
                Drop_Flower_Bullet(240f - i * 120f);
                yield return new WaitForSeconds(1.0f);
            }            

            //花落とし続き
            for (int i = 1; i < 5; i++) {
                Drop_Flower_Bullet(240f - i * 120f);
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(1.0f);            
            for (int i = 0; i < 5; i++) {
                Drop_Flower_Bullet(-180f + i * 120f);
                yield return new WaitForSeconds(1.0f);
            }
            yield return new WaitForSeconds(2.5f);

            _move_Two_Points.Start_Move(new Vector3(transform.position.x, -32f));            
            loop_Count++;
        }
    }   


    //交差弾発射
    private void Shoot_Cross_Bullet(Vector2 offset) {
        UsualSoundManager.Instance.Play_Shoot_Sound();
        ShootSystem[] shoots = cross_Shoot_Obj.GetComponents<ShootSystem>();        
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].offset = offset;
            shoots[i].Shoot();
        }
        Play_Burst_Effect();
    }

    //渦巻き弾発射開始
    private void Start_Spiral_Shoot() {
        UsualSoundManager.Instance.Play_Shoot_Sound();
        ShootSystem[] shoots = spiral_Shoot_Obj.GetComponents<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
        Play_Burst_Effect();
    }

    //全方位弾発射
    private void Shoot_Diffusion_Bullet(Vector2 offset, int index) {
        UsualSoundManager.Instance.Play_Shoot_Sound();
        var child = diffusion_Shoot_Obj.transform.GetChild(index).gameObject;
        ShootSystem[] shoots = child.GetComponents<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].offset = offset;
            shoots[i].Shoot();
        }
        Play_Burst_Effect();
    }

    //花弾を落とす
    private void Drop_Flower_Bullet(float offset_X) {
        GameObject camera = GameObject.FindWithTag("MainCamera");
        var bullet = ObjectPoolManager.Instance.Get_Pool(flower_Bullet).GetObject();        
        bullet.transform.position = new Vector3(camera.transform.position.x + offset_X, 180f, 0);
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -80f);
        bullet.GetComponent<Bullet>().Set_Inactive(5.0f);

        UsualSoundManager.Instance.Play_Shoot_Sound();
    }

    #endregion

    //================================================エフェクト=======================================================
    #region Effect

    //溜め
    private void Play_Charge_Effect() {
        transform.GetChild(5).gameObject.SetActive(true);
    }

    private void Stop_Charge_Effect() {
        transform.GetChild(5).gameObject.SetActive(false);
    }

    //小ためエフェクト
    private void Play_Small_Charge_Effect() {        
        transform.GetChild(7).GetComponent<ParticleSystem>().Play();
    }

    //放出
    private void Play_Burst_Effect() {
        transform.GetChild(6).GetComponent<ParticleSystem>().Play();
    }

    #endregion
}
