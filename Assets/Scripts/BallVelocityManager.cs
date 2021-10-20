using Breakout.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  class BallVelocityManager : MonoBehaviour {
    public delegate void OnMaxVelocityHandler();
    public static OnMaxVelocityHandler OnMaxVelocity;
    private Dictionary<string, Vector2> velocity = new Dictionary<string, Vector2>() {
      { "Easy", new Vector2(1f, 1f) },
      { "EasyWide", new Vector2(1.5f, 1f) },
      { "Medium", new Vector2(1f, 2f) },
      { "MediumWide", new Vector2(2f, 2f) },
      { "Hard", new Vector2(2.25f, 2f) },
      { "VeryHard", new Vector2(2.25f, 3f) },
      { "MaxVelocity", new Vector2(2.25f, 3.25f) }
    };
    [SerializeField]
    private int paddleCollisionCount = 0;
    private PaddleController paddleController;
    private BallDirection currentBallDirection;
    private Vector2 currentVelocity;
    private bool isBreakout;
    private bool isMaxVelocity;

    private void Start() {
      paddleController = GameObject.Find("Paddle").GetComponent<PaddleController>();
      if (paddleController == null) {
        Debug.LogError("No PaddleController in Scene!");
      }
      OnMaxVelocity += SetMaxVelocity;
    }

    public Vector2 GetStartingVelocity(float startingPositionOnX) {
      var startingVelocity = velocity["Easy"];
      startingVelocity.y = -startingVelocity.y;
      if (startingPositionOnX >= 0) {
        startingVelocity.x = -startingVelocity.x;
      }
      currentVelocity = startingVelocity;
      SetBallDirection(currentVelocity);
      return startingVelocity;
    }

    public void SetDataFromPaddleCollision(Collision2D collision) {
      ++paddleCollisionCount;
      var paddleBounds = collision.collider.bounds;
      ContactPoint2D contactPoint = collision.GetContact(0);
      var distanceFromCenter = contactPoint.point.x - paddleBounds.center.x;
      paddleController.SetSegmentHit(distanceFromCenter);
    }

    public Vector2 GetVelocity(string colliderTag) {
      SetVelocity(colliderTag);
      return currentVelocity;
    }

    public void RestoreToTurnStartState() {
      paddleCollisionCount = 0;
      isMaxVelocity = false;
    }

    #region Private Methods
    private void SetVelocity(string colliderTag) {
      if (colliderTag == ColliderTag.Paddle) {
        SetVelocityFromPaddleCollision();
      }
      SetVelocityFromOtherCollision(colliderTag);
    }

    // I believe I have the logic done for max velocity here - this is not true. I still
    // need to set up the scenario where it hits a red/orange brick after a breakout was initiated
    // All that is left is to set up the brick to call the event when the collision is red/orange
    private void SetMaxVelocity() {
      isMaxVelocity = true;
      var newVelocity = velocity["MaxVelocity"];
      if (currentBallDirection == BallDirection.Left) {
        newVelocity.x = -newVelocity.x;
      }
      if (currentVelocity.y < 0) {
        newVelocity.y = -newVelocity.y;
      }
      currentVelocity = new Vector2(newVelocity.x, newVelocity.y);
      BallController.OnForceVelocityChange(currentVelocity);
    }

    private void SetBallDirection(Vector2 currentVelocity) {
      if (currentVelocity.x < 0) {
        currentBallDirection = BallDirection.Left;
      } else {
        currentBallDirection = BallDirection.Right;
      }
    }

    private void SetVelocityFromPaddleCollision() {
      isBreakout = false;
      var newVelocity = GetVelocity();
      newVelocity = DetermineBallDirectionOnX(newVelocity);
      currentVelocity = newVelocity;
    }

    // if breakout is false and y velocity is negative, return
    private void SetVelocityFromOtherCollision(string colliderTag) {
      if (colliderTag == ColliderTag.Brick) {
        // These checks are necessary to eliminate bug when two bricks are hit simultaneously
        if (!isBreakout && currentVelocity.y < 0) {
          return;
        }
        if (isBreakout && currentVelocity.y > 0) {
          return;
        }
        currentVelocity = new Vector2(currentVelocity.x, -currentVelocity.y);
        SetBallDirection(currentVelocity);
      }
      if (colliderTag == ColliderTag.UpperLimit) {
        isBreakout = true;
        currentVelocity = new Vector2(currentVelocity.x, -currentVelocity.y);
        SetBallDirection(currentVelocity);
      }
      if (colliderTag == ColliderTag.RightLimit || colliderTag == ColliderTag.LeftLimit) {
        currentVelocity = new Vector2(-currentVelocity.x, currentVelocity.y);
        SetBallDirection(currentVelocity);
      }
    }

    private Vector2 DetermineBallDirectionOnX(Vector2 velocity) {
      var currentHit = paddleController.CurrentSegmentHit;
      if (currentBallDirection == BallDirection.Left) {
        if (currentHit == PaddleSegmentHit.Left || currentHit == PaddleSegmentHit.Center) {
          return new Vector2(-velocity.x, velocity.y);
        }
        SetBallDirection(velocity);
        return velocity;
      }
      // if BallDirection.Right
      if (currentHit == PaddleSegmentHit.Right || currentHit == PaddleSegmentHit.Center) {
        return velocity;
      }
      SetBallDirection(new Vector2(-velocity.x, velocity.y));
      return new Vector2(-velocity.x, velocity.y);
    }

    private Vector2 GetVelocity() {
      if (isMaxVelocity) {
        return velocity["MaxVelocity"];
      }
      if (paddleCollisionCount < 4) {
        if (paddleController.CurrentSegmentHit == PaddleSegmentHit.Center) {
          return velocity["Easy"];
        }
        return velocity["EasyWide"];
      }
      if (paddleCollisionCount == 4) {
        return velocity["Medium"];
      }
      if (paddleCollisionCount > 4 && paddleCollisionCount <= 7) {
        if (paddleController.CurrentSegmentHit == PaddleSegmentHit.Center) {
          return velocity["Medium"];
        }
        return velocity["MediumWide"];
      }
      if (paddleCollisionCount > 7 && paddleCollisionCount <= 11) {
        return velocity["Hard"];
      }
      return velocity["VeryHard"];
    }
    #endregion
  }
}
