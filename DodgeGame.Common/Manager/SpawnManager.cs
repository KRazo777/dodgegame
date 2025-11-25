using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace DodgeGame.Common.Manager
{
    public class SpawnManager
    {
        private List<Vector2> spawnPoints;

        public SpawnManager()
        {
            spawnPoints = new List<Vector2>
            {
                new Vector2(-11.5f, 9.5f),
                new Vector2(3.5f, 9.5f),
                new Vector2(3.5f, -7.5f),
                new Vector2(-11.5f, -7.5f)
            };
        }

        // Return spawn point and remove it from the list of available spawns
        public Vector2? GetUniqueSpawnPoint()
        {
            if (spawnPoints.Count == 0)
            {
                Console.WriteLine("No spawn points left!");
                return null;
            }

            int index = new Random().Next(spawnPoints.Count);
            Vector2 chosen = spawnPoints[index];
            spawnPoints.RemoveAt(index);
            return chosen;
        }
    
        // Get without removing
        public Vector2? GetRandomSpawnPoint()
        {
            int index = new Random().Next(spawnPoints.Count);
            return spawnPoints[index];
        }
    }
}