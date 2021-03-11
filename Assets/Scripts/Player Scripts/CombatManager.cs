using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CombatManager : MonoBehaviour
{
    public bool canReceiveInput = true;                 // For determining if the character is able to receive an input 
    public bool inputReceived;                          // Checks if an attackInput has been pressed down
    
    public bool isHealing = false;
    public float HoldRegenTime = 1.5f;
    private float holdRegenCounter = 0;

    private CharacterMovement CM;                       
    private CharacterController2D CC2d;
    private Player player;
    [SerializeField] private float AttackMoveForce;
    private bool alreadyDamaged;

    public GameObject HitParticles;
    public ParticleSystem RegenParticles;
    public ParticleSystem DamageParticles;


    public static CombatManager CMInstance;

    //store all cameras in the current scene
    public Cinemachine.CinemachineBrain CBrain;
    private float ortho_size = 0f;
    private bool reset_ortho = true;
    private bool zoom_out = false;

    private float MAX_ORTHO_DIFF = 0.5f;
    private float ortho_diff = 0f;

    void Awake()
    {
        CMInstance = this;
        CM = GetComponent<CharacterMovement>();
        CC2d = GetComponent<CharacterController2D>();
        player = GetComponent<Player>();
        
    }

    private void Start()
    {
        CBrain = FindObjectOfType<Cinemachine.CinemachineBrain>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!CM.player.GM.isPaused)
        {
            Attack();

            if (GetComponent<Player>().GM.alimMet)
            {
                Regen();
            }
        }
        
    }

    public void getBrain()
    {
        CBrain = FindObjectOfType<Cinemachine.CinemachineBrain>();
    }

    //Checks if the player has pushed attack button
    public void Attack()
    {
        if (Input.GetButtonDown("Fire1") && GetComponent<CharacterController2D>().m_Grounded)
        {
            if (canReceiveInput)
            {
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, GetComponent<Rigidbody2D>().velocity.y);
                CM.SetCanMove(false);
                canReceiveInput = false;
                inputReceived = true;
                //CC2d.getRigidbody().AddForce(new Vector2(CC2d.facingDirection() * AttackMoveForce, 0), ForceMode2D.Impulse);
            }
            else
            {
                return;
            }
        }
        else if (!GetComponent<CharacterController2D>().m_Grounded && Input.GetButtonDown("Fire1"))
        {
            GetComponentInChildren<Animator>().SetBool("AerialAttack",true);
        }
    }

    public void Regen()
    {
        if (Input.GetButtonDown("Fire2") && player.actualHeat >= player.RegenCost)
        {
            startRegen();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if (isHealing)
            {
                StopAllCoroutines();
                StartCoroutine(ZoomOut());
                isHealing = false;
                CM.SetCanMove(true);
                canReceiveInput = true;
            }
            GetComponentInChildren<Animator>().SetBool("stopRegen", true);
            GetComponentInChildren<Animator>().SetBool("startRegen", false);
            CM.ableToMove = false;
            RegenParticles.Stop();
        }
        
        if (isHealing)
        {
            holdRegenCounter += Time.deltaTime;
            player.actualHeat -= Time.deltaTime * player.RegenCost / HoldRegenTime;
            Zoom();

            if (player.actualHeat <= 1)
            {
                holdRegenCounter = 0;
                isHealing = false;
                GetComponentInChildren<Animator>().SetBool("stopRegen", true);
                GetComponentInChildren<Animator>().SetBool("startRegen", false);
                CM.SetCanMove(true);
                CM.ableToMove = false;
                canReceiveInput = true;
                RegenParticles.Stop();
                StartCoroutine(ZoomOut());
            }

            if (holdRegenCounter >= HoldRegenTime)
            {
                Debug.Log(player.healthPoints);
                if (player.healthPoints != player.maxHealthPoints)
                {
                    player.healthPoints += 1;
                    holdRegenCounter = 0;
                    Debug.Log(player.healthPoints);
                }
                else if (player.actualHeat < player.RegenCost)
                {
                    holdRegenCounter = 0;
                    isHealing = false;
                    GetComponentInChildren<Animator>().SetBool("stopRegen",true);
                    GetComponentInChildren<Animator>().SetBool("startRegen", false);
                    CM.SetCanMove(true);
                    CM.ableToMove = false;
                    canReceiveInput = true;
                    RegenParticles.Stop();
                    StartCoroutine(ZoomOut());
                }
            }
            player.SendPlayerStatsToGameManager();
        }

    }

    //Function called on the animation to determines the ennemies who'll get hit by the attack
    public void CheckHit()
    {
        DamageParticles.Play();
        if (GetComponent<Player>().GM.alimMet)
        {
            List<GameObject> ActorsAlreadyDamaged = new List<GameObject>();
            Collider2D[] hitActors = Physics2D.OverlapCircleAll(GetComponent<Player>().attackPoint.position, GetComponent<Player>().attackRange);
            foreach (Collider2D actor in hitActors)
            {
                bool alreadySeen = false;
                foreach (GameObject actorDamaged in ActorsAlreadyDamaged)
                {
                    
                    if (actor.GetComponent<Actor>())
                    {
                        if (actor.gameObject == actorDamaged)
                        {
                            alreadySeen = true;
                        }
                    }
                    else if (actor.GetComponentInParent<Actor>())
                    {
                        if (actor.GetComponentInParent<Actor>().gameObject == actorDamaged)
                        {
                            alreadySeen = true;
                        }
                    }
                    
                }

                if (alreadySeen)
                {
                    break;
                }
                if (actor.GetComponent<Actor>())
                {
                    if (!actor.isTrigger && (actor.CompareTag("Ennemy") && !actor.GetComponent<Actor>().GetDead() || actor.CompareTag("Destructible Wall")))
                    {
                        ActorsAlreadyDamaged.Add(actor.gameObject);
                        AddHeat();
                        GameObject a = Instantiate(HitParticles, GetComponent<Player>().attackPoint.position, new Quaternion());
                        a.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90 * CC2d.facingDirection());
                        actor.GetComponent<Actor>().OnHit(gameObject, GetComponent<Player>().GetDamageDone());
                    }
                }
                else if(actor.GetComponentInParent<Actor>())
                {
                    if (!actor.isTrigger && (actor.CompareTag("Ennemy") && !actor.GetComponentInParent<Actor>().GetDead() || actor.CompareTag("Destructible Wall")))
                    {
                        ActorsAlreadyDamaged.Add(actor.GetComponentInParent<Actor>().gameObject);
                        AddHeat();
                        GameObject a = Instantiate(HitParticles, GetComponent<Player>().attackPoint.position, new Quaternion());
                        a.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -90 * CC2d.facingDirection());
                        actor.GetComponentInParent<Actor>().OnHit(gameObject, GetComponent<Player>().GetDamageDone());
                    }
                }
            }
        }
    }
    



        public void PlaySound()
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Text sound/Button next", GetComponent<Transform>().position);
        
        }
    public void AddHeat()
    {
        if (GetComponent<Player>().actualHeat + GetComponent<Player>().AddHeat <= 100)
        {
            GetComponent<Player>().actualHeat += GetComponent<Player>().AddHeat;
        }
        else
        {
            GetComponent<Player>().actualHeat = 100;
        }
        GetComponent<Player>().SendPlayerStatsToGameManager();
    }

    public void Zoom()
    {
        zoom_out = false;
        Cinemachine.CinemachineVirtualCamera cam = CBrain.ActiveVirtualCamera as Cinemachine.CinemachineVirtualCamera;

        if (ortho_size == 0f && reset_ortho)
        {
            ortho_size = cam.m_Lens.OrthographicSize;
            reset_ortho = false;
        }
        if(ortho_diff <= MAX_ORTHO_DIFF)
        {
            cam.m_Lens.OrthographicSize -= 0.01f;
            ortho_diff += 0.01f;
        }
    }

    void startRegen()
    {
        StopAllCoroutines();
        isHealing = true;
        CM.SetCanMove(false);
        canReceiveInput = false;
        if(GetComponentInChildren<Animator>())
        GetComponentInChildren<Animator>().SetBool("startRegen",true);
        CM.ableToMove = true;
        if (!RegenParticles.isPlaying)
        {
            RegenParticles.Play();
        }
    }

   IEnumerator ZoomOut()
    {
        Cinemachine.CinemachineVirtualCamera cam = CBrain.ActiveVirtualCamera as Cinemachine.CinemachineVirtualCamera;
        while (cam.m_Lens.OrthographicSize <= ortho_size) {
            yield return new WaitForSeconds(Time.deltaTime);
            cam.m_Lens.OrthographicSize += 0.01f;
            ortho_diff -= 0.01f;
        }
            cam.m_Lens.OrthographicSize = ortho_size;
            ortho_size = 0f;
            reset_ortho = true;
            yield break;
    }

}
