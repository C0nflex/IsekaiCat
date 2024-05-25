using UnityEditor;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class cameraControlTrigger : MonoBehaviour
{
    public CustomInspectorObjects customInspectorObjects;
    private BoxCollider2D _coll;

    private void Start()
    {
        _coll = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (customInspectorObjects.panCameraOnContact)
            {
                cameraManger.Instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            Vector2 exitDirection = (collision.transform.position -_coll.bounds.center).normalized;

            if(customInspectorObjects.swapCameras && customInspectorObjects.cameraOnLeft != null && customInspectorObjects.cameraOnRight != null)
            {
                //swap cameras
                cameraManger.Instance.SwapCamera(customInspectorObjects.cameraOnLeft, customInspectorObjects.cameraOnRight, exitDirection);
            }

            if (customInspectorObjects.panCameraOnContact)
            {
                cameraManger.Instance.PanCameraOnContact(customInspectorObjects.panDistance, customInspectorObjects.panTime, customInspectorObjects.panDirection, true);
            }
        }
    }
}


    [System.Serializable]
    public class CustomInspectorObjects
    {
        public bool swapCameras = false;
        public bool panCameraOnContact = false;

        [HideInInspector] public CinemachineVirtualCamera cameraOnLeft;
        [HideInInspector] public CinemachineVirtualCamera cameraOnRight;

        [HideInInspector] public PanDirection panDirection;
        [HideInInspector] public float panDistance = 3f;
        [HideInInspector] public float panTime = 0.35f;


    }

    public enum PanDirection
    {
        Up,
        Down,
        Left,
        Right
    }

   
#if UNITY_EDITOR
    [CustomEditor(typeof(cameraControlTrigger))]
public class MyScriptEditor : Editor
    {
        cameraControlTrigger cameraControlTrigger;

        private void OnEnable()
        {
            cameraControlTrigger = (cameraControlTrigger)target;
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (cameraControlTrigger.customInspectorObjects.swapCameras)
            {
                cameraControlTrigger.customInspectorObjects.cameraOnLeft = EditorGUILayout.ObjectField("Camera on Left", cameraControlTrigger.customInspectorObjects.cameraOnLeft,
                    typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;

                cameraControlTrigger.customInspectorObjects.cameraOnRight = EditorGUILayout.ObjectField("Camera on Right", cameraControlTrigger.customInspectorObjects.cameraOnRight,
                    typeof(CinemachineVirtualCamera), true) as CinemachineVirtualCamera;
            }

            if (cameraControlTrigger.customInspectorObjects.panCameraOnContact)
            {
                cameraControlTrigger.customInspectorObjects.panDirection = (PanDirection)EditorGUILayout.EnumPopup("Camera Pan Direction",
                    cameraControlTrigger.customInspectorObjects.panDirection);

                cameraControlTrigger.customInspectorObjects.panDistance = EditorGUILayout.FloatField("Pan Distance", cameraControlTrigger.customInspectorObjects.panDistance);
                cameraControlTrigger.customInspectorObjects.panTime = EditorGUILayout.FloatField("Pan Time", cameraControlTrigger.customInspectorObjects.panTime);
            }

            if (GUI.changed)
            {
                EditorUtility.SetDirty(cameraControlTrigger);
            }
        }
    }

#endif
    



