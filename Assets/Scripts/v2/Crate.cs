using UnityEngine;

namespace V2
{
    // Main script controlling the Crate game object. When a crate is destroyed
    // creates an object in its place.
    public class Crate : MonoBehaviour
    {
        // Spawner used to determine what object should be randomly dropped.
        private Spawner spawner;

        private void Start()
        {
            spawner = FindObjectOfType<Spawner>();
        }

        // Drops an object at the current position, if the spawner allows it,
        // else is empty.
        public void DropObject()
        {
            if (spawner.SpawnPrefabCount > 0)
            {
                Instantiate(spawner.GetNextSpawnPrefab(), transform.position, Quaternion.identity);
            }
        }
    }
}
