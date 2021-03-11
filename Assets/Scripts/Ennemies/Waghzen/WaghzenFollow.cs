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

    private bool playerMeleeRanged = false;
    private bool playerMidRanged = false;
    private bool playerHighRanged = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerMeleeRanged = false;
        playerMidRanged = false;
        playerHighRanged = false;
        rb2dWaghzen = animator.gameObject.GetComponentInParent<Rigidbody2D>();
        waghzen = animator.gameObject.GetComponentInParent<Waghzen>();
        timer = Random.Range(MinTime, MaxTime);

        animator.SetBool("wasWandering", false);
    }

    //OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Vector2 pos = animator.GetComponentInParent<Transform>().transform.position;
            Vector2 a = animator.GetComponentInParent<Waghzen>().attackPoint.position;
            Vector2 b = animator.GetComponentInParent<Waghzen>().attackPoint2.position;
            Vector2 c = animator.GetComponentInParent<Waghzen>().attackPoint3.position;

            Collider2D[] MeleeRangeAttackColliders = Physics2D.OverlapBoxAll((pos + a) / 2, new Vector2(Vector3.Distance(animator.GetComponentInParent<Transform>().position, animator.GetComponentInParent<Waghzen>().attackPoint.position), animator.GetComponentInParent<Waghzen>().attackRange), 0);

            foreach (Collider2D actor in MeleeRangeAttackColliders)
            {
                if (actor.CompareTag("Player") && !playerMeleeRanged)
                {
                    playerMeleeRanged = true;
                }
            }

            if (!playerMeleeRanged)
            {
                Collider2D[] MidRangeAttackColliders = Physics2D.OverlapBoxAll((a + b) / 2, new Vector2(Vector3.Distance(animator.GetComponentInParent<Waghzen>().attackPoint.position, animator.GetComponentInParent<Waghzen>().attackPoint2.position), animator.GetComponentInParent<Waghzen>().attackRange), 0);
                foreach (Collider2D actor in MidRangeAttackColliders)
                {
                    if (actor.CompareTag("Player") && !playerMidRanged)
                    {
                        playerMidRanged = true;
                    }
                }

                if (!playerMidRanged)
                {
                    Collider2D[] HighRangeAttackColliders = Physics2D.OverlapBoxAll((b + c) / 2, new Vector2(Vector3.Distance(animator.GetComponentInParent<Waghzen>().attackPoint2.position, animator.GetComponentInParent<Waghzen>().attackPoint3.position), animator.GetComponentInParent<Waghzen>().attackRange), 0);
                    foreach (Collider2D actor in HighRangeAttackColliders)
                    {
                        if (actor.CompareTag("Player") && !playerHighRanged)
                        {
                            playerHighRanged = true;
                        }
                    }
                }
            }

            if (playerMeleeRanged)
            {
                animator.SetTrigger("Melee");
            }
            else if (playerMidRanged)
            {
                animator.SetTrigger("Jump");
            }
            else if (playerHighRanged)
            {
                animator.SetTrigger("Range");
            }
        }
        /*else
        {*/
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
        //}

        playerMeleeRanged = false;
        playerMidRanged = false;
        playerHighRanged = false;
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
