using NUnit.Framework;
using UnityEngine;

public class IdentityTests
{
    // Verifies that the identity script correctly stores string data
    [Test]
    public void IdentityComponent_StoresCorrectInfo()
    {
        GameObject playerObj = new GameObject();
        ClientPlayerIdentity identity = playerObj.AddComponent<ClientPlayerIdentity>();
        string testName = "TestUser";
        string testId = "UUID-1234";

        identity.PlayerName = testName;
        identity.UniqueId = testId;

        // Assert
        Assert.AreEqual(testName, identity.PlayerName);
        Assert.AreEqual(testId, identity.UniqueId);
    }
}