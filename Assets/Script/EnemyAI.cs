using UnityEngine;
using UnityEngine.AI; // <--- This is REQUIRED for NavMesh

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Transform _playerTransform;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        // 1. Find the Player automatically
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            _playerTransform = playerObj.transform;
        }
        else
        {
            Debug.LogError("Enemy cannot find the Player! Did you forget to Tag the Player object?");
        }
    }

    void Update()
    {
        // 2. Chase the Player every frame
        if (_playerTransform != null)
        {
            _agent.SetDestination(_playerTransform.position);
        }
    }
}