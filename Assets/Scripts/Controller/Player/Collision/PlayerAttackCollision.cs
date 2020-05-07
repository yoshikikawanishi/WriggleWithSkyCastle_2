using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollision : MonoBehaviour {

    private bool is_Hit_Attack = false;    
    private Vector2 offset;
    private BoxCollider2D _collider;
    private GameObject player;

    private readonly string HIT_EFFECT_NAME = "HitEffect";

    private List<string> hit_Attack_Tag_List = new List<string> {
        "EnemyTag",
        "SandbackTag",
        "SandbackGroundTag"
    };


    private void Awake() {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        player = transform.parent.gameObject;
        //ヒットエフェクトのオブジェクトプール
        GameObject effect = Resources.Load("Effect/" + HIT_EFFECT_NAME) as GameObject;
        ObjectPoolManager.Instance.Create_New_Pool(effect, 2);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        foreach (string tag in hit_Attack_Tag_List) {
            if (collision.tag == tag && !is_Hit_Attack) {                
                is_Hit_Attack = true;
                Play_Hit_Effect(collision.transform.position);
            }
        }
    }

    //あたったときtrue
    public bool Hit_Trigger() {
        if (is_Hit_Attack) {
            is_Hit_Attack = false;
            return true;
        }
        return false;
    }
    

    private void Play_Hit_Effect(Vector2 pos) {
        GameObject effect = ObjectPoolManager.Instance.Get_Pool(HIT_EFFECT_NAME).GetObject();
        offset = transform.position + new Vector3(player.transform.localScale.x * transform.localScale.x * 20, 0);
        effect.transform.position = offset + (pos - offset) / 10;
        ObjectPoolManager.Instance.Set_Inactive(effect, 1.5f);
    }


    //攻撃の生成
    public void Make_Collider_Appear(float lifeTime) {        
        is_Hit_Attack = false;        
        GetComponent<BoxCollider2D>().enabled = true;
        Change_Size();        
        Play_Animation();
        Invoke("Make_Collider_Disappear", lifeTime);
    }


    public void Make_Collider_Disappear() {
        GetComponent<BoxCollider2D>().enabled = false;
    }


    //オプションによるサイズの変更
    private void Change_Size() {        
        switch (PlayerManager.Instance.Get_Option()) {
            case PlayerManager.Option.none:         Set_Size(1.0f, new Vector2(10, 0)); break;
            case PlayerManager.Option.bee:          Set_Size(1.0f, new Vector2(10, 0));  break;
            case PlayerManager.Option.butterfly:    Set_Size(1.2f, new Vector2(11, 0)); break;
            case PlayerManager.Option.mantis:       Set_Size(1.5f, new Vector2(14, 0)); break;
            case PlayerManager.Option.spider:       Set_Size(1.0f, new Vector2(10, 0)); break;
        }
    }

    private void Set_Size(float scale, Vector2 position) {
        int direction = transform.localScale.x.CompareTo(0);
        transform.localScale = new Vector3(direction, 1, 1) * scale;
        transform.localPosition = position;
        //のビールアーム入手後範囲広げる
        if (CollectionManager.Instance.Is_Collected("Nitori")) {
            transform.localScale *= 1.2f;
        }
    }


    //オプションとパワーによるアニメーションの変更
    private void Play_Animation() {
        int power = PlayerManager.Instance.Get_Power();
        PlayerManager.Option option = PlayerManager.Instance.Get_Option();

        if (option == PlayerManager.Option.none || option == PlayerManager.Option.mantis) {
            if (power < 32) {
                GetComponent<Animator>().SetTrigger("AttackTrigger");
            }
            else if (power < 64) {
                GetComponent<Animator>().SetTrigger("AttackTrigger2");
            }
            else {
                GetComponent<Animator>().SetTrigger("AttackTrigger3");
            }
        }
        else if (option == PlayerManager.Option.spider) {
            if (power < 32) {
                GetComponent<Animator>().SetTrigger("BlueAttackTrigger");
            }
            else if (power < 64) {
                GetComponent<Animator>().SetTrigger("BlueAttackTrigger2");
            }
            else {
                GetComponent<Animator>().SetTrigger("BlueAttackTrigger3");
            }
        }
        else {
            if (power < 32) {
                GetComponent<Animator>().SetTrigger("YellowAttackTrigger");
            }
            else if (power < 64) {
                GetComponent<Animator>().SetTrigger("YellowAttackTrigger2");
            }
            else {
                GetComponent<Animator>().SetTrigger("YellowAttackTrigger3");
            }
        }
    }

   
    private void Set_Tag(string tag) {
        gameObject.tag = tag;
    }


    /// <summary>
    /// 当たり判定の範囲を返す
    /// </summary>
    /// <returns>長さ２のVector2配列、要素0 : 左下の座標、要素1 : 右上の座標</returns>
    public Vector2[] Get_Collision_Range() {        
        Vector2 center = (Vector2)transform.position + _collider.offset * player.transform.localScale.x;
        Vector2 left_Bottom = center - new Vector2(_collider.size.x, _collider.size.y) * (Vector2)transform.localScale / 2;
        Vector2 right_Top = center + new Vector2(_collider.size.x, _collider.size.y) * (Vector2)transform.localScale / 2;        
        return new Vector2[2] { left_Bottom, right_Top };
    }
}
