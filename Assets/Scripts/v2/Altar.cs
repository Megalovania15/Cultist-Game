using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace V2
{
    public class Altar : MonoBehaviour
    {
        [Serializable]
        public struct Requirement
        {
            public Item item;
            public int count;
        }

        [SerializeField]
        private Requirement[] requirements;

        [SerializeField]
        private UnityEvent onCompleted;

        [SerializeField]
        private UnityEvent onItemAdded;

        public bool Requires(string itemId)
        {
            return requirements.Any(requirement => requirement.count > 0);
        }

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
