using GTA;
using GTA.Math;
using GTA.Native;
using System;

namespace GTAVWebhook.Types
{
    public class ChibakuTensei
    {
        private Ped[] peds;
        private Vehicle[] vehicles;
        private Prop[] props;
        private Entity[] entities;
        public int force = 100;
        private Vector3 playerCoord;
        private Vector3 CTCoordinate;
        private bool CT = false;
        private bool SST = false;
        private bool SSTS = false;
        private double r = 20;
        Random rand = new Random();
        private Boolean isThereEntity;
        private double DesensionZ;
        private Boolean downornot = false;
        private Boolean isDead = false;
        private int CT_counter = 0;
        private int invincibleTimer = 0;
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


        //My Self as a ped
        Ped self = Game.Player.Character;
        Vector3 originalCoord;


        private Vector3 _ctCoordinate;
        private bool _isActive;
        private int _counter;

        public ChibakuTensei()
        {
        }


        public void Update()
        {
            if (Game.Player.Character.IsDead)
            {
                if (!isDead)
                {
                    ResetAllFunctions();
                    isDead = true;
                    // Set timer untuk invincible
                    invincibleTimer = 150; // Sekitar 5 detik (30 fps)
                }
                return;
            }
            else
            {
                if (invincibleTimer > 0)
                {
                    Game.Player.Character.IsInvincible = true;
                    invincibleTimer--;
                }
                else
                {
                    Game.Player.Character.IsInvincible = false;
                    isDead = false;
                }
            }

            // Kode existing
            if (CT_counter <= 0)
            {
                CT = true;
            }

            if (CT && !SST)
            {
                Chibaku_Tensei();
                CT_counter--;
            }

            if (SST && !CT)
            {
                if (levitation())
                {
                    Super_ShinraTensei();
                }
            }

            if (downornot)
            {
                down();
            }
        }

        public void Execute()
        {
            setVar();
            CTCoordinate = Game.Player.Character.Position + new Vector3(20, 20, 40);
            Game.Player.Character.Task.HandsUp(500);

            CT = false;
            CT_counter = 0;
            r = 20;
            downornot = false;
            SST = false;

            if (!CT)
            {
                new System.Media.SoundPlayer("./scripts/Painfiles/CT.wav").Play();
                GTA.UI.Screen.ShowSubtitle("Chibaku Tensei");

                CT = true;
                CT_counter = 300;
            }
        }

        private void ResetAllFunctions()
        {
            CT = false;
            SST = false;
            SSTS = false;
            CT_counter = 0;
            r = 20;
            downornot = false;

            // Reset posisi dan koordinat jika diperlukan
            CTCoordinate = new Vector3();
            playerCoord = new Vector3();
        }

        public void Chibaku_Tensei()
        {
            entities = World.GetNearbyEntities(CTCoordinate, (float)500.0);
            Vector3 cameraPos = CTCoordinate - new Vector3(0, -50, -20);
            Vector3 cameraPoint = CTCoordinate;
            int cam = Function.Call<int>(Hash.CREATE_CAM, "DEFAULT_SCRIPTED_CAMERA", true);
            Function.Call(Hash.SET_CAM_COORD, cam, cameraPos.X, cameraPos.Y, cameraPos.Z);
            Function.Call(Hash.POINT_CAM_AT_COORD, cam, cameraPoint.X, cameraPoint.Y, cameraPoint.Z);
            Function.Call(Hash.SET_CAM_ACTIVE, cam, true);
            Function.Call(Hash.RENDER_SCRIPT_CAMS, true, false, 0, true, true);
            Function.Call(Hash.DESTROY_CAM, cam, true);
            Function.Call(Hash.RENDER_SCRIPT_CAMS, false, false, 0, true, true);

            foreach (Entity en in entities)
            {
                if (en.Equals(Game.Player.Character))
                {
                    pullPlayer(en, CTCoordinate, 20);
                }
                else
                {
                    pull(en, CTCoordinate, 20);
                }
            }

        }

        public void pullPlayer(Entity player, Vector3 location, int power)
        {
            Vector3 coord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, player, true);
            double dx = (coord.X - location.X) * -0.5;
            double dy = (coord.Y - location.Y) * -0.5;
            double dz = (coord.Z - location.Z) * -2;
            double distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double distanceRate = (power / distance) * Math.Pow(1.04, 1 - distance);
            Function.Call(Hash.APPLY_FORCE_TO_ENTITY, player, 1,
                distanceRate * dx,
                distanceRate * dy,
                distanceRate * dz + 0.5f,
                0, 0, 0,
                true, false, true, true, true, true);
        }

        public void pull(Entity en, Vector3 location, int power)
        {
            Vector3 coord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, en, true);
            double dx = (coord.X - location.X) * -1;
            double dy = (coord.Y - location.Y) * -1;
            double dz = (coord.Z - location.Z) * -1;
            double distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double distanceRate = (power / distance) * Math.Pow(1.04, 1 - distance);
            Function.Call(Hash.APPLY_FORCE_TO_ENTITY, en, 1, distanceRate * dx, distanceRate * dy, distanceRate * dz, rand.NextDouble() * rand.Next(-1, 1), rand.NextDouble() * rand.Next(-1, 1), rand.NextDouble() * rand.Next(-1, 1), true, false, true, true, true, true);
        }

        public bool levitation()
        {
            if (playerCoord.Z < originalCoord.Z + 40)
            {
                Function.Call(Hash.SET_ENTITY_COORDS, self, playerCoord.X, playerCoord.Y, playerCoord.Z + 0.005, 1, 0, 0, 1);
                playerCoord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, self, true);
                return false;
            }
            else
            {
                Function.Call(Hash.SET_ENTITY_COORDS, self, playerCoord.X, playerCoord.Y, originalCoord.Z + 40.0001, 1, 0, 0, 1);
                return true;

            }
        }

        public bool down()
        {
            if (playerCoord.Z > DesensionZ + 4)
            {
                Function.Call(Hash.SET_ENTITY_COORDS, self, playerCoord.X, playerCoord.Y, DesensionZ, 1, 0, 0, 1);
                playerCoord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, self, true);
                return false;
            }
            else
            {
                downornot = false;
                return true;
            }
        }

        public void Super_ShinraTensei()
        {
            if (r <= 2500)
            {
                foreach (Entity en in entities)
                {
                    specialPush(en, originalCoord, 25);
                }
                dust();
                r += 7;
            }
            else
            {
                r = 20;
                SST = false;
                downornot = true;
            }
        }

        public void specialPush(Entity en, Vector3 location, int power)
        {
            Vector3 coord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, en, true);
            double dx = (coord.X - location.X);
            double dy = (coord.Y - location.Y);
            double dz = (coord.Z - location.Z);
            double distance = Math.Sqrt(dx * dx + dy * dy + dz * dz);
            double distanceRate = (power / 10) * Math.Pow(1.04, 1 - 10);
            Function.Call(Hash.APPLY_FORCE_TO_ENTITY, en, 1, distanceRate * dx, distanceRate * dy, distanceRate * dz - 5, rand.NextDouble() * rand.Next(-1, 1), rand.NextDouble() * rand.Next(-1, 1), rand.NextDouble() * rand.Next(-1, 1), true, false, true, true, true, true);
        }

        public void dust()
        {
            int dust = Function.Call<int>(Hash.GET_HASH_KEY, "v_ilev_cd_dust");
            for (int t = 0; t < 360; t++)
            {
                playerCoord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, self, true);
                double calcX = originalCoord.X + r * Math.Acos(t);
                double calcY = originalCoord.Y + r * Math.Asin(t);
                Vector3 newCenter = new Vector3((float)calcX, (float)calcY, originalCoord.Z);
                Function.Call(Hash.ADD_EXPLOSION, (float)calcX, (float)calcY, originalCoord.Z, 25, 25, true, true, 0);
            }
        }

        public void setVar()
        {
            playerCoord = Function.Call<Vector3>(Hash.GET_ENTITY_COORDS, self, true);
            peds = World.GetNearbyPeds(self, (float)20.0);
            vehicles = World.GetNearbyVehicles(self, (float)20.0);
            props = World.GetNearbyProps(playerCoord, (float)20.0);
            DesensionZ = playerCoord.Z;
        }
    }
}
