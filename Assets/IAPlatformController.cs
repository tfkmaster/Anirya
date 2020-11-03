using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPlatformController : MonoBehaviour
{
    void Awake()
    {
        transform.GetChild(0).transform.localPosition = new Vector3(
            -GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.GetChild(0).transform.localScale.x - 0.1f,
            GetComponent<SpriteRenderer>().sprite.bounds.extents.y * transform.GetChild(0).transform.localScale.y + 0.3f,
            transform.GetChild(0).transform.position.z);

        transform.GetChild(1).transform.localPosition = new Vector3(
            GetComponent<SpriteRenderer>().sprite.bounds.extents.x * transform.GetChild(0).transform.localScale.x + 0.1f,
            GetComponent<SpriteRenderer>().sprite.bounds.extents.y * transform.GetChild(0).transform.localScale.y + 0.3f,
            transform.GetChild(0).transform.position.z);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
