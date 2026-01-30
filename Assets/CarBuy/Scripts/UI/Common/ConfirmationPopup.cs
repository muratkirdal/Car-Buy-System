using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CarBuy.Data;

namespace CarBuy.UI
{
    public class ConfirmationPopup : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] private UIAnimationConfig m_Config;

        [Header("References")]
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_MessageText;
        [SerializeField] private Button m_YesButton;
        [SerializeField] private Button m_NoButton;

        private Tween m_FadeTween;

        public event ConfirmedHandler Confirmed;

        private void OnDisable()
        {
            KillTween();
            ClearButtonListeners();
        }

        public void Show(string vehicleName, int price)
        {
            ShowWithAction("Purchase", vehicleName, price);
        }

        public void ShowSell(string vehicleName, int price)
        {
            ShowWithAction("Sell", vehicleName, price);
        }

        private void ShowWithAction(string action, string vehicleName, int price)
        {
            ClearButtonListeners();
            m_YesButton.onClick.AddListener(OnYesClicked);
            m_NoButton.onClick.AddListener(OnNoClicked);
            SetupMessage(action, vehicleName, price);
            ShowPopup();
        }

        public void ForceClose()
        {
            KillTween();
            ClearButtonListeners();
            SetCanvasGroupState(0f, false, false);
        }

        private void SetupMessage(string action, string vehicleName, int price)
        {
            m_MessageText.text = $"{action} {vehicleName} for ${price:N0}?";
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

            m_FadeTween = m_CanvasGroup.DOFade(1f, m_Config.PopupFadeInDuration)
                .SetEase(Ease.OutQuad);
        }

        private void HidePopup(bool confirmed)
        {
            ClearButtonListeners();
            KillTween();

            m_FadeTween = m_CanvasGroup.DOFade(0f, m_Config.PopupFadeOutDuration)
                .SetEase(Ease.InQuad)
                .OnComplete(() =>
                {
                    SetCanvasGroupState(0f, false, false);

                    if (confirmed)
                    {
                        Confirmed?.Invoke();
                    }
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

    public delegate void ConfirmedHandler();
}
