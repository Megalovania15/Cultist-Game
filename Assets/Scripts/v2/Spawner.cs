using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace V2
{
    // Main script controlling item spawning in a level. Spawns are configured
    // in the editor.
    public class Spawner : MonoBehaviour
    {
        // Defines how many of an item to spawn.
        [Serializable]
        public struct Spawn
        {
            public GameObject prefab;
            public int count;
        }

        // Item instances which may be spawned into the level.
        [SerializeField]
        private Spawn[] spawns;

        // Working set of prefabs to instantiate into the scene. The number of
        // elements here will be the sum of Spawn.count in spawns, as the
        // config is flattened. The list is shuffled to produce a random
        // distribution of items.
        private List<GameObject> spawnPrefabs = new List<GameObject>();

        public int SpawnPrefabCount
        {
            get { return spawnPrefabs.Count; }
        }

        // Shuffles all items configured in the editor to be spawnable and
        // all items required by all Altars.
        private void Start()
        {
            foreach (var spawn in spawns)
            {
                spawnPrefabs.AddRange(Enumerable.Repeat(spawn.prefab, spawn.count));
            }
            var altars = FindObjectsOfType<Altar>();
            foreach (var altar in altars)
            {
                var requirements = altar.Requirements;
                foreach (var requirement in requirements)
                {
                    spawnPrefabs.AddRange(Enumerable.Repeat(requirement.item.prefab, requirement.count));
                }
            }
            new KnuthShuffler().Shuffle(spawnPrefabs);
        }

        // Gets the next random item to spawn.
        public GameObject GetNextSpawnPrefab()
        {
            if (spawnPrefabs.Count == 0) return null;
            var spawn = spawnPrefabs.Last();
            spawnPrefabs.RemoveAt(spawnPrefabs.Count - 1);
            return spawn;
        }
    }
}