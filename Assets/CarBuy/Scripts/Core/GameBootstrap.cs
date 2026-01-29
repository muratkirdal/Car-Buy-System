using UnityEngine;
using CarBuy.Data;
using CarBuy.UI;

namespace CarBuy.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private ShopController m_ShopController;

        private void Start()
        {
            m_ShopController.Initialize();
            m_ShopController.OpenShop();
        }
    }
}
