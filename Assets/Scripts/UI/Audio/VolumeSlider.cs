using Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    [RequireComponent(typeof(Slider))]
    public class VolumeSlider : MonoBehaviour
    {
        [SerializeField] private string volumeParameterName;
        
        private AudioManager _audioManager;
        private Slider _slider;
        
        private const float MIN_VOLUME = 0f;
        private const float MAX_VOLUME = 1f;

        private bool _isValid;
        
        private void Awake()
        {
            Validate();
            InitSlider();
        }

        private void Start()
        {
            if (!_isValid) return;
            
            _audioManager = AudioManager.Instance;
            RefreshSlider();
        }

        private void RefreshSlider() => 
            _slider.value = _audioManager.GetVolume(volumeParameterName);
        
        private void InitSlider()
        {
            _slider = GetComponent<Slider>();
            
            _slider.minValue = MIN_VOLUME;
            _slider.maxValue = MAX_VOLUME;

            _slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDestroy()
        {
            _slider.onValueChanged.RemoveListener(OnValueChanged);
        }


        private void OnValueChanged(float value) => ChangeVolume(value);

        private void ChangeVolume(float value)
        {
            _audioManager.SetVolume(volumeParameterName, value);
            PlayerPrefs.SetFloat(volumeParameterName, value);
        }
        
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(volumeParameterName))
            {
                Debug.LogError($"{name}: Volume parameter has to be assigned.");
                return;
            }

            _isValid = true;
        }
    }
}