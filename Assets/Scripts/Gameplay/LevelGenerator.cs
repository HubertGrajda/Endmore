using System;
using System.Collections.Generic;
using System.Linq;
using Scripts.Player;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace Scripts.Gameplay
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private Tilemap spawnArea;
        [SerializeField] private Tilemap walls;
        
        [SerializeField] private List<TileBasedSpawnableCollection> spawnableCollections;
        
        private int _currentSeed;
        
        private HashSet<Vector3Int> _positionsWithSpawnAllowed = new();
        
        private readonly HashSet<Vector3Int> _reservedPositions = new();
        
        private const int PLAYER_SAFE_RANGE = 1;
        private const int MAX_SPAWN_ATTEMPTS = 10;
        
        private void Awake()
        {
            _positionsWithSpawnAllowed = GetPositionsWithSpawnAllowed();
        }

        private HashSet<Vector3Int> GetPositionsWithSpawnAllowed()
        {
            var positions = new HashSet<Vector3Int>();
            
            foreach (var position in spawnArea.cellBounds.allPositionsWithin)
            {
                if (!spawnArea.HasTile(position) || walls.HasTile(position)) continue;
                
                positions.Add(position);
            }
            
            return positions;
        }

        public void GenerateLevel(int seed)
        {
            _currentSeed = seed;
            
            Random.InitState(_currentSeed);
            ReservePlayerTiles();
            spawnableCollections.ForEach(SpawnCollection);
        }

        private void SpawnCollection(TileBasedSpawnableCollection collection)
        {
            for (var i = 0; i < collection.ObjectsCount; i++)
            {
                var attemptsLeft = MAX_SPAWN_ATTEMPTS;
                Vector3Int position;
                TileBasedSpawnableConfig config;

                do
                {
                    if (!TryGetRandomFreePosition(out position)) return;
                    if (TryGetRandomSpawnableForTile(position, collection, out config)) break;
                        
                    attemptsLeft--;

                } while (attemptsLeft > 0);
                
                if (config == null) continue;
                
                Spawn(config, position);
            }
        }

        private void ReservePlayerTiles()
        {
            var tilePos = spawnArea.WorldToCell(PlayerController.Instance.transform.position);

            for (var i = tilePos.x - PLAYER_SAFE_RANGE; i <= tilePos.x + PLAYER_SAFE_RANGE; i++)
            {
                for (var j = tilePos.y - PLAYER_SAFE_RANGE; j <= tilePos.y + PLAYER_SAFE_RANGE; j++)
                {
                    _reservedPositions.Add(new Vector3Int(i, j));
                }
            }
        }
        
        private bool TryGetRandomFreePosition(out Vector3Int drawnPosition)
        {
            drawnPosition = default;

            if (!TryGetFreePositions(out var freePositions)) return false;

            drawnPosition = freePositions[Random.Range(0, freePositions.Count)];

            return true;
        }
        
        private bool TryGetRandomSpawnableForTile(Vector3Int position, TileBasedSpawnableCollection collection,
            out TileBasedSpawnableConfig config)
        {
            config = default;
            
            if (!TryGetFreePositions(out var freePositions)) return false;
            
            
            var configsWithConditionsMet = collection.SpawnableConfigs
                .Where(config => config.PlacementConditions.Met(position, freePositions) && config.GameStateConditions.Met())
                .ToList();
            
            var count = configsWithConditionsMet.Count;
            if (count == 0) return false;
            
            config = configsWithConditionsMet[Random.Range(0, count)];
            
            return config != null;
        }

        private void Spawn(TileBasedSpawnableConfig spawnableConfig, Vector3Int tilePosition)
        {
            var worldPosition = spawnArea.GetCellCenterWorld(tilePosition);
            var spawnableInstance = SpawnableFactory.SpawnFromPool(spawnableConfig);
            spawnableInstance.transform.position = worldPosition;
            
            ReservePositionsForSpawnedSpawnable(tilePosition, spawnableConfig);
        }
        
        private void ReservePositionsForSpawnedSpawnable(Vector3Int spawnPosition, TileBasedSpawnableConfig spawnableConfig)
        {
            var positionsReservedByConditions = spawnableConfig.PlacementConditions
                .GetRequiredReservedPositions(spawnPosition);
            
            _reservedPositions.Add(spawnPosition);
            
            foreach (var positionToReserve in positionsReservedByConditions)
            {
                _reservedPositions.Add(positionToReserve);
            }
        }

        public void ClearLevel()
        {
            _reservedPositions.Clear();
        }

        private bool TryGetFreePositions(out List<Vector3Int> freePositions)
        {
            freePositions = _positionsWithSpawnAllowed.Except(_reservedPositions).ToList();
            return freePositions.Any();
        }

        [ContextMenu("Recreate")]
        public void Recreate()
        {
            if (!Application.isPlaying) return;
            
            ClearLevel();
            GenerateLevel(Random.Range(1, int.MaxValue));
        }

        [Serializable]
        private class TileBasedSpawnableCollection
        {
            [field: SerializeField] public int ObjectsCount { get; private set; }
            [field: SerializeField] public List<TileBasedSpawnableConfig> SpawnableConfigs { get; private set; }
        }
    }
}