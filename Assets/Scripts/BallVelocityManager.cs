using Breakout.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Breakout {
  class BallVelocityManager : MonoBehaviour {
    private Dictionary<string, Vector2> velocity = new Dictionary<string, Vector2>() {
      { "Easy", new Vector2(1f, 1f) },
      { "EasyWide", new Vector2(1.5f, 1f) },
      { "Medium", new Vector2(1f, 2f) },
      { "MediumWide", new Vector2(2f, 2f) },
      { "Hard", new Vector2(2.25f, 2f) },
      { "VeryHard", new Vector2(2.25f, 3f) }
    };
    [SerializeField]
    private int paddleCollisionCount = 0;
    private PaddleController paddleController;
    private BallDirection currentBallDirection;
    private Vector2 currentVelocity;
    private bool isBreakout;

    private void Start() {
      paddleController = GameObject.Find("Paddle").GetComponent<PaddleController>();
      if (paddleController == null) {
        Debug.LogError("No PaddleController in Scene!");
      }
    }

    public Vector2 GetStartingVelocity() {
      var startingVelocity = velocity["Easy"];
      startingVelocity.y = -startingVelocity.y;
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

    public void ResetPaddleCollisionCount() {
      paddleCollisionCount = 0;
    }

    #region Private Methods
    private void SetVelocity(string colliderTag) {
      if (colliderTag == ColliderTag.Paddle) {
        SetVelocityFromPaddleCollision();
      }
      SetVelocityFromOtherCollision(colliderTag);
    }

    // When the ball hits two bricks at the same time the logic in the SetVelocityFromOtherCollision
    // method gets called twice and flips the velocity on X which causes a bug. This logic with the
    // up/down shit below was an attempt at fixing that but I realized that when there is a "breakout"
    // you can't depend on the ball direction alone. I am thinking I need some sort of "isBreakout" flag
    // that can be checked or something like that
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
