using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptController {

    //制御対象のスクリプト
    MonoBehaviour[] scripts;
    MonoBehaviour that;

    //初期化
    public void Initialize(MonoBehaviour that) {
        //アタッチされている他のスクリプトを取得する
        scripts = that.GetComponents<MonoBehaviour>();
        this.that = that;
    }

    //スクリプトを無効にする
    public void Suspend() {
        for (int i = 0; i < scripts.Length; i++) {
            //自分自身は除外する
            if (scripts[i].GetInstanceID() == that.GetInstanceID()) {
                continue;
            }
            scripts[i].enabled = false;
            scripts[i].StopAllCoroutines();
        }
    }
    //スクリプトを有効にする
    public void Resume() {
        for (int i = 0; i < scripts.Length; i++) {
            //自分自身は除外する
            if (scripts[i].GetInstanceID() == that.GetInstanceID()) {
                continue;
            }
            scripts[i].enabled = true;
        }
    }
}