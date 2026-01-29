using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CarBuy.Data;

namespace CarBuy.UI.Common
{
    public class ConfirmationPopup : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_MessageText;
        [SerializeField] private Button m_YesButton;
        [SerializeField] private Button m_NoButton;

        [Header("Animation")]
        [SerializeField] private float m_FadeInDuration = 0.2f;
        [SerializeField] private float m_FadeOutDuration = 0.15f;

        private Action<bool> m_OnComplete;
        private Tweener m_FadeTween;

        public void ApplySettings(UIAnimationSettings settings)
        {
            m_FadeInDuration = settings.PopupFadeInDuration;
            m_FadeOutDuration = settings.PopupFadeOutDuration;
        }

        private void OnDisable()
        {
            KillTween();
            ClearButtonListeners();
        }

        private void OnDestroy()
        {
            KillTween();
        }

        public void Show(string vehicleName, int price, Action<bool> onComplete)
        {
            m_OnComplete = onComplete;

            SetupMessage(vehicleName, price);
            SetupButtonListeners();
            ShowPopup();
        }

        public void ForceClose()
        {
            KillTween();
            ClearButtonListeners();
            SetCanvasGroupState(0f, false, false);
            m_OnComplete?.Invoke(false);
            m_OnComplete = null;
        }

        private void SetupMessage(string vehicleName, int price)
        {
            m_MessageText.text = $"Purchase {vehicleName} for ${price:N0}?";
        }

        private void SetupButtonListeners()
        {
            ClearButtonListeners();
            m_YesButton.onClick.AddListener(OnYesClicked);
            m_NoButton.onClick.AddListener(OnNoClicked);
        }

        private void ClearButtonListeners()
        {
            m_YesButton.onClick.RemoveListener(OnYesClicked);
            m_NoButton.onClick.RemoveListener(OnNoClicked);
        }

        private void ShowPopup()
        {
            SetCanvasGroupState(0f, true, true);
            KillTween();

            m_FadeTween = m_CanvasGroup.DOFade(1f, m_FadeInDuration)
                .SetEase(Ease.OutQuad);
        }

        private void HidePopup(bool confirmed)
        {
            ClearButtonListeners();
            KillTween();

            m_FadeTween = m_CanvasGroup.DOFade(0f, m_FadeOutDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    SetCanvasGroupState(0f, false, false);
                    m_OnComplete?.Invoke(confirmed);
                    m_OnComplete = null;
                });
        }

        private void SetCanvasGroupState(float alpha, bool interactable, bool blocksRaycasts)
        {
            m_CanvasGroup.alpha = alpha;
            m_CanvasGroup.interactable = interactable;
            m_CanvasGroup.blocksRaycasts = blocksRaycasts;
        }

        private void OnYesClicked()
        {
            HidePopup(true);
        }

        private void OnNoClicked()
        {
            HidePopup(false);
        }

        private void KillTween()
        {
            m_FadeTween?.Kill();
        }
    }
}
