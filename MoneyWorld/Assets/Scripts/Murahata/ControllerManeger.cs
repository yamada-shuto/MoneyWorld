using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerManeger : MonoBehaviour {

    [SerializeField]
    public float rayDistance;
    public Ray ray;
    public LayerMask mask;
    private RaycastHit hit;

    // Use this for initialization
    void Start () {
        //  レイを取得
        ray = new Ray(transform.position, transform.forward);
	}
	
	// Update is called once per frame
	void Update () {
        //  SteamVRのデバイスを取得
        SteamVR_TrackedObject trackedObject = GetComponent<SteamVR_TrackedObject>();
        var device = SteamVR_Controller.Input((int)trackedObject.index);

      

        //  レイのポジションを更新
        ray.origin = transform.position;
        ray.direction = transform.forward;

        if(Physics.Raycast(ray,out hit, rayDistance,mask))
        {
            //  コントローラーのトリガーが押されていたら
            if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                hit.transform.parent = transform;           //  コントローラーと接続
                hit.rigidbody.useGravity = false;          // 　重力を切る
            }

            //  コントローラーのトリガーが離されたら
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                hit.transform.parent = null;                //  コントローラーから離す
                hit.rigidbody.useGravity = true;          // 　重力をかける
            }
        }

       

        // Rayを表示   開始地点    長さ　　　　　色
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
    }
}
