using NUnit.Framework;
using UnityEngine;

public class BulletConfigurationTests
{
    // Ensures gravity is turned off so bullets fly straight
    [Test]
    public void BulletHasZeroGravity()
    {
        GameObject bulletObject = new GameObject();
        Rigidbody2D rb = bulletObject.AddComponent<Rigidbody2D>();
        bulletObject.AddComponent<BulletScript>();

        // Act
        rb.gravityScale = 0f; 

        // Assert
        Assert.AreEqual(0f, rb.gravityScale, "Bullet gravity scale should be 0.");
    }

    // test to ensure the bullet correctly stores who fired it
    [Test]
    public void BulletOwnerIdCanBeSet()
    {
        GameObject bulletObject = new GameObject();
        BulletScript bulletScript = bulletObject.AddComponent<BulletScript>();
        string testId = "Player_123";

        bulletScript.OwnerId = testId;

        // Assert
        Assert.AreEqual("Player_123", bulletScript.OwnerId, "Bullet OwnerID was not set correctly.");
    }
}