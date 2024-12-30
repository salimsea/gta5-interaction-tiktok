using GTA;
using GTA.Math;
using System;

namespace GTAVWebhook.Types
{
    public class KickFlip
    {
        private Vehicle currentVehicle = null;
        private bool isFlipping = false;
        private float flipAngle = 0f;
        private float flyHeight = 0f;
        private const float FLIP_SPEED = 2f;
        private const float FLY_SPEED = 0.5f;
        private const float MAX_HEIGHT = 10f;

        public KickFlip()
        {
        }

        public void Update()
        {
            if (isFlipping && currentVehicle != null && currentVehicle.Exists())
            {
                DoFlipWithFly();
            }
        }

        public void Execute()
        {
            if (Game.Player.Character.IsInVehicle() && !isFlipping)
            {
                isFlipping = true;
                flipAngle = 5f;
                flyHeight = 5f;
                currentVehicle = Game.Player.Character.CurrentVehicle;
            }
        }

        private void DoFlipWithFly()
        {
            Vector3 position = currentVehicle.Position;
            Vector3 rotation = currentVehicle.Rotation;
            float currentSpeed = currentVehicle.Speed; 

            if (flyHeight < MAX_HEIGHT)
            {
                flyHeight += FLY_SPEED;
                position.Z += FLY_SPEED;
                currentVehicle.Position = position;
                currentVehicle.IsEngineRunning = true;
                currentVehicle.ForwardSpeed = currentSpeed;
            }
            else
            {
                flipAngle += FLIP_SPEED;
                rotation.Y = flipAngle;
                currentVehicle.Rotation = rotation;
                currentVehicle.IsEngineRunning = true;
                currentVehicle.ForwardSpeed = currentSpeed;

                if (flipAngle >= 360f)
                {
                    isFlipping = false;
                    flipAngle = 0f;
                    currentVehicle.Rotation = new Vector3(0f, 0f, rotation.Z);
                }
            }
        }

    }
}
