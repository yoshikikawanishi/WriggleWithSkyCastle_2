using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionBox : MonoBehaviour {

    [SerializeField] private Texture2D option_Box_Texture;

    private enum Kind {
        bee = 2,
        butterfly = 1,
        mantis = 0,
        spider = 3,
        random = 4,
    }
    [SerializeField] private Kind kind;

    private List<string> open_Tags = new List<string> {
        "PlayerAttackTag",
        "PlayerChargeAttackTag",
        "PlayerBulletTag",
    };   

    private bool is_Opened = false;


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        //箱を開ける判定
        foreach (string tag in open_Tags) {
            if(collision.tag == tag && !is_Opened) {
                StartCoroutine("Open_Cor");
                is_Opened = true;
                Destroy(gameObject, 5.0f);
            }
        }        
    }



    //箱を開ける
    private IEnumerator Open_Cor() {      
        //効果音
        GetComponent<AudioSource>().Play();
        //画像をスライスして、アニメーション再生する
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        TextureSlicer tex_Slice = new TextureSlicer();
        Sprite[] sprites = tex_Slice.Slice_Sprite(option_Box_Texture, new Vector2Int(35, 26));
        int start_Index = 5 * (int)kind;
        for(int i = start_Index; i < start_Index + 5; i++) {
            _sprite.sprite = sprites[i];
            yield return new WaitForSeconds(0.08f);
        }
        //オプションを出す
        if (kind == Kind.random) {
            PlayerManager.Option po = new PlayerManager.Option();
            do {
                int r = Random.Range(0, 4);
                switch (r) {
                    case 0: po = PlayerManager.Option.bee; break;
                    case 1: po = PlayerManager.Option.butterfly; break;
                    case 2: po = PlayerManager.Option.mantis; break;
                    case 3: po = PlayerManager.Option.spider; break;
                }
            } while (po == PlayerManager.Instance.Get_Option());
            transform.GetChild(0).GetComponent<OptionItem>().option = po;
        }
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).SetParent(null);        
    }

}
