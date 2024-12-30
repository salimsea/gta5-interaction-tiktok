using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace GTAVWebhook.Types
{
    public class ChangeCharacter
    {
        private bool _isChanging;
        private int _changeTimer;
        private PedHash[] _characterModels = {
            PedHash.Franklin,
            PedHash.Michael,
            PedHash.Trevor,
            PedHash.Orleans,
            PedHash.Zombie01,
            PedHash.MovAlien01,
            PedHash.Jesus01,
            PedHash.Clown01SMY,
            PedHash.Snowcop01SMM
        };
        private Random _random;
        private Model _currentModel;

        public ChangeCharacter()
        {
            _changeTimer = 10;
            _random = new Random();
        }

        public void Update()
        {
            if (_isChanging)
            {
                if (_changeTimer > 0)
                {
                    _changeTimer--;
                }
                else
                {
                    StartChanging();
                    _changeTimer = 10;
                    _isChanging = false;
                }
            }
        }

        public void Execute()
        {
            if (!_isChanging)
            {
                _isChanging = true;
                _changeTimer = 10;
            }
        }

        private void StartChanging()
        {
            try
            {
                PedHash randomModel = _characterModels[_random.Next(_characterModels.Length)];
                _currentModel = new Model(randomModel);

                if (!_currentModel.IsLoaded)
                {
                    _currentModel.Request();
                }

                if (_currentModel.IsLoaded)
                {
                    Vector3 position = Game.Player.Character.Position;
                    float heading = Game.Player.Character.Heading;
                    int health = Game.Player.Character.Health;
                    int armor = Game.Player.Character.Armor;

                    Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, _currentModel.Hash);

                    Game.Player.Character.Position = position;
                    Game.Player.Character.Heading = heading;
                    Game.Player.Character.Health = health;
                    Game.Player.Character.Armor = armor;
                    Game.Player.Character.MaxHealth = 500;
                    Game.Player.Character.CanRagdoll = true;
                    Game.Player.Character.CanSwitchWeapons = true;

                    _currentModel.MarkAsNoLongerNeeded();
                }
            }
            catch (Exception ex)
            {
                Logger.Log("Changing Character " + ex.Message);
            }
        }

        public void Dispose()
        {
            if (_currentModel != null && _currentModel.IsLoaded)
            {
                _currentModel.MarkAsNoLongerNeeded();
            }
        }
    }
}
