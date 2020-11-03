using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfWander : StateMachineBehaviour
{

    private Wolf wolfScript;
    private Rigidbody2D rb2dWolf;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wolfScript = animator.gameObject.GetComponent<Wolf>();
        rb2dWolf = animator.gameObject.GetComponent<Rigidbody2D>();


    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Mathf.Abs(animator.transform.position.x) - Mathf.Abs(wolfScript.DestinationNode.transform.position.x) <= 0.2f)
        {
            if (animator.transform.position.x <= wolfScript.DestinationNode.transform.position.x)
            {
                rb2dWolf.velocity = new Vector2(wolfScript.speed, rb2dWolf.velocity.y);
            }
            else
            {
                rb2dWolf.velocity = new Vector2(-wolfScript.speed, rb2dWolf.velocity.y);
            }
        }
        else
        {

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
