using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子供に配置した１マスのブロックをランダムで上昇、落下させる攻撃
/// </summary>
public class GridGroundManager : MonoBehaviour {    

    private List<GridGroundController> blocks = new List<GridGroundController>();
    private int block_Num;
    private float span = 1f;


    void Start() {        
        //取得
        for(int i = 0; i < transform.childCount; i++) {
            var block = transform.GetChild(i).GetComponent<GridGroundController>();
            if(block != null) {
                blocks.Add(block);
            }
        }
        block_Num = blocks.Count;

        StartCoroutine("Start_Random_Raise", 1.0f);
    }

	
    //ランダムにブロック上下を開始する
    public void Start_Random_Raise(float span) {
        this.span = span;
        StartCoroutine("Random_Raise_Cor");
    }


    //ランダムにブロック上下を終了
    public void Quit_Random_Raise() {
        StopCoroutine("Random_Raise_Cor");
    }


    //ブロックを停止させる
    public void Freeze_Blocks() {
        for(int i = 0; i < block_Num; i++) {
            if (blocks[i].Get_State() == GridGroundController.State.idle)
                continue;
            blocks[i].Freeze();
        }
    }


    //ブロックをもとの位置に戻す
    public void Restore_Blocks_To_Original_Pos() {
        for(int i = 0; i < block_Num; i++) {
            if (blocks[i].Get_State() == GridGroundController.State.idle)
                continue;
            blocks[i].Restore_To_Original_Pos();
        }
    }



    private IEnumerator Random_Raise_Cor() {
        if (blocks.Count == 0)
            yield break;
        List<GridGroundController> list;
        while (true) {
            list = Idle_Block_List();
            list[Random.Range(0, list.Count)].Start_Raise(1.0f);
            yield return new WaitForSeconds(span);
        }
    }


    private List<GridGroundController> Idle_Block_List() {
        List<GridGroundController> list = new List<GridGroundController>();
        for (int i = 0; i < block_Num; i++) {
            if (blocks[i].Get_State() == GridGroundController.State.idle)
                list.Add(blocks[i]);
        }
        return list;
    }
   
}
