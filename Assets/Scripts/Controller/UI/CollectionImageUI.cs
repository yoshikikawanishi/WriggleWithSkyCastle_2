using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionImageUI : MonoBehaviour {

    [SerializeField] private string collection_Name;

	// Use this for initialization
	void Start () {
        //入手していなければ表示しない
        if (CollectionManager.Instance.Is_Collected(collection_Name)) {
            GetComponent<Image>().color = new Color(1, 1, 1, 1);            
        }
        else {
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }
	}
	
}
