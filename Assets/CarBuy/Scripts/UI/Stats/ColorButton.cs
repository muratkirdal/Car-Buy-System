using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using CarBuy.Data;

namespace CarBuy.UI.Stats
{
    public class ColorButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_ColorSwatch;
        [SerializeField] private GameObject m_SelectedIndicator;

        [Header("Scale Settings")]
        [SerializeField] private float m_SelectedScale = 1.1f;
        [SerializeField] private float m_NormalScale = 1.0f;

        private RectTransform m_RectTransform;
        private Vector3 m_SelectedScaleVector;
        private Vector3 m_NormalScaleVector;

        public UnityEvent<ColorButton> Clicked = new();

        public VehicleColorOption ColorOption { get; private set; }

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            CacheScaleVectors();
            WireButtonClick();
        }

        public void Initialize(VehicleColorOption colorOption)
        {
            ColorOption = colorOption;
            WireButtonClick();
            UpdateColorSwatch(colorOption.Color);
            SetSelected(false);
        }

        public void SetSelected(bool selected)
        {
            UpdateSelectedIndicator(selected);
            UpdateScale(selected);
        }

        private void CacheScaleVectors()
        {
            m_SelectedScaleVector = new Vector3(m_SelectedScale, m_SelectedScale, m_SelectedScale);
            m_NormalScaleVector = new Vector3(m_NormalScale, m_NormalScale, m_NormalScale);
        }

        private void WireButtonClick()
        {
            m_Button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            Clicked.Invoke(this);
        }

        private void UpdateColorSwatch(Color color)
        {
            m_ColorSwatch.color = color;
        }

        private void UpdateSelectedIndicator(bool visible)
        {
            m_SelectedIndicator.SetActive(visible);
        }

        private void UpdateScale(bool selected)
        {
            m_RectTransform.localScale = selected ? m_SelectedScaleVector : m_NormalScaleVector;
        }

        private void OnDestroy()
        {
            m_Button.onClick.RemoveListener(HandleButtonClick);
        }
    }
}
