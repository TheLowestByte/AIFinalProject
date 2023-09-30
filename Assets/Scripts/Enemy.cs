using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    public int currentNode, previousNode;
    public enum EnemyState
    {
        patrol,
        chase
    };
    public enum PatrolType
    {
        all,
        set1,
        set2
    };
    EnemyState enemyState = EnemyState.patrol;
    public PatrolType patrolType;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GameManager.gm.player.transform.position);
        agent = GetComponent<NavMeshAgent>();
        changeNode();
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyState)
        {
            case EnemyState.patrol: Patrol(); break;
            case EnemyState.chase: Chase(); break;
            default: break;
        }
    }
    void changeNode()
    {
        Transform[] nodes;
        switch (patrolType)
        {
            case PatrolType.all: nodes = GameManager.gm.AllNodes; break;
            case PatrolType.set1: nodes = GameManager.gm.Patrol1; break;
            case PatrolType.set2: nodes = GameManager.gm.Patrol2; break;
            default: nodes = GameManager.gm.AllNodes; break;
        }
        while ((nodes[currentNode].position - transform.position).magnitude < 3)
        {
            currentNode = Random.Range(0, nodes.Length);
        }
        previousNode = currentNode;
    }
    void Patrol()
    {
        switch (patrolType)
        {
            case PatrolType.all: agent.destination = GameManager.gm.AllNodes[currentNode].transform.position; break;
            case PatrolType.set1: agent.destination = GameManager.gm.Patrol1[currentNode].transform.position; break;
            case PatrolType.set2: agent.destination = GameManager.gm.Patrol2[currentNode].transform.position; break;
            default: break;
        }
    }
    void Chase()
    {
        agent.destination = GameManager.gm.player.transform.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Node")
        {
            changeNode();
        }
        if (other.tag == "Player")
        {
            enemyState = EnemyState.chase;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            enemyState = EnemyState.patrol;
        }
    }
}
