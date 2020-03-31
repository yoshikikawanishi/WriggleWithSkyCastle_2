using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour {

    [SerializeField] private GameObject[] drop_Rock = new GameObject[3];    

    private PlayerController player_Controller;
    private PlayerTransition player_Transition;
    private Rigidbody2D player_Rigid;
    private Renderer _renderer;
   
    private float player_Default_Speed;

    private float player_Speed_In_Hit_WaterFall = 50f;
    private float water_Fall_Power = 900f;    

    private bool is_Hit_Player = false;


	// Use this for initialization
	void Start () {
        //取得
        GameObject player = GameObject.FindWithTag("PlayerTag");
        if(player == null) {
            Debug.Log("Player is not found");
            gameObject.SetActive(false);
            return;
        }
        player_Controller = player.GetComponent<PlayerController>();
        player_Transition = player.GetComponent<PlayerTransition>();
        player_Rigid = player.GetComponent<Rigidbody2D>();
        _renderer = GetComponent<Renderer>();
        player_Default_Speed = player_Transition.Get_Max_Speed();

        StartCoroutine("Drop_Rock_Cor");
	}
	

	// Update is called once per frame
	void Update () {
        if(player_Controller == null) 
            return;
        if (player_Controller.Get_Is_Ride_Beetle()) 
            return;
                
        if (is_Hit_Player) {
            //下に押さえつける
            player_Rigid.AddForce(new Vector2(0, -water_Fall_Power));
            //移動速度下げる            
            if (player_Transition.Get_Max_Speed() > player_Speed_In_Hit_WaterFall + 10f) {
                player_Transition.Set_Max_Speed(player_Speed_In_Hit_WaterFall);                
            }
        }
       
	}    


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            is_Hit_Player = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "PlayerBodyTag") {
            is_Hit_Player = false;
            //移動速度戻す
            player_Transition.Set_Max_Speed(player_Default_Speed);
        }
    }


    //岩を落とす
    private IEnumerator Drop_Rock_Cor() {
        int loop_Count = 0;
        float random = 0;        

        while (true) {
            yield return new WaitForSeconds(Random.Range(2f, 3f));
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
