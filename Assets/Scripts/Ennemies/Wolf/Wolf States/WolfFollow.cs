using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfFollow : StateMachineBehaviour
{
    private Wolf wolf;
    private AIWolf aiWolf;
    private Rigidbody2D rb2dWolf;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wolf = animator.gameObject.GetComponent<Wolf>();
        aiWolf = animator.gameObject.GetComponent<AIWolf>();
        rb2dWolf = animator.gameObject.GetComponent<Rigidbody2D>();
        animator.gameObject.GetComponent<SpriteRenderer>().color = new Color(150f, 0, 0);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (wolf.player.transform.position.x + aiWolf.nearPlayerStop < animator.gameObject.transform.position.x)
        {
            rb2dWolf.velocity = new Vector2(-wolf.FollowSpeed, rb2dWolf.velocity.y);
        }
        else if(wolf.player.transform.position.x - aiWolf.nearPlayerStop > animator.gameObject.transform.position.x)
        {
            rb2dWolf.velocity = new Vector2(wolf.FollowSpeed, rb2dWolf.velocity.y);
        }
        else
        {
            rb2dWolf.velocity = Vector2.zero;
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
