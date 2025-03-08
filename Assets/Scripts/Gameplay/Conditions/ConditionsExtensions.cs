using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Gameplay
{
    public static class ConditionsExtensions
    {
        public static bool Met(this List<GameStateCondition> conditions) =>
            conditions.Count == 0 || 
            conditions.All(condition => condition == null || condition.Met());
        
        public static bool Met(this List<PlacementCondition> conditions,
            Vector3Int position, List<Vector3Int> freePositions) =>
            conditions.All(condition => condition == null || condition.Met(position, freePositions));
            
        public static List<Vector3Int> GetRequiredReservedPositions(this List<PlacementCondition> conditions,
            Vector3Int position) =>
            conditions.SelectMany(condition => condition.GetRequiredReservedPositions(position)).ToList();
    }
}