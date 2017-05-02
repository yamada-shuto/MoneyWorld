using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // キャラクターコントローラー
    //private CharacterController m_EnemyController;
    // ナビゲーター
    private UnityEngine.AI.NavMeshAgent m_Agent;
    // アニメーター
    private Animator m_Animator;
    // 目的地
    public Vector3 m_Destination;
    // 歩く速度
    public float m_WalkSpeed;
    // 移動する方向と速度
    private Vector3 m_Velocity;
    // 方向
    private Vector3 m_Direction;
    // 到着したか判定用
    private bool m_isArrived;
    // Use this for initialization
    void Start ()
    {
        //// キャラクターコントローラーを取得する
        //m_EnemyController = GetComponent<CharacterController>();
        // 
        m_Agent = GetComponent<NavMeshAgent>();
        // アニメーターを取得する
        m_Animator = GetComponent<Animator>();
        // 移動する方向と速度を初期化
        m_Velocity = Vector3.zero;
        // 到着したかの判定用
        m_isArrived = false;
        // 目的地を設定
        m_Agent.SetDestination(m_Destination);
	}

    // Update is called once per frame
    void Update()
    {
        // 到着していたらなにもしない
        if (m_isArrived)
            return;
        // アニメーターのSpeedの値を設定する
        m_Animator.SetFloat("Speed", m_Agent.speed);
        // 目的地から現在の位置までの距離が1未満か判定する
        if (Vector3.Distance(m_Destination, transform.position) < 0.8f)
        {
            // bool型変数を真にする
            m_isArrived = true;
            // アニメーターのSpeedの値に0を設定する
            m_Animator.SetFloat("Speed", 0.0f);
        }
    }
}
