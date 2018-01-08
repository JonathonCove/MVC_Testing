using System;

namespace DataManager.Interfaces
{
    public interface ITemporal
    {

        int? LastUpdateByID { get; set; }
        byte? LastUpdatedByType { get; set; }
        int? CreatedByID { get; set; }
        byte? CreatedByType { get; set; }
        DateTime CreationDate { get; set; }
        DateTime ModifiedDateTime { get; set; }
        Guid UniqueObjectID { get; set; }

    }
}
