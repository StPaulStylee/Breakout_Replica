using UnityEngine;

namespace Assets.Scripts {
  internal interface IBallVelocityBehavior {
    Vector2 GetCurrentVelocity();
    Vector2 GetStartingVelocity();
    void SetPreviousVelocity();
    void SetCurrentVelocity();
    void OnCollision();
    void OnTrigger();
  }
}
