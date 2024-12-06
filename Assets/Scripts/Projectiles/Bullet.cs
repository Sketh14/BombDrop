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
        private Vector2 _startPosition;
        private const float _maxRange = 100f;

        private void OnDisable() { _releasedToPool = false; }

        private void OnEnable()
        {
            _startPosition = transform.position;
            // Debug.Log($"_startPosition : {_startPosition}");
        }

        private void FixedUpdate()
        {
            //Apply Movement
            transform.position = transform.position + (transform.right * _SpeedMult);

            if (Vector3.SqrMagnitude(transform.position - new Vector3(_startPosition.x, _startPosition.y, transform.position.z)) >= (_maxRange * _maxRange))
            {
                // gameObject.SetActive(false);
                _releasedToPool = true;
                PoolManager.Instance.ObjectPool[(int)_poolToUse].Release(gameObject);
                // Debug.Log($"Start Pos : {_startPosition} | Current pos : {transform.position} | "
                // + $" Magnitude : {Vector3.SqrMagnitude(transform.position - new Vector3(_startPosition.x, _startPosition.y, transform.position.z))}");
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"Hit | Collider : {other.name} | Tag : {other.tag}");
            // gameObject.SetActive(false);
            // return;

            if (_releasedToPool) return;

            _releasedToPool = true;
            PoolManager.Instance.ObjectPool[(int)_poolToUse].Release(gameObject);
            if (other.CompareTag(UniversalConstants.StatComponent))
                other.GetComponent<IStatComponent>().TakeDamage(_Damage);
        }
    }
}
