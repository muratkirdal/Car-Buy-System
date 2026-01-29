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

        /// <summary>
        /// Animates the alpha channel from current value to fully opaque over the specified duration.
        /// </summary>
        /// <remarks>
        /// Preserves RGB channels while only modifying alpha. If a fade is already in progress,
        /// it will be cancelled and the new fade will start from the current alpha value.
        /// </remarks>
        /// <param name="duration">The duration of the fade in seconds. Zero or negative sets alpha immediately.</param>
        public void FadeIn(float duration)
        {
            Fade(k_AlphaOpaque, duration);
        }

        /// <summary>
        /// Animates the alpha channel from current value to fully transparent over the specified duration.
        /// </summary>
        /// <remarks>
        /// Preserves RGB channels while only modifying alpha. If a fade is already in progress,
        /// it will be cancelled and the new fade will start from the current alpha value.
        /// </remarks>
        /// <param name="duration">The duration of the fade in seconds. Zero or negative sets alpha immediately.</param>
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

        private void OnDisable()
        {
            KillFadeTween();
        }

        private void OnDestroy()
        {
            KillFadeTween();
        }
    }
}
