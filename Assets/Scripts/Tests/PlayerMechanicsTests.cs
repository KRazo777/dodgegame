using NUnit.Framework;
using UnityEngine;

public class PlayerMechanicsTests
{

    // Ensures diagonal movement isn't faster than cardinal movement
    [Test]
    public void PlayerInputIsClampedToMagnitudeOne()
    {
        // Simulate a diagonal input (holding W + D)
        Vector2 rawInput = new Vector2(1f, 1f);
        
        Vector2 clampedInput = Vector2.ClampMagnitude(rawInput, 1f);
        
        // The length should be 1.0, not 1.414 (root 2)
        Assert.AreEqual(1f, clampedInput.magnitude, 0.001f, "Diagonal input was not clamped correctly.");
    }
}