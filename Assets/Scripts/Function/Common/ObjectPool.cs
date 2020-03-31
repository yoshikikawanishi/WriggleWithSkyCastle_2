using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
    private List<GameObject> _poolObjList;
    private GameObject _poolObj;

    private bool is_Pooled = false;


    /// <summary>
    /// オブジェクトプールを作成
    /// </summary>
    /// <param name="obj">オブジェクト</param>
    /// <param name="maxCount">数</param>
    public void CreatePool(GameObject obj, int maxCount) {
        _poolObj = obj;
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < maxCount; i++) {
            var newObj = CreateNewObject();
            newObj.SetActive(false);
            _poolObjList.Add(newObj);
        }
        is_Pooled = true;
    }


    /// <summary>
    /// オブジェクトプールからオブジェクトを生成
    /// </summary>
    /// <returns>生成したオブジェクト</returns>
    public GameObject GetObject() {
        // 使用中でないものを探して返す
        foreach (var obj in _poolObjList) {
            if (obj.activeSelf == false) {
                obj.SetActive(true);
                return obj;
            }
        }

        // 全て使用中だったら新しく作って返す
        var newObj = CreateNewObject();
        newObj.SetActive(true);
        _poolObjList.Add(newObj);

        return newObj;
    }


    private GameObject CreateNewObject() {
        var newObj = Instantiate(_poolObj);
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        return newObj;
    }


    /// <summary>
    /// オブジェクトプールが存在するかどうか
    /// </summary>
    /// <returns></returns>
    public bool Is_Pooled() {
        return is_Pooled;
    }

}
