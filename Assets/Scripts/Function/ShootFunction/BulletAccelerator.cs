using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAccelerator : MonoBehaviour {


    /// <summary>
    /// 弾をまっすぐ加速させる
    /// </summary>
    /// <param name="bullet_List"></param>
	public void Accelerat_Bullet(List<GameObject> bullet_List, float acc_Rate, float acc_Time) {
        if (bullet_List[0].GetComponent<Rigidbody2D>() == null) {
            Debug.Log("Bullet Not Attached Rigidbody");
            return;
        }
        StartCoroutine(Accelerate_Bullet_Routine(bullet_List, acc_Rate, acc_Time));          
    }

    public void Accelerat_Bullet(GameObject bullet, float acc_Rate, float acc_Time) {
        List<GameObject> bullet_List = new List<GameObject> { bullet };
        StartCoroutine(Accelerate_Bullet_Routine(bullet_List, acc_Rate, acc_Time));
    }


    //加速させるコルーチン
    private IEnumerator Accelerate_Bullet_Routine(List<GameObject> bullet_List, float acc_Rate, float acc_Time) {        
        
        List<GameObject> remove_List = new List<GameObject>();

        for (float t = 0; t < acc_Time; t += Time.deltaTime) {

            for (int i = 0; i < bullet_List.Count; i++) {
                //途中で消えたものは取り除く
                if (!bullet_List[i].activeSelf) {
                    remove_List.Add(bullet_List[i]);
                    continue;
                }
                bullet_List[i].GetComponent<Rigidbody2D>().velocity *= acc_Rate;
            }

            //途中で消えたものは取り除く
            for(int i = 0; i < remove_List.Count; i++) {
                bullet_List.Remove(remove_List[i]);
            }
            remove_List.Clear();

            yield return new WaitForSeconds(0.015f);

        }
    }

}
