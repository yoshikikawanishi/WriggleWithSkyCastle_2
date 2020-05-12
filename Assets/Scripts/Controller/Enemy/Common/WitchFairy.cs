using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WitchFairyBattleMovie))]
public class WitchFairy : FairyEnemy {

    private Rigidbody2D _rigid;
    private WitchFairyBattleMovie _movie;
    private SearchLight search_Light;
    private ChildColliderTrigger side_Collider;
    private GameObject player;
    private GameObject main_Camera;

    private bool is_Searching = true;


    void Start() {
        _rigid = GetComponent<Rigidbody2D>();
        _movie = GetComponent<WitchFairyBattleMovie>();
        search_Light = transform.Find("SearchLight").GetComponent<SearchLight>();
        search_Light.gameObject.SetActive(false);
        side_Collider = transform.Find("SideCollision").GetComponent<ChildColliderTrigger>();

        player = GameObject.FindWithTag("PlayerTag");
        main_Camera = GameObject.FindWithTag("MainCamera");

        //被弾の当たり判定は子供の方で行う
        Destroy(GetComponent<EnemyCollisionDetection>());
    }

		
	void Update () {

        if (!is_Searching)
            return;

        //カメラが近付いてくるまで待機
        if (Mathf.Abs(main_Camera.transform.position.x - transform.position.x) > 270f) {
            if (search_Light.gameObject.activeSelf)
                search_Light.gameObject.SetActive(false);
            _rigid.velocity = new Vector2(0, 0);
            return;
        }

        //ライト点灯
        if (!search_Light.gameObject.activeSelf)
            search_Light.gameObject.SetActive(true);

        //歩く
        _rigid.velocity = new Vector2(-transform.localScale.x.CompareTo(0) * 40f, _rigid.velocity.y);
        //反転
        if (side_Collider.Hit_Trigger()) {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);                        
        }

        //ライトが当たると戦闘開始
        if (search_Light.Is_Detect()) {
            is_Searching = false;
            Start_Battle();
        }
        
	}


    //被弾すると戦闘開始
    public override void Damaged(int damage, string attacked_Tag) {
        base.Damaged(damage, attacked_Tag);
        if (is_Searching) {
            is_Searching = false;
            Start_Battle();
        }
    }


    //やられると戦闘終了
    public override void Vanish() {        
        if (!is_Searching) {
            _movie.Finish_Battle();
        }
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");
    }


    //戦闘開始時の処理
    private void Start_Battle() {
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if (player == null)
            return;

        _movie.Start_Battle_Movie(gameObject);
        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<RedFairy>());
        Destroy(search_Light.gameObject);
        Turn_To_Player();
        StartCoroutine("Attack_Cor");
    }


    //攻撃
    private IEnumerator Attack_Cor() {
        GetComponent<Animator>().SetBool("AttackBool", true);
        ShootSystem[] shoots = GetComponentsInChildren<ShootSystem>();
        yield return new WaitForSeconds(3.5f);

        _movie.Display_Message(2, 2);
        int player_Life = PlayerManager.Instance.Get_Life();

        float center_Angle = 0;
        for(int i = 0; i < 2; i++) {
            Turn_To_Player();
            center_Angle += Random.Range(10, 50);
            shoots[0].center_Angle_Deg = center_Angle - 5f;
            shoots[1].center_Angle_Deg = center_Angle + 5f;
            shoots[0].Shoot();
            shoots[1].Shoot();            
            yield return new WaitForSeconds(2.9f);
        }

        if(PlayerManager.Instance.Get_Life() == player_Life)
            _movie.Display_Message(3, 3);
        StartCoroutine("Attack_Cor");
    }


    //自機の方向を向く
    private void Turn_To_Player() {
        if (player == null)
            return;
        int direction = (player.transform.position.x - transform.position.x).CompareTo(0);
        if (direction == 0)
            direction = 1;
        float size = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(-direction, 1, 1) * size;
    }
}
