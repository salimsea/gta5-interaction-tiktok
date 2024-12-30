using System;
using GTA.Math;
using GTA.Native;
using GTA;

namespace GTAVWebhook.Types
{
    public class Earthquake
    {
        private bool _isShaking = false;
        private int _shakeTimer = 0;
        private float _intensity = 0.0f;
        private const float MAX_INTENSITY = 2.0f;
        private const int DURATION = 300; // 10 detik pada 30 fps
        private Random _random = new Random();

        public void Update()
        {
            if (_isShaking)
            {
                if (_shakeTimer > 0)
                {
                    ApplyShake();
                    _shakeTimer--;
                }
                else
                {
                    StopEarthquake();
                }
            }
        }

        public void Execute()
        {
            if (!_isShaking)
            {
                StartEarthquake();
            }
        }

        private void StartEarthquake()
        {
            _isShaking = true;
            _shakeTimer = DURATION;
            _intensity = 0.1f;

            // Efek visual dan suara
            Function.Call(Hash.SHAKE_GAMEPLAY_CAM, "SMALL_EXPLOSION_SHAKE", 1.0f);
            GTA.UI.Screen.ShowSubtitle("~r~Earthquake!", 3000);

            // Shake semua kendaraan di sekitar
            Vehicle[] nearbyVehicles = World.GetNearbyVehicles(Game.Player.Character.Position, 100f);
            foreach (Vehicle vehicle in nearbyVehicles)
            {
                if (vehicle != null && vehicle.Exists())
                {
                    vehicle.ApplyForce(new Vector3(0, 0, 1f));
                }
            }
        }

        private void ApplyShake()
        {
            if (_intensity < MAX_INTENSITY)
            {
                _intensity += 0.01f;
            }

            // Shake camera
            Function.Call(Hash.SHAKE_GAMEPLAY_CAM, "SMALL_EXPLOSION_SHAKE", _intensity);

            // Shake player dan kendaraan sekitar
            Vector3 shakeOffset = new Vector3(
                (float)(_random.NextDouble() - 0.5) * _intensity,
                (float)(_random.NextDouble() - 0.5) * _intensity,
                (float)(_random.NextDouble() - 0.5) * _intensity
            );

            if (Game.Player.Character.IsInVehicle())
            {
                Vehicle vehicle = Game.Player.Character.CurrentVehicle;
                vehicle.ApplyForce(shakeOffset * 5f);
            }
            else
            {
                Game.Player.Character.ApplyForce(shakeOffset * 2f);
            }

            // Shake objek sekitar
            Entity[] nearbyEntities = World.GetNearbyEntities(Game.Player.Character.Position, 50f);
            foreach (Entity entity in nearbyEntities)
            {
                if (entity != null && entity.Exists() && !entity.Equals(Game.Player.Character))
                {
                    entity.ApplyForce(shakeOffset);
                }
            }
        }

        private void StopEarthquake()
        {
            _isShaking = false;
            _intensity = 0f;
            Function.Call(Hash.STOP_GAMEPLAY_CAM_SHAKING, true);
        }
    }

}
