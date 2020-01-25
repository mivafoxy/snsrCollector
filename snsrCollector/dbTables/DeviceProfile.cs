using System;
using System.Collections.Generic;

namespace snsrCollector
{
    public partial class DeviceProfile
    {
        public DeviceProfile()
        {
            DeviceObject = new HashSet<DeviceObject>();
        }

        public string IdKey { get; set; }
        public string DeviceLdFkey { get; set; }
        public string ModelProfileFkey { get; set; }

        public virtual DeviceLogical DeviceLdFkeyNavigation { get; set; }
        public virtual ModelProfile ModelProfileFkeyNavigation { get; set; }
        public virtual ICollection<DeviceObject> DeviceObject { get; set; }
    }
}
