using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "TradePanelConfig", menuName = "CarBuy/Configs/Trade Panel")]
    public class TradePanelConfig : ScriptableObject
    {
        [Header("Price Colors")]
        [SerializeField] private Color m_AffordableColor = Color.green;
        [SerializeField] private Color m_UnaffordableColor = Color.red;
        [SerializeField] private Color m_SellColor = Color.yellow;

        public Color AffordableColor => m_AffordableColor;
        public Color UnaffordableColor => m_UnaffordableColor;
        public Color SellColor => m_SellColor;
    }
}
