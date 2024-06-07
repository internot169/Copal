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

    public GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        // grab turn count from game manager, then scale count according to turn count. 
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        count += gm.turns * 2;

        // spawn the number of enemies. 
        for (int i = 0; i < count; i++){
            //pick position within a circular range of the player spawner. 
            float x = Random.Range(-range, range);
            // the flat plane is the XZ plane in unity. Y is the height(so to speak).
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
