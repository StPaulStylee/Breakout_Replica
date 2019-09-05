using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout
{
    public class EventsController : MonoBehaviour
    {
        public delegate void OnBallEventHandler();
        public static OnBallEventHandler OnBrickCollision;
        public static OnBallEventHandler OnPaddleCollision;

        [SerializeField]
        private BallController ball;
        [SerializeField]
        private GameObject[] bricks;
        // Start is called before the first frame update
        void Start()
        {
            bricks = GameObject.FindGameObjectsWithTag("Brick");
            OnBrickCollision += DisableBrickIsTrigger;
            OnPaddleCollision += EnableBrickIsTrigger;
        }

        private void DisableBrickIsTrigger()
        {
            Debug.Log("Disable those bricks!!");
            foreach (GameObject brick in bricks)
            {
                if (brick != null)
                {
                    Collider2D collider = brick.GetComponent<Collider2D>();
                    collider.enabled = false;
                }
            }
        }

        private void EnableBrickIsTrigger()
        {
            Debug.Log("Enable those Bricks!!");
            foreach (GameObject brick in bricks)
            {
                if (brick != null)
                {
                    Collider2D collider = brick.GetComponent<Collider2D>();
                    collider.enabled = true;
                }
            }
        }

    }
}