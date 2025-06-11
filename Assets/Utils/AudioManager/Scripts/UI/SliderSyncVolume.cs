using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AudioManager
{
    /// <summary>
    /// Keeps a UI Slider in sync with the AudioManager master volume
    /// or a specific AudioCategory volume, and vice-versa.
    /// Attach to the same GameObject as the Slider.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public sealed class SliderSyncVolume : MonoBehaviour
    {
        // ──────────────────────────────────────────────────────────────
        #region Inspector

        [Header("Volume Settings")]
        [Tooltip("If true, this slider controls the selected category; otherwise it controls the master volume.")]
        [SerializeField] private bool useCategoryVolume = false;

        [SerializeField, ConditionalHide(nameof(useCategoryVolume))]
        private AudioCategory category = AudioCategory.Music;

        [Header("Label")]
        [SerializeField, Tooltip("Optional label that is written to the attached TextMeshProUGUI.")]
        private string label = "Volume";

        [SerializeField] private TextMeshProUGUI labelText;

        #endregion
        // ──────────────────────────────────────────────────────────────

        private Slider slider;

        // ──────────────────────────────────────────────────────────────
        #region Unity lifecycle

        private void Awake()
        {
            slider = GetComponent<Slider>();

            // Defensive coding: this should always exist because of RequireComponent,
            // but guard just in case someone removes it at runtime.
            if (!slider)
            {
                Debug.LogError($"{nameof(SliderSyncVolume)} requires a Slider component.");
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            RefreshFromAudioManager();
            UpdateLabel();
        }

        private void OnEnable() => slider.onValueChanged.AddListener(OnSliderValueChanged);

        private void OnDisable() => slider.onValueChanged.RemoveListener(OnSliderValueChanged);

        private void OnDestroy() => slider.onValueChanged.RemoveListener(OnSliderValueChanged);

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Keep label and ConditionalHide responsive in the editor
            UpdateLabel();
            if (!Application.isPlaying && slider == null)
                slider = GetComponent<Slider>();
        }
#endif
        #endregion
        // ──────────────────────────────────────────────────────────────

        #region Public API

        /// <summary>
        /// Initialise the component at runtime if you want to assign
        /// the category in code instead of the Inspector.
        /// </summary>
        public void Init(AudioCategory newCategory)
        {
            category = newCategory;
            useCategoryVolume = true;
            RefreshFromAudioManager();
            UpdateLabel();
        }

        #endregion
        // ──────────────────────────────────────────────────────────────

        #region Internals

        private void UpdateLabel()
        {
            if (labelText == null) return;
            labelText.text = string.IsNullOrEmpty(label)
                ? (useCategoryVolume ? category.ToString() : "Volume")
                : label;
        }

        private void RefreshFromAudioManager()
        {
            if (AudioManager.Instance == null) return;

            slider.SetValueWithoutNotify(
                useCategoryVolume
                    ? AudioManager.Instance.GetVolume(category)
                    : AudioManager.Instance.GetVolume());
        }

        private void OnSliderValueChanged(float value)
        {
            if (AudioManager.Instance == null) return;

            if (useCategoryVolume)
                AudioManager.Instance.SetVolume(category, value);
            else
                AudioManager.Instance.SetVolume(value);
        }

        #endregion
    }
}