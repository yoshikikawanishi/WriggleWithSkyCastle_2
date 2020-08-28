using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// インスペクタで設定したオブジェクト編隊を生成する
/// </summary>
public class GeneratorSystem : MonoBehaviour {

    public enum Kind {
        liner,
        rotate,        
    }

    public enum PosKind {
        global,
        local,
        generator
    }

    [System.Serializable]
    public class Param {
        public string name;
        public Kind kind;
        public GameObject obj;
        public GameObject parent;
        public bool is_Object_Pool;
        public int num = 1;
        public float span;
        public float after_Span;
        public PosKind pos_Kind = PosKind.global;
        public LinerParam liner = new LinerParam();
        public RotateParam rotate = new RotateParam();
    }
    
    [System.Serializable]
    public class LinerParam {                
        public Vector2 initial_Pos;
        public Vector2 inter_Vector;        
    }

    [System.Serializable]
    public class RotateParam {        
        public Vector2 center_Pos;
        public float radius;
        public float spread_Radius;
        public float initial_Angle_Deg;
        public float inter_Angle_Deg;        
    }

    public List<Param> list = new List<Param>();

    private int count = 0;
    private bool is_End_Generate = false;
    private bool has_Created_Pool = false;


    void Start() {
        Create_Object_Pool();
    }


    /// <summary>
    /// リストのパラメータで順番に生成開始する
    /// </summary>
    public void Start_Generate() {
        if (!has_Created_Pool)
            Create_Object_Pool();
        is_End_Generate = false;
        Stop_Generate();
        count = 0;
        StartCoroutine("Generate_Cor");
    }	


    /// <summary>
    /// 生成終了する
    /// </summary>
    public void Stop_Generate() {
        StopAllCoroutines();
    }


    /// <summary>
    /// 生成終了時にtrue
    /// 一度呼ばれるとfalseになる
    /// </summary>
    /// <returns></returns>
    public bool Is_End_Generate() {
        if (is_End_Generate) {
            is_End_Generate = false;
            return true;
        }
        return false;            
    }


    //オブジェクトプール生成
    private void Create_Object_Pool() {
        //オブジェクトプール
        if (has_Created_Pool)
            return;
        for (int i = 0; i < list.Count; i++) {
            if (list[i].is_Object_Pool) {
                ObjectPoolManager.Instance.Create_New_Pool(list[i].obj, list[i].num);
            }
        }
        has_Created_Pool = true;
    }


    //オブジェクト編隊を生成するルーチン
    private IEnumerator Generate_Cor() {
        if (count >= list.Count) {
            is_End_Generate = true;
            yield break;
        }

        Param param = list[count];
        if (param.obj == null) {
            Debug.LogWarning("Generator System Error\n obj is not set in inspector");
            yield break;            
        }                   

        GameObject obj;
        Vector2 position = Vector2.zero;
        Vector2 generator_Position = transform.position;        

        for (int i = 0; i < param.num; i++) {
            //生成
            obj = Generate_Obj(param.obj, param.parent, param.is_Object_Pool);
            //座標を計算
            switch (param.kind) {
                case Kind.liner: position = Liner_Formation_Position(param, i); break;
                case Kind.rotate: position = Rotate_Formation_Position(param, i); break;
            }
            //座標を設定
            if (param.pos_Kind == PosKind.local) {
                obj.transform.localPosition = position;
                obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            }
            else if(param.pos_Kind == PosKind.generator) {                
                obj.transform.position = generator_Position + position;
            }
            else {
                obj.transform.position = position;
            }
            yield return new WaitForSeconds(param.span);
        }

        //次
        yield return new WaitForSeconds(param.after_Span);
        count++;
        StartCoroutine("Generate_Cor");

    }


    //オブジェクト単体を生成する
    //オブジェクトプールしているかどうかによる違いを対処
    private GameObject Generate_Obj(GameObject prefab, GameObject parent, bool is_Object_Pool) {
        GameObject obj;
        if (is_Object_Pool) {
            obj = ObjectPoolManager.Instance.Get_Pool(prefab).GetObject();
        }
        else {
            obj = Instantiate(prefab);
        }
        if(parent != null)
            obj.transform.SetParent(parent.transform);
        return obj;
    }


    //直線生成用の座標を計算
    private Vector2 Liner_Formation_Position(Param param, int index) {
        Vector2 position;
        position = param.liner.initial_Pos + param.liner.inter_Vector * index;        
        return position;
    }


    //円形生成用の座標を計算
    private Vector2 Rotate_Formation_Position(Param param, int index) {
        Vector2 position;
        float angle = (param.rotate.initial_Angle_Deg + param.rotate.inter_Angle_Deg * index) * Mathf.Deg2Rad;
        float radius = param.rotate.radius + param.rotate.spread_Radius * index;
        position = param.rotate.center_Pos
                 + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return position;
    }


    //------------- Editor用 ---------------
    public void Add_List(int i) {
        list.Insert(i, new Param());
    }

    public void Remove_List(int i) {
        list.RemoveAt(i);
    }
}
