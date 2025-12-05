using NUnit.Framework;
using DodgeGame.Common.Game; // Using the actual shared namespace

public class GameRoomLogicTests
{
    // TEST 1: Constructor Initialization
    // Verifies that the GameRoom constructor correctly assigns the critical identities.
    [Test]
    public void GameRoom_Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        string expectedHostId = "Host_User_1";
        string expectedRoomId = "Room_Alpha";
        string expectedName = "Player 1";

        // Act
        // We instantiate the ACTUAL GameRoom class
        GameRoom room = new GameRoom(expectedHostId, expectedRoomId, expectedName);

        // Assert
        Assert.AreEqual(expectedHostId, room.HostUniqueId, "HostUniqueId was not set correctly.");
        Assert.AreEqual(expectedRoomId, room.RoomId, "RoomId was not set correctly.");
        
        // Also verify the collections are created (not null)
        Assert.IsNotNull(room.Players, "Players dictionary should be initialized.");
        Assert.IsNotNull(room.Entities, "Entities dictionary should be initialized.");
    }

    // Verifies that the GameRoom can store specific Player objects
    [Test]
    public void GameRoom_CanStoreAndRetrievePlayers()
    {
        GameRoom room = new GameRoom("Host", "RoomID", "Room");
        string playerId = "Player_1";
        string playerName = "CoolPlayer";
        
        // Create a real Player object
        Player newPlayer = new Player(playerId, playerName, EntityType.Player);

        room.Players.Add(playerId, newPlayer);

        Assert.IsTrue(room.Players.ContainsKey(playerId), "Player was not added to dictionary.");
        Assert.AreEqual(playerName, room.Players[playerId].Name, "Retrieved player has wrong name.");
    }
}