using UnityEngine;

public class DuckEnemyRegister : MonoBehaviour
{
    private EnemyManager _enemyManager;

    void Start()
    {
        _enemyManager = FindObjectOfType<EnemyManager>();
        _enemyManager.RegisterEnemy(GetComponentInParent<EnemyController>());
    }
}