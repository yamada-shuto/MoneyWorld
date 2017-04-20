using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    // キャラクターコントローラー
    private CharacterController m_CharacterController;
    // アニメーター
    private Animator m_Animator;
    // 移動速度
    private Vector3 m_Velocity;
    // 速度
    public float m_Speed;

    // Use this for initialization
    void Start()
    {
        // キャラクターコントローラーを取得
        m_CharacterController = GetComponent<CharacterController>();
        // アニメーターを取得
        m_Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // 接地してるか判定
        if (m_CharacterController.isGrounded)
        {
            // 移動キーの入力を取得をして変数に代入する
            m_Velocity = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            // 移動速度の値が0.1より大きいか判定
            if (m_Velocity.magnitude > 0.1f)
            {
                // アニメーターのSpeedに移動速度の値を設定する
                m_Animator.SetFloat("Speed", m_Velocity.magnitude);
                // 向いている方向を移動する方向に設定する
                transform.LookAt(transform.position + m_Velocity);
            }
            else
            {
                // アニメーターのSpeedに0を設定する
                m_Animator.SetFloat("Speed", 0.0f);
            }
        }
        // 重力をかける
        m_Velocity.y += Physics.gravity.y * Time.deltaTime;
        // 移動する
        m_CharacterController.Move(m_Velocity * Time.deltaTime * m_Speed);

    }

}
