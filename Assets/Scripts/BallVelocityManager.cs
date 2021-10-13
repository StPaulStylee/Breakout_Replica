using Breakout;
using Breakout.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Breakout {
  [RequireComponent(typeof(PaddleController))]
  class BallVelocityManager : MonoBehaviour {
    private Dictionary<string, Vector2> velocity = new Dictionary<string, Vector2>() {
      { "Easy", new Vector2(1f, 1f) },
      { "EasyWide", new Vector2(1.5f, 1f) },
      { "Medium", new Vector2(1f, 2f) },
      { "MediumWide", new Vector2(2f, 2f) },
      { "Hard", new Vector2(2.25f, 2.5f) },
      { "VeryHard", new Vector2(2.25f, 3f) }
    };
    [SerializeField]
    private int paddleCollisionCount = 0;
    private PaddleController paddleController;
    private BallDirection currentBallDirection;
    private Vector2 currentVelocity;

    private void Start() {
      paddleController = GameObject.Find("Paddle").GetComponent<PaddleController>();
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

    private void SetVelocity(string colliderTag) {
      if (colliderTag == ColliderTag.Paddle) {
        SetVelocityFromPaddleCollision();
      }
      SetVelocityFromOtherCollision(colliderTag);
    }

    private void SetBallDirection(Vector2 currentVelocity) {
      if (currentVelocity.x < 0) {
        currentBallDirection = BallDirection.Left;
        return;
      }
      currentBallDirection = BallDirection.Right;
    }

    private void SetVelocityFromPaddleCollision() {
      var newVelocity = GetVelocity();
      newVelocity = DetermineBallDirectionOnX(newVelocity);
      currentVelocity = newVelocity;
    }

    private void SetVelocityFromOtherCollision(string colliderTag) {
      if (colliderTag == ColliderTag.Brick || colliderTag == ColliderTag.UpperLimit) {
        currentVelocity = new Vector2(currentVelocity.x, -currentVelocity.y);
      }
      if (colliderTag == ColliderTag.RightLimit || colliderTag == ColliderTag.LeftLimit) {
        currentVelocity = new Vector2(-currentVelocity.x, currentVelocity.y);
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
      Debug.Log(paddleCollisionCount);
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
  }
}
