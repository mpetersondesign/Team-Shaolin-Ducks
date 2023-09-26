using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    //Create a dictionary to hold our states, using a string key for naming them
    public Dictionary<string, State> States = new Dictionary<string, State>();

    //Create a list of strings to hold the keys of all the states added to the machine
    public List<string> AddedStateNames = new List<string>();

    //Define the current state we're in
    public State CurrentState;

    private void Start()
    {
        //Initialize all states on machine object
        InitializeStates();
    }

    public void InitializeStates()
    {
        //Grab all states attached to this object, store them in an array
        State[] attachedStates = GetComponents<State>();

        //For each state in the array
        for (int i = 0; i < attachedStates.Length; i++)
        {
            //Add the state's name to addedstatenames list
            AddedStateNames.Add(attachedStates[i].Key);

            //Run initialize on the current state in array
            attachedStates[i].Initialize(this);
        }
    }

    private void Update()
    {
        //? acts as a null check, meaning if CurrentState isnt null, run Tick()
        CurrentState?.Tick();
    }

    public void ChangeState(string key)
    {
        //Identify the next state by searching our dictionary for given key
        State nextState = States[key];

        //Run exit function on our current state
        CurrentState.Exit(key, nextState);

        //Run enter function on our next state
        nextState.Enter(CurrentState.Key, CurrentState);

        //Set currentState reference to next state
        CurrentState = nextState;
    }

    public void SetCurrentState(string key) => CurrentState = States[key];
    public void AttachState(string key, State state) => States.Add(key, state);
    public void RemoveState(string key) => States.Remove(key);
}

[System.Serializable]
//Create an "abstract" (Meaning we can change parameters after initialization) class
//Make sure that you add : MonoBehaviour to include Unity's library functions
public abstract class State : MonoBehaviour
{
    //Create a string for state name that will define its place in our dictionary
    [SerializeField] public string Key;

    //Define the machine that will inherit this state
    //Protected meaning nothing outside of the class can interfere with the value of the variable
    protected StateMachine Machine;

    //Identify the state machine we want to attach this state to
    public void Initialize(StateMachine machine)
    {
        //Grab the state machine and set it to machine
        Machine = machine;

        //Tell that statemachine to run AttachState function
        machine.AttachState(Key, this);

        //Initialize this state if applicable
        this.OnStateInitialize();
    }

    protected abstract void OnStateInitialize();

    //Create functions for Enter, Update, and Exit
    //Optionally pass through previous/next state info for additional tweaking
    public abstract void Enter(string previous_key, State previous_state);
    public abstract void Tick();
    public abstract void Exit(string next_key, State next_state);



}
