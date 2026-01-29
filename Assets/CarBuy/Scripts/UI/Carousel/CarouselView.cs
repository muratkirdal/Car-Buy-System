using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CarBuy.Data;
using DG.Tweening;

namespace CarBuy.UI.Carousel
{
    public class CarouselView : MonoBehaviour
    {
        private const float k_SpacingRatio = 0.2f;

        [Header("References")]
        [SerializeField] private RectTransform m_Container;
        [SerializeField] private CarouselItem m_ItemPrefab;
        [SerializeField] private Button m_LeftButton;
        [SerializeField] private Button m_RightButton;

        [Header("Settings")]
        [SerializeField] private int m_VisibleItems = 7;
        [SerializeField] private float m_TransitionDuration = 0.3f;
        [SerializeField] private AnimationCurve m_TransitionCurve;

        private List<VehicleData> m_Vehicles;
        private List<CarouselItem> m_CarouselItems;
        private List<RectTransform> m_ItemRectTransforms;
        private HashSet<string> m_OwnedVehicleIds;
        private VehicleData m_CurrentVehicle;
        private Sequence m_AnimationSequence;
        private int m_CurrentIndex;
        private float m_ItemWidth;
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

        public void ApplySettings(CarouselSettings settings)
        {
            m_VisibleItems = settings.VisibleItems;
            m_TransitionDuration = settings.TransitionDuration;
        }

        public void Initialize(IReadOnlyList<VehicleData> vehicles)
        {
            m_Vehicles = new List<VehicleData>(vehicles);
            m_CarouselItems = new List<CarouselItem>();
            m_ItemRectTransforms = new List<RectTransform>();
            m_OwnedVehicleIds = new HashSet<string>();

            foreach (var vehicle in m_Vehicles)
            {
                CarouselItem item = SpawnCarouselItem(vehicle);
                m_CarouselItems.Add(item);
                m_ItemRectTransforms.Add(item.GetComponent<RectTransform>());
            }

            CacheLayoutValues();

            m_CurrentIndex = 0;
            m_CurrentVehicle = m_Vehicles[0];

            UpdateItemVisualStates();
            SnapItemsToFinalPositions(m_CurrentIndex);
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
            m_ItemWidth = m_ItemRectTransforms[0].sizeDelta.x;
            float spacing = m_ItemWidth * k_SpacingRatio;
            m_TotalItemWidth = m_ItemWidth + spacing;
        }

        private float CalculateItemTargetX(int itemIndex, int selectedIndex)
        {
            int offset = itemIndex - selectedIndex;
            return offset * m_TotalItemWidth;
        }

        private void SnapItemsToFinalPositions(int selectedIndex)
        {
            for (int i = 0; i < m_ItemRectTransforms.Count; i++)
            {
                RectTransform itemRect = m_ItemRectTransforms[i];

                float targetX = CalculateItemTargetX(i, selectedIndex);
                Vector2 position = itemRect.anchoredPosition;
                position.x = targetX;
                itemRect.anchoredPosition = position;
            }
        }

        private void AnimateToIndex(int targetIndex)
        {
            m_AnimationSequence = DOTween.Sequence();

            for (int i = 0; i < m_ItemRectTransforms.Count; i++)
            {
                RectTransform itemRect = m_ItemRectTransforms[i];

                float targetX = CalculateItemTargetX(i, targetIndex);
                Vector2 targetPos = new Vector2(targetX, itemRect.anchoredPosition.y);

                Tween tween = itemRect.DOAnchorPos(targetPos, m_TransitionDuration)
                    .SetEase(m_TransitionCurve);

                m_AnimationSequence.Join(tween);
            }
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
            m_AnimationSequence?.Kill();
        }

        public delegate void SelectedVehicleHandler(int index, VehicleData vehicle);
    }
}
