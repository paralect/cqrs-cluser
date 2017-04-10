namespace DataService
{
    using System.Collections.Generic;

    public interface IShipmentDataService
    {
        IEnumerable<Shipment> GetAllShipments();
    }
}