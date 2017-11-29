using System;

namespace OrmLite.Model
{
    public interface IModifiedDate
    {
        DateTime CreatedOn { get; set; }
        DateTime LastUpdated { get; set; }
    }
}