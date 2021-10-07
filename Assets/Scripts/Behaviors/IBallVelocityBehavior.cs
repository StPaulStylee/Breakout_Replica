using UnityEngine;

namespace Assets.Scripts {
  internal interface IBallVelocityBehavior {
    Vector2 GetCurrentVelocity();
    void SetCurrentVelocity();
    void OnCollision();
    void OnTrigger();
  }
}
