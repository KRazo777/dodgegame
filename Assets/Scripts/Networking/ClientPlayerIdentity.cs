using UnityEngine;

// This script will be attached to every player instance (including the local player) 
// in the game scene to hold their unique network ID.
public class ClientPlayerIdentity : MonoBehaviour
{
    
    public string UniqueId { get; set; } = string.Empty;
    
    public string PlayerName { get; set; } = string.Empty;
}