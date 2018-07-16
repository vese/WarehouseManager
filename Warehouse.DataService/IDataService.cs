using System.ServiceModel;

namespace DataService
{
    [ServiceContract]
    public interface IDataService
    {
        /// <summary>
        /// Возвращает таблицу с информацией о заполненности склада.
        /// </summary>
        /// <param name="success">False, если произошло исключение</param>
        /// <param name="exceptionString">Информация об исключении</param>
        /// <returns>Таблица или информация об исключении</returns>
        [OperationContract]
        string ShowStatus(out bool success, out string exceptionString);

        /// <summary>
        /// Размещает на складе N контейнеров
        /// </summary>
        /// <param name="N">Количество контейнеров</param>
        /// <param name="success">False, если произошло исключение</param>
        /// <param name="exceptionString">Информация об исключении</param>
        /// <returns>Сообщение о произведенных действиях или информация об исключении</returns>
        [OperationContract]
        string PlaceContainers(ushort N, out bool success, out string exceptionString);

        /// <summary>
        /// Достаёт N контейнеров из ангара с указанным id.
        /// </summary>
        /// <param name="N">Количество контейнеров</param>
        /// <param name="hangarId">Id ангара</param>
        /// <param name="success">False, если произошло исключение</param>
        /// <param name="exceptionString">Информация об исключении</param>
        /// <returns>Сообщение о произведенных действиях или информация об исключени</returns>
        [OperationContract]
        string FreeContainers(ushort N, string hangarId, out bool success, out string exceptionString);
    }
}
