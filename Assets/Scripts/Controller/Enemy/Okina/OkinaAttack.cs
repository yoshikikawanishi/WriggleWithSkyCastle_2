using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkinaAttack : BossEnemyAttack {

    [Space]
    [SerializeField] private GameObject fairy_Crystal_Red_Fairy;
    [SerializeField] private GameObject fairy_Crystal_Green_Fairy;

    private OkinaShoot _shoot;
    private OkinaEffect _effect;
    private MoveConstTime _move_Const_Time;
    private PlayerController player_Controller;

    private class Config {
        public readonly static Vector2 nutral_Pos = new Vector2(160f, 0);
    }    


    void Start() {
        //取得
        _shoot = GetComponentInChildren<OkinaShoot>();
        _effect = GetComponentInChildren<OkinaEffect>();
        _move_Const_Time = GetComponent<MoveConstTime>();
        player_Controller = GameObject.FindWithTag("PlayerTag").GetComponent<PlayerController>();
        //オブジェクトプール生成
        ObjectPoolManager.Instance.Create_New_Pool(fairy_Crystal_Red_Fairy, 3);
        ObjectPoolManager.Instance.Create_New_Pool(fairy_Crystal_Green_Fairy, 3);
    }


    public override void Stop_Attack() {
        Stop_Melody_A1();
        Stop_Melody_B1();
    }

    //===========================================================================================
    #region ChangePhase
    protected override void Action_In_Change_Phase() {

    }

    #endregion
    //===========================================================================================
    #region A1    

    protected override void Start_Melody_A1() {
        StartCoroutine("Melody_A1_Cor");
    }

    private IEnumerator Melody_A1_Cor() {
        base.Set_Can_Switch_Attack(false);

        while(true) {

            _effect.Play_Small_Power_Charge_Effect();
            yield return new WaitForSeconds(0.5f);
            _shoot.Shoot_Kuani_Shoot1();
            Move_Random(48f);
            for(float t = 0; t < 5.5f; t += Time.deltaTime) {
                if(melody_Manager.Get_Now_Melody() != MelodyManager.Melody.A1) {
                    Stop_Melody_A1();
                    base.Set_Can_Switch_Attack(true);
                    Restart_Attack();
                }
                yield return null;
            }
            Move_To_Nutral_Pos();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Move_Random(float distance) {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector3 vec = distance * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);
        _move_Const_Time.Start_Move(transform.position + vec, 0);
    }

    private void Move_To_Nutral_Pos() {
        _move_Const_Time.Start_Move(Config.nutral_Pos, 1);
    }

    private void Stop_Melody_A1() {
        StopCoroutine("Melody_A1_Cor");
        Move_To_Nutral_Pos();
        _shoot.Stop_Kunai_Shoot1();
    }


    #endregion
    //===========================================================================================    
    #region B1

    private class Crystal {
        public Vector2 pos;
        public string kind;
        public Crystal(Vector2 pos, string kind) {
            this.pos = pos;
            this.kind = kind;
        }
    }

    private class ConfigB1 {
        private static readonly Crystal[] crystal_Pos1 = {
            new Crystal(new Vector2(80f, -72f), "red"),
            new Crystal(new Vector2(80f, 72f), "green"),
            new Crystal(new Vector2(-80f, 72f), "green"),
            new Crystal(new Vector2(-80f, -72f), "red"),
        };
        private static readonly Crystal[] crystal_Pos2 = {
            new Crystal(new Vector2(128f, 72f), "green"),
            new Crystal(new Vector2(128f, -72f), "green"),
            new Crystal(new Vector2(-128f, -72f), "green"),
            new Crystal(new Vector2(-128f, 72f), "green"),
        };
        private static readonly Crystal[] crystal_Pos3 = {
            new Crystal(new Vector2(-128f, 72f), "green"),
            new Crystal(new Vector2(-160f, 72f), "green"),
            new Crystal(new Vector2(128f, -72f), "red"),
            new Crystal(new Vector2(160f, -72f), "red"),
        };
        public static readonly Crystal[][] positions = {
            crystal_Pos1,
            crystal_Pos2,
            crystal_Pos3
        };
        public static readonly float span = 0.3f;
    }

    protected override void Start_Melody_B1() {
        StartCoroutine("Melody_B1_Cor");
    }

    private IEnumerator Melody_B1_Cor() {
        base.Set_Can_Switch_Attack(false);
        float span = ConfigB1.span;

        _effect.Play_Small_Power_Charge_Effect();
        yield return new WaitForSeconds(1.5f);
        _effect.Play_Ban_Flying_Effect();        
        yield return new WaitForSeconds(1.5f);
        player_Controller.To_Disable_Ride_Beetle();

        while (true) {

            int r = Random.Range(0, 3);
            Crystal[] c = ConfigB1.positions[r];
            for(int i = 0; i < 4; i++) {
                Generate_Fairy_Crystal(c[i]);
                yield return new WaitForSeconds(span);
            }
            yield return new WaitForSeconds(2.0f);
            for (float t = 0; t < 8.0f; t += Time.deltaTime) {
                if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B1) {
                    Stop_Melody_B1();
                    base.Set_Can_Switch_Attack(true);
                    base.Restart_Attack();
                }
                yield return null;
            }
            
        }
    }

    private void Generate_Fairy_Crystal(Crystal c) {
        GameObject obj;
        if (c.kind == "red") 
            obj = ObjectPoolManager.Instance.Get_Pool(fairy_Crystal_Red_Fairy).GetObject();                    
        else 
            obj = ObjectPoolManager.Instance.Get_Pool(fairy_Crystal_Green_Fairy).GetObject();        
        obj.transform.position = c.pos;
    }

    private void Stop_Melody_B1() {
        StopCoroutine("Melody_B1_Cor");
        _effect.Release_Ban_Flying_Effect();
        player_Controller.To_Enable_Ride_Beetle();
    }

    private void Delete_Fairies() {

    }

    #endregion
    //===========================================================================================
    #region B2
    protected override void Start_Melody_B2() {
        StartCoroutine("Melody_B2_Cor");
    }

    private IEnumerator Melody_B2_Cor() {
        base.Set_Can_Switch_Attack(false);
        float angle = 0;

        while (true) {
            angle = Random.Range(0, 360f);
            _move_Const_Time.Start_Move(Config.nutral_Pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 40f, 2);
            yield return new WaitUntil(_move_Const_Time.End_Move);

            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }

            _shoot.Shoot_Blue_Laser_Left();
            yield return new WaitForSeconds(0.5f);

            angle = Random.Range(0, 360f);
            _move_Const_Time.Start_Move(Config.nutral_Pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 40f, 2);
            yield return new WaitUntil(_move_Const_Time.End_Move);

            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }

            _shoot.Shoot_Blue_Laser_Right();
            yield return new WaitForSeconds(1.5f);

            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.B2) {
                break;
            }
        }
        _move_Const_Time.Start_Move(Config.nutral_Pos);
        yield return new WaitUntil(_move_Const_Time.End_Move);

        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_B2() {
        StopCoroutine("Molody_B2_Cor");
    }

    #endregion
    //===========================================================================================

    protected override void Start_Melody_Bridge() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_C() {
        throw new System.NotImplementedException();
    }

    //===========================================================================================
    #region Chorus1
    protected override void Start_Melody_Chorus1() {
        StartCoroutine("Melody_Chorus1_Cor");
    }

    private IEnumerator Melody_Chorus1_Cor() {
        base.Set_Can_Switch_Attack(false);
        _shoot.Shoot_Black_Blue_Bullet();
        while (true) {
            yield return null;
            if (melody_Manager.Get_Now_Melody() != MelodyManager.Melody.chorus1)
                break;
        }
        _shoot.Stop_Black_Blue_Shoot();
        base.Set_Can_Switch_Attack(true);
        Restart_Attack();
    }

    private void Stop_Melody_Chorus1() {
        _shoot.Stop_Black_Blue_Shoot();
    }
    #endregion
    //===========================================================================================
    protected override void Start_Melody_Chorus2() {
        
    }
    //===========================================================================================
    protected override void Start_Melody_Intro() {
        throw new System.NotImplementedException();
    }

    protected override void Start_Melody_Pre_Chorus() {
        throw new System.NotImplementedException();
    }

    //===============================================================
    protected override void Start_Melody_A2() {
        throw new System.NotImplementedException();
    }

}
