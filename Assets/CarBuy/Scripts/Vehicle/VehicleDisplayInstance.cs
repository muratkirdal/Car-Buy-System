using UnityEngine;
using DG.Tweening;

namespace CarBuy.Vehicle
{
    public class VehicleDisplayInstance : MonoBehaviour
    {
        private const float k_AlphaOpaque = 1.0f;
        private const float k_AlphaTransparent = 0.0f;

        [SerializeField] private Renderer[] m_PaintableRenderers;
        [SerializeField] private string m_ColorPropertyName = "_BaseColor";

        private MaterialPropertyBlock m_PropertyBlock;
        private Tweener m_FadeTween;
        private Color m_CurrentColor = Color.white;
        private int m_ColorPropertyId;

        private void OnDisable()
        {
            KillFadeTween();
        }

        private void OnDestroy()
        {
            KillFadeTween();
        }

        public void SetColor(Color color)
        {
            EnsurePropertyBlockInitialized();
            m_CurrentColor = color;

            foreach (var renderer in m_PaintableRenderers)
            {
                m_PropertyBlock.SetColor(m_ColorPropertyId, color);
                renderer.SetPropertyBlock(m_PropertyBlock);
            }
        }

        public void FadeIn(float duration)
        {
            Fade(k_AlphaOpaque, duration);
        }

        public void FadeOut(float duration)
        {
            Fade(k_AlphaTransparent, duration);
        }

        private void Fade(float targetAlpha, float duration)
        {
            KillFadeTween();

            if (duration <= 0f)
            {
                SetAlpha(targetAlpha);
                return;
            }

            float startAlpha = m_CurrentColor.a;
            m_FadeTween = DOTween.To(() => startAlpha, x => SetAlpha(x), targetAlpha, duration)
                .SetEase(Ease.Linear);
        }

        private void SetAlpha(float alpha)
        {
            Color newColor = new Color(m_CurrentColor.r, m_CurrentColor.g, m_CurrentColor.b, alpha);
            SetColor(newColor);
        }

        private void KillFadeTween()
        {
            m_FadeTween?.Kill();
        }

        private void EnsurePropertyBlockInitialized()
        {
            if (m_PropertyBlock == null)
            {
                m_PropertyBlock = new MaterialPropertyBlock();
                m_ColorPropertyId = Shader.PropertyToID(m_ColorPropertyName);
            }
        }
    }
}
