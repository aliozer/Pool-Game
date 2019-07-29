using AO.Utilities;
using PoolGame.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PoolGame
{
    public class FocusCameraState : BaseCameraState
    {

        public GameObject FocusedGameObject { get; }
        public Quaternion InitialRotation { get; }

        private Vector3 CameraLocalPosition = new Vector3(0.0f, 3.7f, -8.5f);
        

        private BaseDirectionRule FocusCameraDirectionRule;
        public FocusCameraState(PoolCamera camera, GameObject focusedGameObject, Quaternion initialRotation) : this(camera, focusedGameObject)
        {
            InitialRotation = initialRotation;
        }

        public FocusCameraState(PoolCamera camera, GameObject focusedGameObject) : base(camera)
        {
            FocusedGameObject = focusedGameObject;
        }

        private void Camera_ChangeEnabled(bool value)
        {
            if (value)
            {
                Camera.Updated += Camera_Updated;
            }
            else
            {
                Camera.Updated -= Camera_Updated;
                Camera.ChangeEnabled -= Camera_ChangeEnabled;
            }

        }


        private void Camera_Updated(PoolCamera value)
        {
            if (!UIUtil.IsMouseOverUI())
            {
                FocusCameraDirectionRule.Execute();
            }

            //Camera.transform.LookAt(FocusedGameObject.transform);
            Camera.transform.localRotation = Quaternion.Euler(23.445f, 0.0f, 0.0f);
            Camera.transform.parent.transform.position = FocusedGameObject.transform.position;
            OnCameraPositionChanged();
        }

        public override void Handle()
        {
            Camera.ChangeEnabled += Camera_ChangeEnabled;

            Camera.SetEnabled(true);
            

            FocusCameraDirectionRule = new FocusCameraDirectionRule(Camera.transform.parent);

            Camera.transform.parent.rotation = InitialRotation;
            Camera.transform.parent.transform.position = FocusedGameObject.transform.position;

            Camera.transform.localPosition = CameraLocalPosition;

            OnCameraPositionChanged();

        }
    }
}
