using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public int spawnedAltarPieces, spawnedBuckets, spawnedCandles, noDrop;

	public enum SpawnObjectType
    {
        None,
        AltarPiece,
		Bucket,
		Candle,
		Bomb
	}

	[SerializeField]
	private int bombsToSpawn = 0;

    [SerializeField]
    private int disappointmentsToSpawn = 0;

    public List<SpawnObjectType> objectsToSpawn = new List<SpawnObjectType>();

	// Use this for initialization
	void Start ()
    {
        Altar altarP1 = GameObject.Find("P1 Altar").GetComponent<Altar>();
        Altar altarP2 = GameObject.Find("P2 Altar").GetComponent<Altar>();

		var requiredAltarPieceCount = altarP1.requiredAltarPieces + altarP2.requiredAltarPieces;
		var requiredBucketCount = altarP1.requiredBuckets + altarP2.requiredBuckets;
		var requiredCandleCount = altarP1.requiredCandles + altarP2.requiredCandles;

        //adds all the items needed to spawn to a list and then randomises them
		for (int i = 0; i < requiredAltarPieceCount; i++)
		{
			objectsToSpawn.Add(SpawnObjectType.AltarPiece);
        }
		for (int i = 0; i < requiredBucketCount; i++)
		{
			objectsToSpawn.Add(SpawnObjectType.Bucket);
        }
        for (int i = 0; i < requiredCandleCount; i++)
        {
            objectsToSpawn.Add(SpawnObjectType.Candle);
        }
        for (int i = 0; i < bombsToSpawn; i++)
        {
            objectsToSpawn.Add(SpawnObjectType.Bomb);
        }
        for (int i = 0; i < disappointmentsToSpawn; i++)
        {
            objectsToSpawn.Add(SpawnObjectType.None);
        }
        var random = new System.Random();
        objectsToSpawn = objectsToSpawn
            .OrderBy(_ => random.Next())
            .ToList();
    }

    // Update is called once per frame
    void Update ()
    {
		
	}
}
