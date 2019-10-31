using UnityEngine;

internal interface IBallVelocityBehavior
{
    Vector2 GetCurrentVelocity();
    void SetCurrentVelocity();
}
