﻿using System;
using System.Collections.Generic;

namespace snsrCollector.dbTables
{
    public partial class ModelLogicalDeviceObject
    {
        public ModelLogicalDeviceObject()
        {
            DeviceObject = new HashSet<DeviceObject>();
        }

        public string IdKey { get; set; }
        public string ModelLdFkey { get; set; }
        public string ModelProfileFkey { get; set; }
        public int ObjectId { get; set; }
        public bool IsEditable { get; set; }
        public bool IsInitable { get; set; }
        public bool IsShown { get; set; }

        public virtual ModelLogicalDevice ModelLdFkeyNavigation { get; set; }
        public virtual ModelProfile ModelProfileFkeyNavigation { get; set; }
        public virtual ObjectDict Object { get; set; }
        public virtual ICollection<DeviceObject> DeviceObject { get; set; }
    }
}
