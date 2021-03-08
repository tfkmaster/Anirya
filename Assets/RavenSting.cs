using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenSting : StateMachineBehaviour
{
    private Raven raven;
    private float thrustMult = 2.0f;

    private float distToPlayer;
    private float traveledDist;
    private Vector2 startPos;
    private Vector2 stingDirection;

    public float MAX_STING_MAGNITUDE = 15f;

    private float playerReachedTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        raven = animator.GetComponent<Raven>();
        distToPlayer = Vector3.Distance(raven.MoveTo.position, raven.transform.position);
        traveledDist = 0f;
        playerReachedTimer = 0.02f;
        startPos = raven.transform.position;
        stingDirection = raven.MoveTo.position - raven.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        traveledDist = Vector3.Distance(startPos, raven.transform.position);

        if (raven.rb2D.velocity.magnitude > MAX_STING_MAGNITUDE)
        {
            raven.rb2D.velocity *= 1 - ((raven.rb2D.velocity.magnitude / MAX_STING_MAGNITUDE) - 1f);
        }


        raven.rb2D.AddForce(stingDirection.normalized * raven.thrust * thrustMult, ForceMode2D.Impulse);

        if (raven.IsColliding())
        {
            animator.SetBool("IsStunned", true);
            animator.SetBool("IsStung", false);
        }

        if(traveledDist >= distToPlayer)
        {
            playerReachedTimer -= Time.deltaTime;
        }

        if(playerReachedTimer <= 0f)
        {
            animator.SetBool("IsStunned", true);
            animator.SetBool("IsStung", false);
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
