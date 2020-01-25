using snsrCollector.db;
using System;
using System.Collections.Generic;
using System.Text;

namespace snsrCollector.modules.sensoric
{
    public static class SensoricConsts
    {
        public const string Temperature = "T";
        public const string Humidity = "H";
        public const string Rain = "RD";
        public const string WindSpeed = "WS";
        public const string Illumination = "LM";
        public const string Propane_2p = "MQ2P";
        public const string Butane_2b = "MQ2B";
        public const string Metane_2m = "MQ2M";
        public const string Hydrogen = "MQ2H";
        public const string CH = "MQ3";
        public const string CH_4 = "MQ4";
        public const string EnvironmentGas = "MQ5";
        public const string Propane_6p = "MQ6P";
        public const string Butane_6p = "MQ6B";
        public const string CO = "MQ9O";
        public const string CH_9 = "MQ9H";
        public const string MQ135NH = "MQ135NH";
        public const string MQ135H2S = "MQ135H2S";
        public const string MQ135CH = "MQ135CH";


        public static Dictionary<string, int> GetSupportedSensorsToObjects()
        {
            var sensorsToObjectIds = new Dictionary<string, int>();

            sensorsToObjectIds.Add(Temperature, ObjectIdConsts.IdTemperature);
            sensorsToObjectIds.Add(Humidity, ObjectIdConsts.IdHumidity);
            sensorsToObjectIds.Add(Rain, ObjectIdConsts.IdRain);
            sensorsToObjectIds.Add(WindSpeed, ObjectIdConsts.IdWindSpeed);
            sensorsToObjectIds.Add(Illumination, ObjectIdConsts.IdIllumination);
            sensorsToObjectIds.Add(Propane_2p, ObjectIdConsts.IdPropane_2p);
            sensorsToObjectIds.Add(Butane_2b, ObjectIdConsts.IdButane_2b);
            sensorsToObjectIds.Add(Metane_2m, ObjectIdConsts.IdMetane_2m);
            sensorsToObjectIds.Add(Hydrogen, ObjectIdConsts.IdHydrogen);
            sensorsToObjectIds.Add(CH, ObjectIdConsts.IdCH);
            sensorsToObjectIds.Add(CH_4, ObjectIdConsts.IdCH_4);
            sensorsToObjectIds.Add(EnvironmentGas, ObjectIdConsts.IdEnvironmentGas);
            sensorsToObjectIds.Add(Propane_6p, ObjectIdConsts.IdPropane_6p);
            sensorsToObjectIds.Add(Butane_6p, ObjectIdConsts.IdButane_6p);
            sensorsToObjectIds.Add(CO, ObjectIdConsts.IdCO);
            sensorsToObjectIds.Add(CH_9, ObjectIdConsts.IdCH_9);
            sensorsToObjectIds.Add(MQ135NH, ObjectIdConsts.IdMQ135NH);
            sensorsToObjectIds.Add(MQ135H2S, ObjectIdConsts.IdMQ135H2S);
            sensorsToObjectIds.Add(MQ135CH, ObjectIdConsts.IdMQ135CH);

            return sensorsToObjectIds;
        }
    }
}
