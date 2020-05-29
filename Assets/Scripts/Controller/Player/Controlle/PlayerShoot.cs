using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    private PlayerManager player_Manager;
    private PlayerController player_Controller;
    private PlayerEffect player_Effect;
    private PlayerSoundEffect player_SE;
    private GameObject main_Camera;
    private CameraShake camera_Shake;

    //弾
    [SerializeField] private GameObject normal_Bullet;
    [SerializeField] private GameObject bee_Bullet;
    [SerializeField] private GameObject butterfly_Bullet;
    [SerializeField] private GameObject mantis_Bullet;
    [SerializeField] private GameObject spider_Bullet;
    [SerializeField] private GameObject charge_Shoot_Obj;
    [SerializeField] private GameObject charge_Kick_Shoot_Obj;

    private float charge_Time = 0;
    private float[] charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };

    //チャージ段階
    private int charge_Phase = 0;
    //チャージショットに必要なパワー
    private readonly int essential_Power_In_Charge_Shoot = 30;
    //パワー
    private int player_Power = 0;


	// Use this for initialization
	void Start () {
        //弾のオブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(normal_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(bee_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(butterfly_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(mantis_Bullet, 5);
        ObjectPoolManager.Instance.Create_New_Pool(spider_Bullet, 5);
        //取得
        player_Manager = PlayerManager.Instance;
        player_Controller = GetComponent<PlayerController>();
        player_Effect = GetComponentInChildren<PlayerEffect>();
        player_SE = GetComponentInChildren<PlayerSoundEffect>();
        main_Camera = GameObject.FindWithTag("MainCamera");
        camera_Shake = main_Camera.GetComponent<CameraShake>();
    }


    //Update
    private void Update() {        
        if (!player_Controller.Get_Is_Ride_Beetle()) {
            if(charge_Time > charge_Span[0]) {
                Charge_Shoot();
            }
        }
    }


    #region NormalShoot

    public struct ShootStatus {
        public ObjectPool bullet_Pool;        
        public int num;        
        public float span;
        public float bullet_Speed;
        public float width;
    }
    private ShootStatus normal_Shoot = new ShootStatus();
    private ShootStatus option_Shoot = new ShootStatus();
    [HideInInspector] public float shoot_Interval = 0.25f;


    //ショットを打つ
    public void Shoot() {
        if(Time.timeScale == 0) {
            return;
        }

        Change_Shoot_Status();

        StartCoroutine("Shoot_Cor", normal_Shoot);
        StartCoroutine("Shoot_Cor", option_Shoot);
    }    


    private IEnumerator Shoot_Cor(ShootStatus s) {           
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < s.num; j++) {
                GameObject bullet = s.bullet_Pool.GetObject();
                bullet.transform.position = transform.position;
                bullet.transform.position += new Vector3(0, (-s.width * s.num) / 2) + new Vector3(0, s.width * j);
                bullet.transform.position += new Vector3(transform.localScale.x * 32f, 0);
                bullet.transform.localScale = transform.localScale;
                bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(s.bullet_Speed * transform.localScale.x, 0);
                if (s.bullet_Pool == ObjectPoolManager.Instance.Get_Pool(spider_Bullet))
                    Add_Diffusion_Shoot_Vel(bullet, s.num, j);
                else if (s.bullet_Pool == ObjectPoolManager.Instance.Get_Pool(butterfly_Bullet))
                    Add_Diffusion_Shoot_Vel(bullet, s.num, j);
                player_SE.Play_Shoot_Sound();
                bullet.GetComponent<Bullet>().Set_Inactive(10);
            }
            yield return new WaitForSeconds(s.span);
        }
    }


    //ショットのステータスを変える
    private void Change_Shoot_Status() {
        PlayerManager.Option option = player_Manager.Get_Option();
        int power = player_Manager.Get_Power();
        
        //弾数
        if (power < 32) 
            option_Shoot.num = 2;        
        else if (power < 64) 
            option_Shoot.num = 3;        
        else if (power < 128) 
            option_Shoot.num = 4;        
        else 
            option_Shoot.num = 5;                

        //弾の種類、速度、幅、弾        
        switch (option) {
            case PlayerManager.Option.none:
                Set_Shoot_Status(ref option_Shoot, normal_Bullet, 900f, 12f, 0.12f);
                normal_Shoot.num = 0;
                break;
            case PlayerManager.Option.bee:
                Set_Shoot_Status(ref option_Shoot, bee_Bullet, 1000f, 3f, 0.08f);
                option_Shoot.num--;
                normal_Shoot.num = 0;
                break;
            case PlayerManager.Option.butterfly:                
                Set_Shoot_Status(ref option_Shoot, butterfly_Bullet, 700f, 18f, 0.12f);
                option_Shoot.num --;
                normal_Shoot.num = 2;
                break;
            case PlayerManager.Option.mantis:                
                Set_Shoot_Status(ref option_Shoot, mantis_Bullet, 700f, 8f, 0.12f);
                normal_Shoot.num = 0;
                break;
            case PlayerManager.Option.spider:
                Set_Shoot_Status(ref option_Shoot, spider_Bullet, 400f, 0f, 0.08f);
                option_Shoot.num += 2;
                normal_Shoot.num = 0;
                break;
        }

        //時間
        shoot_Interval = option_Shoot.span * 3;

        //通常ショット
        Set_Shoot_Status(ref normal_Shoot, normal_Bullet, 900f, 12f, option_Shoot.span);

        if (option_Shoot.num <= 0)
            option_Shoot.num = 1;
    }


    //弾の種類、速度、幅、回数を代入する
    private void Set_Shoot_Status(ref ShootStatus s, GameObject bullet, float speed, float width, float span) {        
        s.bullet_Pool   = ObjectPoolManager.Instance.Get_Pool(bullet);        
        s.bullet_Speed  = speed;
        s.width         = width;
        s.span          = span;
    }


    //蜘蛛拡散弾の縦方向の速度を加算
    private void Add_Diffusion_Shoot_Vel(GameObject bullet, int num, int index) {
        float center = (num - 1) / 2;
        bullet.GetComponent<Rigidbody2D>().velocity += new Vector2(0, (center - index) * -130f);
    }


    #endregion

    #region ChargeShoot
    //チャージショット
    public void Charge_Shoot() {
        if (charge_Phase == 3) {
            StartCoroutine("Charge_Shoot_Cor");
        }
        charge_Time = 0;
        player_Effect.Start_Shoot_Charge(0);
        player_SE.Stop_Charge_Sound();
    }

    private IEnumerator Charge_Shoot_Cor() {
        if (BeetlePowerManager.Instance.beetle_Power < essential_Power_In_Charge_Shoot)
            yield break;
        //パワー減らす
        BeetlePowerManager.Instance.Decrease(essential_Power_In_Charge_Shoot);
        //生成
        var obj = Instantiate(charge_Shoot_Obj);
        obj.transform.position = transform.position + new Vector3(transform.localScale.x * 128f, 0);        
        ShootSystem[] shoots = obj.GetComponentsInChildren<ShootSystem>();

        player_SE.Play_Charge_Shoot_Sound();
        camera_Shake.Shake(0.25f, new Vector2(0, 1.2f), false);

        //ショット
        shoots[0].Shoot();
        yield return new WaitForSeconds(1f / 14f);
        shoots[1].Shoot();
    }


    //チャージショットのチャージ
    public void Charge() {
        Change_Charge_Span();
        //0段階目
        if(charge_Time < charge_Span[0]) {
            if (charge_Phase != 0) {
                charge_Phase = 0;
                player_Effect.Start_Shoot_Charge(0);                
            }
        }
        //1段階目
        else if(charge_Time < charge_Span[1]) {
            if(charge_Phase != 1) {
                charge_Phase = 1;
                player_Effect.Start_Shoot_Charge(1);
                player_SE.Start_Charge_Sound();
            }
        }
        //2段階目
        else if(charge_Time < charge_Span[2]) {
            if (charge_Phase != 2) {
                charge_Phase = 2;
                player_Effect.Start_Shoot_Charge(2);
                player_SE.Change_Charge_Sound_Pitch(1.15f);
            }
        }
        //チャージ完了
        else {
            if (charge_Phase != 3) {
                charge_Phase = 3;
                player_Effect.Start_Shoot_Charge(3);
                player_SE.Change_Charge_Sound_Pitch(1.3f);
            }
        }
        charge_Time += Time.deltaTime;
    }   



    //パワーによってチャージ時間を変える
    private void Change_Charge_Span() {
        //値が変化したときだけ判別
        if(player_Manager.Get_Power() == player_Power) {
            return;
        }
        player_Power = player_Manager.Get_Power();

        if (player_Power < 16) {
            charge_Span = new float[3] { 0.3f, 1.0f, 2.0f };            
        }
        else if(player_Power < 32) {
            charge_Span = new float[3] { 0.27f, 0.85f, 1.7f };
        }
        else if(player_Power < 64) {
            charge_Span = new float[3] { 0.24f, 0.7f, 1.4f };
        }
        else if(player_Power < 128) {
            charge_Span = new float[3] { 0.21f, 0.55f, 1.1f };
        }
        else {
            charge_Span = new float[3] { 0.2f, 0.4f, 0.8f };
        }
    }
    #endregion


    //チャージキックのショット用
    public void Shoot_Charge_Kick_Shoot() {
        var obj = Instantiate(charge_Kick_Shoot_Obj);
        obj.transform.position = transform.position;
        ShootSystem[] shoots = obj.GetComponentsInChildren<ShootSystem>();
        for(int i = 0; i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }
}
