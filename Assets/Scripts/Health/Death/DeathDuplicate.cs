using System.Collections.Generic;
using UnityEngine;

public class DeathDuplicate : Death
{
    
    public List<GameObject> possibleChildrenPrefabs;

    public int minChildren;
    
    public int maxChildren;
    
    public override void Start()
    {
        //
    }

    public override void Update()
    {
        //
    }

    public override void Die()
    {
        int childCount = Random.Range(minChildren, maxChildren);
        for (int index = 0; index < childCount; index++)
        {
            Instantiate(possibleChildrenPrefabs[Random.Range(0, possibleChildrenPrefabs.Count)], gameObject.transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
