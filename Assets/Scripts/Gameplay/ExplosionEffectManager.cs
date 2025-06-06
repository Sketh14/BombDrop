using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace BombDrop.Gameplay
{
    public class ExplosionEffectManager
    {
        // public Transform ExplosionTransform;
        // private Task _delayTask;
        private const int _delayTime = 1000;

        private CancellationTokenSource _cts;

        public ExplosionEffectManager()
        {
            // _delayTask = Task.Delay(1000);
            _cts = new CancellationTokenSource();
        }

        ~ExplosionEffectManager() { _cts.Cancel(); }

        // System.Text.StringBuilder _debugStringBuilder = new System.Text.StringBuilder();
        public async Task FlashLights(Transform explosionTransform, float lightIntensity)
        {
            // ref float inten = ExplosionTransform.GetChild(0).GetComponent<Light>().intensity;
            Light explosionLight = explosionTransform.GetChild(0).GetComponent<Light>();

            const float cLerpMultiplier = 2.0f;
            float lightIntensityMult = 0.1f;
            while (true)
            {
                explosionLight.intensity = lightIntensity * Mathf.Sin(lightIntensityMult * 180f * Mathf.Deg2Rad);
                lightIntensityMult += Time.deltaTime * cLerpMultiplier;
                // _debugStringBuilder.Append(Mathf.Sin(lightIntensityMult * 180f));
                // _debugStringBuilder.Append(',');
                await Task.Yield();
                if (lightIntensityMult > 1f || _cts.IsCancellationRequested) break;
            }

            // Debug.Log($"{_debugStringBuilder}");
            // await _delayTask;
            await Task.Delay(_delayTime);
            // explosionTransform.gameObject.SetActive(false);
        }
    }
}
