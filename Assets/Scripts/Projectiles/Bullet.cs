/*

*/

using FrontLineDefense.Global;
using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] protected float _SpeedMult = 1f;
        protected Vector3 _SpeedVec;
        [SerializeField] private PoolManager.PoolType _poolToUse;

        protected virtual void Update()
        {
            //Apply Movement
            transform.position = transform.position + (_SpeedVec * Time.deltaTime);
        }

        public void SetStats(in Vector2 initialSpeed)
        {
            _SpeedVec = new Vector3(initialSpeed.x * _SpeedMult, initialSpeed.y * _SpeedMult, 0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Hit | Collider : {other.name} | Tag : {other.tag}");
            // gameObject.SetActive(false);
            PoolManager.Instance.ObjectPool[(int)_poolToUse].Release(gameObject);
            if (other.CompareTag(UniversalConstants.Player))
            {
                other.GetComponent<IStatComponent>().TakeDamage(10f);
            }
        }
    }
}
