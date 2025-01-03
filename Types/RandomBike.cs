﻿using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace GTAVWebhook.Types
{
    public class RandomBike
    {
        private readonly Random _random;
        private readonly VehicleHash[] _carModels = {
            // Super Cars
            VehicleHash.Hakuchou,     // Suzuki Hayabusa inspired
            VehicleHash.Bati,         // Ducati 848 inspired
            VehicleHash.Bati2,        // Ducati racing variant
            VehicleHash.CarbonRS,     // Ducati Desmosedici RR inspired
            VehicleHash.Double,       // Ducati StreetFighter inspired
            VehicleHash.Akuma,        // Kawasaki Z1000 inspired
            VehicleHash.Thrust,       // Suzuki B-King inspired
            VehicleHash.Defiler,      // Ducati Diavel inspired
            VehicleHash.Vindicator,   // BMW K1600 inspired
            VehicleHash.Lectro,       // Electric sport bike
    
            // Classic Bikes
            VehicleHash.Daemon,       // Harley Davidson inspired
            VehicleHash.Innovation,   // Custom chopper
            VehicleHash.Hexer,        // Harley Softail inspired
            VehicleHash.Sovereign,    // Harley touring inspired
            VehicleHash.Wolfsbane,    // Classic Bobber style
    
            // Modern Choppers
            VehicleHash.Avarus,       // Custom chopper modern
            VehicleHash.Chimera,      // Three-wheeled bike
            VehicleHash.Nightblade,   // Custom night rod
            VehicleHash.ZombieA,      // Chopper variant A
            VehicleHash.ZombieB,      // Chopper variant B
    
            // Dirt/Off-road Bikes
            VehicleHash.Sanchez,      // Dirt bike
            VehicleHash.Sanchez2,     // Racing dirt bike
            VehicleHash.BF400,        // Motocross bike
            VehicleHash.Manchez,      // Enduro bike
            VehicleHash.Cliffhanger   // Trail bike
        };

        private Vehicle _currentVehicle;
        private int _switchCarTimer;
        private bool _isChangingCar;
        private Vector3 _lastPosition;

        public RandomBike()
        {
            _random = new Random();
            _switchCarTimer = 30;
        }

        public void Update()
        {
            if (_isChangingCar)
            {
                if (_switchCarTimer > 0)
                {
                    _switchCarTimer--;
                }
                else
                {
                    SwitchRandomBike();
                    _switchCarTimer = 1;
                    _isChangingCar = false;
                }
            }
        }

        public void Execute()
        {
            if (!_isChangingCar)
            {
                _lastPosition = Game.Player.Character.Position;
                _isChangingCar = true;
            }
        }

        private void SwitchRandomBike()
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

            _currentVehicle = World.CreateVehicle(randomModel, _lastPosition);
            if (_currentVehicle != null)
            {
                _currentVehicle.Heading = Game.Player.Character.Heading;
                _currentVehicle.PlaceOnGround();
                _currentVehicle.Repair();
                _currentVehicle.IsEngineRunning = true;

                _currentVehicle.Velocity = _velocity;
                _currentVehicle.Speed = _speed > 0 ? _speed : 50f;

                Function.Call(Hash.SET_VEHICLE_FORWARD_SPEED, _currentVehicle, _speed > 0 ? _speed : 50f);

                Game.Player.Character.Task.WarpIntoVehicle(_currentVehicle, VehicleSeat.Driver);

                Script.Wait(5);
                _currentVehicle.Speed = _speed > 0 ? _speed : 50f;
            }
        }

    }
}
