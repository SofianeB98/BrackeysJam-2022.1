using UnityEngine;

public class PersistentObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject peristentObjectPrefab = null;

    private static bool hasSpawned = false;
        
    private void Awake()
    {
        var go = GameObject.Find("PersistentObjects");
        if (go != null && PersistentObjectSpawner.hasSpawned)
            return;
            
        SpawnPersistentObjects();
            
        PersistentObjectSpawner.hasSpawned = true;
    }

    private void SpawnPersistentObjects()
    {
        GameObject persistentObject = Instantiate(this.peristentObjectPrefab);
    }
}