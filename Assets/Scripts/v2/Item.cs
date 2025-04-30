using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public string id;
    public string displayName;
    public Sprite preview;
    public GameObject prefab;
}
