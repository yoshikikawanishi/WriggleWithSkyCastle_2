using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObject : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        var obj1 = GameObject.Instantiate(Resources.Load("CommonScripts") as GameObject);
        obj1.transform.position = new Vector3(1, 1, 0);
        var obj2 = GameObject.Instantiate(Resources.Load("UsualSounds") as GameObject);
        obj2.transform.position = new Vector3(1, 1, 0);
        var obj3 = GameObject.Instantiate(Resources.Load("BGM") as GameObject);
        obj3.transform.position = new Vector3(1, 1, 0);
    }
}
