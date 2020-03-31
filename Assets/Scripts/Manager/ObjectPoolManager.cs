using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager> {
    private Dictionary<string, ObjectPool> pool_Dictionary = new Dictionary<string, ObjectPool>();

    private new void Awake() {
        base.dontDestroyOnLoad = false;
    }

    //オブジェクトプールの追加
    public void Create_New_Pool(GameObject obj, int num) {
        //もうすでに存在する場合作らない
        if (pool_Dictionary.ContainsKey(obj.name)) {
            return;
        }
        ObjectPool _pool = gameObject.AddComponent<ObjectPool>();
        _pool.CreatePool(obj, num);
        pool_Dictionary.Add(obj.name, _pool);
    }


    //オブジェクトプールの受け渡し
    public ObjectPool Get_Pool(GameObject obj) {
        if (pool_Dictionary.ContainsKey(obj.name)) {
            return pool_Dictionary[obj.name];
        }
        else {
            Create_New_Pool(obj, 1);
            return pool_Dictionary[obj.name];
        }
    }


    //名前からオブジェクトプールの受け渡し
    public ObjectPool Get_Pool(string obj_Name) {
        if (pool_Dictionary.ContainsKey(obj_Name)) {
            return pool_Dictionary[obj_Name];
        }
        else {
            Debug.Log("Not Exist " + obj_Name + " Pool");
            return null;
        }
    }

    //オブジェクトをtime秒後に非アクティブ化
    public void Set_Inactive(GameObject obj, float time) {
        StartCoroutine(Set_Inactive_Cor(obj, time));                
    }

    private IEnumerator Set_Inactive_Cor(GameObject obj, float time) {
        if (time > Time.deltaTime)
            yield return new WaitForSeconds(time - Time.deltaTime);
        else
            yield return null;
        obj.SetActive(false);
    }


    //現在オブジェクトプールされているオブジェクトの表示
    public void Debug_Print() {
        foreach(string key in pool_Dictionary.Keys) {
            Debug.Log(key);
        }
    }

}
