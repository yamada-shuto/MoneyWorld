using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManeger : MonoBehaviour {

    [SerializeField]
    public float rayDistance;                                      　// レイの長さ

    public Ray ray;                                 

    public LayerMask UnitLayer;                                     //  ユニットレイヤー
    public LayerMask GroundLayer;                                   //  地面レイヤー

    private GameObject miniUnit;                                    //  コントローラーにだす小さいユニット                        

    RaycastHit hitUnit;                                             //  当たったオブジェクトの情報
    RaycastHit hitGround;                                           //  当たった地面の情報

    bool isGrab;                                                    //  ユニットをつかんでいるか
    bool canSet;                                                    //  設置できるか

    SteamVR_TrackedObject trackedObject;
    SteamVR_Controller.Device device ;

    Vector3 unitSettingPos;                                         //  ユニットを設置する座標

    GameObject Unit;                                                 //  ユニット

    //  定数を宣言=================================================================
    const int CHOOSE = 0;
    const int INSTALL = 1;

    int nowMode;                                                    //  現在のコントローラーのモード

    // Use this for initialization
    void Start () {
        //  SteamVRのデバイスを取得
        trackedObject = GetComponent<SteamVR_TrackedObject>();
        device = SteamVR_Controller.Input((int)trackedObject.index);

        //  レイを取得
        ray = new Ray(transform.position, transform.forward);

        isGrab = false;
        canSet = false;


        //  モードを設定
        nowMode = CHOOSE;
    }
	
    
    
    // Update is called once per frame
	void Update () {
       
        //  レイのポジションを更新
        ray.origin = transform.position;
        ray.direction = transform.forward;


        switch(nowMode)
        { 
            //  ユニットの選択モード
            case CHOOSE:               
                GrabUnit();             //  トリガーを引いてユニットを決定
               
                break;
            //  ユニット設置モード  
            case INSTALL:               
                //  設置可能な地面か？
                if (GetGroundPos() == true)
                    SetUnit();          //  トリガーを押すとユニットを設置する

                break;

               
        }


        // Rayを表示   開始地点    長さ　　　　      　色
        Debug.DrawRay(ray.origin, ray.direction *10, Color.red);
    }



    //  Unitにレイが当たったらつかむことができる
    void GrabUnit()
    {
        //  何か物をつかんでいなかったらレイを飛ばす
        if (isGrab == false)
        {
            if (Physics.Raycast(ray, out hitUnit, rayDistance, UnitLayer))
            {
                //  コントローラーのトリガーが押されていたら
                if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
                {
                    //  対象のクローンを作成する
                    GameObject obj = GameObject.Find(hitUnit.collider.name);
                    miniUnit = Instantiate(obj);

                    //  コントローラーと接続
                    miniUnit.transform.parent = transform;
                    //  ミニユニットを少し小さくする
                    miniUnit.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
                    //  ミニユニットをコントローラーの座標より少し上に設定
                    miniUnit.transform.position = new Vector3(transform.position.x, transform.position.y + 0.05f, transform.position.z);
                    //  ミニユニットをコントローラーと水平にする
                    miniUnit.transform.rotation = transform.rotation;
                    //  重力を切る
                    miniUnit.transform.GetComponent<Rigidbody>().useGravity = false;
                    //  コライダーを切る
                    miniUnit.transform.GetComponent<Collider>().enabled = false;
                    //  つかむフラグをオン
                    isGrab = true;
                    //  プレイヤーモードを設置モードにする
                    nowMode = INSTALL;
                }
            }

        }
       
    }


    //  地面との交点を取得
    bool GetGroundPos()
    {
        //  地面との距離を測定
        if (Physics.Raycast(ray, out hitGround, rayDistance, GroundLayer))
        {
            //  当たった地面の座標を保存
            unitSettingPos = new Vector3(hitGround.point.x, hitGround.point.y, hitGround.point.z);

            return true;
        }
        return false;
    }

    //  ユニットを設置する
    void SetUnit()
    {
        //  トリガーが押されたらユニットを設置する
        if(device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            //  重力を切る
            miniUnit.transform.GetComponent<Rigidbody>().useGravity = true;
            //  コライダーを切る
            miniUnit.transform.GetComponent<Collider>().enabled = true;
            //  大きさを元に戻す
            miniUnit.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            //  指定した座標にユニットを移動させる
            miniUnit.transform.position = new Vector3(unitSettingPos.x, unitSettingPos.y + 0.5f ,  unitSettingPos.z);
            //  傾けない
            

            isGrab = false;
            //  モードを選ぶモードにする
            nowMode = CHOOSE;
            //  子を自由にする
            miniUnit.transform.parent = null;

        }
    }
}


////  Unitにレイが当たったらつかむことができる
//void GrabUnit()
//{
//    //  何か物をつかんでいなかったらレイを飛ばす
//    if (isGrab == false)
//    {
//        if (Physics.Raycast(ray, out hitUnit, rayDistance, UnitLayer))
//        {
//            //  コントローラーのトリガーが押されていたら
//            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
//            {
//                hitUnit.transform.parent = transform;           //  コントローラーと接続
//                hitUnit.rigidbody.useGravity = false;          // 　重力を切る
//                hitUnit.collider.enabled = false;               //  コライダーを切る
//                isGrab = true;                              //  つかむフラグをオン
//            }
//        }

//    }
//    //  物をつかんでたら
//    else
//    {
//        //  コントローラーのトリガーが離されたら
//        if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
//        {

//            hitUnit.collider.enabled = true;               //  コライダーをオン
//            hitUnit.rigidbody.useGravity = true;          // 　重力をかける
//            hitUnit.transform.parent = null;                //  コントローラーから離す
//            isGrab = false;

//        }
//    }
//}