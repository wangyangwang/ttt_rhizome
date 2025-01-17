﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ThingSpace
{
    public class CameraSwitcher : MonoBehaviour
    {

        public static System.Action OnCameraSwitch;
        public static CameraSwitcher main;

        public Transform ActiveCam { get; private set; }

        [Range(1, 200)]
        public float intervals = 20f;

        [SerializeField] GameObject mainCam;
        [SerializeField] GameObject followCam;


        public bool useMain { get; private set; }
        CameraFollowThing cameraFollowThing;

        int switchCounter = 0;
        int mainCamTurnQueue = 5;
        float hue = 0;
        float time = 0;

        public int farPlateSize = 5;
        public int nearPlateSize = 1;

        void Awake()
        {
            main = this;
            ActiveCam = gameObject.GetComponentInChildren<Camera>(false).transform;
            cameraFollowThing = followCam.GetComponent<CameraFollowThing>();
        }

        // Use this for initialization
        void Start()
        {

            ChangeCamera();
            InvokeRepeating("ChangeCamera", intervals, intervals);

        }

        // Update is called once per frame
        void Update()
        {
            if (ThingGod.god.things.Count < 1) return;
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                ChangeCamera();
            }

            //update background color
            Color bgColor = Color.HSVToRGB(hue, 0.3f, 0.73f);
            Camera.main.backgroundColor = bgColor;
            time += 0.1f * Time.deltaTime;
            hue = (Mathf.Sin(time) + 1) / 2f;
        }


        void ChangeCamera()
        {
            switchCounter++;



            if (switchCounter % mainCamTurnQueue == 0 || ThingGod.god.things.Count < 1)
            {
                useMain = true;
            }
            else
            {
                useMain = false;
            }


            //Debug.LogFormat("has {0} things, trying to switch to main? {1}", ThingGod.god.things.Count, useMain);

            //randomly choose one Thing
            if (!useMain)
            {
                GameObject oneRandomThing = ThingGod.god.things[(int)Random.Range(0, ThingGod.god.things.Count)].gameObject;
                //Debug.Log(oneRandomThing + ", thing count: " + ThingGod.god.things.Count);
                cameraFollowThing.followTarget = oneRandomThing.transform;                
            }


            foreach (var t in ThingGod.god.things)
            {
                t.plate.fontSize = useMain ? farPlateSize : nearPlateSize;
            }

            //set active
            mainCam.SetActive(useMain);
            followCam.SetActive(!useMain);
            ActiveCam = useMain ? mainCam.transform : followCam.transform;
            if (OnCameraSwitch != null) OnCameraSwitch();
        }



    }
}