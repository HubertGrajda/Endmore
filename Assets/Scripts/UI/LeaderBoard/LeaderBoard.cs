using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Scripts.Gameplay;
using UnityEngine;

namespace Scripts.UI
{
    public class LeaderBoard : MonoBehaviour
    {
        [SerializeField] private GameObject noRecordsObject;
        [SerializeField] private Transform recordsContainer;
        [SerializeField] private LevelAttemptVisualizer levelAttemptPrefab;

        [Inject] private IGameService _gameService;

        private void Start()
        {
            var allAttempts = _gameService.LevelToAttemptsData
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
                var recordInstance = Instantiate(levelAttemptPrefab, recordsContainer);
                var container = recordInstance.gameObject.scene.GetSceneContainer();
                GameObjectInjector.InjectRecursive(recordInstance.gameObject, container);
                
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