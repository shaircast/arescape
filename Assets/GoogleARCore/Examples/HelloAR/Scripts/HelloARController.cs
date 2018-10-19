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
using GoogleARCore.Examples.CloudAnchors;
using TMPro;

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;

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


        public int state = 0;

        public Text textUI;
        // state#Entered는 그 스테이트 처음 들어갔을 때 초기화시키고 재실행되지 않는 부분들을 위한 변수.
        private bool state0Entered = true;
        private bool state1Entered = true;
        private bool state2Entered = true;
        private bool state3Entered = true;
        private bool state4Entered = true;
        private bool state6Entered = true;

        public GameObject startingMarkPrefab;
        private GameObject startingMark;
        private bool doesStartingMarkExist = false;
        
        public GameObject rosettaPrefab;
        private GameObject rosetta;
        private bool doesRosettaExist = false;

        public GameObject geigerPrefab;
        private GameObject geiger;
        public List<GameObject> wordPiecesPrefab;
        private List<GameObject> wordPieces;
        public float pieceScatterDist;
        public float catchDist = 0.5f;
        
        private Vector3 screenCenterCoord;
        
        public AudioClip GeigerSound;
        AudioSource audio;

        public GameObject ballPrefab;
        private GameObject ball;
        public GameObject mazePrefab;
        private GameObject maze;

        private GameObject selectedTotem;
        public List<GameObject> totemPrefab;
        private List<GameObject> totem;
        public Button totem0btn;
        public Button totem1btn;
        public Button totem2btn;
        public Button totem3btn;
        private int buttonPress = -1;
        public float totemforce = 1f;

        void Start()
        {
            // 직관적 사용을 위한 기기 중앙점 설정.
            screenCenterCoord = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.2f, 0));
            // 변수초기화
            wordPieces = new List<GameObject>();
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
                if (Frame.Raycast(screenCenterCoord.x, screenCenterCoord.y, raycastFilter, out hit))
                {
                    // 맨 처음에 스톤 없으면 새로 생성
                    if (!doesStartingMarkExist)
                    {
                        startingMark = Instantiate(startingMarkPrefab);
                        doesStartingMarkExist = true;
                    }
                    
                    // 맞은 곳의 공중으로 좌표 계속 갱신
                    startingMark.transform.position = hit.Pose.position;
                                        
                    // 이하는 터치 있으면 실행되는 부분
                    Touch touch;
//                    if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//                    {
//                        return;
//                    }
                    if (!Input.GetMouseButtonDown(0))
                    {
                        return;}
          
                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    Anchor anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make Andy model a child of the anchor.
                    startingMark.transform.parent = anchor.transform;
                    // 평면 추적 멈추기 #TODO
//                    ARCoreSessionConfig.
                    

                    state = 1;
                }
            }
            else if (state == 1)
            {

                // #TODO 대사 넣어야함
                textUI.text = "책상 위의 공간을 응시해주세요. 무엇인가가 있습니다.";
                
                Touch touch;
//                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//                {
//                    return;
//                }
                if (!Input.GetMouseButtonDown(0))
                {
                    return;}
                
                textUI.text = "위대한 지도자시여, 머나먼 시간을 건너 이 메세지가 그대에게 도달하였습니다. 당신은 전생에 고대 문명의 지도자였지만 당신을 시기한 적들로 인해 암살을 당하고 강력한 무기를 빼앗겼습니다. 그 무기로 인해 우리의 문명은 멸망했습니다.";
                
                state = 2; 
            }
            else if (state == 2)
            {

                Touch touch;
//                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//                {
//                    return;
//                }
                if (!Input.GetMouseButtonDown(0))
                {
                    return;}

                textUI.text = "이 메세지는 전생한 당신의 영혼에 반응하여 시공간을 이어주는 포탈을 만들어 줄 것입니다. 당신만이 재앙의 무기를 멈추는 단어를 알아낼 수 있습니다. 포탈을 통해 글자를 과거로 보내주십시오.";
               
                //글자가 새겨진 커다란 벽이 나타난다.
                state = 3;

            }
            else if (state == 3)
            {
                
                Touch touch;
//                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//                {
//                    return;
//                }
                if (!Input.GetMouseButtonDown(0))
                {
                    return;}
                //벽이 열리고 그들의 도시가 보인다.

                textUI.text = "이것이 우리 문명의 마지막 모습입니다.";

                state = 4;
            }
            else if (state == 4)
            {
                Touch touch;
//                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//                {
//                    return;
//                }
                if (!Input.GetMouseButtonDown(0))
                {
                    return;}
                
                textUI.text = "첫 번째 단어는 당신의 영혼과 특수한 파장을 공유합니다. 이 장비를 통해 찾아낼 수 있습니다.";

                state = 5;
            }
            
            else if (state == 5) // 뒤로 미뤄둠
            {

                // 첫 실행 초기화
                if (state1Entered)
                {
                    // 가이거계수기 등장, 글자오브젝트 등장
                    geiger = Instantiate(geigerPrefab);
                    geiger.transform.position = FirstPersonCamera.transform.position + FirstPersonCamera.transform.forward * 0.05f
                                                                                     + FirstPersonCamera.transform.right * 0.055f
                                                                                     - FirstPersonCamera.transform.up * 0.02f;
                    geiger.transform.rotation = Quaternion.Euler(Vector3.forward);
                    geiger.transform.parent = FirstPersonCamera.transform;

                    rosetta = Instantiate(rosettaPrefab, startingMark.transform.position + Vector3.up * 0.2f, Quaternion.identity);
//                    maze = Instantiate(mazePrefab, startingMark.transform.position, Quaternion.identity);
//                    MazePoint mp = maze.GetComponent<MazePoint>();
//                    GameObject sp = mp.startingPoint;
//                    GameObject ep = mp.endingPoint;
//                    ball = Instantiate(ballPrefab, sp.transform.position, Quaternion.identity);
                    for (int i = 0; i < wordPiecesPrefab.Count; i++)
                    {
                        // 비석 주변으로 원형으로 뿌리기
                        float deg = 360f / wordPiecesPrefab.Count * i * Mathf.PI / 180f;
                        Vector3 piecePos = new Vector3(rosetta.transform.position.x + pieceScatterDist * Mathf.Cos(deg), 
                            rosetta.transform.position.y, 
                            rosetta.transform.position.z + pieceScatterDist * Mathf.Sin(deg));
                        GameObject tempPiece = Instantiate(wordPiecesPrefab[i], piecePos, Random.rotation);
                        tempPiece.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
                        wordPieces.Add(tempPiece);
                    }
                    
                    
                    
                    state1Entered = false;
                }
                
                Debug.Log(Vector3.Distance(FirstPersonCamera.transform.position, wordPieces[4].transform.position));

                if (Vector3.Distance(FirstPersonCamera.transform.position, wordPieces[4].transform.position) < catchDist)
                {
                    // 빛나고 소리
                    geiger.SetActive(false);
                    for (int i = 0; i < wordPieces.Count; i++)
                    {
                        wordPieces[i].SetActive(false);
                    }
                    textUI.text = "잘하셨습니다! 두 번째 단어는 우리 도시의 미로에 잠들어 있는 대지의 글자입니다. 미로를 풀어 단어를 찾아낼 수 있습니다. 화면을 터치하세요.";
                    state = 6;
                }

            }
            else if (state == 6)
            {
                if (state6Entered)
                {
                    maze = Instantiate(mazePrefab, startingMark.transform.position, Quaternion.identity);
                    MazePoint mp = maze.GetComponent<MazePoint>();
                    GameObject sp = mp.startingPoint;
                    ball = Instantiate(ballPrefab, sp.transform.position, Quaternion.identity);


                    state6Entered = false;
                }
                
                Touch touch;
//                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
//                {
//                    return;
//                }
                if (!Input.GetMouseButtonDown(0))
                {
                    return;}

                state = 7;

            }
            else if (state == 7)
            {
                textUI.gameObject.SetActive(false);
                totem0btn.gameObject.SetActive(true);
                totem1btn.gameObject.SetActive(true);
                totem2btn.gameObject.SetActive(true);
                totem3btn.gameObject.SetActive(true);

                Rigidbody brb = ball.GetComponent<Rigidbody>();
                MazePoint mp = maze.GetComponent<MazePoint>();
                
                if (buttonPress == 0) //totem0
                {
                    brb.AddForce((mp.t0Point.transform.position - maze.transform.position).normalized * totemforce);
                }
                else if (buttonPress == 1) //totem1
                {
                    brb.AddForce((mp.t1Point.transform.position - maze.transform.position).normalized * totemforce);
                }
                else if (buttonPress == 2) // totem2
                {
                    brb.AddForce((mp.t2Point.transform.position - maze.transform.position).normalized * totemforce);
                }
                else if (buttonPress == 3) // totem3
                {
                    brb.AddForce((mp.t3Point.transform.position - maze.transform.position).normalized * totemforce);
                }
                
             
                
            }
            else if (state == 8)
            {
                
                textUI.gameObject.SetActive(true);
                totem0btn.gameObject.SetActive(false);
                totem1btn.gameObject.SetActive(false);
                totem2btn.gameObject.SetActive(false);
                totem3btn.gameObject.SetActive(false);
                textUI.text = "위대한 지도자여! 당신은 두가지 단어를 알아냈습니다. 우리의 문명은 멸망을 면했습니다. 적들은 잠시 물러날 것입니다.. 하지만 언제든 그들의 위협이 있는 한 우리의 메세지는 세대와 문명을 넘어 당신에게 전달될 것입니다.";
                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    return;
                }

                // 전송 버튼을 누른다.
            }
            else if (state == 999)
            {
                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    textUI.text = "우리의 문명은 멸망을 면했습니다. 적들은 잠시 물러날 것입니다.. 하지만 언제든 그들의 위협이 있는 한 우리의 메세지는 세대와 문명을 넘어 당신에게 전달될 것입니다.";
                }

                // 강한 빛과 함께 포털과 사진이 사라진다...
            }
        }
        
        /* Pseudo Code*/
        // 고대문자가 우리가 찾는 고대문자인지 확인하고 맞다면 true값을 반환
//        void IsCorrectAncientScript(GameObject AncientScript) {
//            return AncientScript.isAnswer;
//        }
//        
//        /* Pseudo Code*/
//        // 고대문자를 활성화시킨다
//        void ActivateAncientScript() {
//            // 가이거 계수기 소리가 플레이 된다
//            audio = GetComponent<AudioSource>();
//            // Geiger Sound Effect 
//            audio.PlayOneShot(GeigerSound);
//            // 고대문자가 빛난다.
//            // Make Ancient Script Shining
//            GetComponent(Halo).enabled = true;
//        }


        public void btn0Pressed()
        {
            buttonPress = 0;
        }
        public void btn1Pressed()
        {
            buttonPress = 1;
        }
        public void btn2Pressed()
        {
            buttonPress = 2;
        }
        public void btn3Pressed()
        {
            buttonPress = 3;
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
        
    
        
    }
}
