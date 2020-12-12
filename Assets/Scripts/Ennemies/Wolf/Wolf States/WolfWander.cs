using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfWander : StateMachineBehaviour
{

    private Wolf wolf;
    private AIWolf aiWolf;
    private Rigidbody2D rb2dWolf;

    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wolf = animator.gameObject.GetComponentInParent<Wolf>();
        aiWolf = animator.gameObject.GetComponentInParent<AIWolf>();
        rb2dWolf = animator.gameObject.GetComponentInParent<Rigidbody2D>();
        aiWolf.SelectANode();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        if (animator.transform.position.x - aiWolf.DestinationNode.transform.position.x >= 0.2f || animator.transform.position.x - aiWolf.DestinationNode.transform.position.x <= -0.2f)
        {
            if (animator.transform.position.x > aiWolf.DestinationNode.transform.position.x)
            {
                rb2dWolf.velocity = new Vector2(-wolf.WanderSpeed, rb2dWolf.velocity.y);
            }
            else if(animator.transform.position.x < aiWolf.DestinationNode.transform.position.x)
            {
                rb2dWolf.velocity = new Vector2(+wolf.WanderSpeed, rb2dWolf.velocity.y);
            }
        }
        else
        {
            aiWolf.SelectANode();
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
