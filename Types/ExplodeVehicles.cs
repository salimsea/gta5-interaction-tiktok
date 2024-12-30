using GTA;
using GTA.Native;
using System;
using System.Collections.Generic;

namespace GTAVWebhook.Types
{
    public class ExplodeVehicles
    {
        private readonly List<Vehicle> _spawnedVehicles;
        private readonly Random _random;
        private bool _isExploding;
        private int _explosionTimer;

        public ExplodeVehicles()
        {
            _spawnedVehicles = new List<Vehicle>();
            _random = new Random();
            _explosionTimer = 0;
            _isExploding = false;
        }

        public void Update()
        {

        }

        public void Execute(bool isSelf)
        {
            StartExplosion(isSelf);
        }

        private void CreateExplosionEffect(Vehicle vehicle)
        {
            var position = vehicle.Position;
            Function.Call(Hash.ADD_EXPLOSION,
                position.X,
                position.Y,
                position.Z,
                7, // Explosion type
                1.0f, // Damage scale
                true, // Is audible
                false, // Is invisible
                0.0f // Camera shake
            );
        }

        public void StartExplosion(bool isSelf)
        {
            if (isSelf)
            {
                var playerVehicle = Game.Player.Character.CurrentVehicle;
                if (playerVehicle != null && playerVehicle.Exists())
                {
                    CreateExplosionEffect(playerVehicle);
                    playerVehicle.Explode();
                }
            }
            else
            {
                Vehicle[] nearbyVehicles = World.GetNearbyVehicles(Game.Player.Character, 20);
                foreach (Vehicle vehicle in nearbyVehicles)
                {
                    vehicle.Explode();
                }
            }
        }
    }
}
