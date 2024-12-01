using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Crate : MonoBehaviour {

    public GameObject altarPiece, bucket, candle, bomb;

    private GameObject objToDrop = null;

    private int altarPieceCount, bucketCount, candleCount;
    private float emptyCrates;

    private Spawner spawner;
    private LevelGeneration levelGen;
    private Altar altarP1, altarP2;

    // Use this for initialization
    void Start ()
    {
        spawner = FindObjectOfType<Spawner>();
        levelGen = FindObjectOfType<LevelGeneration>();

        altarP1 = GameObject.Find("P1 Altar").GetComponent<Altar>();
        altarP2 = GameObject.Find("P2 Altar").GetComponent<Altar>();

        altarPieceCount = altarP1.requiredAltarPieces + altarP2.requiredAltarPieces;
        bucketCount = altarP1.requiredBuckets + altarP2.requiredBuckets;
        candleCount = altarP1.requiredCandles + altarP2.requiredCandles;
        
        //emptyCrates = levelGen.crateCount * 0.4f;
        

        //print("empty crates: " + emptyCrates);
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void DropObject()
    {
        if (spawner.objectsToSpawn.Count > 0)
        {
            var spawnObjectType = spawner.objectsToSpawn.Last();
            switch (spawnObjectType)
            {
                case Spawner.SpawnObjectType.None:
                    break;

                case Spawner.SpawnObjectType.AltarPiece:
                    Instantiate(altarPiece, transform.position, Quaternion.identity);
                    break;

                case Spawner.SpawnObjectType.Bucket:
                    Instantiate(bucket, transform.position, Quaternion.identity);
                    break;

                case Spawner.SpawnObjectType.Candle:
                    Instantiate(candle, transform.position, Quaternion.identity);
                    break;

                case Spawner.SpawnObjectType.Bomb:
                    Instantiate(bomb, transform.position, Quaternion.identity);
                    break;
            }
            spawner.objectsToSpawn.RemoveAt(spawner.objectsToSpawn.Count - 1);
            Debug.Log("Spawne item. Number of items left to spawn: " + spawner.objectsToSpawn.Count);
        }
        //RollForDrop();

        //if (spawner.spawnedAltarPieces < altarPieceCount && objToDrop == altarPiece)
        //{
        //    Instantiate(altarPiece, transform.position, Quaternion.identity);
        //    spawner.spawnedAltarPieces++;
        //    //print("altar pieces spawned: " + spawner.spawnedAltarPieces + "/" + altarPieceCount);
        //}


        //else if (spawner.spawnedBuckets < bucketCount && objToDrop == bucket)
        //{
        //    Instantiate(bucket, transform.position, Quaternion.identity);
        //    spawner.spawnedBuckets++;
        //    //print("buckets spawned: " + spawner.spawnedBuckets + "/" + bucketCount);
        //}


        //else if (spawner.spawnedCandles < candleCount && objToDrop == candle)
        //{
        //    Instantiate(candle, transform.position, Quaternion.identity);
        //    spawner.spawnedCandles++;
        //    //print("candles spawned: " + spawner.spawnedCandles + "/" + candleCount);
        //}

        //else if (objToDrop == bomb)
        //    Instantiate(bomb, transform.position, Quaternion.identity);
            
    }

    void RollForDrop()
    {
        int roll = Random.Range(0, 4);

        objToDrop = null;

        if (roll == 0)
        {
            if (spawner.noDrop < Mathf.RoundToInt(emptyCrates))
            {
                int newRoll = Random.Range(0, 2);

                if (newRoll == 0)
                    objToDrop = null;
                else if (newRoll == 1)
                    objToDrop = bomb;
                
                spawner.noDrop++;
                //print("number of null drops: " + spawner.noDrop);
            }

            else
                RollForDrop();
        }


        else if (roll == 1)
        {
            objToDrop = altarPiece;

            if (spawner.spawnedAltarPieces == altarPieceCount)
            {
                //print("rerolled");
                RollForDrop();
            }
        }

        else if (roll == 2)
        {
            objToDrop = bucket;

            if (spawner.spawnedBuckets == bucketCount)
            {
                //print("rerolled");
                RollForDrop();
            }
        }

        else if (roll == 3)
        {
            objToDrop = candle;

            if (spawner.spawnedCandles == candleCount)
            {
                //print("rerolled");
                RollForDrop();
            }
        }
            
    }
}
