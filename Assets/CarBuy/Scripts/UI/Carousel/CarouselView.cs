using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CarBuy.Data;
using DG.Tweening;

namespace CarBuy.UI.Carousel
{
    public class CarouselView : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private CarouselConfig m_Config;

        [Header("References")]
        [SerializeField] private RectTransform m_Container;
        [SerializeField] private CarouselItem m_ItemPrefab;
        [SerializeField] private Button m_LeftButton;
        [SerializeField] private Button m_RightButton;

        private List<VehicleData> m_Vehicles;
        private List<CarouselItem> m_CarouselItems;
        private HashSet<string> m_OwnedVehicleIds;
        private VehicleData m_CurrentVehicle;
        private Tween m_ContainerTween;
        private int m_CurrentIndex;
        private float m_ItemWidth;
        private float m_Spacing;
        private float m_TotalItemWidth;

        public event SelectedVehicleHandler VehicleSelected;

        private void OnEnable()
        {
            WireButtonEvents();
        }

        private void OnDisable()
        {
            UnwireButtonEvents();
            KillAnimation();
        }

        private void OnDestroy()
        {
            KillAnimation();
        }

        public void Initialize(IReadOnlyList<VehicleData> vehicles)
        {
            m_Vehicles = new List<VehicleData>(vehicles);
            m_CarouselItems = new List<CarouselItem>();
            m_OwnedVehicleIds = new HashSet<string>();

            foreach (var vehicle in m_Vehicles)
            {
                CarouselItem item = SpawnCarouselItem(vehicle);
                m_CarouselItems.Add(item);
            }

            CacheLayoutValues();

            m_CurrentIndex = 0;
            m_CurrentVehicle = m_Vehicles[m_CurrentIndex];

            UpdateItemVisualStates();
            SnapContainerToIndex(m_CurrentIndex);
        }

        public void SelectIndex(int index)
        {
            int clampedIndex = Mathf.Clamp(index, 0, m_Vehicles.Count - 1);

            if (clampedIndex == m_CurrentIndex) return;

            m_CurrentIndex = clampedIndex;
            m_CurrentVehicle = m_Vehicles[clampedIndex];

            UpdateItemVisualStates();

            KillAnimation();
            AnimateToIndex(clampedIndex);

            VehicleSelected?.Invoke(clampedIndex, m_CurrentVehicle);
        }

        public void MarkItemAsOwned(string vehicleId)
        {
            m_OwnedVehicleIds.Add(vehicleId);
            UpdateItemVisualStates();
        }

        private void WireButtonEvents()
        {
            m_LeftButton.onClick.AddListener(NavigateLeft);
            m_RightButton.onClick.AddListener(NavigateRight);
        }

        private void UnwireButtonEvents()
        {
            m_LeftButton.onClick.RemoveListener(NavigateLeft);
            m_RightButton.onClick.RemoveListener(NavigateRight);
        }

        private void NavigateLeft()
        {
            int newIndex = WrapIndex(m_CurrentIndex - 1);
            SelectIndex(newIndex);
        }

        private void NavigateRight()
        {
            int newIndex = WrapIndex(m_CurrentIndex + 1);
            SelectIndex(newIndex);
        }

        private void CacheLayoutValues()
        {
            var layoutGroup = m_Container.GetComponent<HorizontalLayoutGroup>();
            m_ItemWidth = m_CarouselItems[0].GetComponent<RectTransform>().sizeDelta.x;
            m_Spacing = layoutGroup.spacing;
            m_TotalItemWidth = m_ItemWidth + m_Spacing;
        }

        private float CalculateContainerTargetX(int selectedIndex)
        {
            float centerSlotIndex = (m_Config.VisibleItems - 1) / 2f;
            float centerSlotOffset = centerSlotIndex * m_TotalItemWidth;

            return centerSlotOffset - selectedIndex * m_TotalItemWidth;
        }

        private void SnapContainerToIndex(int selectedIndex)
        {
            Vector2 position = m_Container.anchoredPosition;
            position.x = CalculateContainerTargetX(selectedIndex);
            m_Container.anchoredPosition = position;
        }

        private void AnimateToIndex(int targetIndex)
        {
            float targetX = CalculateContainerTargetX(targetIndex);
            Vector2 targetPos = new Vector2(targetX, m_Container.anchoredPosition.y);

            m_ContainerTween = m_Container.DOAnchorPos(targetPos, m_Config.TransitionDuration)
                .SetEase(m_Config.TransitionCurve);
        }

        private int WrapIndex(int index)
        {
            int count = m_Vehicles.Count;
            return (index % count + count) % count;
        }

        private CarouselItem SpawnCarouselItem(VehicleData vehicleData)
        {
            CarouselItem item = Instantiate(m_ItemPrefab, m_Container);
            item.Initialize(vehicleData);
            return item;
        }

        private void UpdateItemVisualStates()
        {
            for (int i = 0; i < m_CarouselItems.Count; i++)
            {
                CarouselItem item = m_CarouselItems[i];
                string vehicleId = item.VehicleData.Id;

                bool isSelected = i == m_CurrentIndex;
                bool isOwned = m_OwnedVehicleIds.Contains(vehicleId);

                ItemState state = isSelected switch
                {
                    true when isOwned => ItemState.SelectedOwned,
                    true => ItemState.Selected,
                    _ => isOwned ? ItemState.Owned : ItemState.Default
                };

                item.SetState(state);
            }
        }

        private void KillAnimation()
        {
            m_ContainerTween?.Kill();
        }

        public delegate void SelectedVehicleHandler(int index, VehicleData vehicle);
    }
}