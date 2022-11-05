using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Class for an AI Agent to handle AI states
/// </summary>
public class AIStateController : MonoBehaviour
{
    [Header("State Control")]
    [Tooltip("The current state the FSM is in")]
    public AIState currentState;
    [Tooltip("Whehter this FSM is active")]
    public bool isActive;

    [Header("Navigation")]
    [Tooltip("The NavMeshAgent used by this controller")]
    [SerializeField] private NavMeshAgent agent;

    private void Awake()
    {
        // Set up AI
        Setup();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            // Update current state
            currentState.UpdateState(this);
        }
    }

    /// <summary>
    /// Sets up the FSM
    /// </summary>
    public void Setup()
    {

    }
}
