using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarumiShoot : MonoBehaviour {

    [SerializeField] private GameObject snow_Shoot_Obj;
    [SerializeField] private ShootSystem yellow_Talisman_Shoot;
    [SerializeField] private ShootSystem yellow_Talisman_Shoot_Strong;
    [SerializeField] private JizoBullet jizo_Bullet;
    [SerializeField] private Bullet big_Bullet;

    private const float BIG_BULLET_LIFETIME = 8;


    void Start() {
        ObjectPoolManager.Instance.Create_New_Pool(jizo_Bullet.gameObject, 5);
    }


    public void Shoot_Snow_Shoot() {
        ShootSystem[] shoots = snow_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        for(int i = 0;  i < shoots.Length; i++) {
            shoots[i].Shoot();
        }
    }


    public void Stop_Snow_Shoot() {
        ShootSystem[] shoots = snow_Shoot_Obj.GetComponentsInChildren<ShootSystem>();
        for (int i = 0; i < shoots.Length; i++) {
            shoots[i].Stop_Shoot();
        }
    }


    public void Shoot_Yellow_Talisman_Shoot() {
        yellow_Talisman_Shoot.center_Angle_Deg = Random.Range(0, 10f);
        yellow_Talisman_Shoot.Shoot();
    }


    public void Shoot_Yellow_Talisman_Shoot_Strong() {
        yellow_Talisman_Shoot_Strong.center_Angle_Deg = Random.Range(0, 10f);
        yellow_Talisman_Shoot_Strong.Shoot();
    }


    public void Start_Jizo_Bullet_Dropping(float span) {
        StartCoroutine("Jizo_Bullet_Dropping_Cor", span);
    }


    private IEnumerator Jizo_Bullet_Dropping_Cor(float span) {
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");
        Vector2 pos = new Vector2(0, 100f);
        while (true) {            
            var bullet = ObjectPoolManager.Instance.Get_Pool(jizo_Bullet.gameObject).GetObject();
            pos = new Vector2(main_Camera.transform.position.x + Random.Range(-100f, 260f), pos.y);
            bullet.transform.position = pos;
            ObjectPoolManager.Instance.Set_Inactive(bullet, 5.0f);
            yield return new WaitForSeconds(span);
        }
    }


    public void Stop_Jizo_Bullet_Dropping() {
        StopCoroutine("Jizo_Bullet_Dropping_Cor");
    }


    public void Shoot_Big_Bullet(Vector2 deposit_Pos) {
        StartCoroutine("Controlle_Big_Bullet_Cor", deposit_Pos);
    }


    private IEnumerator Controlle_Big_Bullet_Cor(Vector2 deposit_Pos) {
        GameObject main_Camera = GameObject.FindWithTag("MainCamera");

        GameObject bullet = Instantiate(big_Bullet.gameObject);
        bullet.SetActive(true);
        bullet.transform.position = transform.position;
        bullet.transform.SetParent(main_Camera.transform);

        MoveTwoPoints bullet_Move = bullet.GetComponent<MoveTwoPoints>();
        ShootSystem[] bullet_Shoots = bullet.GetComponentsInChildren<ShootSystem>();
        VerticalVibeMotion vibe_Motion = bullet.GetComponent<VerticalVibeMotion>();

        bullet_Move.Start_Move(new Vector3(deposit_Pos.x, deposit_Pos.y, 10));
        yield return new WaitUntil(bullet_Move.End_Move);

        vibe_Motion.enabled = true;

        foreach(var shoot in bullet_Shoots) {
            shoot.Shoot();
        }

        yield return new WaitForSeconds(BIG_BULLET_LIFETIME);
        vibe_Motion.enabled = false;
        bullet_Move.Start_Move(new Vector3(bullet.transform.localPosition.x, 280f, 10));
        yield return new WaitUntil(bullet_Move.End_Move);
        Destroy(bullet);
    }
}
