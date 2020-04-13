using System;
using System.Collections.Generic;

namespace snsrCollector.dbTables
{
    public partial class ProfileType
    {
        public ProfileType()
        {
            ModelProfile = new HashSet<ModelProfile>();
        }

        public int IdKey { get; set; }
        public string TypeName { get; set; }

        public virtual ICollection<ModelProfile> ModelProfile { get; set; }
    }
}
