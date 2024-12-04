
using FrontLineDefense.Global;
using UnityEngine;

namespace FrontLineDefense.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private PoolManager.PoolType[] _availableEnemyPools;
        [SerializeField] private Transform _terrainTranform;

        private void OnDestroy()
        {
            GameManager.Instance.OnMapGenerated -= SpawnEnemies;
        }

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.OnMapGenerated += SpawnEnemies;
        }

        private void SpawnEnemies(Vector3[] enemyPositions)
        {

            GameObject enemyToSpawn;
            for (int i = 0; i < enemyPositions.Length; i++)
            {
                int randomEnemyIndex = Random.Range(0, _availableEnemyPools.Length);
                enemyToSpawn = PoolManager.Instance.ObjectPool[(int)_availableEnemyPools[randomEnemyIndex]].Get();
                enemyToSpawn.transform.position = new Vector3(
                                  _terrainTranform.position.x + (enemyPositions[i].x * _terrainTranform.localScale.x)
                                , _terrainTranform.position.y + (enemyPositions[i].y * _terrainTranform.localScale.y) + 0.05f
                                , _terrainTranform.position.z + (enemyPositions[i].z * _terrainTranform.localScale.z));
                enemyToSpawn.SetActive(true);
                // Debug.Log($"Spawning Enemie | i : {i} | pos : {enemyToSpawn.transform.position} | scale : {_terrainTranform.localScale}");
            }
        }
    }
}
