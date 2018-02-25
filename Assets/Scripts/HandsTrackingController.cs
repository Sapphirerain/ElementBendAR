// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace HoloLensHandTracking
{

    /// <summary>
    /// HandsManager determines if the hand is currently detected or not.
    /// </summary>
    public class HandsTrackingController : MonoBehaviour
    {
        /// <summary>
        /// HandDetected tracks the hand detected state.
        /// Returns true if the list of tracked hands is not empty.
        /// </summary>

        public bool HandDetected
        {
            get { return trackedHands.Count > 0; }
        }

        // Cube that acts as the parent of 
        public GameObject TrackingObject;
        public GameObject FireBase;
        public GameObject currFireBall;

        public GameObject WaterBase;
        public GameObject currWaterBall;

        private int numFireballs;

        //public FireBall fireScript;


        public float fireScaleMultiplier = 0.01f;

        public Color DefaultColor = Color.green;
        public Color TapColor = Color.blue;
        public Color HoldColor = Color.red;

        private HashSet<uint> trackedHands = new HashSet<uint>();
        private Dictionary<uint, GameObject> trackingObject = new Dictionary<uint, GameObject>();
        private GestureRecognizer gestureRecognizer;
        private uint activeId;

        void Awake()
        {
            numFireballs = 0;

            InteractionManager.InteractionSourceDetected += InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated += InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost += InteractionManager_InteractionSourceLost;

            gestureRecognizer = new GestureRecognizer();
            gestureRecognizer.SetRecognizableGestures(GestureSettings.Tap | GestureSettings.Hold);
            gestureRecognizer.Tapped += GestureRecognizerTapped;
            gestureRecognizer.HoldStarted += GestureRecognizer_HoldStarted;
            gestureRecognizer.HoldCompleted += GestureRecognizer_HoldCompleted;
            gestureRecognizer.HoldCanceled += GestureRecognizer_HoldCanceled;            
            gestureRecognizer.StartCapturingGestures();
        }

        void ChangeObjectColor(GameObject obj, Color color)
        {            
            var rend = obj.GetComponentInChildren<Renderer>();
            if (rend)
            {
                rend.material.color = color;
                Debug.LogFormat("Color Change: {0}", color.ToString());
            }
        }

        private void GestureRecognizer_HoldStarted(HoldStartedEventArgs args)
        {
            uint id = args.source.id;            
           
            if (trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], HoldColor);
              

                //Fireball creation moved to HoldCompleted
            }
        }

        private void GestureRecognizer_HoldCompleted(HoldCompletedEventArgs args)
        {

            uint id = args.source.id;            
            //StatusText.text = $"HoldCompleted - Kind:{args.source.kind.ToString()} - Id:{id}";
            if(trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], DefaultColor);
               // FireStop();
            }

            
        }

        private void GestureRecognizer_HoldCanceled(HoldCanceledEventArgs args)
        {

            uint id = args.source.id;            
            if (trackingObject.ContainsKey(activeId))
            {

                ChangeObjectColor(trackingObject[activeId], DefaultColor);
              //  FireStop();
            }
        }

        bool fireWater = true;

        private void GestureRecognizerTapped(TappedEventArgs args)
        {

            if (fireWater && currFireBall == null && currWaterBall == null)
            {
                fireWater = false;
                currFireBall = Instantiate(FireBase);
                numFireballs++;

                currFireBall.transform.SetParent(trackingObject[activeId].GetComponent<Transform>());
                currFireBall.transform.position = new Vector3(trackingObject[activeId].transform.position.x, trackingObject[activeId].transform.position.y + 0.1f, trackingObject[activeId].transform.position.z) ;
            }
            else if (currWaterBall == null && currFireBall == null)
            {
                fireWater = true;

                currWaterBall = Instantiate(WaterBase);
                numFireballs++;

                currWaterBall.transform.SetParent(trackingObject[activeId].GetComponent<Transform>());
                currWaterBall.transform.position = new Vector3(trackingObject[activeId].transform.position.x, trackingObject[activeId].transform.position.y + 0.1f, trackingObject[activeId].transform.position.z);
            } else
            {
                if (currFireBall != null)
                {
                    
                    currFireBall.GetComponent<FireBall>().Shoot();
                    currFireBall = null;
                    //Destroy(currFireBall);
                    numFireballs--;
                    //currFireBall.GetComponent<FireBall>().Scale();
                    //currFireBall.transform.position = new Vector3(TrackingObject.transform.position.x + 3, TrackingObject.transform.position.y, TrackingObject.transform.position.z);
                }

                if (currWaterBall != null)
                {
                    currWaterBall.GetComponent<FireBall>().Shoot();
                    currWaterBall = null;
                }
            }


            uint id = args.source.id;
            //StatusText.text = $"Tapped - Kind:{args.source.kind.ToString()} - Id:{id}";
            if (trackingObject.ContainsKey(activeId))
            {
                ChangeObjectColor(trackingObject[activeId], TapColor);
            }
        }
        

        private void InteractionManager_InteractionSourceDetected(InteractionSourceDetectedEventArgs args)
        {
            uint id = args.state.source.id;
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }            
            trackedHands.Add(id);
            activeId = id;

            var obj = Instantiate(TrackingObject) as GameObject;
            Vector3 pos;

            if (args.state.sourcePose.TryGetPosition(out pos))
            {
                obj.transform.position = pos;
            }

            trackingObject.Add(id, obj);
        }

        private void InteractionManager_InteractionSourceUpdated(InteractionSourceUpdatedEventArgs args)
        {
            uint id = args.state.source.id;
            Vector3 pos;
            Quaternion rot;

            if (args.state.source.kind == InteractionSourceKind.Hand)
            {
                if (trackingObject.ContainsKey(id))
                {
                    if (args.state.sourcePose.TryGetPosition(out pos))
                    {
                        trackingObject[id].transform.position = pos;
                    }

                    if (args.state.sourcePose.TryGetRotation(out rot))
                    {
                        trackingObject[id].transform.rotation = rot;
                    }
                }
            }

        }

        private void InteractionManager_InteractionSourceLost(InteractionSourceLostEventArgs args)
        {
            if (currFireBall != null)
            {
                currFireBall.GetComponent<FireBall>().Shoot();
                currFireBall = null;
                //Destroy(currFireBall);
                numFireballs--;
                //currFireBall.GetComponent<FireBall>().Scale();
                //currFireBall.transform.position = new Vector3(TrackingObject.transform.position.x + 3, TrackingObject.transform.position.y, TrackingObject.transform.position.z);
            }

            if (currWaterBall != null)
            {
                currWaterBall.GetComponent<FireBall>().Shoot();
                currWaterBall = null;
            }


            uint id = args.state.source.id;
            // Check to see that the source is a hand.
            if (args.state.source.kind != InteractionSourceKind.Hand)
            {
                return;
            }

            if (trackedHands.Contains(id))
            {
                trackedHands.Remove(id);
            }

            if (trackingObject.ContainsKey(id))
            {
                var obj = trackingObject[id];
                trackingObject.Remove(id);
                Destroy(obj);
            }
            if (trackedHands.Count > 0)
            {
                activeId = trackedHands.First();
            }
        }

        void OnDestroy()
        {                        
            InteractionManager.InteractionSourceDetected -= InteractionManager_InteractionSourceDetected;
            InteractionManager.InteractionSourceUpdated -= InteractionManager_InteractionSourceUpdated;
            InteractionManager.InteractionSourceLost -= InteractionManager_InteractionSourceLost;

            gestureRecognizer.Tapped -= GestureRecognizerTapped;
            gestureRecognizer.HoldStarted -= GestureRecognizer_HoldStarted;
            gestureRecognizer.HoldCompleted -= GestureRecognizer_HoldCompleted;
            gestureRecognizer.HoldCanceled -= GestureRecognizer_HoldCanceled;
            gestureRecognizer.StopCapturingGestures();
        }
    }
}