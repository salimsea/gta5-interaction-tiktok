using GTA;
using System;

namespace GTAVWebhook.Types
{
    public class RemoveVehicles
    {
        private bool _isRemoving;
        private int _removeTimer;

        public RemoveVehicles()
        {
            _removeTimer = 30;
        }

        public void Update()
        {
            if (_isRemoving)
            {
                if (_removeTimer > 0)
                {
                    _removeTimer--;
                }
                else
                {
                    StartRemoving();
                    _removeTimer = 30;
                    _isRemoving = false;
                }
            }
        }

        public void Execute()
        {
            if (!_isRemoving)
            {
                _isRemoving = true;
                _removeTimer = 30;
            }
        }

        public void StartRemoving()
        {
            try
            {
                var playerVehicle = Game.Player.Character.CurrentVehicle;
                if (playerVehicle != null && playerVehicle.Exists())
                {
                    playerVehicle.Delete();
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Removing Vehicles {ex.Message}");
            }
        }
    }
}
