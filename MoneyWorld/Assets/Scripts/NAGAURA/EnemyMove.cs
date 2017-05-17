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
    // 歩く速度
    public float m_WalkSpeed;
    // 移動する方向と速度
    private Vector3 m_Velocity;
    // 方向
    private Vector3 m_Direction;
    // 到着したか判定用
    private bool m_isArrived;
    // スクリプト
    private Destination m_Destination;
    // Use this for initialization
    void Start ()
    {
        //// キャラクターコントローラーを取得する
        //m_EnemyController = GetComponent<CharacterController>();
        // エージェントを取得する
        m_Agent = GetComponent<NavMeshAgent>();
        // アニメーターを取得する
        m_Animator = GetComponent<Animator>();
        // 到着したかの判定用
        m_isArrived = false;
        // スクリプトを取得する
        m_Destination = GetComponent<Destination>();
        // 目的地を設定する
        m_Destination.SetDestination();
        // エージェントの目的地を設定する
        if (m_Agent.pathStatus != NavMeshPathStatus.PathInvalid){m_Agent.SetDestination(m_Destination.GetDestination()); }
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
        if (Vector3.Distance(m_Destination.GetDestination(), transform.position) < 0.8f)
        {
            // アニメーターのSpeedの値に0を設定する
            m_Animator.SetFloat("Speed", 0.0f);
            // 目的地を設定する
            m_Destination.SetDestination();
            // エージェントの目的地を設定する
            m_Agent.SetDestination(m_Destination.GetDestination());
        }
    }
}
