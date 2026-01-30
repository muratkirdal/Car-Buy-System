using UnityEngine;

namespace CarBuy.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [Header("Controllers")]
        [SerializeField] private ShopMediator m_ShopMediator;

        private void Start()
        {
            m_ShopMediator.Initialize();
            m_ShopMediator.OpenShop();
        }
    }
}
