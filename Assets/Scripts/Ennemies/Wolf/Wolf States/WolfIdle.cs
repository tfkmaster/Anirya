﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfIdle : StateMachineBehaviour
{
    private float counter;
    bool startCounter;

    public float MinTime;
    public float MaxTime;
    private float timer;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        counter = 0;
        startCounter = false;

        timer = Random.Range(MinTime, MaxTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (startCounter)
        {
            counter += Time.deltaTime;
            if(counter >= animator.gameObject.GetComponentInParent<Wolf>().waitTime)
            {
                animator.SetBool("playerGrounded", true);
            }
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            animator.SetTrigger("StartWandering");
        }

            if (animator.gameObject.GetComponentInParent<Wolf>().player.GetComponent<CharacterController2D>().m_Grounded)
        {
            startCounter = true;
        }
        else
        {
            animator.SetBool("playerGrounded", false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
