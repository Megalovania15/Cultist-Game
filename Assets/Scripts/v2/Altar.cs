using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace V2
{
    // Main script controlling the Altar game object. It is initially in an
    // incomplete state and items must be collected by a player and brought
    // back to the altar. The items needed to complete the Altar are configured
    // as requirements in the editor.
    public class Altar : MonoBehaviour
    {
        // An item type described by a scriptable object and how many are
        // required.
        [Serializable]
        public struct Requirement
        {
            public Item item;
            public int count;
        }

        // Items required before the altar is considered completed.
        [SerializeField]
        private Requirement[] requirements;

        // Event fired when the Altar is completed.
        [SerializeField]
        private UnityEvent onCompleted;

        // Event fired when an item is brought back to the Altar.
        [SerializeField]
        private UnityEvent onItemAdded;

        // Returns whether an item given by its unique ID is still required to
        // complete the Altar.
        public bool Requires(string itemId)
        {
            return requirements.Any(requirement => requirement.count > 0);
        }

        // Tries to add an item to the Altar. Returns true if the item is
        // required and thus added, else false.
        public bool Add(string itemId)
        {
            var index = Array.FindIndex(requirements, requirement => requirement.item.id == itemId);
            if (index == -1 || requirements[index].count == 0)
            {
                return false; // We do not need this item.
            }
            --requirements[index].count;
            onItemAdded.Invoke();
            if (Completed)
            {
                onCompleted.Invoke();
            }
            return true;
        }

        public bool Completed
        {
            get { return requirements.All(requirement => requirement.count == 0); }
        }

        public Requirement[] Requirements
        {
            get { return (Requirement[])requirements.Clone(); }
        }
    }
}
