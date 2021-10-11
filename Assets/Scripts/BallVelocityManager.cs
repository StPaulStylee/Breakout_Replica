using Breakout;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Breakout {
  class BallVelocityManager : MonoBehaviour {
    [SerializeField]
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
      return startingVelocity;
    }
    public Vector2 GetCurrentVelocity(PaddleController paddleController, BallDirection direction, int paddleCollisionCount) {
      //return SetCurrentVelocity(paddleController, direction, paddleCollisionCount);
      return Vector2.down;
    }

    public void SetVelocityData(Collision2D collision) {
      ++paddleCollisionCount;
      var paddleBounds = collision.collider.bounds;
      ContactPoint2D contactPoint = collision.GetContact(0);
      var distanceFromCenter = contactPoint.point.x - paddleBounds.center.x;
      paddleController.SetSegmentHit(distanceFromCenter);
    }

    public void SetVelocity(string colliderTag) {

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
  }
}
