using UnityEngine;
using UnityEngine.UI;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class ColorButton : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private ColorButtonConfig m_Config;

        [Header("References")]
        [SerializeField] private Button m_Button;
        [SerializeField] private Image m_ColorSwatch;
        [SerializeField] private GameObject m_SelectedIndicator;

        private RectTransform m_RectTransform;
        private Vector3 m_SelectedScaleVector;
        private Vector3 m_NormalScaleVector;
        private int m_Index;
        private VehicleColorOption m_ColorOption;

        public event ClickedHandler Clicked;

        public VehicleColorOption ColorOption => m_ColorOption;

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
            CacheScaleVectors();
        }

        private void OnEnable()
        {
            m_Button.onClick.AddListener(HandleButtonClick);
        }

        private void OnDisable()
        {
            m_Button.onClick.RemoveListener(HandleButtonClick);
        }

        public void Initialize(int index, VehicleColorOption colorOption)
        {
            m_Index = index;
            m_ColorOption = colorOption;
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
            m_SelectedScaleVector = new Vector3(m_Config.SelectedScale, m_Config.SelectedScale, m_Config.SelectedScale);
            m_NormalScaleVector = new Vector3(m_Config.NormalScale, m_Config.NormalScale, m_Config.NormalScale);
        }

        private void HandleButtonClick()
        {
            Clicked?.Invoke(m_Index, m_ColorOption);
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

        public delegate void ClickedHandler(int index, VehicleColorOption colorOption);
    }
}
