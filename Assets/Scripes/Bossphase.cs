using UnityEngine;
using static Bossphase;

public class Bossphase : MonoBehaviour
{
    [System.Serializable]
    public struct BossObject
    {
        public GameObject prefab2;
        [Range(0f, 1f)]
        public float bossChance;
    }

    public BossObject[] objects2;

    public float minBSpawnRate = 0.3f;
    public float maxBSpawnRate = 0.6f;

    private void OnEnable()
    {
        Invoke(nameof(BSpawn), Random.Range(minBSpawnRate, maxBSpawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void BSpawn()
    {
        float bossChance = Random.value;

        foreach (var obj in objects2)
        {
            if (bossChance < obj.bossChance)
            {
                GameObject obstacle = Instantiate(obj.prefab2);

                float randomYPosition = Random.Range(0f, 3.5f);

                obstacle.transform.position = new Vector3(transform.position.x, randomYPosition, transform.position.z);
                break;
            }

            bossChance -= obj.bossChance;
        }

        Invoke(nameof(BSpawn), Random.Range(minBSpawnRate, maxBSpawnRate));
    }
}

