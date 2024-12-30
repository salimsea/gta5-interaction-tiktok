using GTA;
using GTA.Math;
using GTA.Native;
using System;
using System.Windows.Forms;

namespace GTAV_MOD
{
    public class Main : Script
    {
        private Entity[] entities;
        private Random rand = new Random();
        private Vector3 playerCoord;
        private Vector3 CTCoordinate;
        private VehicleHash[] carModels = {
            VehicleHash.Adder,
            VehicleHash.T20,
            VehicleHash.Zentorno,
            VehicleHash.EntityXF,
            VehicleHash.Osiris
        };
        private Vehicle currentVehicle = null;
        private int switchCarTimer = 30;
        private bool isChangingCar = false;
        private Vector3 lastPosition;

        private bool isFlipping = false;
        private float flipAngle = 0f;
        private float flyHeight = 0f;
        private const float FLIP_SPEED = 2f;
        private const float FLY_SPEED = 0.5f;
        private const float MAX_HEIGHT = 10f;

        private readonly Random _random = new Random();
        private readonly VehicleHash[] _carModels = {
            // Super Cars
            VehicleHash.Vigilante,    // Batmobile inspired
            VehicleHash.Krieger,      // Mercedes-AMG One inspired
            VehicleHash.Vagner,       // Aston Martin Valkyrie inspired
            VehicleHash.T20,          // McLaren P1 inspired
            VehicleHash.Zentorno,     // Lamborghini Sesto Elemento inspired
            VehicleHash.Adder,        // Bugatti Veyron inspired
            VehicleHash.EntityXF,     // Koenigsegg inspired
            VehicleHash.Osiris,       // Pagani Huayra inspired
            VehicleHash.Tezeract,     // Lamborghini Terzo Millennio inspired
            VehicleHash.Emerus,       // McLaren Senna inspired
            VehicleHash.Autarch,      // Scuderia Cameron Glickenhaus inspired
            VehicleHash.Tigon,        // De Tomaso P72 inspired
            VehicleHash.Infernus,     // Classic Lamborghini inspired
            VehicleHash.Tempesta,     // Lamborghini Huracan inspired
            VehicleHash.Thrax,        // Bugatti Divo inspired
    
            // Sports Cars
            VehicleHash.Pariah,       // Ferrari inspired
            VehicleHash.Neon,         // Porsche Mission E inspired
            VehicleHash.Comet5,       // Porsche 911 GT2 RS inspired
            VehicleHash.Elegy,        // Nissan GT-R inspired
            VehicleHash.Jester,       // Toyota Supra inspired
            VehicleHash.Massacro,     // Aston Martin Vanquish inspired
            VehicleHash.Lynx,         // Jaguar F-Type inspired
            VehicleHash.Seven70,      // Aston Martin One-77 inspired
            VehicleHash.Specter,      // Aston Martin DB10 inspired
            VehicleHash.Feltzer2,     // Mercedes-Benz SL inspired
    
            // Luxury Cars
            VehicleHash.Cognoscenti,  // Bentley inspired
            VehicleHash.Schafter3,    // Mercedes-Benz E-Class inspired
            VehicleHash.Windsor,      // Rolls-Royce inspired
            VehicleHash.Superd,       // Rolls-Royce Wraith inspired
            VehicleHash.Alpha,        // BMW M3 inspired
    
            // Muscle Cars
            VehicleHash.Gauntlet,     // Dodge Challenger inspired
            VehicleHash.Dominator,    // Ford Mustang inspired
            VehicleHash.Phoenix,      // Pontiac Firebird inspired
            VehicleHash.Dukes,        // Dodge Charger inspired
            VehicleHash.Blade,        // Chevrolet Chevelle inspired
    
            // Classic Cars
            VehicleHash.Monroe,       // Ferrari 250 GT inspired
            VehicleHash.Stinger,      // Ferrari 250 GTO inspired
            VehicleHash.JB700,        // Aston Martin DB5 inspired
            VehicleHash.Coquette2,    // C2 Corvette inspired
            VehicleHash.Pigalle,      // Citroën SM inspired
    
            // Sport Classics
            VehicleHash.Turismo2,     // Ferrari F40 inspired
            VehicleHash.Cheetah2,     // Ferrari Testarossa inspired
            VehicleHash.Infernus2,    // Lamborghini Diablo inspired
            VehicleHash.Torero,       // Lamborghini Countach inspired
            VehicleHash.Stromberg,    // Lotus Esprit inspired
    
            // Modern Sports
            VehicleHash.Kuruma,       // Mitsubishi Lancer Evo X inspired
            VehicleHash.Sultan,       // Subaru Impreza WRX inspired
            VehicleHash.Banshee,      // Dodge Viper inspired
            VehicleHash.Buffalo,      // Dodge Charger modern inspired
            VehicleHash.Carbonizzare  // Ferrari California inspired
        };

        private Vehicle _currentVehicle;
        private int _switchCarTimer = 30;
        private bool _isChangingCar;
        private Vector3 _lastPosition;

        private bool _isRemoving;
        private int _removeTimer = 30;

        private bool isTeleporting = false;
        private float teleportRadius = 100f;

        private bool isStunting = false;
        private float jumpForce = 15f;
        private float rotationSpeed = 3f;
        private int stuntType = 0;

        private Ped[] _aliens = new Ped[5];
        private Model _alienModel = new Model(PedHash.MovAlien01);
        private bool _isAlienSpawned = false;
        private WeaponHash _alienWeapon = WeaponHash.APPistol;

        private bool _isChanging;
        private int _changeTimer;
        private PedHash[] _characterModels = {
            // Story Mode Characters
            PedHash.Franklin,
            PedHash.Michael,
            PedHash.Trevor,
        
            // Animals
            PedHash.Chop,
            PedHash.Husky,
            PedHash.Rottweiler,
            PedHash.Retriever,
            PedHash.Shepherd,
            PedHash.Cat,
            PedHash.Cow,
            PedHash.Deer,
            PedHash.Pig,
            PedHash.Rat,
            PedHash.Chimp,
            PedHash.Rhesus,
            PedHash.Hen,
            PedHash.Cormorant,
            PedHash.Crow,
            PedHash.Pigeon,
            PedHash.Seagull,
        
            // Special Characters
            PedHash.Orleans,
            PedHash.Zombie01,
            PedHash.MovAlien01,
            PedHash.Jesus01,
            PedHash.Clown01SMY,
            PedHash.MimeSMY,
            PedHash.Stripper01SFY,
            PedHash.Snowcop01SMM
        };
        private Model _currentModel;




        //My Self as a ped
        Ped self = Game.Player.Character;

        public Main()
        {
            this.Tick += onTick;
            this.KeyUp += onKeyUp;
        }

        private void onTick(object sender, EventArgs e)
        {
            if (_isChangingCar)
            {
                if (_switchCarTimer > 0)
                {
                    _switchCarTimer--;
                }
                else
                {
                    SwitchRandomCar();
                    _switchCarTimer = 1;
                    _isChangingCar = false;
                }
            }

            if (isFlipping && currentVehicle != null && currentVehicle.Exists())
            {
                DoFlipWithFly();
            }

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

            if (!isStunting && _currentVehicle != null && _currentVehicle.Exists())
            {
                if (!_currentVehicle.IsOnAllWheels)
                {
                    Vector3 rotation = _currentVehicle.Rotation;
                    _currentVehicle.Rotation = new Vector3(
                        SmoothRotation(rotation.X),
                        rotation.Y,
                        SmoothRotation(rotation.Z)
                    );
                }
            }

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

            if (_isChanging)
            {
                if (_changeTimer > 0)
                {
                    _changeTimer--;
                }
                else
                {
                    StartChanging();
                    _changeTimer = 5;
                    _isChanging = false;
                }
            }
        }

        private void onKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.H)
            {
                if (!isChangingCar)
                {
                    lastPosition = Game.Player.Character.Position;
                    isChangingCar = true;
                }
            }

            if (e.KeyCode == Keys.K)
            {
                if (!_isChanging)
                {
                    _isChanging = true;
                    _changeTimer = 5;
                }
            }
        }

        private void StartChanging()
        {
            try
            {
                // Pilih model random
                PedHash randomModel = _characterModels[_random.Next(_characterModels.Length)];
                _currentModel = new Model(randomModel);

                if (!_currentModel.IsLoaded)
                {
                    _currentModel.Request();
                }

                if (_currentModel.IsLoaded)
                {
                    // Simpan posisi dan status
                    Vector3 position = Game.Player.Character.Position;
                    float heading = Game.Player.Character.Heading;
                    int health = Game.Player.Character.Health;
                    int armor = Game.Player.Character.Armor;

                    // Cara yang benar untuk mengganti model karakter
                    Function.Call(Hash.SET_PLAYER_MODEL, Game.Player, _currentModel.Hash);

                    // Set ulang posisi dan status
                    Game.Player.Character.Position = position;
                    Game.Player.Character.Heading = heading;
                    Game.Player.Character.Health = health;
                    Game.Player.Character.Armor = armor;
                    Game.Player.Character.MaxHealth = 500;
                    Game.Player.Character.CanRagdoll = true;
                    Game.Player.Character.CanSwitchWeapons = true;

                    // Cleanup
                    _currentModel.MarkAsNoLongerNeeded();
                }
            }
            catch (Exception ex)
            {
            }
        }


        public void Dispose()
        {
            if (_currentModel != null && _currentModel.IsLoaded)
            {
                _currentModel.MarkAsNoLongerNeeded();
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
                    // Spawn alien dalam formasi lingkaran
                    float angle = i * (360f / _aliens.Length);
                    float x = playerPos.X + (float)Math.Cos(angle * Math.PI / 180) * 10;
                    float y = playerPos.Y + (float)Math.Sin(angle * Math.PI / 180) * 10;

                    _aliens[i] = World.CreatePed(_alienModel, new Vector3(x, y, playerPos.Z));

                    if (_aliens[i] != null)
                    {
                        // Setup alien properties
                        _aliens[i].MaxHealth = 500;
                        _aliens[i].Health = 500;
                        _aliens[i].Armor = 100;
                        //_aliens[i].Weapons(_alienWeapon, 1000);
                        _aliens[i].CanRagdoll = true;
                        _aliens[i].BlockPermanentEvents = true;

                        // Make aliens attack player
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


        private float SmoothRotation(float rotation)
        {
            if (Math.Abs(rotation) > 5f)
            {
                return rotation * 0.95f;
            }
            return 0f;
        }

        private void StartStuntRotation()
        {
            switch (stuntType)
            {
                case 0: // Backflip
                    _currentVehicle.ApplyForce(Vector3.Zero,
                        new Vector3(rotationSpeed, 0, 0));
                    break;

                case 1: // Frontflip
                    _currentVehicle.ApplyForce(Vector3.Zero,
                        new Vector3(-rotationSpeed, 0, 0));
                    break;

                case 2: // Left barrel roll
                    _currentVehicle.ApplyForce(Vector3.Zero,
                        new Vector3(0, 0, rotationSpeed));
                    break;

                case 3: // Right barrel roll
                    _currentVehicle.ApplyForce(Vector3.Zero,
                        new Vector3(0, 0, -rotationSpeed));
                    break;
            }

            // Reset stunt setelah beberapa detik
            Script.Wait(2000);
            isStunting = false;
        }

        private void DoStuntJump()
        {
            if (_currentVehicle != null && _currentVehicle.Exists())
            {
                isStunting = true;

                // Simpan kecepatan saat ini
                float currentSpeed = _currentVehicle.Speed;

                // Random stunt type (0-3)
                stuntType = _random.Next(4);

                // Aplikasikan gaya ke atas
                _currentVehicle.Velocity = new Vector3(
                    _currentVehicle.Velocity.X,
                    _currentVehicle.Velocity.Y,
                    jumpForce
                );

                // Pertahankan kecepatan maju
                _currentVehicle.ForwardSpeed = currentSpeed;

                // Mulai rotasi berdasarkan tipe stunt
                StartStuntRotation();
            }
        }



        private void RandomTeleport()
        {
            if (!isTeleporting)
            {
                isTeleporting = true;

                // Generate random offset dalam radius
                float randomX = (float)(rand.NextDouble() * 2 - 1) * teleportRadius;
                float randomY = (float)(rand.NextDouble() * 2 - 1) * teleportRadius;

                // Ambil posisi player saat ini
                Vector3 currentPos = Game.Player.Character.Position;

                // Hitung posisi teleport baru
                Vector3 newPosition = new Vector3(
                    currentPos.X + randomX,
                    currentPos.Y + randomY,
                    currentPos.Z
                );

                // Teleport player
                Game.Player.Character.Position = newPosition;

                // Jika dalam kendaraan, teleport kendaraannya juga
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
            }
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

        private void SwitchRandomCar()
        {
            VehicleHash randomModel = _carModels[_random.Next(_carModels.Length)];
            Vector3 _velocity = Vector3.Zero;
            float _speed = 0f;

            if (_currentVehicle != null && _currentVehicle.Exists())
            {
                _velocity = _currentVehicle.Velocity;
                _speed = _currentVehicle.Speed;
                _currentVehicle.Delete();
            }

            // Buat mobil baru dengan posisi dan heading yang sama
            _currentVehicle = World.CreateVehicle(randomModel, _lastPosition);
            if (_currentVehicle != null)
            {
                _currentVehicle.Heading = Game.Player.Character.Heading;
                _currentVehicle.PlaceOnGround();
                _currentVehicle.Repair();
                _currentVehicle.IsEngineRunning = true;

                // Terapkan velocity dan speed yang sama
                _currentVehicle.Velocity = _velocity;
                _currentVehicle.Speed = _speed > 0 ? _speed : 50f;

                // Paksa kecepatan awal
                Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, _currentVehicle, _speed > 0 ? _speed : 50f);

                // Langsung masuk ke mobil
                Game.Player.Character.Task.WarpIntoVehicle(_currentVehicle, VehicleSeat.Driver);

                // Tambahan untuk memastikan mobil tetap bergerak
                Script.Wait(5);
                _currentVehicle.Speed = _speed > 0 ? _speed : 50f;
            }
        }




        private void DoFlipWithFly()
        {
            Vector3 position = currentVehicle.Position;
            Vector3 rotation = currentVehicle.Rotation;
            float currentSpeed = currentVehicle.Speed; // Ambil kecepatan saat ini

            // Terbang dulu
            if (flyHeight < MAX_HEIGHT)
            {
                flyHeight += FLY_SPEED;
                position.Z += FLY_SPEED;
                currentVehicle.Position = position;
                // Pertahankan mesin tetap menyala dan bergerak
                currentVehicle.IsEngineRunning = true;
                currentVehicle.ForwardSpeed = currentSpeed; // Gunakan kecepatan yang sama
            }
            // Setelah mencapai ketinggian, mulai flip
            else
            {
                flipAngle += FLIP_SPEED;
                rotation.Y = flipAngle;
                currentVehicle.Rotation = rotation;
                // Pertahankan kecepatan saat flip
                currentVehicle.IsEngineRunning = true;
                currentVehicle.ForwardSpeed = currentSpeed; // Tetap gunakan kecepatan yang sama

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
