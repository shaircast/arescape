//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

using System.Net;
using TMPro;

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
//        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyPlanePrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;


        private int state = 0;
        // state#Entered는 그 스테이트 처음 들어갔을 때 초기화시키고 재실행되지 않는 부분들을 위한 변수.
        private bool state0Entered = true;
        private bool doesRosettaExist = false;
        private bool state1Entered = true;
        private bool state2Entered = true;
        private bool state3Entered = true;
        private bool state4Entered = true;
        public GameObject rosettaPrefab;
        private GameObject rosetta;
        private Vector3 screenCenter;

        void Start()
        {
            // 직관적 사용을 위한 기기 중앙점 설정.
            screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.2f, 0));
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);

            // If the player has not touched the screen, we are done with this update.
//            Touch touch;
//            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//            {
//                return;
//            }


            if (state == 0) // 게임 처음 시작해서 아무것도 없는 상태
            {
                // 첫 실행 초기화 
                if (state0Entered)
                {
                    state0Entered = false;
                }
                
                // 감지된 평면 찾아서 스톤 위치 결정.
                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                                                  TrackableHitFlags.FeaturePointWithSurfaceNormal;
                    // 중앙점에서 빔을 쏴서 감지된 평면에 맞은 곳 공중에 스톤 위치.
                if (Frame.Raycast(screenCenter.x, screenCenter.y, raycastFilter, out hit))
                {
                    // 맨 처음에 스톤 없으면 새로 생성
                    if (!doesRosettaExist)
                    {
                        rosetta = Instantiate(rosettaPrefab);
                        doesRosettaExist = true;
                    }
                    
                    // 맞은 곳의 공중으로 좌표 계속 갱신
                    rosetta.transform.position = hit.Pose.position + Vector3.up * 0.3f;
                                        
                    // 이하는 터치 있으면 실행되는 부분
                    Touch touch;
                    if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                    {
                        return;
                    }
          
                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make Andy model a child of the anchor.
                    rosetta.transform.parent = anchor.transform;
                    // 평면 추적 멈추기 #TODO
//                    ARCoreSessionConfig.
                    

                    state = 1;
                }
            }
            else if (state == 1)
            {
                
            }
            else if (state == 2)
            {
                
            }
            else if (state == 3)
            {
                
            }
            else if (state == 4)
            {
                
            }
            else if (state == 5)
            {
                
            }
            
            

            // Raycast against the location the player touched to search for planes.
//            TrackableHit hit;
//            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
//                TrackableHitFlags.FeaturePointWithSurfaceNormal;
//
//            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
//            {
//                
//                // Choose the Andy model for the Trackable that got hit.
//                GameObject prefab = AndyPlanePrefab;
//              
//
//                // Instantiate Andy model at the hit pose.
//                var andyObject = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);
//
//                // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
//                andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
//
//                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
//                // world evolves.
//                var anchor = hit.Trackable.CreateAnchor(hit.Pose);
//
//                // Make Andy model a child of the anchor.
//                andyObject.transform.parent = anchor.transform;
//                
//            }
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("카메라 권한이 필요합니다.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("오류가 발생했습니다. 앱을 재시작해주세요.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        
    
        
//        public TrackableHit HitFromScreenCoord(float x, float y)
//        {
//            TrackableHit hit;
//            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
//                                              TrackableHitFlags.FeaturePointWithSurfaceNormal;
//            Frame.Raycast(x, y, raycastFilter, out hit);
//
//            return hit;
//        }
        
        
    }
}
