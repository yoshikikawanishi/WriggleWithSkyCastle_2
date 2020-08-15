using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 両端キュー
/// </summary>
/// <typeparam name="Type"></typeparam>
public class DEQueue<Type> {
    
    private List<Type> list = new List<Type>();
    
    /// <summary>
    /// キューの先頭に挿入
    /// </summary>    
    public void Add_First(Type element) {
        list.Insert(0, element);
    }

    /// <summary>
    /// キューの末尾に挿入
    /// </summary>    
    public void Add_Last(Type element) {
        list.Add(element);
    }


    /// <summary>
    /// キューの先頭から取り出す
    /// </summary>    
    public Type Remove_First() {
        Type first = Get_First();
        list.RemoveAt(0);
        return first;
    }

    /// <summary>
    /// キューの末尾から取り出す
    /// </summary>    
    public Type Remove_Last() {
        Type last = Get_Last();
        list.RemoveAt(list.Count - 1);
        return last;
    }


    /// <summary>
    /// キューの先頭を取得
    /// </summary>    
    public Type Get_First() {
        return list[0];
    }

    /// <summary>
    /// キューの末尾を取得
    /// </summary>    
    public Type Get_Last() {
        return list[list.Count - 1];
    }


    /// <summary>
    /// キューを削除
    /// </summary>
    public void Clear() {
        list.Clear();
    }

}
