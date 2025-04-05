using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace V2
{
    public class Spawner : MonoBehaviour
    {
        [Serializable]
        public struct Spawn
        {
            public GameObject prefab;
            public int count;
        }

        [SerializeField]
        private Spawn[] spawns;

        private List<GameObject> spawnPrefabs = new List<GameObject>();

        public int SpawnPrefabCount
        {
            get { return spawnPrefabs.Count; }
        }

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

        public GameObject GetNextSpawnPrefab()
        {
            if (spawnPrefabs.Count == 0) return null;
            var spawn = spawnPrefabs.Last();
            spawnPrefabs.RemoveAt(spawnPrefabs.Count - 1);
            return spawn;
        }
    }
}