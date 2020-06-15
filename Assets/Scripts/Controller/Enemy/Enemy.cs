using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の体力を管理
/// 敵の被弾時、消滅時の処理を行う、継承で処理変更
/// </summary>
public class Enemy : MonoBehaviour {
    
    [SerializeField] private bool is_Pooled = false;
    [SerializeField] public bool is_One_Life = false;    
    [SerializeField] private bool shake_Camera_In_Vanish = false;
    [Space]
    [SerializeField] private int life = 5;
    [SerializeField] private int power_Value = 0;
    [SerializeField] private int score_Value = 0;
    [SerializeField] private float drop_Life_Probability = 1;
    [SerializeField] private float drop_Option_Box_Probability = 0;

    private SpriteRenderer _sprite;
    private Color default_Color, damaged_Color;    
    private SpiderFootingEnemy spider_Footing_Enemy;
    private PoisonedEnemy poisoned_Enemy;

    private CameraShake camera_Shake;

    private bool is_Exist = true;
    private int default_Life;

    private GameObject vanish_Shoot_If_Collected_Hina;


	// Use this for initialization
	void Awake () {
        //取得
        _sprite = GetComponent<SpriteRenderer>();
        default_Color = _sprite.color;
        damaged_Color = default_Color * new Color(1.0f, 0.7f, 0.7f, 1.0f);
        default_Life = life;
        spider_Footing_Enemy = gameObject.AddComponent<SpiderFootingEnemy>();
        poisoned_Enemy = gameObject.AddComponent<PoisonedEnemy>();
        vanish_Shoot_If_Collected_Hina = Resources.Load("Effect/EnemyVanishShoot") as GameObject;
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
    }


    private void OnEnable() {
        if (is_Pooled) {
            _sprite.color = default_Color;
            life = default_Life;
            is_Exist = true;
        }
    }


    //被弾時の処理
    public virtual void Damaged(int damage, string attacked_Tag) {
        life -= damage;
        //毒ダメージ発生
        if(attacked_Tag == "PlayerAttackTag" && PlayerManager.Instance.Get_Option() == PlayerManager.Option.spider) {
            poisoned_Enemy.Start_Poisoned_Damaged(false);
        }
        //消滅
        if (life <= 0 && is_Exist) {
            Vanish_Action(attacked_Tag);
            is_Exist = false;
            return;
        }
        //被弾時の点滅
        if (is_Exist) {
            StartCoroutine("Blink");
        }
    }


    //消滅前のアクション
    // EnemyCollisionDetectionのDamagedで変更
    private void Vanish_Action(string attacked_Tag) {
        //雛の収集アイテムを集めていれば弾を出す
        if (CollectionManager.Instance.Is_Collected("Hina")) {
            GameObject shoot = ObjectPoolManager.Instance.Get_Pool(vanish_Shoot_If_Collected_Hina).GetObject();
            shoot.transform.position = transform.position;
            ObjectPoolManager.Instance.Set_Inactive(shoot, 1.0f);
        }
        //蜘蛛にやられたとき蜘蛛の巣を張る
        if((attacked_Tag == "PlayerAttackTag" && PlayerManager.Instance.Get_Option() == PlayerManager.Option.spider) || attacked_Tag == "Poison") {
            spider_Footing_Enemy.Generate_Footing_Vanish();
        }        
        else {
            Vanish();
        }
    }


    //消滅時の処理
    public virtual void Vanish() {
        Play_Vanish_Effect();
        Put_Out_Item();
        StopAllCoroutines();

        if (camera_Shake != null && shake_Camera_In_Vanish) {
            camera_Shake.Shake(0.25f, new Vector2(0.8f, 1f), false);            
        }

        if (is_Pooled || is_One_Life) {            
            gameObject.SetActive(false);
            return;
        }        
        Destroy(gameObject);
    }


    //消滅時のエフェクト
    public virtual void Play_Vanish_Effect() {        
        GameObject effect_Prefab = Resources.Load("Effect/EnemyVanishEffect") as GameObject;
        var effect = Instantiate(effect_Prefab);
        effect.transform.position = transform.position;
        Destroy(effect, 1.5f);
    }


    //アイテムの放出
    protected void Put_Out_Item() {
        gameObject.AddComponent<PutOutSmallItems>().Put_Out_Item(power_Value, score_Value);
        
        //回復アイテム
        if (Random.Range(1, 100) <= drop_Life_Probability) {
            ObjectPoolManager.Instance.Create_New_Pool(Resources.Load("Object/LifeUpItem") as GameObject, 2);
            var life_Item = ObjectPoolManager.Instance.Get_Pool("LifeUpItem").GetObject();
            life_Item.transform.position = transform.position;
        }
        //オプションボックス
        if (Random.Range(1, 100) <= drop_Option_Box_Probability) {
            //オプションの決定
            string kind = "";
            while (true) {
                int r = Random.Range(0, 4);                
                switch (r) {                    
                    case 0: kind = "Bee"; break;
                    case 1: kind = "Butterfly"; break;
                    case 2: kind = "Mantis"; break;
                    case 3: kind = "Spider"; break;
                }                
                if (kind.ToLower() != PlayerManager.Instance.Get_Option().ToString())
                    break;
            }
            //生成
            var box = Instantiate(Resources.Load("Object/OptionBox" + kind) as GameObject);
            box.transform.position = transform.position + new Vector3(0, 8f);
        }
    }

    //点滅
    private IEnumerator Blink() {
        _sprite.color = damaged_Color;
        yield return new WaitForSeconds(0.1f);
        //色が点滅中に変わっていないことを確認
        if (_sprite.color == damaged_Color)        
            _sprite.color = default_Color;
    }	


    public void Set_Life(int life) {
        if (life > 0)
            this.life = life;
    }

}
