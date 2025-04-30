using UnityEngine;

// Exposes an item description on a game object.
public class ItemInstance : MonoBehaviour {
    [field: SerializeField]
    public Item Properties { get; private set; }
}
