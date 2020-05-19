using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField] private GameObject mantis_Attack_Bullet;

    //コンポーネント
    private PlayerController _controller;    
    private PlayerEffect player_Effect;
    private PlayerSoundEffect player_SE;
    private Animator _anim;
    private Rigidbody2D _rigid;
    private PlayerAttackCollision attack_Collision;    
    private PlayerManager player_Manager;    

    private GameObject spider_Footing;         


    // Use this for initialization
    void Awake () {
        //取得
        _controller = GetComponent<PlayerController>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();        
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        attack_Collision = GetComponentInChildren<PlayerAttackCollision>();        
        player_Manager = PlayerManager.Instance;        
        spider_Footing = Resources.Load("Object/SpiderFooting") as GameObject;
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(mantis_Attack_Bullet, 5);
    }
    

    //攻撃用フィールド変数
    private float attack_Time = 0.18f;
    private float attack_Span = 0.17f;
    private bool knock_Back = true;


    private void Set_Attack_Status(float attack_Time, float attack_Span, bool knock_Back) {
        this.attack_Time = attack_Time;
        this.attack_Span = attack_Span;
        this.knock_Back = knock_Back;
    }


    //攻撃
    public void Attack() {
        if (_controller.can_Attack) {
            //オプションによって変える
            switch (player_Manager.Get_Option()) {
                case PlayerManager.Option.none:     Set_Attack_Status(0.18f, 0.17f, true); break;
                case PlayerManager.Option.bee:      Set_Attack_Status(0.18f, 0.01f, true); break;
                case PlayerManager.Option.butterfly: Set_Attack_Status(0.18f, 0.13f, false); break;
                case PlayerManager.Option.mantis:   Set_Attack_Status(0.24f, 0.17f, true); break;
                case PlayerManager.Option.spider:   Set_Attack_Status(0.18f, 0.17f, true); break;
            }
            StartCoroutine("Attack_Cor");
        }
    }


    //攻撃用コルーチン
    public IEnumerator Attack_Cor() {
        _controller.can_Attack = false;       
        _anim.SetTrigger("AttackTrigger");
        attack_Collision.Make_Collider_Appear(attack_Time);        
        player_SE.Play_Attack_Sound();

        switch (player_Manager.Get_Option()) {
            case PlayerManager.Option.bee:      StartCoroutine("Bee_Shoot_Cor"); break;
            case PlayerManager.Option.mantis:   Mantis_Shoot(); break;
            case PlayerManager.Option.butterfly: _rigid.velocity = new Vector2(_rigid.velocity.x, 200f); break;
            case PlayerManager.Option.spider:   Gen_Spider_Footing(); break;
        }      
        
        _rigid.velocity += new Vector2(transform.localScale.x * 5f, 0); //Rigidbodyのスリープ状態を解除する        
        for (float t = 0; t < attack_Time; t += Time.deltaTime) {
            //敵と衝突時
            if (attack_Collision.Hit_Trigger()) {                
                StartCoroutine("Do_Hit_Attack_Process");
                yield return new WaitForSeconds(0.05f);                
                break;
            }
            yield return null;
        }

        yield return new WaitForSeconds(attack_Span);
        _controller.can_Attack = true;
    }


    //オプションが蜂の時のショット    
    private IEnumerator Bee_Shoot_Cor() {
        ObjectPool bullet_Pool = ObjectPoolManager.Instance.Get_Pool("PlayerBeeBullet");
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                var bullet = bullet_Pool.GetObject();
                bullet.transform.position = transform.position + new Vector3(0, -6f + j * 6f);
                bullet.transform.localScale = transform.localScale;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(900f * transform.localScale.x, 0);
            }
            player_SE.Play_Shoot_Sound();
            yield return new WaitForSeconds(0.1f);
        }
    }


    //オプションがカマキリの時ショット
    private void Mantis_Shoot() {
        ObjectPool bullet_Pool = ObjectPoolManager.Instance.Get_Pool("PlayerMantisAttackBullet");
        float angle;
        Vector3 pos;
        for(int i = 0; i < 15; i++) {
            var bullet = bullet_Pool.GetObject();
            angle = Random.Range(-30f, 70f) * Mathf.Deg2Rad;
            pos = new Vector2(Mathf.Cos(angle) * transform.localScale.x, Mathf.Sin(angle) * 0.5f) * 32f;
            bullet.transform.position = transform.position + pos;               
            bullet.GetComponent<Rigidbody2D>().velocity = (bullet.transform.position - transform.position) * Random.Range(15f, 90f);
            bullet.GetComponent<Rigidbody2D>().velocity += _rigid.velocity;
            bullet.GetComponent<Bullet>().Set_Inactive(5.0f);
        }
        player_SE.Play_Shoot_Sound();
    }


    //オプションが蜘蛛の時足場生成
    private void Gen_Spider_Footing() {
        var footing = ObjectPoolManager.Instance.Get_Pool(spider_Footing).GetObject();
        footing.transform.position = transform.position + new Vector3(transform.localScale.x * 40f, -32f);
        ObjectPoolManager.Instance.Set_Inactive(footing, 10f);
    }

    
    //敵と衝突時の処理
    private IEnumerator Do_Hit_Attack_Process() {
        if (knock_Back) {
            float force = _controller.is_Landing ? 170f : 30f;              //ノックバック
            _rigid.velocity = new Vector2(force * -transform.localScale.x, 10f);
        }        
        player_SE.Play_Hit_Attack_Sound();                                  //効果音                                                               
        float tmp = Time.timeScale;                                         //ヒットストップ        
        Time.timeScale = 0.4f;
        yield return new WaitForSeconds(0.06f);
        Time.timeScale = tmp;
    }
   
}
