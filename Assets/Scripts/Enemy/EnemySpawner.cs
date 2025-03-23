
using BombDrop.Global;
using BombDrop.Utils;
using UnityEngine;

namespace BombDrop.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private PoolManager.PoolType[] _availableEnemyPools;
        [SerializeField] private Transform _terrainTranform;

        private const float cSpawnXOffset = -3.0f, cSpawnYOffset = 1.7f;

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
            GameManager.Instance.EnemiesLeft = enemyPositions.Length / 2;

            GameObject enemyToSpawn;
            Vector3 spawnDir = Vector3.zero, spawnPos = Vector3.zero;
            float spawnAngle;
            int randomEnemyIndex;
            // for (int i = 0; i < enemyPositions.Length; i++)
            for (int i = 1; i < enemyPositions.Length; i += 2)
            {
                randomEnemyIndex = Random.Range(0, _availableEnemyPools.Length);
                enemyToSpawn = PoolManager.Instance.ObjectPool[(int)_availableEnemyPools[randomEnemyIndex]].Get();

                spawnPos = new Vector3(
                                  _terrainTranform.position.x + (enemyPositions[i].x * _terrainTranform.localScale.x) //+ cSpawnXOffset
                                , _terrainTranform.position.y + (enemyPositions[i].y * _terrainTranform.localScale.y) //+ cSpawnYOffset
                                , _terrainTranform.position.z + (enemyPositions[i].z * _terrainTranform.localScale.z));

                ///*
                spawnDir.Set(_terrainTranform.position.x + (enemyPositions[i - 1].x * _terrainTranform.localScale.x) //+ cSpawnXOffset
                                , _terrainTranform.position.y + (enemyPositions[i - 1].y * _terrainTranform.localScale.y) //+ cSpawnYOffset
                                , _terrainTranform.position.z + (enemyPositions[i - 1].z * _terrainTranform.localScale.z));
                spawnDir = (spawnDir - spawnPos).normalized;
                spawnAngle = MathfHelper.Atan2Approximation1(spawnDir.y, spawnDir.x) * Mathf.Rad2Deg;
                enemyToSpawn.transform.rotation = Quaternion.Euler(0f, 0f, spawnAngle - 180);//*/

                //Finding normal to the direction vector
                // Vector3 normalToDirectionVec = new Vector3(-(spawnDir.y - spawnPos.y),
                //             (spawnDir.x - spawnPos.x), 0f).normalized;
                spawnPos += (spawnDir * 3f) + (enemyToSpawn.transform.up * 1.35f);
                // spawnPos += normalToDirectionVec * 1.5f;
                // spawnPos.y += cSpawnYOffset;     //This will only raise in y-axis | This will result in shifting directly up and not perpendicular to the direction vector
                // spawnPos.y = _terrainTranform.position.y + ((enemyPositions[i].y + enemyPositions[i - 1].y) / 2 * _terrainTranform.localScale.y) + cSpawnYOffset;
                // Debug.Log($"Spawn Dir : {spawnDir} | i : {i} | spawnPos : {spawnPos} | spawnAngle : {spawnAngle}");
                enemyToSpawn.transform.position = spawnPos;
                enemyToSpawn.SetActive(true);

                // Debug.Log($"Spawning Enemie | i : {i} | pos : {enemyToSpawn.transform.position} | scale : {_terrainTranform.localScale}");
            }
        }
    }
}
