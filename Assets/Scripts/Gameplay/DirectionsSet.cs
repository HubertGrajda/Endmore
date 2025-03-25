using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Gameplay
{
    public enum Direction
    {
        None = 0,
        Left = 1,
        Right = 2,
        Down = 3,
        Up = 4,
    }
    
    [CreateAssetMenu(fileName = "DirectionsSet", menuName = "ScriptableObjects/DirectionsSet")]
    public class DirectionsSet : ScriptableObject
    {
        [field: SerializeField] public List<Direction> Directions { get; private set; }

        public List<Vector2> GetVectors() => Directions.Select(GetVector).ToList();
        
        private Vector2 GetVector(Direction direction) => direction switch 
        {
            Direction.None => Vector2.zero,
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Down => Vector2.down,
            Direction.Up => Vector2.up,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}