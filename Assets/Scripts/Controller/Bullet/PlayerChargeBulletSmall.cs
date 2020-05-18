using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChargeBulletSmall : MonoBehaviour {

    public enum DivideDirection {
        right, 
        left
    }
    [SerializeField] DivideDirection divide_Direction = DivideDirection.right;

    [SerializeField] private GameObject green_Rice_Bullet;
    [SerializeField] private GameObject yellow_Rice_Bullet;

    private ObjectPool green_Rice_Pool;
    private ObjectPool yellow_Rice_Pool;



    // Use this for initialization
    void Start () {
        ObjectPoolManager.Instance.Create_New_Pool(green_Rice_Bullet, 10);
        ObjectPoolManager.Instance.Create_New_Pool(yellow_Rice_Bullet, 10);
        green_Rice_Pool = ObjectPoolManager.Instance.Get_Pool(green_Rice_Bullet);
        yellow_Rice_Pool = ObjectPoolManager.Instance.Get_Pool(yellow_Rice_Bullet);
    }

    //OnEnable
    private void OnEnable() {
        StartCoroutine("Divide_Cor");
    }

    private IEnumerator Divide_Cor() {
        yield return new WaitForSeconds(2.0f);
        //生成
        GameObject[] bullets = new GameObject[2];
        bullets[0] = green_Rice_Pool.GetObject();               bullets[1] = yellow_Rice_Pool.GetObject();
        bullets[0].transform.position = transform.position;     bullets[1].transform.position = transform.position;

        //回転
        bullets[0].transform.rotation = transform.rotation;     bullets[0].transform.Rotate(new Vector3(0, 0, 200f));
        bullets[1].transform.rotation = transform.rotation;
        if (divide_Direction == DivideDirection.right)
            bullets[1].transform.Rotate(new Vector3(0, 0, 70f));
        else
            bullets[1].transform.Rotate(new Vector3(0, 0, -70f));
        //発射
        for (int i = 0; i < 2; i++) {
            bullets[i].GetComponent<Rigidbody2D>().velocity = bullets[i].transform.right * 50f;
            ObjectPoolManager.Instance.Set_Inactive(bullets[i], 8.0f);
        }
        gameObject.SetActive(false);
    }
}
