using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyCollision : MonoBehaviour {

    private PlayerController player_Controller;
    private PlayerDamaged player_Damaged;
    
    //被弾タグリスト
    private List<string> damaged_Tag_List = new List<string>() {
        "EnemyTag",
        "EnemyBulletTag",
        "DamagedGroundTag",
    };

    //1フレーム内に２回以上被弾しないようにする
    private bool is_Damaged = false;

    //デフォルトのサイズ、オフセット
    private Vector2 default_Size;
    private Vector2 default_Offset;


	// Use this for initialization
	void Awake () {
        //取得
        player_Controller = transform.parent.GetComponent<PlayerController>();
        player_Damaged = transform.parent.GetComponent<PlayerDamaged>();
        //初期値代入
        default_Size = GetComponent<CapsuleCollider2D>().size;
        default_Offset = GetComponent<CapsuleCollider2D>().offset;
    }


    private void LateUpdate() {
        //１フレーム内に２回以上被弾しないようにする
        if (is_Damaged) {
            is_Damaged = false;
        }    
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if (is_Damaged) return;

        foreach(string tag in damaged_Tag_List) {
            if(collision.tag == tag) {
                player_Damaged.StartCoroutine("Damaged");
                is_Damaged = true;
            }
        }
        //Miss時
        if(collision.tag == "MissZoneTag" && !player_Controller.Get_Is_Ride_Beetle()) {
            player_Damaged.Miss();
        }
    }


    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        if (is_Damaged) return;

        foreach (string tag in damaged_Tag_List) {
            if (collision.gameObject.tag == tag) {
                player_Damaged.StartCoroutine("Damaged");
                is_Damaged = true;
            }
        }
        //Miss時
        if (collision.gameObject.tag == "MissZoneTag" && !player_Controller.Get_Is_Ride_Beetle()) {
            player_Damaged.Miss();
        }
    }
    

    //サイズの変更
    public void Change_Collider_Size(Vector2 size, Vector2 offset) {
        GetComponent<CapsuleCollider2D>().size = size;
        GetComponent<CapsuleCollider2D>().offset = offset;
    }

    public void Change_Collider_Size(Vector2 size) {
        GetComponent<CapsuleCollider2D>().size = size;
    }

    public void Back_Default_Collider() {
        GetComponent<CapsuleCollider2D>().size = default_Size;
        GetComponent<CapsuleCollider2D>().offset = default_Offset;
    }

    //当たり判定の表示
    public void Display_Sprite() {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    public void Hide_Sprite() {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    //無敵化
    public void Become_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }

    public void Release_Invincible() {
        gameObject.layer = LayerMask.NameToLayer("PlayerLayer");
    }

    public IEnumerator Become_Invincible_Cor(float span) {
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
        yield return new WaitForSeconds(span);
        gameObject.layer = LayerMask.NameToLayer("PlayerLayer");
    }

    public bool Is_Invincible() {
        if(gameObject.layer == LayerMask.NameToLayer("InvincibleLayer")) {
            return true;
        }
        return false;
    }
}
