using NUnit.Framework;
using DodgeGame.Common.Packets.Serverbound;
using DodgeGame.Common.Game;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools; 

public class NetworkPacketTests
{
    // TEST 1: Packet Configuration
    [Test]
    public void JoinGamePacket_HasCorrectPacketID()
    {
        ushort testNetId = 55;
        string testRoomId = "Room_Alpha";

        var packet = new JoinGameRequestPacket(testNetId, testRoomId);

        // We know from PacketIds.cs that JoinGameRequest should be ID 4
        Assert.AreEqual(4, packet.Id, "JoinGameRequestPacket has the wrong ID! Server will reject it.");
    }

    // Verifies the constructor correctly stores the data we intend to send
    [Test]
    public void JoinGamePacket_StoresCorrectData()
    {
        ushort expectedId = 99;
        string expectedRoom = "Lobby_1";

        var packet = new JoinGameRequestPacket(expectedId, expectedRoom);

        Assert.AreEqual(expectedId, packet.ClientNetworkId, "Packet failed to store ClientNetworkId.");
        Assert.AreEqual(expectedRoom, packet.RoomId, "Packet failed to store RoomId.");
    }

    // Client Identity Logic test
    [Test]
    public void ClientPlayerIdentity_CanStoreNetworkDetails()
    {
        var gameObject = new UnityEngine.GameObject();
        var identity = gameObject.AddComponent<ClientPlayerIdentity>();
        string uuid = "User_Unique_123";
        string name = "PlayerOne";

        identity.UniqueId = uuid;
        identity.PlayerName = name;

        // Assert
        Assert.AreEqual(uuid, identity.UniqueId);
        Assert.AreEqual(name, identity.PlayerName);
    }
}