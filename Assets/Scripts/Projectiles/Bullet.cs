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

            PoolManager.Instance.ObjectPool[(int)_poolToUse].Release(gameObject);
            if (other.CompareTag(UniversalConstants.Player))
            {
                other.GetComponent<IStatComponent>().TakeDamage(10f);
            }
        }
    }
}
