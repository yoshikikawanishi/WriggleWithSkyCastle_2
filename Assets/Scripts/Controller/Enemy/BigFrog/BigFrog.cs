 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFrog : BossEnemy {

    [SerializeField] private ShootSystem bubble_Shoot_Left;
    [SerializeField] private ShootSystem bubble_Shoot_Right;
    [SerializeField] private ShootSystem sleeping_Bubble_Shoot;
    [Space]
    [SerializeField] private GameObject collection_Box;

    private Animator _anim;
    private GameObject player;    


	// Use this for initialization
	void Start () {
        //アイテム取得済みの場合消す
        if (CollectionManager.Instance.Is_Collected("BigFrog")) {
            Destroy(gameObject);
        }
        //取得
        _anim = GetComponent<Animator>();
        player = GameObject.FindWithTag("PlayerTag");        
	}
	
	
    //戦闘開始
    public override void Start_Battle() {
        base.Start_Battle();
        gameObject.layer = LayerMask.NameToLayer("EnemyLayer"); //敵判定
        sleeping_Bubble_Shoot.Stop_Shoot();                     //泡止める
        _anim.SetTrigger("GetUpTrigger");                       //起きる
        StartCoroutine("Attack_Cor");                           //戦闘開始
    }


    //撃破時の処理
    protected override void Clear() {
        base.Clear();
        Stop_Battle();
        BigFrogMovie.Instance.Start_Clear_Movie();
    }


    //攻撃の停止
    private void Stop_Battle() {
        StopCoroutine("Attack_Cor");
        StopCoroutine("Bubble_Shoot_Cor");
        _anim.SetTrigger("SleepTrigger");
        collection_Box.SetActive(true);
    }


    private IEnumerator Attack_Cor() {
        while (true) {
            Roar();
            yield return new WaitForSeconds(2.5f);
            StartCoroutine("Blink_Cor");
            StartCoroutine("Bubble_Shoot_Cor");
            yield return new WaitForSeconds(5.0f);
        }
    }


    //バブルショット用
    private IEnumerator Bubble_Shoot_Cor() {
        if (player == null)
            yield break;

        int num = 15;
        float span = 0.1f;
        
        //ショットの向き
        int direction = (transform.position.x - player.transform.position.x).CompareTo(0);
        if (direction == 0) { direction = 1; }
        transform.localScale = new Vector3(direction, 1, 1);
        ShootSystem shoot = (direction > 0) ? bubble_Shoot_Left : bubble_Shoot_Right;

        yield return new WaitForSeconds(1.0f);

        //ショット
        _anim.SetBool("OpenMouseBool", true);
        for(int i = 0; i < num; i++) {            
            shoot.max_Speed = Random.Range(15f, 130f);
            shoot.Shoot();
            yield return new WaitForSeconds(span - Time.deltaTime);
        }
        _anim.SetBool("OpenMouseBool", false);
    }

    //咆哮用
    private void Roar() {
        GetComponent<Roaring>().Roar(160f, 2.5f, 1000f);
    }

    //攻撃前点滅
    private IEnumerator Blink_Cor() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        for (int i = 0; i < 3; i++) {
            _sprite.color = new Color(0.7f, 0.7f, 0.7f);
            yield return new WaitForSeconds(0.1f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
