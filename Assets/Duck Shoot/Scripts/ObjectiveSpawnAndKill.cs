using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using SystemRandom = System.Random;

namespace Duck_Shoot
{
    [RequireComponent(typeof(Objective))]
    public class ObjectiveSpawnAndKill : MonoBehaviour
    {
        [Tooltip("Define quantos patos vão aparecer por round.")]
        [Min(1)]
        public int duckSpawnRate = 1;

        [Tooltip("Define quantos segundos dura um round.")]
        public int duckSpawnLifetime = 2;

        [Tooltip("Define quanto tempo durara o jogo até ele acabar.")]
        public int timeUntilObjectiveEnds = 60;

        [Tooltip("Define a distancia do pato do lugar de Spawn")]
        public Vector3 SpawnDistanceFromTarget = new Vector3(-3, 0, 0);

        [Tooltip("Define a rotação do pato ao nascer")]
        public Vector3 DuckRotation;

        [Tooltip("O prefab do Pato.")]
        public GameObject DuckPrefab;

        [Tooltip("O texto que notificará quanto tempo falta")]
        public TextMeshProUGUI TimeCounter;

        private List<GameObject> m_SpawnTargets;
        private List<GameObject> m_SpawnedDucks = new List<GameObject>();
        private int m_CurrentTime = 0;

        private Coroutine m_TimerCoroutine;
        private Coroutine m_SpawnerCoroutine;
        private Coroutine m_SpawnAndKillRoutine;

        EnemyManager m_EnemyManager;
        Objective m_Objective;

        int m_KillTotal;

        void Start()
        {
            m_Objective = GetComponent<Objective>();
            DebugUtility.HandleErrorIfNullGetComponent<Objective, ObjectiveSpawnAndKill>(m_Objective, this, gameObject);

            m_EnemyManager = FindObjectOfType<EnemyManager>();
            DebugUtility.HandleErrorIfNullFindObject<EnemyManager, ObjectiveSpawnAndKill>(m_EnemyManager, this);
            m_EnemyManager.onRemoveEnemy += OnKillEnemy;

            m_SpawnTargets = GameObject.FindGameObjectsWithTag("SpawnSpot").ToList();

            m_Objective.title = $"Elimine o máximo de patos em {timeUntilObjectiveEnds}s";

            if (string.IsNullOrEmpty(m_Objective.description))
                m_Objective.description = GetUpdatedCounterAmount();

            if (duckSpawnRate > m_SpawnTargets.Count) {
                Debug.LogError("Há mais patos do que lugares para eles spawnarem, capando número de patos.");
                Debug.LogError("Dica: Verifique se você tageou corretamente os spots com \"SpawnSpot\".");

                duckSpawnRate = m_SpawnTargets.Count;
            }

            m_CurrentTime = timeUntilObjectiveEnds;

            m_TimerCoroutine = StartCoroutine(TimerUntilFinishRoutine());
            m_SpawnerCoroutine = StartCoroutine(DuckSpawnerRoutine());
        }

        #region Timer

        private IEnumerator TimerUntilFinishRoutine()
        {
            TimeCounter.text = $"Restam {m_CurrentTime}";

            while (m_CurrentTime > 0) {
                yield return new WaitForSeconds(1);

                m_CurrentTime--;

                TimeCounter.text = $"Restam {m_CurrentTime}";
            }

            EndGame();
        }

        private void EndGame()
        {
            StopCoroutine(m_TimerCoroutine);

            StaticGameVariables.DucksKilled = m_KillTotal;
            
            SceneManager.LoadScene("DuckWinScene");
        }

        private void UpdateObjectiveWithTime()
        {
            var notificationText = $"Você matou {m_KillTotal} patos, restam {m_CurrentTime}s";

            m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
        }

        #endregion

        #region Ducker Spawner

        private IEnumerator DuckSpawnerRoutine()
        {
            while (m_CurrentTime > 0) {
                foreach (var mSpawnedDuck in m_SpawnedDucks) {
                    Destroy(mSpawnedDuck);
                }

                var random = new SystemRandom();
                var randomSpawnTargets = m_SpawnTargets.ToArray().OrderBy(a => random.Next()).Take(duckSpawnRate);

                foreach (var spawnGameObject in randomSpawnTargets) {
                    var spawnPosition = spawnGameObject.transform.position + SpawnDistanceFromTarget;
                    var duckPrefab = Instantiate(DuckPrefab, spawnPosition, Quaternion.Euler(DuckRotation.x, DuckRotation.y, DuckRotation.z));

                    m_SpawnedDucks.Add(duckPrefab);
                }

                yield return new WaitForSeconds(duckSpawnLifetime);
            }

            StopCoroutine(m_SpawnerCoroutine);
        }

        #endregion

        void OnKillEnemy(EnemyController enemy, int remaining)
        {
            if (m_Objective.isCompleted)
                return;

            m_KillTotal = m_EnemyManager.numberOfEnemiesTotal - remaining;

            var notificationText = $"Você matou {m_KillTotal} patos, restam {m_CurrentTime}s";
            m_Objective.UpdateObjective(string.Empty, GetUpdatedCounterAmount(), notificationText);
        }

        string GetUpdatedCounterAmount()
        {
            return m_KillTotal + " patos";
        }
    }
}