using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools; 


public class BulletLogicTests 
{
    // A UnityTest behaves like a Coroutine, allowing us to wait for frames
    [UnityTest]
    public IEnumerator BulletDestroysAfterMaxBounces()
    {
        //setup
        GameObject bulletObject = new GameObject();
        
        bulletObject.AddComponent<Rigidbody2D>();
        bulletObject.AddComponent<CircleCollider2D>();
        
        BulletScript bulletScript = bulletObject.AddComponent<BulletScript>();
        bulletScript.maxBounces = 3; 

        bulletScript.OnCollisionEnter2D(null);
        bulletScript.OnCollisionEnter2D(null);
        bulletScript.OnCollisionEnter2D(null);

        // Wait one frame to let Unity process the Destroy() command
        yield return null;

        // assert result
        // If the logic works, the "bulletScript" component (or object) should be null (destroyed)

        // WILL ONLY WORK WHEN TESTING IN PLAYING ONLINE MODE 
        // BULLET DESTRUCTION ONLY WORKS WHEN NETWORK IS ACTIVE, AS BULLET DESTRUCTION PACKET NEEDS TO BE SENT
        bool isDestroyed = bulletScript == null; 
        
        Assert.IsTrue(isDestroyed, "Bullet failed to destroy after reaching max bounces.");
    }
}