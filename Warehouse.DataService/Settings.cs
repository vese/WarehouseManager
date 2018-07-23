namespace DataService
{
    internal static class Settings
    {
        #region ShowStatus
        internal const int ColumnWidth = 18;
        internal static string[] TableHeaders = { "Site ID", "Hangar ID", "Hangar capacity", "Stored containers" };
        #endregion
        #region PlaceContainers
        internal static string CanNotPlaceContainersMessage = "There is not enough free place in warehouse.";
        internal static string WarehouseIsFilledMessage = "Warehouse is filled.";
        internal static string PlacedMessage = "Containers placed in the warehouse: ";
        internal static string PartiallyPlacedMessage = "Containers partially placed in the warehouse: ";
        #endregion
        #region FreeContainers
        internal static string HangarDoesNotExistMessage = "Hangar doesn't exist.";
        internal static string NotEnoughContainersMessage = "There are not enough containers in the hangar.";
        internal static string ContainersTakenAwayMessage = "Containers taken away.";
        #endregion
    }
}