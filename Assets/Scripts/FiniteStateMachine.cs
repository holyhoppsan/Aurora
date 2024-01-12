using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class FSMState : MonoBehaviour
    {
        protected FiniteStateMachine _fsm;
        void Awake()
        {
            _fsm = GetComponentInParent<FiniteStateMachine>();
        }

        public abstract void Enter();
        public abstract void Tick();
        public abstract void Exit();
    }

    public class FiniteStateMachine : MonoBehaviour
    {
        [SerializeField]
        private string _startingState;

        [SerializeField]
        private bool _debugEnabled = false;

        [SerializeField]
        private List<FSMState> _states = new List<FSMState>();

        private FSMState _currentState;

        public void Transition<T>() where T : FSMState
        {
            if (_currentState != null)
            {
                _currentState.Exit();
            }

            FSMState newState = FindStateOfType<T>();
            if (newState == null)
            {
                throw new KeyNotFoundException($"Couldn't find current state '{typeof(T)}' to enter.");
            }

            newState.Enter();
            _currentState = newState;
        }

        private FSMState FindStateOfType<T>() where T : FSMState
        {
            return _states.Find(state => state is T);
        }

        private void Awake()
        {
            _currentState = _states.Find(state => state.GetType().Name == _startingState);
            _currentState.Enter();
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            _currentState.Tick();
        }

        private void OnGUI()
        {
            if (_debugEnabled)
            {
                GUI.Label(new Rect(10, 10, 300, 20), $"CurrentState: {_currentState.GetType().Name}.");
            }
        }
    }


}
