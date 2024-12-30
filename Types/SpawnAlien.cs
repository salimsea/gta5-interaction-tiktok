using GTA;
using GTA.Math;
using System;

namespace GTAVWebhook.Types
{
    public class SpawnAlien
    {

        private Ped[] _aliens = new Ped[5];
        private Model _alienModel = new Model(PedHash.MovAlien01);
        private bool _isAlienSpawned = false;

        public SpawnAlien()
        {
        }

        public void Update()
        {
            if (_isAlienSpawned)
            {
                foreach (Ped alien in _aliens)
                {
                    if (alien != null && alien.Exists())
                    {
                        if (alien.Position.DistanceTo(Game.Player.Character.Position) > 100f)
                        {
                            Vector3 newPos = Game.Player.Character.Position +
                                Game.Player.Character.ForwardVector * 10f;
                            alien.Position = newPos;
                        }

                        if (!alien.IsInCombat)
                        {
                            alien.Task.FightAgainst(Game.Player.Character);
                        }
                    }
                }
            }
        }

        public void Execute()
        {
            if (!_isAlienSpawned)
            {
                SpawnAliens();
            }
            else
            {
                DespawnAliens();
            }
        }

        private void SpawnAliens()
        {
            if (!_alienModel.IsLoaded)
            {
                _alienModel.Request();
            }

            if (_alienModel.IsLoaded)
            {
                Vector3 playerPos = Game.Player.Character.Position;

                for (int i = 0; i < _aliens.Length; i++)
                {
                    float angle = i * (360f / _aliens.Length);
                    float x = playerPos.X + (float)Math.Cos(angle * Math.PI / 180) * 10;
                    float y = playerPos.Y + (float)Math.Sin(angle * Math.PI / 180) * 10;

                    _aliens[i] = World.CreatePed(_alienModel, new Vector3(x, y, playerPos.Z));

                    if (_aliens[i] != null)
                    {
                        _aliens[i].MaxHealth = 500;
                        _aliens[i].Health = 500;
                        _aliens[i].Armor = 100;
                        _aliens[i].CanRagdoll = true;
                        _aliens[i].BlockPermanentEvents = true;

                        _aliens[i].Task.FightAgainst(Game.Player.Character);
                    }
                }

                _isAlienSpawned = true;
                GTA.UI.Screen.ShowSubtitle("~r~Aliens have arrived!", 3000);
            }
        }

        private void DespawnAliens()
        {
            foreach (Ped alien in _aliens)
            {
                if (alien != null && alien.Exists())
                {
                    alien.Delete();
                }
            }
            _isAlienSpawned = false;
            GTA.UI.Screen.ShowSubtitle("~g~Aliens eliminated!", 3000);
        }
    }
}
