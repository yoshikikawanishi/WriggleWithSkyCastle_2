using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextLayer : MonoBehaviour {

    public int sorting_Order = 0;

	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().sortingOrder = sorting_Order;
    }
	
}
