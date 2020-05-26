using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamaged : MonoBehaviour {

    //被弾時の弾消しボム
    [SerializeField] private GameObject bomb_Prefab;    

    //コンポーネント
    private PlayerSoundEffect player_SE;    

    //無敵時間
    private float invincible_Time_Length = 1.0f;


    private void Start() {
        //取得
        player_SE = GetComponentInChildren<PlayerSoundEffect>();        
    }


    //被弾時の処理
    public IEnumerator Damaged() {
        if (GetComponentInChildren<PlayerBodyCollision>().Is_Invincible())
            yield break;
        if (PlayerManager.Instance.Reduce_Life() == 0) 
            yield break;

        PlayerBodyCollision body_Collision = GetComponentInChildren<PlayerBodyCollision>();

        invincible_Time_Length = 1.0f;
        if (GetComponent<PlayerController>().Get_Is_Ride_Beetle()) {
            invincible_Time_Length = 3.0f;
        }

        Put_Out_Power(PlayerManager.Instance.Get_Power() / 8);      //パワーの減少
        BeetlePowerManager.Instance.StartCoroutine("Increase_Cor", 25);     //飛行パワー増加
        StartCoroutine("Blink");                                    //点滅
        player_SE.Play_Damaged_Sound();                             //効果音
        Occure_Knock_Back();                                        //反動
        body_Collision.Become_Invincible();                         //無敵化  
        Play_Delete_Bullet_Bomb();                                  //弾消しようのボム

        yield return new WaitForSeconds(invincible_Time_Length + 1.0f);    //無敵時間

        body_Collision.Release_Invincible();                        //戻す        
    }


    //点滅
    private IEnumerator Blink() {
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        float span = invincible_Time_Length / 30;
        for (int i = 0; i < 15; i++) {
            _sprite.color = new Color(0.5f, 0.5f, 0.5f, 0.3f);
            yield return new WaitForSeconds(span * 0.8f);
            _sprite.color = new Color(0.5f, 0.5f, 0.5f, 1);
            yield return new WaitForSeconds(span * 1.2f);
        }
    }


    //反動
    private void Occure_Knock_Back() {
        PlayerController _controller = GetComponent<PlayerController>();
        float force = _controller.is_Landing ? 200f : 100f;
        GetComponent<Rigidbody2D>().velocity = new Vector2(force * -transform.localScale.x, 100f);
    }


    //パワーの減少
    private void Put_Out_Power(int value) {
        PlayerManager.Instance.Set_Power(PlayerManager.Instance.Get_Power() - value);
        //アイテムの放出
        var power = Resources.Load("Object/Power") as GameObject;
        ObjectPool power_Pool = ObjectPoolManager.Instance.Get_Pool(power);
        for(int i = 0; i < value - 4; i++) {
            var p = power_Pool.GetObject();
            p.transform.position = transform.position + new Vector3(0, 64f);
            Vector2 velocity = new Vector2(Random.Range(-15f, 15f) * i, Random.Range(300f, 500f));
            p.GetComponent<Rigidbody2D>().velocity = velocity;
        }
    }


    //ボムを出す
    private void Play_Delete_Bullet_Bomb() {        
        var bomb = Instantiate(bomb_Prefab);
        bomb.transform.position = transform.position;
        bomb.transform.localScale = new Vector3(1, 1, 1);
    }


    //MissZoneに当たったときの処理
    public void Miss() {
        PlayerManager.Instance.Reduce_Life();
        if(PlayerManager.Instance.Get_Life() > 0)
            GameManager.Instance.Miss();
    }
}
