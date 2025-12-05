using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BulletLifeCycleTests
{
    //  Verifies the object is destroyed after hitting the bounce limit
    [UnityTest]
    public IEnumerator BulletDestroysAfterMaxBounces()
    {
        GameObject bulletObject = new GameObject();
        bulletObject.AddComponent<Rigidbody2D>();
        bulletObject.AddComponent<CircleCollider2D>();
        
        BulletScript bulletScript = bulletObject.AddComponent<BulletScript>();
        bulletScript.maxBounces = 3;

        // Simulate collisions
        bulletScript.OnCollisionEnter2D(null); 
        bulletScript.OnCollisionEnter2D(null); 
        bulletScript.OnCollisionEnter2D(null); 

        // Wait for Unity to process the Destroy() call
        yield return null;

        // WILL ONLY PASS WHEN TESTING IN PLAYING ONLINE MODE 
        // DESTORY ONLY WORKS WHEN NETWORK IS ACTIVE, AS BULLET DESTRUCTON PACKET NEEDS TO BE SENT
        Assert.IsTrue(bulletScript == null, "Bullet failed to destroy after reaching max bounces.");
    }
}