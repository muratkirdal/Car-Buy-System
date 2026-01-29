using UnityEngine;
using CarBuy.Data;
using DG.Tweening;

namespace CarBuy.Vehicle
{
    public class VehicleShowcase : MonoBehaviour
    {
        [Header("Platform")]
        [SerializeField] private Transform m_PlatformTransform;
        [SerializeField] private Transform m_VehicleSpawnPoint;
        [SerializeField] private float m_RotationSpeed = 10f;

        [Header("Transition")]
        [SerializeField] private float m_TransitionDuration = 0.5f;
        [SerializeField] private AnimationCurve m_FadeOutCurve;
        [SerializeField] private AnimationCurve m_FadeInCurve;

        private VehicleDisplayInstance m_CurrentVehicle;
        private Sequence m_TransitionSequence;

        public void ApplySettings(VehicleDisplaySettings settings)
        {
            m_RotationSpeed = settings.PlatformRotationSpeed;
            m_TransitionDuration = settings.TransitionDuration;
        }

        private void Update()
        {
            m_PlatformTransform.Rotate(Vector3.up, m_RotationSpeed * Time.deltaTime);
        }

        private void OnDisable()
        {
            KillTransition();
        }

        private void OnDestroy()
        {
            KillTransition();
        }

        public void DisplayVehicle(VehicleData vehicle, int colorIndex = 0)
        {
            VehicleDisplayInstance oldVehicle = m_CurrentVehicle;
            VehicleDisplayInstance newVehicle = SpawnVehicle(vehicle);

            m_CurrentVehicle = newVehicle;

            ApplyColor(newVehicle, vehicle, colorIndex);
            TransitionVehicles(oldVehicle, newVehicle);
        }

        public void SetColor(VehicleColorOption colorOption)
        {
            if (m_CurrentVehicle == null) return;
            m_CurrentVehicle.SetColor(colorOption.Color);
        }

        private VehicleDisplayInstance SpawnVehicle(VehicleData vehicle)
        {
            VehicleDisplayInstance spawnedObject = Instantiate
            (
                vehicle.ModelInfo.PrefabReference,
                m_VehicleSpawnPoint.position,
                m_VehicleSpawnPoint.rotation,
                m_VehicleSpawnPoint
            );
            spawnedObject.gameObject.SetActive(true);

            return spawnedObject;
        }

        private void ApplyColor(VehicleDisplayInstance displayInstance, VehicleData vehicle, int colorIndex)
        {
            int safeIndex = Mathf.Clamp(colorIndex, 0, vehicle.Colors.Length - 1);
            Color color = vehicle.Colors[safeIndex].Color;
            displayInstance.SetColor(color);
        }

        private void TransitionVehicles(VehicleDisplayInstance oldVehicle, VehicleDisplayInstance newVehicle)
        {
            KillTransition();
            m_TransitionSequence = DOTween.Sequence();

            if (oldVehicle != null)
            {
                oldVehicle.FadeOut(m_TransitionDuration);
                m_TransitionSequence.AppendCallback(() =>
                {
                    Destroy(oldVehicle.gameObject);
                });
            }

            newVehicle.FadeIn(m_TransitionDuration);

            m_TransitionSequence.AppendInterval(m_TransitionDuration);
        }

        private void KillTransition()
        {
            m_TransitionSequence?.Kill();
        }
    }
}
