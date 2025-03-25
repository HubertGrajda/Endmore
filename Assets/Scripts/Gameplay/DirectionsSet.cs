using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts.Gameplay
{
    public enum Direction
    {
        None = 0,
        Left = 1,
        Right = 2,
        Down = 3,
        Up = 4,
        TopLeft = 5,
        TopRight = 6,
        BottomLeft = 7,
        BottomRight = 8
    }
    
    [CreateAssetMenu(fileName = "DirectionsSet", menuName = "ScriptableObjects/DirectionsSet")]
    public class DirectionsSet : ScriptableObject
    {
        [field: SerializeField] public List<Direction> Directions { get; private set; }

        public List<Vector2> GetVectors() => Directions.Select(GetVector).ToList();

        public Vector2 GetRandomVector()
        {
            var vectors = GetVectors();
            var drawnDirection = (Vector3)vectors[Random.Range(0, vectors.Count)];

            return drawnDirection;
        }
        
        private Vector2 GetVector(Direction direction) => direction switch 
        {
            Direction.None => Vector2.zero,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Down => Vector2.down,
            Direction.Up => Vector2.up,
            Direction.TopLeft => new Vector2(-1, 1),
            Direction.TopRight => new Vector2(1, 1),
            Direction.BottomLeft => new Vector2(-1, -1),
            Direction.BottomRight => new Vector2(1, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}