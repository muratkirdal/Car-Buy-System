using UnityEngine;

namespace CarBuy.Data
{
    [CreateAssetMenu(fileName = "PurchasePanelConfig", menuName = "CarBuy/Configs/Purchase Panel")]
    public class PurchasePanelConfig : ScriptableObject
    {
        [Header("Price Colors")]
        [SerializeField] private Color m_AffordableColor = Color.green;
        [SerializeField] private Color m_UnaffordableColor = Color.red;

        public Color AffordableColor => m_AffordableColor;
        public Color UnaffordableColor => m_UnaffordableColor;
    }
}
