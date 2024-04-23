using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("prefab to spawn")]
    public GameObject prefab;
    [Header("adjustable values for rooms")]
    public int count;
    [Tooltip("the radius within which to spawn")]
    public float range;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++){
            //pick position:
            float x = Random.Range(-range, range);
            float z = Random.Range(-range, range);

            //TODO: make sure to offset y if we want that. 
            Instantiate(prefab, new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
