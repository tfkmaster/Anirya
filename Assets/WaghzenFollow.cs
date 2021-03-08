using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaghzenFollow : StateMachineBehaviour
{
    private Rigidbody2D rb2dWaghzen;
    private Waghzen waghzen;
    public float MinTime;
    public float MaxTime;
    private float timer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb2dWaghzen = animator.gameObject.GetComponentInParent<Rigidbody2D>();
        waghzen = animator.gameObject.GetComponentInParent<Waghzen>();
        timer = Random.Range(MinTime, MaxTime);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            animator.SetTrigger("Jump");
        }
        else
        {
            if (!waghzen.player.GetComponent<Player>().GetDead())
            {
                if (waghzen.player.transform.position.x < animator.GetComponentInParent<Transform>().position.x)
                {
                    rb2dWaghzen.velocity = new Vector2(-waghzen.FollowSpeed, rb2dWaghzen.velocity.y);
                }
                else if (waghzen.player.transform.position.x > animator.GetComponentInParent<Transform>().transform.position.x)
                {
                    rb2dWaghzen.velocity = new Vector2(waghzen.FollowSpeed, rb2dWaghzen.velocity.y);
                }
                else
                {
                    rb2dWaghzen.velocity = Vector2.zero;
                }
            }
            else
            {
                rb2dWaghzen.velocity = Vector2.zero;
                animator.SetBool("playerDead", true);
            }
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
