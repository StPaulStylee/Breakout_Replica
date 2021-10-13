using Breakout;
using Breakout.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Breakout {
  class BallVelocityManager : MonoBehaviour {
    private Dictionary<string, Vector2> velocity = new Dictionary<string, Vector2>() {
      { "Easy", new Vector2(1f, 1f) },
      { "EasyWide", new Vector2(1.5f, 1f) },
      { "Medium", new Vector2(1f, 2f) },
      { "MediumWide", new Vector2(2f, 2f) },
      { "Hard", new Vector2(2f, 2.5f) },
      { "VeryHard", new Vector2(2f, 3f) }
    };
    [SerializeField]
    private int paddleCollisionCount = 0;
    private PaddleController paddleController;
    private BallDirection currentBallDirection;
    private Vector2 currentVelocity;
    private float previousVelocityOnX;

    private void Start() {
      paddleController = GameObject.Find("Paddle").GetComponent<PaddleController>();
      if (paddleController == null) {
        Debug.LogError("No paddle found!");
      }
    }

    public Vector2 GetStartingVelocity() {
      var startingVelocity = velocity["Easy"];
      startingVelocity.y = -startingVelocity.y;
      currentVelocity = startingVelocity;
      SetBallDirection(currentVelocity);
      return startingVelocity;
    }

    public Vector2 GetCurrentVelocity(PaddleController paddleController, BallDirection direction, int paddleCollisionCount) {
      //return SetCurrentVelocity(paddleController, direction, paddleCollisionCount);
      return Vector2.down;
    }

    public void SetDataFromPaddleCollision(Collision2D collision) {
      ++paddleCollisionCount;
      var paddleBounds = collision.collider.bounds;
      ContactPoint2D contactPoint = collision.GetContact(0);
      var distanceFromCenter = contactPoint.point.x - paddleBounds.center.x;
      paddleController.SetSegmentHit(distanceFromCenter);
    }

    public void SetVelocity(string colliderTag) {
      if (colliderTag == ColliderTag.Paddle) {
        SetVelocityFromPaddleCollision();
      }
      if (colliderTag == ColliderTag.Brick) {

      }
      if (colliderTag == ColliderTag.UpperLimit) {

      }
      if (colliderTag == ColliderTag.RightLimit ||colliderTag == ColliderTag.LeftLimit) {

      }
    }

    public void OnCollision() {
      throw new NotImplementedException();
    }

    public void OnTrigger() {
      throw new NotImplementedException();
    }

    //public Vector2 SetCurrentVelocity(PaddleController paddleController, BallDirection direction, int paddleCollisionCount) {

    //}

    public void SetPreviousVelocity() {
      throw new NotImplementedException();
    }
    private void SetBallDirection(Vector2 currentVelocity) {
      if (currentVelocity.x < 0) {
        currentBallDirection = BallDirection.Left;
        return;
      }
      currentBallDirection = BallDirection.Right;
    }

    private void SetVelocityFromPaddleCollision() {
      var previousHit = paddleController.PreviousSegmentHit;
      if (currentBallDirection == BallDirection.Left) {
        currentVelocity = GetVelocity();
        //if (previousHit == PaddleSegmentHit.Center || previousHit == PaddleSegmentHit.Left) {
        //  // get appropriate velocity and continue in current x direction
        //}
        //if (previousHit == PaddleSegmentHit.Right) {
        //  // get appropriate velocity and  go back in the direction you came
        //}
      }
      if (currentBallDirection == BallDirection.Right) {
        if (previousHit == PaddleSegmentHit.Center || previousHit == PaddleSegmentHit.Right) {
          // get appropriate velocity and continue in current x direction
        }
        if (previousHit == PaddleSegmentHit.Left) {
          // get appropriate velocity and go back in the direction you came
        }
      }
    }

    private Vector2 GetVelocity() {
      if (paddleCollisionCount < 4) {
         if (paddleController.CurrentSegmentHit == PaddleSegmentHit.Center) {
          return velocity["Easy"];
        }
        return velocity["EasyWide"];
      }
    }


  }
}
