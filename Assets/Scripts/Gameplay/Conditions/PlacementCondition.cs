using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Gameplay
{
    public abstract class PlacementCondition : ScriptableObject
    {
        public abstract bool Met(Vector3Int position, List<Vector3Int> freePositions);

        public abstract List<Vector3Int> GetRequiredReservedPositions(Vector3Int position);
    }
}