using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class StatsPanelView : MonoBehaviour
    {
        private readonly List<ColorButton> m_SpawnedColorButtons = new List<ColorButton>();

        [Header("Header")]
        [SerializeField] private TMP_Text m_VehicleNameText;
        [SerializeField] private Image m_ClassIcon;
        [SerializeField] private ClassIconLibrary m_ClassIconLibrary;

        [Header("Stats")]
        [SerializeField] private StatSlider m_SpeedSlider;
        [SerializeField] private StatSlider m_AccelerationSlider;
        [SerializeField] private StatSlider m_HandlingSlider;

        [Header("Colors")]
        [SerializeField] private ColorButton m_ColorButtonPrefab;
        [SerializeField] private Transform m_ColorContainer;

        public event ColorSelectedHandler ColorSelected;

        private void OnDisable()
        {
            foreach (var button in m_SpawnedColorButtons)
            {
                button.Clicked -= HandleColorButtonClicked;
                Destroy(button.gameObject);
            }
        }

        public void DisplayVehicle(VehicleData vehicle)
        {
            m_VehicleNameText.text = vehicle.DisplayName;
            m_ClassIcon.sprite = m_ClassIconLibrary.GetIcon(vehicle.Class);
            m_SpeedSlider.SetValue(vehicle.Stats.Speed);
            m_AccelerationSlider.SetValue(vehicle.Stats.Acceleration);
            m_HandlingSlider.SetValue(vehicle.Stats.Handling);

            foreach (var button in m_SpawnedColorButtons)
            {
                button.Clicked -= HandleColorButtonClicked;
                Destroy(button.gameObject);
            }
            m_SpawnedColorButtons.Clear();

            for (int i = 0; i < vehicle.Colors.Length; i++)
            {
                ColorButton button = Instantiate(m_ColorButtonPrefab, m_ColorContainer);
                button.Initialize(i, vehicle.Colors[i]);
                button.Clicked += HandleColorButtonClicked;
                m_SpawnedColorButtons.Add(button);
            }

            SelectColor(0);
        }

        private void SelectColor(int colorIndex)
        {
            for (int i = 0; i < m_SpawnedColorButtons.Count; i++)
            {
                m_SpawnedColorButtons[i].SetSelected(i == colorIndex);
            }

            ColorButton selectedButton = m_SpawnedColorButtons[colorIndex];
            ColorSelected?.Invoke(colorIndex, selectedButton.ColorOption);
        }

        private void HandleColorButtonClicked(int index, VehicleColorOption colorOption)
        {
            SelectColor(index);
        }

        public delegate void ColorSelectedHandler(int colorIndex, VehicleColorOption colorOption);
    }
}
