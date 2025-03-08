using System.Collections.Generic;
using System.Linq;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.UI
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject noRecordsObject;
        [SerializeField] private Transform recordsContainer;
        [SerializeField] private RecordVisualizer recordPrefab;

        private GameManager _gameManager;

        private void Awake()
        {
            _gameManager = GameManager.Instance;
        }

        private void Start()
        {
            var allAttempts = _gameManager.LevelToAttemptsData
                .SelectMany(x => x.Value)
                .OrderBy(y => y)
                .ToList();

            var hasRecords = allAttempts.Count != 0;
            
            HandleNoRecordsObject(hasRecords);
            
            if (!hasRecords) return;
            
            VisualizeRecords(allAttempts);
        }

        private void VisualizeRecords(List<LevelAttemptData> allAttempts)
        {
            foreach (var levelToAttemptsData in allAttempts)
            {
                var recordInstance = Instantiate(recordPrefab, recordsContainer);
                recordInstance.VisualizeAttempt(levelToAttemptsData);
            }
        }

        private void HandleNoRecordsObject(bool hasRecords)
        {
            if (noRecordsObject == null) return;
            
            noRecordsObject.gameObject.SetActive(!hasRecords);
        }
    }
}