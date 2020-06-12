using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトを長方形に並べて生成する
/// </summary>
public class ObjectRectFormationGenerator : MonoBehaviour {

    [SerializeField] private GameObject gen_Object;
    public Transform parent;
    public Vector2Int num;
    public Vector2 width;
    

	// Use this for initialization
	void Start () {
        //オブジェクトプール
        ObjectPoolManager.Instance.Create_New_Pool(gen_Object, num.x * num.y);    
	}


    public void Generate(Vector2 left_Bottom_Position) {        
        for(int i = 0; i < num.x; i++) {
            for(int j = 0; j < num.y; j++) {
                var obj = ObjectPoolManager.Instance.Get_Pool(gen_Object).GetObject();
                obj.transform.localPosition = left_Bottom_Position + new Vector2(i * width.x, j * width.y);                
                obj.transform.SetParent(parent);
            }
        }
    }


    public void Generate(Vector2Int num, Vector2 width, Vector2 left_Bottom_Position) {
        this.num = num;
        this.width = width;
        Generate(left_Bottom_Position);
    }
	
}
