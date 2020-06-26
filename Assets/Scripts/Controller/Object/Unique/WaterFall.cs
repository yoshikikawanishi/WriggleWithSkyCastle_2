using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour {

    [SerializeField] private GameObject[] drop_Rock = new GameObject[3];    
    
    private Renderer _renderer;

    private Vector2 drop_Span_Range = new Vector2(5f, 6f);


    void Start () {
        _renderer = GetComponent<Renderer>();
        StartCoroutine("Drop_Rock_Cor");
	}


    //岩を落とす
    private IEnumerator Drop_Rock_Cor() {
        int loop_Count = 0;
        float random = 0;        

        while (true) {
            yield return new WaitForSeconds(Random.Range(drop_Span_Range.x, drop_Span_Range.y));
            yield return new WaitUntil(Is_Visible);

            if (drop_Rock[loop_Count % drop_Rock.Length] == null)
                continue;
            var rock = Instantiate(drop_Rock[loop_Count % drop_Rock.Length]);
            random = Random.Range(-48f, 48f);
            rock.transform.position = new Vector3(transform.position.x + random, 180f);

            loop_Count++;
        }
    }

    private bool Is_Visible() {
        return _renderer.isVisible;
    }

}
