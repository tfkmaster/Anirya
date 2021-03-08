using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavenAttack : StateMachineBehaviour
{
    private Raven raven;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        raven = animator.GetComponent<Raven>();
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (raven.launchSting)
        {
            animator.SetBool("IsStung", true);
            animator.SetBool("IsAttacking", false);
            raven.launchSting = false;
        }

        /*if (raven.launchSting)
        {
            raven.rb2D.gravityScale = 0f;
            raven.rb2D.velocity = Vector2.zero;

            Vector3 direction = raven.MoveTo.position - raven.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if (raven.transform.position.y > raven.player.transform.position.y)
            {
                Debug.Log("rotation : " + raven.transform.rotation);
                raven.transform.rotation = Quaternion.AngleAxis(angle - 180.0f, Vector3.forward);
                Debug.Log("rotation : " + raven.transform.rotation);
            }
            else
            {
                raven.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            raven.rb2D.AddForce(direction.normalized * raven.thrust * thrustMult, ForceMode2D.Impulse);
            raven.launchSting = false;
        } */
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Exit");
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsStung", true);
        //raven.rb2D.gravityScale = 0.1f;
        //raven.transform.rotation = Quaternion.Euler(Vector3.zero);
    }

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
