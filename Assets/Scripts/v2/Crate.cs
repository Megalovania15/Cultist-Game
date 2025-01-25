using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace V2
{
    public class Crate : MonoBehaviour
    {
        private Spawner spawner;

        private void Start()
        {
            spawner = FindObjectOfType<Spawner>();
        }

        public void DropObject()
        {
            if (spawner.SpawnPrefabCount > 0)
            {
                Instantiate(spawner.GetNextSpawnPrefab(), transform.position, Quaternion.identity);
            }
        }
    }
}
