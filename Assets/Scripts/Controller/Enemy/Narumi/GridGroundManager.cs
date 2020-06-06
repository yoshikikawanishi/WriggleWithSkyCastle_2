using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 子供に配置した１マスのブロックをランダムで上昇、落下させる攻撃
/// </summary>
public class GridGroundManager : MonoBehaviour {    

    private List<GridGroundController> blocks = new List<GridGroundController>();
    private int block_Num;    


    void Start() {        
        //取得
        for(int i = 0; i < transform.childCount; i++) {
            var block = transform.GetChild(i).GetComponent<GridGroundController>();
            if(block != null) {
                blocks.Add(block);
            }
        }
        block_Num = blocks.Count;

        //StartCoroutine("Start_Random_Raise", 1.0f);
    }

	
    //ランダムにブロック上下を開始する
    public void Start_Random_Raise(float span) {        
        StartCoroutine("Random_Raise_Cor", span);
    }


    //ランダムにブロック上下を終了
    public void Quit_Random_Raise() {
        StopCoroutine("Random_Raise_Cor");
    }


    //ブロックを指定した順番に上下させる
    public void Start_Blocks_Raise(List<int> order, float span) {
        StartCoroutine(Blocks_Raise_Cor(order, span));
    }


    //ブロックを停止させる
    public void Freeze_Blocks() {
        for(int i = 0; i < block_Num; i++) {
            if (blocks[i].Get_State() == GridGroundController.State.idle)
                continue;
            blocks[i].Freeze();
        }
    }


    //浮いているブロックの中からランダムにショットを打つ
    public void Start_Random_Shoot(float span) {
        StartCoroutine("Random_Shoot_Cor", span);
    }

    public void Quit_Random_Shoot() {
        StopCoroutine("Random_Shoot_Cor");
    }


    //ブロックをもとの位置に戻す
    public void Restore_Blocks_To_Original_Pos() {
        for(int i = 0; i < block_Num; i++) {
            if (blocks[i].Get_State() == GridGroundController.State.idle)
                continue;
            blocks[i].Restore_To_Original_Pos();
        }
    }


    //操るブロックの数
    public int Num() {
        return blocks.Count;
    }

    //==============================================================================================

    //ランダムに上昇させる
    private IEnumerator Random_Raise_Cor(float span) {
        if (blocks.Count == 0)
            yield break;
        List<GridGroundController> list;
        while (true) {
            list = Idle_Block_List();
            list[Random.Range(0, list.Count)].Start_Raise(1.5f);
            yield return new WaitForSeconds(span);
        }
    }


    //指定した準場に上昇させる
    private IEnumerator Blocks_Raise_Cor(List<int> order, float span) {
        if (blocks.Count == 0)
            yield break;

        for(int i = 0; i < order.Count; i++) {
            int index = order[i];
            if (index < 0)
                index = 0;
            else if (index > blocks.Count - 1)
                index = blocks.Count - 1;

            if (blocks[index].Get_State() == GridGroundController.State.idle) {
                blocks[index].Start_Raise(1.5f);
            }

            yield return new WaitForSeconds(span);
        }
    }


    //ランダムにショット撃つ
    private IEnumerator Random_Shoot_Cor(float span) {
        if (blocks.Count == 0)
            yield break;
        if(Active_Block_List().Count == 0) {
            StartCoroutine("Random_Raies_Cor", 0.2f);
            yield return new WaitForSeconds(1.0f);
            Quit_Random_Raise();
        }

        Freeze_Blocks();
        List<GridGroundController> list;
        while (true) {
            list = Active_Block_List();
            int index = Random.Range(0, list.Count);
            while (list[index].Is_Nearly_Player()) {
                index = Random.Range(0, list.Count);
            }
            list[index].StartCoroutine("Shoot_Cor");
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


    private List<GridGroundController> Active_Block_List() {
        List<GridGroundController> list = new List<GridGroundController>();
        for (int i = 0; i < block_Num; i++) {
            if (blocks[i].Get_State() != GridGroundController.State.idle)
                list.Add(blocks[i]);
        }
        return list;
    }
   
}
