using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kisume : TalkCharacter {

    private enum State {
        idle,
        dropping,
        landing,
        with_Yamame,
    }
    private State state = State.idle;

    [SerializeField] private CollectionBox collection_Box;

    private Animator _anim;
    private Rigidbody2D _rigid;
    private ChildColliderTrigger foot_Collision;
    private GameObject landing_Effect;
    private CameraShake camera_Shake;

    private float default_Height;


    new void Start() {
        base.Start();
        //取得
        _anim = GetComponent<Animator>();
        _rigid = GetComponent<Rigidbody2D>();
        foot_Collision = transform.Find("Foot").GetComponent<ChildColliderTrigger>();
        landing_Effect = transform.Find("LandingEffect").gameObject;
        camera_Shake = GameObject.FindWithTag("MainCamera").GetComponent<CameraShake>();
        default_Height = transform.position.y;

        //ヤマメと一緒になる
        if(CollectionManager.Instance.Is_Collected("Kisume") && CollectionManager.Instance.Is_Collected("Yamame")) {
            Change_Status_With_Yamame();
        }
    }
    

    protected override float Action_Before_Talk() {
        //会話前に落下開始
        if (state == State.idle) {
            state = State.dropping;
            _rigid.gravityScale = 80f;
            PlayerMovieFunction.Instance.Disable_Controlle_Player();
            return 1.5f;
        }
        else if(state == State.landing) {
            //会話内容変更
            Change_Message_Status("KisumeText", 3, 4);
            _anim.SetTrigger("AppearTrigger");
            return 1.0f;
        }
        return 0f;
    }


    protected override void Action_In_End_Talk() {
        base.Action_In_End_Talk();
        //アイテム出す
        if(state != State.with_Yamame) {
            if (!collection_Box.gameObject.activeSelf) {
                collection_Box.gameObject.SetActive(true);
            }
            _anim.SetTrigger("DisappearTrigger");
        }
        PlayerMovieFunction.Instance.Enable_Controlle_Player();        
    }


    void Update() {
        if(state == State.idle) {
            transform.position = new Vector3(transform.position.x, default_Height + Mathf.Sin(Time.time * 2) * 5f);
        }
        //落下
        else if(state == State.dropping) {
            //着地
            if (foot_Collision.Hit_Trigger()) {
                state = State.landing;
                camera_Shake.Shake(0.2f, new Vector2(1, 1), false);                
                landing_Effect.SetActive(true);
                _anim.SetTrigger("AppearTrigger");
            }
        }        
    }


    private void Change_Status_With_Yamame() {        
        state = State.with_Yamame;
        Change_Message_Status("YamameText", 11, 15);
        transform.position = new Vector3(3856f, -78f);
        _anim.SetTrigger("AppearTrigger");
    }
}
