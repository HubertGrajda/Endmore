using UnityEngine;

namespace Scripts.Gameplay
{
    [CreateAssetMenu(fileName = "Coin", menuName = "ScriptableObjects/Spawnable/Coin")]
    public class CoinConfig : TileBasedSpawnableConfig<Coin>
    {
        [field: Header("Coin Settings")]
        [field: SerializeField] public int CoinValue { get; private set; }

        public override void OnGet(Spawnable spawnable)
        {
            base.OnGet(spawnable);
            ScoreManager.Instance.IncreaseTotalPointsBy(CoinValue);
        }
    }
}