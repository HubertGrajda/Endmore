using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "FreeTilesCondition", menuName = "ScriptableObjects/PlacementCondition/FreeTilesCondition")]
    public class FreeTilesCondition : PlacementCondition
    {
        [SerializeField] private DirectionsSet directions;
        [SerializeField] private int range = 1;
        
        public override bool Met(Vector3Int position, List<Vector3Int> freePositions)
        {
            var tilesToCheck = GetTilesInRangeByType(position);

            return tilesToCheck.All(freePositions.Contains);
        }
        
        public override List<Vector3Int> GetRequiredReservedPositions(Vector3Int position) => 
            GetTilesInRangeByType(position);
        
        private List<Vector3Int> GetTilesInRangeByType(Vector3Int position)
        {
            var positions = new List<Vector3Int>();

            for (var i = 1; i <= range; i++)
            {
                foreach (var directionVector in directions.GetVectors())
                {
                    var directionAsVector3Int = Vector3Int.FloorToInt(directionVector);
                    positions.Add(position + directionAsVector3Int * i);
                }
            }

            return positions;
        }
    }
}