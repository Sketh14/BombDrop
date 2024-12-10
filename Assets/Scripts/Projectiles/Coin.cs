// #define THROW_TEST
// #define COLLECTION_TEST

using Task = System.Threading.Tasks.Task;

using UnityEngine;
using System.Threading;
using FrontLineDefense.Global;

namespace FrontLineDefense.Projectiles
{

    public class Coin : MonoBehaviour
    {
        [SerializeField] private float _forceMultiplier;
        private Rigidbody _coinRb;
        private const int _waitTimeBeforeCollection = 3000;
        private CancellationTokenSource _cts;
        // [SerializeField] private PoolManager.PoolType _coinPool;
#if THROW_TEST
        [SerializeField] private bool _throwAgain;
#endif
#if COLLECTION_TEST
        [SerializeField] private Transform _collectionPoint;
#endif

        private void OnDestroy()
        {
            _cts.Cancel();
        }

        private void Awake()
        {
            _coinRb = GetComponent<Rigidbody>();
            _cts = new CancellationTokenSource();
        }

        void OnEnable()
        {
            _coinRb.isKinematic = false;
            Vector3 throwForce = Vector3.up * _forceMultiplier;
            //Add x | z offset
            throwForce.x = Random.Range(0, 1f);
            throwForce.z = Random.Range(0, 1f);
            _coinRb.AddForce(throwForce, ForceMode.VelocityChange);
            GetCollected();
        }

        private async void GetCollected()
        {
            //Fly toward the top left or top right
            await Task.Delay(_waitTimeBeforeCollection);
            _coinRb.isKinematic = true;

            bool reached = false;
            float timePassed = 0f;
            Vector3 startPos = transform.position;
            while (!reached)
            {
                if (_cts.Token.IsCancellationRequested) return;

                timePassed += Time.deltaTime;

                if (timePassed >= 1.0f)
                {
                    timePassed = 0f;
                    reached = true;
                    PoolManager.Instance.ObjectPool[(int)PoolManager.PoolType.COIN].Release(gameObject);
                    break;
                }

#if COLLECTION_TEST
                transform.position = Vector3.Lerp(startPos, _collectionPoint.position, timePassed);
#else
                transform.position = Vector3.Lerp(startPos, GameManager.Instance.PlayerTransform.position + (Vector3.up * 10f), timePassed);
#endif

                await Task.Yield();
            }
        }

#if THROW_TEST
        private async void Update()
        {
            if (!_throwAgain) return;
            _throwAgain = !_throwAgain;

            _coinRb.isKinematic = true;
            await Task.Delay(1000);

            _coinRb.isKinematic = false;
            Vector3 throwForce = Vector3.up * _forceMultiplier;
            //Add x | z offset
            throwForce.x = Random.Range(1, 3);
            throwForce.z = Random.Range(1, 3);
            _coinRb.AddForce(throwForce, ForceMode.VelocityChange);
        }
#endif
    }
}
