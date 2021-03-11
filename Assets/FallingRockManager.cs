using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRockManager : MonoBehaviour
{
    public Transform LimitLeft;
    public Transform LimitRight;
    public GameObject FallingRock;

    public int minRock;
    public int maxRock;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateRocks()
    {
        StartCoroutine(InstanciateRocksEnumerator());
    }

    private IEnumerator InstanciateRocksEnumerator()
    {
        int number = Random.Range(minRock, maxRock);
        for (int i = 0; i < number; i++)
        {
            Instantiate(FallingRock, new Vector3(Random.Range(LimitLeft.transform.position.x, LimitRight.transform.position.x), Random.Range(LimitRight.transform.position.y + 3, LimitRight.transform.position.y - 3), 0), new Quaternion());
            yield return new WaitForSeconds(Random.Range(0.4f,0.6f));
        }
    }


}
