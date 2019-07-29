using AO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{

    public class CameraManager : BaseMonoBehaviour
    {
        public event Action<PoolCamera> PositionChanged;

        [SerializeField]
        private PoolCamera TopCamera;

        [SerializeField]
        private PoolCamera TournamentCamera;

        [SerializeField]
        private PoolCamera FocusCamera;

        [SerializeField]
        private PoolCamera InitialCamera;

        [SerializeField]
        private PoolCamera ReplayCamera;

        private GameObject FocusCameraParent;

        private BaseCameraState state;


        public BaseCameraState State {
            get { return state; }
            set {


                if (PrevState != null && !PrevState.Equals(State))
                {
                    PrevState = State;
                }
                else
                {
                    PrevState = null;
                }

                if (State != null)
                {
                    State.Camera.SetEnabled(false);
                }

                state = value;

                state.Handle();
            }
        }

        private BaseCameraState PrevState;

        private void OnEnable()
        {
            FocusCameraParent = new GameObject("FocusCameraParent");
            FocusCamera.transform.parent = FocusCameraParent.transform;
            FocusCameraParent.transform.parent = transform;
        }

        protected override void OnAwake()
        {
            base.OnAwake();

            AllCameraDisabled();


            InitialState();
        }

        private void AllCameraDisabled()
        {
            TopCamera.SetEnabled(false);
            TournamentCamera.SetEnabled(false);
            FocusCamera.SetEnabled(false);
            InitialCamera.SetEnabled(false);
        }

        public void InitialState()
        {

            Unbinding();
            State = new InitialCameraState(InitialCamera);
            Binding();
        }


        public void FocusedState(GameObject focusedGameObject, Quaternion initialRotation)
        {

            Unbinding();
            State = new FocusCameraState(FocusCamera, focusedGameObject, initialRotation);
            Binding();
        }

        public void ReplayState()
        {
            Unbinding();
            State = new ReplayCameraState(ReplayCamera);
            Binding();
        }

        public void TournamentState()
        {

            Unbinding();
            State = new TournamentCameraState(TournamentCamera);
            Binding();
        }

        public void TopState()
        {

            Unbinding();
            State = new TopCameraState(TopCamera);
            Binding();
        }

        public void Previous()
        {
            if (PrevState != null)
            {
                Unbinding();
                State = PrevState;
                Binding();
            }

            //throw new Exception("buraya bak bi");

        }

        private void Binding()
        {
            State.CameraPositionChanged += State_CameraPositionChanged;
        }

        private void Unbinding()
        {
            if (State != null)
                State.CameraPositionChanged -= State_CameraPositionChanged;
            
        }

        private void State_CameraPositionChanged(PoolCamera camera)
        {
            PositionChanged?.Invoke(camera);
        }

        public void Reset()
        {
            InitialState();
        }
    }
}
