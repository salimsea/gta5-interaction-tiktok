using GTA;
using GTA.Math;
using System;

namespace GTAVWebhook.Types
{
    public class RandomTeleport
    {

        private Random rand = new Random();
        private bool isTeleporting = false;
        private float teleportRadius = 100f;

        public RandomTeleport()
        {
        }

        public void Update()
        {
           
        }

        public void Execute()
        {
            StartRandomTeleport();
        }

        private void StartRandomTeleport()
        {
            if (!isTeleporting)
            {
                isTeleporting = true;

                float randomX = (float)(rand.NextDouble() * 2 - 1) * teleportRadius;
                float randomY = (float)(rand.NextDouble() * 2 - 1) * teleportRadius;

                Vector3 currentPos = Game.Player.Character.Position;

                Vector3 newPosition = new Vector3(
                    currentPos.X + randomX,
                    currentPos.Y + randomY,
                    currentPos.Z
                );

                Game.Player.Character.Position = newPosition;

                if (Game.Player.Character.IsInVehicle())
                {
                    Vehicle vehicle = Game.Player.Character.CurrentVehicle;
                    vehicle.Position = newPosition;
                    vehicle.PlaceOnGround();
                    vehicle.Repair();
                }

                isTeleporting = false;
            }
        }
    }
}
