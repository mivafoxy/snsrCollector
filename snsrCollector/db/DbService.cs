using Microsoft.EntityFrameworkCore;
using snsrCollector.communication;
using snsrCollector.modules;
using snsrCollector.modules.sensoric;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace snsrCollector.db
{
    public static class DbService
    {
        public static List<IChannel> LoadCommChannels()
        {
            List<IChannel> commChannels = LoadAndConvertToCommChannelTree();
            return commChannels;
        }

        public static void WriteValuesToDb(List<ModuleObjectValue> moduleValues)
        {
            using (var db = new snsrContext())
            {
                foreach (var objectValue in moduleValues)
                {
                    string deviceObjectKey =
                        db.DeviceObject.Where(
                            dob => 
                                dob.DeviceLdFkey == objectValue.LogicalDeviceId && 
                                dob.ObjectDictId == objectValue.ObjectId).
                            Select(
                                dobResult => 
                                    dobResult.IdKey).
                            FirstOrDefault();

                    db.DeviceObjectValue.Add(
                        new DeviceObjectValue()
                            {
                                DeviceObjectFkey = deviceObjectKey,
                                ObjectValue = objectValue.ModuleValue.ToString(),
                                ReceiveTime = objectValue.ReceivedTime,
                                IdKey = Guid.NewGuid().ToString()
                            });
                }

                db.SaveChanges();
            }
        }

        private static List<IChannel> LoadAndConvertToCommChannelTree()
        {
            List<IChannel> channels = GetRootDeviceChannels();

            // Теперь в канал необходимо загрузить все устройства, ему принадлежащие.
            AppendDevicesTo(channels);

            return channels;
        }

        private static List<IChannel> GetRootDeviceChannels()
        {
            using (var db = new snsrContext())
            {
                var channels = new List<IChannel>();

                var rootDevices =
                    db.Device.Where(
                        root => root.DeviceType == ModelTypeConsts.RootDeivceType).
                    Select(
                        model => model).ToList();

                foreach (var rootDevice in rootDevices)
                {
                    // Взять главный логический прибор

                    var mainLogicalDeviceIds =
                        db.DeviceLogical.Where(
                            ld =>
                                rootDevice.MainLogicalDevice == ld.IdKey).
                        Select(
                            mainLd => mainLd.IdKey);

                    // Взять ссылки на профили из главного логического прибора

                    var deviceProfiles =
                        db.DeviceProfile.Where(
                            profile =>
                                mainLogicalDeviceIds.Contains(
                                    profile.DeviceLdFkey)).
                        Select(
                            profile => profile).
                        ToList();

                    foreach (var deviceProfile in deviceProfiles)
                    {
                        var profileAttributes = 
                            db.DeviceObject.Where(
                                devObject => 
                                    devObject.DeviceProfileFkey == deviceProfile.IdKey).
                            Select(
                                devObject => devObject).
                            ToList();

                        string host =
                            GetAttributeValue(
                                ModelObjectConsts.Ip,
                                profileAttributes);

                        string port =
                            GetAttributeValue(
                                ModelObjectConsts.Port,
                                profileAttributes);

                        //
                        // Начиная отсюда, если добавляться будут новые типы каналов, можно будет доделать логику создания каналов определённого типа.
                        // Пока что создаются TCP - сервера.
                        //

                        var channel =
                            new TcpServerChannel(
                                host,
                                int.Parse(port),
                                rootDevice.SerialNumber);

                        channels.Add(channel);
                    }
                }

                return channels;
            }
        }

        private static string GetAttributeValue(int objectId, ICollection<DeviceObject> deviceObjects)
        {
            foreach (var deviceObject in deviceObjects)
            {
                if (deviceObject.ObjectDictId == objectId)
                    return deviceObject.StartValue;
            }

            throw new Exception("Incorrect model.");
        }

        private static void AppendDevicesTo(ICollection<IChannel> channels)
        {
            using (var db = new snsrContext())
            {
                foreach (var channel in channels)
                {
                    var rootDevice =
                        db.Device.Where(
                            device =>
                                device.SerialNumber == channel.GetChannelId()).
                        Select(
                            dev => dev).
                        FirstOrDefault();

                    var childs =
                        db.Device.Where(
                            device =>
                                device.NetworkRightDevice.Any(
                                    network =>
                                        network.LeftDeviceId == rootDevice.IdKey)).
                        Select(
                            child => child).
                        ToList();

                    NewModulesToChannelFrom(
                        childs.ToList(),
                        channel);
                }
            }
        }

        private static void NewModulesToChannelFrom(List<Device> devices, IChannel channel)
        {
            //
            // Пока что создаётся DataModule, потому что других нет в системе.
            // Добавление новых модулей, которые работают по иному протоколу необходимо 
            // реализовать фабрику.
            //
            using (var db = new snsrContext())
            {
                foreach (var device in devices)
                {
                    List<DeviceLogical> logicalDeviceIds =
                        db.DeviceLogical.Where(
                            ld => 
                                ld.DeviceFkey == device.IdKey).
                            Select(
                                ldResult => ldResult).
                            Include(
                                ld => ld.DeviceObject).
                            ToList();

                    SensoricModule dataModule =
                        new SensoricModule(
                            channel.GetChannelId(),
                            device.SerialNumber,
                            logicalDeviceIds);

                    channel.LinkChannelWithPollModule(dataModule);
                }
            }

            
        }
    }
}
