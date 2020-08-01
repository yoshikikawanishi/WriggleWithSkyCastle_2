using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager> {

    //ステータス
    //必ずSetter, Getterとかを使うこと
    public enum Option {
        none,
        bee,
        butterfly,
        mantis,
        spider,
    };
    [SerializeField] Option option = Option.none;
    [SerializeField] private int life = 3;
    [SerializeField] private int stock = 3;
    [SerializeField] private int power = 0;
    [SerializeField] private int score = 0;        

    //上限値
    public const int MAX_LIFE = 9;
    public const int MAX_STOCK = 9;
    public const int MAX_POWER = 400;
    public const int MAX_SCORE = 9999999;
    //スコア50000点おきに残機アップ
    private const int STOCK_UP_SCORE = 50000;


    private new void Awake() {
        base.Awake();
        #if UNITY_EDITOR
        Set_Life(DebugModeManager.Instance.Player_Life);
        Set_Power(DebugModeManager.Instance.Player_Power);
        Set_Option(DebugModeManager.Instance.Player_Option);
        #endif
    }


    //Reduce
    public int Reduce_Life() {
        life--;
        if(life == 0) {
            GameManager.Instance.Miss();
        }
        return life;
    }

    public int Reduce_Stock() {
        stock--;              
        return stock;
    }        


    //Add
    public void Add_Life() {
        if (life < MAX_LIFE) {
            life++;
        }
    }
    
    public void Add_Stock() {
        if (stock < MAX_SCORE) {            
            stock++;
        }
    }

    public void Add_Power() {
        if (power < MAX_POWER) {
            power++;
        }
    }

    public void Add_Score(int value) {
        //スコア2万点おきに残機アップ
        if ((score + value) / STOCK_UP_SCORE > score / STOCK_UP_SCORE)
            Add_Stock();

        score += value;
        if (score > MAX_SCORE) {
            score = MAX_SCORE;
        }
    }

    
    //Getter
    public int Get_Life() {
        return life;
    }

    public int Get_Stock() {
        return stock;
    }

    public int Get_Power() {
        return power;
    }

    public int Get_Score() {
        return score;
    }

    public Option Get_Option() {
        return option;
    }
    

    //Setter
    public void Set_Life(int life) {
        if (life > MAX_LIFE) {
            return;
        }
        if (life >= 0) {
            this.life = life;
        }
        if (life == 0) {
            GameManager.Instance.Miss();
        }
    }

    public void Set_Stock(int stock) {  
        if(stock > MAX_STOCK) {
            return;
        }   
        this.stock = stock;       
    }  
    
    public void Set_Power(int power) {
        if(power > MAX_POWER) {
            return;
        }
        if(power < 0) {
            power = 0;
            return;
        }
        this.power = power;
    }

    public void Set_Score(int score) {
        if(score > MAX_SCORE) {
            return;
        }
        if(score < 0) {
            score = 0;
            return;
        }
        //スコア2万点おきに残機アップ
        if (score / STOCK_UP_SCORE > this.score / STOCK_UP_SCORE)
            Add_Stock();

        this.score = score;
    }

    public void Set_Option(Option option) {
        this.option = option;
    }

    public void Set_Option(string option_Name) {
        foreach(Option value in Enum.GetValues(typeof(Option))) {
            if(value.ToString() == option_Name) {
                this.option = value;
            }
        }
    }

}
