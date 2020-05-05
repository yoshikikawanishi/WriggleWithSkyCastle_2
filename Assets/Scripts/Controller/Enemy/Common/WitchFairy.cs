using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WitchFairyBattleMovie))]
public class WitchFairy : FairyEnemy {

    private WitchFairyBattleMovie _movie;
    private ChildColliderTrigger search_Light_Collider;
    private GameObject player;

    private bool is_Searching = true;


    void Start() {
        _movie = GetComponent<WitchFairyBattleMovie>();
        search_Light_Collider = GetComponentInChildren<ChildColliderTrigger>();
        player = GameObject.FindWithTag("PlayerTag");
    }

	
	//ライトが当たると戦闘開始
	void Update () {
        if (search_Light_Collider.Hit_Trigger() && is_Searching) {
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
        Destroy(search_Light_Collider.gameObject);
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
