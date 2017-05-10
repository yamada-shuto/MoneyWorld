//  title : タレットクラス　
//  update: 2017/04/24
//  author: 村端優真


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


//  ユニットのタレットに関するクラス
public class Turret : MonoBehaviour {

    public String EnemyTag;


    //  タレットステータス
    [SerializeField]
    private int m_hp;             //  HP
    [SerializeField]
    private int m_power;          //  攻撃力
    [SerializeField]
    public float m_range;        //  射程
    [SerializeField]
    private float coolTime;       //  クールタイム        
     
    private float coolTimer;    //    現在のクールタイム
    GameObject enemy;
   


	// Use this for initialization
	void Start () {
        coolTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {

        //  敵を探索する
        enemy = serchEnemy(EnemyTag);

        //  クールタイムを終えていたら
        if (coolTimer <= 0)
        {
            //  敵を認識できたら
            if (enemy != null)
            {
                //  攻撃する
                Attack(enemy);
                //  クールタイマーをリセットする
                coolTimer = coolTime;

                Debug.Log("敵を攻撃する");
            }
        }
   


        coolTimer -= 1 % Time.time;
    }

    //============================
    //  敵に向かって攻撃
    //  敵
    //  なし
    //============================
    void Attack(GameObject enemy)
    {
        //  プレハブの弾丸を作成する
        GameObject bullet = GameObject.Instantiate((GameObject)Resources.Load("Resources/Bullet"));


    }

    // ===========================
    //  一番近い敵を取得
    //============================
    GameObject serchEnemy(string tagName)
    {
        float dis = 0;           //距離
        float nearDis = 0;          //最も近いオブジェクトの距離        
        GameObject targetObj = null; //オブジェクト

        

        //  指定したタグのオブジェクトをすべて取得する
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tagName))
        {

            //  自身と取得したオブジェクトの距離を取得
            dis = Vector3.Distance(obj.transform.position, this.transform.position);

            //  より近いオブジェクトか、距離が０のオブジェクトなら更新
            if (nearDis > dis || dis <= 0)
            {
                nearDis = dis;          //  距離を保存            
                targetObj = obj;        //  ターゲットを更新

                //  敵を見つけたらログを表示
                Debug.Log("敵を発見");

            }
            

        }

        

        //最も近かったオブジェクトを返す
        return targetObj;
    }

}
