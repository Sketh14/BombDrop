/*

*/

using FrontLineDefense.Global;
using UnityEngine;

namespace FrontLineDefense.Projectiles
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] protected float _SpeedMult = 1f;
        [SerializeField] private PoolManager.PoolType _poolToUse;
        [SerializeField] protected float _Damage = 1f;
        private bool _releasedToPool;

        private void OnDisable() { _releasedToPool = false; }

        private void FixedUpdate()
        {
            //Apply Movement
            transform.position = transform.position + (transform.right * _SpeedMult);
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Hit | Collider : {other.name} | Tag : {other.tag}");
            // gameObject.SetActive(false);
            // return;

            if (_releasedToPool) return;

            _releasedToPool = true;
            PoolManager.Instance.ObjectPool[(int)_poolToUse].Release(gameObject);
            if (other.CompareTag(UniversalConstants.Player))
                other.GetComponent<IStatComponent>().TakeDamage(_Damage);
        }
    }
}
