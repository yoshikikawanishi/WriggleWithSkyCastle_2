using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EternalVineShoot : MonoBehaviour {

    [SerializeField] private Bullet vine_Bullet;
    [SerializeField] private GameObject vine_Shoot_Divide_Effect_Prefab;

    private readonly float shoot_Span = 0.08f;
    private readonly float bullet_Lifetime = 2f;
    private readonly int bullet_Num = 50;    


    void Start() {
        ObjectPoolManager.Instance.Create_New_Pool(vine_Bullet.gameObject, 30);
    }


    public void Shoot_Vine_Shoot(int divide_Count) {
        VineList VList = new VineList();
        VList.Create_List(bullet_Num, transform.position, Mathf.PI, 0);
        StartCoroutine(Vine_Shoot_Cor(VList, divide_Count));
    }


    public void Stop_Vine_Shoot() {
        StopAllCoroutines();
    }


    //vine shoot
    private IEnumerator Vine_Shoot_Cor(VineList VList, int divide_Count) {
        if (VList.list.Count == 0)
            VList.Create_List(bullet_Num, transform.position, Mathf.PI, Random.Range(-0.05f, 0.05f));
        //エフェクト
        Play_Divide_Effect(VList.list[0]);
        yield return new WaitForSeconds(0.5f);

        int count = 1;        
        foreach(Vector2 pos in VList.list) {          
            Generate_Vine_Bullet(pos);
            //画面外に出たら終わる
            if (Mathf.Abs(pos.x) > 250f || Mathf.Abs(pos.y) > 140f)
                yield break;            
            //枝分かれを作る
            if (count % divide_Count == 0) {
                divide_Count += 3;
                count = 0;
                VineList v = new VineList();
                float initial_Angle = VList.Angle() + (Random.Range(0, 2) - 0.5f) * Mathf.PI; 
                v.Create_List(bullet_Num, pos, initial_Angle, Random.Range(-0.1f, 0.1f));
                StartCoroutine(Vine_Shoot_Cor(v, divide_Count-1));
            }
            count++;
            yield return new WaitForSeconds(shoot_Span);
        }
    }


    //vine bulletを生成
    private void Generate_Vine_Bullet(Vector2 pos) {
        GameObject bullet = ObjectPoolManager.Instance.Get_Pool(vine_Bullet.gameObject).GetObject();
        bullet.transform.position = pos;
        ObjectPoolManager.Instance.Set_Inactive(bullet, bullet_Lifetime);
        UsualSoundManager.Instance.Play_Shoot_Sound_Small();
    }


    private class VineList {

        private readonly float bullet_Span = 8f;
        public List<Vector2> list = new List<Vector2>();
        private float angle = 0;

        /// <summary>
        ///vine bulletを生成する座標を計算        
        /// </summary>
        /// <param name="num">生成する弾の数</param>
        /// <param name="initial_Pos">開始位置</param>
        /// <param name="initial_Angle">開始角度[rad]</param>
        /// <param name="curve_Angle">カーブ角度[rad]</param>
        public void Create_List(int num, Vector2 initial_Pos, float initial_Angle, float curve_Angle) {
            list.Clear();
            angle = initial_Angle;
            Vector2 pos = initial_Pos;
            list.Add(initial_Pos);

            for (int i = 0; i < num; i++) {
                angle += curve_Angle;
                curve_Angle *= 0.95f;                
                pos = pos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * bullet_Span;
                list.Add(pos);
            }
        }

        public float Angle() {
            return angle;
        }
    }


    private void Play_Divide_Effect(Vector2 pos) {
        var obj = Instantiate(vine_Shoot_Divide_Effect_Prefab);
        obj.transform.position = pos;
        Destroy(obj, 1.0f);
    }
}
