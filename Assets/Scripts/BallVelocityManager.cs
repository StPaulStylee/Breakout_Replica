using Breakout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts {
  class BallVelocityManager {
    [SerializeField]
    private Dictionary<string, Vector2> velocity = new Dictionary<string, Vector2>() {
      { "Easy", new Vector2(1f, 1f) },
      { "EasyWide", new Vector2(1.5f, 1f) },
      { "Medium", new Vector2(1f, 2f) },
      { "MediumWide", new Vector2(2f, 2f) },
      { "Hard", new Vector2(2f, 2.5f) },
      { "VeryHard", new Vector2(2f, 3f) }
    };

    private Vector2 currentVelocity;
    private float previousVelocityOnX;
    public Vector2 GetStartingVelocity() {
      var startingVelocity = velocity["Easy"];
      startingVelocity.y = -startingVelocity.y;
      currentVelocity = startingVelocity;
      return startingVelocity;
    }
    public Vector2 GetCurrentVelocity(PaddleController paddleController, BallDirection direction, int paddleCollisionCount) {
      return SetCurrentVelocity(paddleController, direction, paddleCollisionCount);
    }

    public void OnCollision() {
      throw new NotImplementedException();
    }

    public void OnTrigger() {
      throw new NotImplementedException();
    }

    public Vector2 SetCurrentVelocity(PaddleController paddleController, BallDirection direction, int paddleCollisionCount) {

    }

    public void SetPreviousVelocity() {
      throw new NotImplementedException();
    }
  }
}
