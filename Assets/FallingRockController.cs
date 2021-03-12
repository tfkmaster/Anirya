using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockController : Ennemy
{
    private bool rockLaunched = false;
    public float rotationSpeed;
    public float fallSpeed;
    public ParticleSystem[] FXs;
    public ParticleSystem Explosion;
    public AudioClip RockExplosion;

    // Start is called before the first frame update
    protected override void Start()
    {
        LaunchRock();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (rockLaunched)
        {
            
            gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, gameObject.transform.rotation.eulerAngles.z + Time.deltaTime * rotationSpeed);
            gameObject.transform.parent.transform.position -= new Vector3(0, Time.deltaTime * fallSpeed, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().OnHit(this.gameObject, damageDone);
            StopFXs();
            GetComponentInParent<AudioSource>().clip = RockExplosion;
            GetComponentInParent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            GetComponentInParent<AudioSource>().Play();
            Destroy(gameObject);
            Instantiate(Explosion, gameObject.transform.parent.transform.position, new Quaternion());
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StopFXs();
            GetComponentInParent<AudioSource>().clip = RockExplosion;
            GetComponentInParent<AudioSource>().pitch = Random.Range(0.8f, 1.2f);
            GetComponentInParent<AudioSource>().Play();
            Destroy(gameObject);
            Instantiate(Explosion, gameObject.transform.parent.transform.position,new Quaternion());
        }
    }

    public void LaunchRock()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        rockLaunched = true;
    }

    private void StopFXs()
    {
        foreach(ParticleSystem ps in FXs)
        {
            ps.Stop();
        }
    }
}
