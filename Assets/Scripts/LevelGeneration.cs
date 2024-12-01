using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour {

    public GameObject player1, player2;
    public GameObject[] spawnPositions;

    public GameObject levelBounds1, levelBounds2, levelBounds3, levelBounds4;
    public GameObject crate, rock;
    public GameObject[] walls;

    public int crateCount, rockCount, wallCount;

    [SerializeField]
    private List<GameObject> levelObjs;

    private float threshold = 1.5f;

    void Awake()
    {
        levelObjs = new List<GameObject>();

        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
            levelObjs.Add(wall);
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            levelObjs.Add(player);
        foreach (GameObject altar in GameObject.FindGameObjectsWithTag("Altar"))
            levelObjs.Add(altar);

        

        /*if (spawnPositions.Length != 0)
            PlacePlayers();*/

        
    }

    // Use this for initialization
    void Start()
    {
        Spawner spawner = FindObjectOfType<Spawner>();

        crateCount = spawner.objectsToSpawn.Count;

        SpawnObjs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlacePlayers()
    {
        player1.transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position;
        player2.transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position;

        if (player1.transform.position == player2.transform.position)
        {
            player1.transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position;
            player2.transform.position = spawnPositions[Random.Range(0, spawnPositions.Length)].transform.position;
        }
    }

    void SpawnObjs()
    {
        /*for (int i = 0; i <= wallCount; i++)
            levelObjs.Add(Instantiate(walls[Random.Range(0, walls.Length)], FindPos(), Quaternion.identity) as GameObject);*/

        for (int i = 0; i < crateCount; i++)
            levelObjs.Add(Instantiate(crate, FindPos(levelBounds1, levelBounds2), Quaternion.identity) as GameObject);

        for (int i = 0; i < rockCount; i++)
            levelObjs.Add(Instantiate(rock, FindPos(levelBounds1, levelBounds2), Quaternion.identity) as GameObject);

        if (levelBounds3 != null && levelBounds4 != null)
        {
            for (int i = 0; i < crateCount; i++)
                levelObjs.Add(Instantiate(crate, FindPos(levelBounds3, levelBounds4), Quaternion.identity) as GameObject);

            for (int i = 0; i < rockCount; i++)
                levelObjs.Add(Instantiate(rock, FindPos(levelBounds3, levelBounds4), Quaternion.identity) as GameObject);
        }
    }

    //method by robertbu at http://answers.unity3d.com/questions/416160/how-to-stop-objects-spawning-on-top-of-each-other.html [accessed 10 June 2017]
    Vector3 FindPos(GameObject minBounds, GameObject maxBounds)
    {
        Vector3 pos = Vector3.zero;

        for (int i = 0; i < 1000; i++)
        {
            float dx = Random.Range(minBounds.transform.position.x, maxBounds.transform.position.x);
            float dy = Random.Range(minBounds.transform.position.y, maxBounds.transform.position.y);

            pos = new Vector3(dx, dy, 0);

            int j;

            for (j = 0; j < levelObjs.Count; j++)
            {
                if (Vector3.Distance(pos, levelObjs[j].transform.position) < threshold)
                    break;
            }

            if (j >= levelObjs.Count)
                return pos;
        }

        return pos;
    }
}
