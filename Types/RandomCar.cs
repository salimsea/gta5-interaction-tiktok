using GTA;
using GTA.Math;
using System;

namespace GTAVWebhook.Types
{
    public class RandomCar
    {
        private readonly Random _random;
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
        private int _switchCarTimer;
        private bool _isChangingCar;
        private Vector3 _lastPosition;

        public RandomCar()
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
                    SwitchRandomCar();
                    _switchCarTimer = 30;
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

        private void SwitchRandomCar()
        {
            VehicleHash randomModel = _carModels[_random.Next(_carModels.Length)];

            if (_currentVehicle != null && _currentVehicle.Exists())
            {
                _currentVehicle.Delete();
            }

            _currentVehicle = World.CreateVehicle(randomModel, _lastPosition);
            if (_currentVehicle != null)
            {
                _currentVehicle.Heading = Game.Player.Character.Heading;
                _currentVehicle.PlaceOnGround();
                _currentVehicle.Repair();
                _currentVehicle.IsEngineRunning = true;
                Game.Player.Character.Task.WarpIntoVehicle(_currentVehicle, VehicleSeat.Driver);
            }
        }
    }
}
