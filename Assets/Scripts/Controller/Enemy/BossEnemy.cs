﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : MonoBehaviour {

    //コンポーネント
    private PutOutSmallItems _put_Out_Item;
    private SpriteRenderer _sprite;
    private CameraShake _camera_Shake;
    private PoisonedEnemy poisoned_Enemy;

    //体力
    public List<int> life = new List<int>();
    [Space]
    //パワースコア
    public int power_Value = 0;
    public int score_Value = 0;
    [Space]
    //エフェクト
    [SerializeField] private GameObject phase_Change_Bomb_Prefab;
    [SerializeField] private GameObject clear_Converge_Effect_Prefab;
    [SerializeField] private GameObject clear_Effect_Prefab;

    //体力
    private List<int> DEFAULT_LIFE = new List<int>();
    //現在のフェーズ
    private int now_Phase = 1;     
    //無敵化
    private bool is_Invincible = false;
    //最後に被弾した時間
    private float last_Damaged_Time;
    //フェーズ中に自機が被弾したかどうか
    private int player_Life;
    private bool is_Player_No_Damaged = true;

    public enum State {
        idle,
        battle,
        cleared,
    }
    [HideInInspector] public State state = State.idle;


    protected void Awake() {
        //取得
        _put_Out_Item   = gameObject.AddComponent<PutOutSmallItems>();
        _sprite         = GetComponent<SpriteRenderer>();
        _camera_Shake   = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        poisoned_Enemy  = gameObject.AddComponent<PoisonedEnemy>();
        //初期値代入
        DEFAULT_LIFE = new List<int>(life);
        player_Life = PlayerManager.Instance.Get_Life();
    }


    protected void Update() {        
        Observe_Player_Life();
    }

    //========================戦闘開始、フェーズ切り替え時、クリア時の処理　：　行いたい処理を継承して追加すること====================

    //戦闘開始時の処理
    public virtual void Start_Battle() {        
        state = State.battle;
    }


    //フェーズ切り替え時の処理    
    protected virtual void Change_Phase() {
        var effect = Instantiate(phase_Change_Bomb_Prefab);
        effect.transform.position = transform.position;

        _put_Out_Item.Put_Out_Item(power_Value, score_Value);
        if (is_Player_No_Damaged) {
            _put_Out_Item.Put_Out_Stock_Up_Item();
        }
        is_Player_No_Damaged = true;

        Set_Now_Phase(now_Phase + 1);
    }


    //クリア時の処理
    protected virtual void Clear() {
        if (state == State.cleared)
            return;
        state = State.cleared;
        StartCoroutine("Clear_Cor");
    }


    //クリア後の処理
    protected virtual void Do_After_Clear_Process() {

    }


    //被弾時のエフェクト
    protected virtual void Play_Damaged_Effect(string damaged_Tag) {
        StartCoroutine("Blink", new Color(0.5f, 0.25f, 0.25f));
        //TODO: 被弾時のエフェクト
        if (damaged_Tag == "PlayerBulletTag") {
            if (life[now_Phase - 1] <= 30)
                UsualSoundManager.Instance.Play_Enemy_Damaged_Sound();
            else
                UsualSoundManager.Instance.Play_Enemy_Damaged_Sound_Big();
        }
    }

    //=========================================================================================================================

    //被弾
    public void Damaged(int damage, string damaged_Tag) {
        if (state != State.battle) {
            return;
        }
        if (is_Invincible) {
            Play_Invincible_Effect();
            return;
        }

        //連続で被弾時ダメージ減らす
        if(Time.time - last_Damaged_Time < 0.6f) {
            damage = (int)(damage * 0.7f);
            if (damage < 1)
                damage = 1;
        }
        
        //被弾
        if(life[now_Phase - 1] > 0) {
            life[now_Phase - 1] -= damage;
            Play_Damaged_Effect(damaged_Tag);
        }

        //毒ダメージ
        if(damaged_Tag == "PlayerAttackTag" && PlayerManager.Instance.Get_Option() == PlayerManager.Option.spider) {
            poisoned_Enemy.Start_Poisoned_Damaged(true);
        }
        else {
            //毒以外でダメージ時、最後に被弾した時間を記録
            last_Damaged_Time = Time.time;
        }

        //フェーズ終了
        if(life[now_Phase - 1] <= 0) {            
            if (now_Phase < life.Count)
                Change_Phase();            
            else if (now_Phase == life.Count)
                Clear();
        }        
    }


    //無敵時のエフェクト
    private void Play_Invincible_Effect() {
        StartCoroutine("Blink", new Color(0.7f, 0.7f, 0.7f));
    }


    //点滅
    private IEnumerator Blink(Color blink_Color) {
        _sprite.color = blink_Color;        
        yield return new WaitForSeconds(0.1f);        
        if (_sprite.color == blink_Color)
            _sprite.color = new Color(0.5f, 0.5f, 0.5f);
    }


    //１フェーズ中自機が被弾したかどうか、Updateで呼ぶこと
    private void Observe_Player_Life() {
        if (!is_Player_No_Damaged)
            return;
        if(player_Life != PlayerManager.Instance.Get_Life()) {
            player_Life = PlayerManager.Instance.Get_Life();
            is_Player_No_Damaged = false;
        }
    }    

    
    //クリア時の処理
    protected IEnumerator Clear_Cor() {        
        //無敵化
        gameObject.layer = LayerMask.NameToLayer("InvincibleLayer");

        //時間ゆっくりに、エフェクト出す
        var converge_Effect = Instantiate(clear_Converge_Effect_Prefab);
        converge_Effect.transform.position = transform.position;
        BackGroundEffector.Instance.Start_Change_Color(new Color(0, 0, 0), 1f);

        Time.timeScale = 0.15f;
        yield return new WaitForSeconds(0.1f);
        Time.timeScale = 1;

        BackGroundEffector.Instance.Change_Color_Default(1f);

        //撃破エフェクト
        var effect = Instantiate(clear_Effect_Prefab);
        effect.transform.position = transform.position;
        _put_Out_Item.Put_Out_Item(power_Value * 2, score_Value * 2);

        _camera_Shake.Shake(0.8f, new Vector2(1.2f, 1.2f), true);

        Do_After_Clear_Process();        
    }


    //===============================================================================================================

    //Getter
    public int Get_Now_Phase() {
        return now_Phase;
    }

    public int Get_Default_Life(int phase) {
        return DEFAULT_LIFE[phase - 1];
    }

    //Setter
    public void Set_Now_Phase(int phase) {
        if(phase <= life.Count) {
            now_Phase = phase;
        }
    }

    public void Set_Is_Invincible(bool is_Invincible) {
        this.is_Invincible = is_Invincible;
    }    

}
