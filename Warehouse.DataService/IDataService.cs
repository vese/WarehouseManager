using System.ServiceModel;

namespace DataService
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        string ShowStatus(out bool success, out string exceptionString);

        [OperationContract]
        string PlaceContainers(ushort N, out bool success, out string exceptionString);

        [OperationContract]
        string FreeContainers(ushort N, string hangarId, out bool success, out string exceptionString);
    }
}
