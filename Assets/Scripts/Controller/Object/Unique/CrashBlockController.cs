using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashBlockController : MonoBehaviour {

    [SerializeField] private bool is_Pooled = false;

    [SerializeField]
    private List<string> destroyer_Tag_List = new List<string> {
        "PlayerBulletTag",        
        "PlayerAttackTag",
        "PlayerChargeAttackTag",        
        "PlayerKickTag",
    };

    [SerializeField]
    private List<string> repel_Tag_List = new List<string> {

    };
    [Space]
    [SerializeField] private int life = 1;
    [Space]
    [SerializeField] private Sprite damaged_Sprite;

    private Sprite default_Sprite;
    private int default_Life;
    private Color default_Color;

    //Awake
    private void Awake() {
        default_Sprite = GetComponent<SpriteRenderer>().sprite;
        default_Life = life;
        default_Color = GetComponent<SpriteRenderer>().color;
    }


    //OnEnable
    private void OnEnable() {
        GetComponent<SpriteRenderer>().sprite = default_Sprite;
        life = default_Life;
    }


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        foreach(string tag in destroyer_Tag_List) {
            if(collision.tag == tag) {
                Damaged();
            }
        }    
        foreach(string tag in repel_Tag_List) {
            if(collision.tag == tag) {
                Play_Repel_Effect();
            }
        }
    }

    //OnCollisionEnter
    private void OnCollisionEnter2D(Collision2D collision) {
        foreach (string tag in destroyer_Tag_List) {
            if (collision.gameObject.tag == tag) {
                Damaged();
            }
        }
        foreach (string tag in repel_Tag_List) {
            if (collision.gameObject.tag == tag) {
                Play_Repel_Effect();
            }
        }
    }


    //攻撃を受けた時の処理
    private void Damaged() {
        life--;
        if (life > 0) {
            StartCoroutine("Shake_Cor");
            GetComponent<SpriteRenderer>().sprite = damaged_Sprite;
        }
        else if(life == 0) {
            Play_Effect();
            if (is_Pooled)
                gameObject.SetActive(false);
            else
                Destroy(gameObject);
        }
    }


    //揺れる
    private IEnumerator Shake_Cor() {
        Vector2 default_Pos = transform.position;
        for (float t = 0; t < 0.25f; t += 0.016f) {
            transform.position = default_Pos + new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * Time.timeScale;
            yield return null;
        }
        transform.position = default_Pos;
    }


    //消滅エフェクト
    private void Play_Effect() {
        if (transform.childCount == 0) {
            return;
        }
        GameObject effect = transform.GetChild(0).gameObject;
        if (is_Pooled) {
            effect = Instantiate(effect);            
        }
        effect.transform.position = transform.position;
        effect.transform.SetParent(null);
        effect.SetActive(true);
        Destroy(effect, 1.0f);
        
    }


    //無敵エフェクト
    private void Play_Repel_Effect() {
        StartCoroutine("Repel_Effect_Cor");
    }

    private IEnumerator Repel_Effect_Cor() {        
        GetComponent<SpriteRenderer>().color = default_Color + new Color(0.2f, 0.2f, 0.2f);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = default_Color;
    }


    //プログラムから壊す
    public void Crash() {
        Play_Effect();
        if (is_Pooled)
            gameObject.SetActive(false);
        else
            Destroy(gameObject);
    }

}
