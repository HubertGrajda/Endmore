using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "FreeTilesCondition", menuName = "ScriptableObjects/PlacementCondition/FreeTilesCondition")]
    public class FreeTilesCondition : PlacementCondition
    {
        [SerializeField] private Neighbour type;
        [SerializeField] private int range = 1;
        
        private enum Neighbour
        {
            NoNeighbours,
            LeftTileFree,
            RightTileFree,
            TopTileFree,
            BottomTileFree,
        }

        public override bool Met(Vector3Int position, List<Vector3Int> freePositions)
        {
            var tilesToCheck = GetTilesInRangeByType(position, type, range);

            return tilesToCheck.All(freePositions.Contains);
        }
        
        public override List<Vector3Int> GetRequiredReservedPositions(Vector3Int position) => 
            GetTilesInRangeByType(position, type, range);
        
        private List<Vector3Int> GetTilesInRangeByType(Vector3Int position, Neighbour direction, int range)
        {
            var positions = new List<Vector3Int>();

            for (var i = 1; i <= range; i++)
            {
                switch (direction)
                {
                    case Neighbour.NoNeighbours:
                        positions.Add(position + new Vector3Int(-i, 0, 0));
                        positions.Add(position + new Vector3Int(i, 0, 0));
                        positions.Add(position + new Vector3Int(0, i, 0));
                        positions.Add(position + new Vector3Int(0, -i, 0));
                        break;
                    case Neighbour.LeftTileFree:
                        positions.Add(position + new Vector3Int(-i, 0, 0));
                        break;
                    case Neighbour.RightTileFree:
                        positions.Add(position + new Vector3Int(i, 0, 0));
                        break;
                    case Neighbour.TopTileFree:
                        positions.Add(position + new Vector3Int(0, i, 0));
                        break;
                    case Neighbour.BottomTileFree:
                        positions.Add(position + new Vector3Int(0, -i, 0));
                        break;
                }
            }

            return positions;
        }
    }
}