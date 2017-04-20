using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    // キャラクターコントローラー
    //private CharacterController m_EnemyController;
    private UnityEngine.AI.NavMeshAgent m_Agent;
    // アニメーター
    private Animator m_Animator;
    // 目的地
    private Vector3 m_Destination;
    // 歩く速度
    public float m_WalkSpeed;
    // 移動する方向と速度
    private Vector3 m_Velocity;
    // 方向
    private Vector3 m_Direction;
    // 到着したか判定用
    private bool m_isArrived;
    //
    private Vector3 m_Current;
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
        // 
        m_isArrived = false;
        // 目的地を設定
        m_Agent.SetDestination(m_Destination);
        // 
        m_Current = transform.position;
        var randDestination = Random.insideUnitCircle * 8;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // 到着していたらなにもしない
        if (m_isArrived)
            return;
        // 接地しているか判定する
        if (m_EnemyController.isGrounded)
        {
            // 移動する速度と方向を初期化する
            m_Velocity = Vector3.zero;
            // アニメーターのSpeedの値に99を設定する
            m_Animator.SetFloat("Speed", 99.0f);
            // 現在の位置から目的地への方向を計算する
            m_Direction = (m_Destination - transform.position).normalized;
            // 目的地の向きに視線を設定する
            transform.LookAt(new Vector3(m_Destination.x, transform.position.y, m_Destination.z));
            // 移動速度を設定する
            m_Velocity = m_Direction * m_WalkSpeed;
        }
        // 重力をかける
        m_Velocity.y += Physics.gravity.y * Time.deltaTime;
        // 移動する
        m_EnemyController.Move(m_Velocity * Time.deltaTime);
        // 目的地から現在の位置までの距離が1未満か判定する
        if (Vector3.Distance(m_Destination, transform.position) < 0.8f)
        {
            // 
            m_isArrived = true;
            // アニメーターのSpeedの値に0を設定する
            m_Animator.SetFloat("Speed", 0.0f);
        }
    }
    void Walk(Vector3 position)
    {
        if (state == State.chase)
        {
            m_Agent.SetDestination(position);
        }
        m_Animator.SetFloat("Speed", m_Agent.speed);
    }
}
