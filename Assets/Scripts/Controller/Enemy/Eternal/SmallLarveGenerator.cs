using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallLarveGenerator : MonoBehaviour {

    [SerializeField] private SmallLarva s_Larva_Prefab;    

    private float rate = 1f;
    private readonly float SCREEN_HEIGHT = 150f;
    private readonly float SCREEN_WIDTH = 240f;

    void Start() {
        ObjectPoolManager.Instance.Create_New_Pool(s_Larva_Prefab.gameObject, 10);
    }


    /// <summary>
    /// 小さいラルバを画面端に生成するのを開始する
    /// </summary>
    /// <param name="rate">1秒あたりに何体生成するか</param>
    public void Start_Gen(float rate) {
        this.rate = rate;
        StartCoroutine("Generate_Cor");
    }


    public void Stop_Gen() {
        StopCoroutine("Generate_Cor");
    }


    public void Disable_All_Larva() {
        Stop_Gen();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach(var obj in objs) {
            if (obj.GetComponent<SmallLarva>() == null)
                continue;
            obj.SetActive(false);
        }
    }


    private IEnumerator Generate_Cor() {
        float span = 1f / rate;
        while (true) {
            var larva = ObjectPoolManager.Instance.Get_Pool(s_Larva_Prefab.gameObject).GetObject();
            larva.transform.position = Define_Generate_Pos();
            yield return new WaitForSeconds(span);
        }
    }


    private Vector2 Define_Generate_Pos() {
        Vector2 result = new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT);
        switch(Random.Range(0, 4)) {
            case 0: return result * new Vector2(Random.Range(-1f, 1f), 1);
            case 1: return result * new Vector2(Random.Range(-1f, 1f), -1);
            case 2: return result * new Vector2(1, Random.Range(-1f, 1f));
            case 3: return result * new Vector2(-1, Random.Range(-1f, 1f));
        }
        return result;
    }


    

}
