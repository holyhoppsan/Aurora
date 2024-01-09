using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

namespace FSM
{
    public interface IState
    {
        void Enter();
        void Update();
        void Exit();
    }

    public class FiniteStateMachine : MonoBehaviour
    {
        [SerializeField]
        private string _startingState;

        [SerializeField]
        private bool _debugEnabled = false;
        private Dictionary<string, IState> _states;

        private string _currentStateId;

        public string CurrenState
        {
            get
            {
                return _currentStateId;
            }

            set
            {
                Transition(value);
            }
        }

        private void Transition(string state)
        {
            IState currentState = null;
            if (!_states.TryGetValue(_currentStateId, out currentState))
            {
                throw new KeyNotFoundException($"Could find current state '{_currentStateId}' to exit.");
            }

            currentState.Exit();

            IState newState = null;
            if (!_states.TryGetValue(state, out newState))
            {
                throw new KeyNotFoundException($"Could find current state '{state}' to enter.");
            }

            newState.Enter();
            _currentStateId = state;
        }

        private void Awake()
        {
            _states = new Dictionary<string, IState>();

            var logoScreenState = new LogoScreenState();
            _states.Add(logoScreenState.GetType().Name, logoScreenState);

            _currentStateId = _startingState;
            _states[_currentStateId].Enter();
        }

        // Start is called before the first frame update
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
            _states[_currentStateId].Update();
        }

        private void OnGUI()
        {
            if (_debugEnabled)
            {
                GUI.Label(new Rect(10, 10, 300, 20), $"CurrentState: {CurrenState}.");
            }
        }
    }


}
