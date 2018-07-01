using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WMDBService
{
    [ServiceContract]
    public interface IWMDBService
    {
        /// <summary>
        /// Получает из бд список всех площадок.
        /// </summary>
        /// <returns>Cписок всех площадок</returns>
        [OperationContract]
        List<Site> GetAllSites();

        /// <summary>
        /// Получает из бд список не пустых и не полных площадок.
        /// </summary>
        /// <param name="empty">Если true, то добавляет к результату пустые площадки</param>
        /// <param name="full">Если true, то добавляет к результату полные площадки</param>
        /// <returns>Список площадок</returns>
        [OperationContract]
        List<Site> GetSites(bool empty, bool full);

        /// <summary>
        /// Проверяет существование площадки с указанным id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True, если существует</returns>
        [OperationContract]
        bool SiteExists(string id);

        /// <summary>
        /// Получает список всех ангаров на площадке с указанным id.
        /// </summary>
        /// <param name="siteId">Id площадки</param>
        /// <returns>Список ангаров</returns>
        [OperationContract]
        List<Hangar> GetAllHangars(string siteId);

        /// <summary>
        /// Получает список всех не пустых и не полных ангаров на площадке с указанным id.
        /// </summary>
        /// <param name="siteId">Id площадки</param>
        /// <param name="empty">Если true, то добавляет к результату пустые ангары</param>
        /// <param name="full">Если true, то добавляет к результату полные ангары</param>
        /// <returns>Список ангаров</returns>
        [OperationContract]
        List<Hangar> GetHangars(string siteId, bool empty, bool full);

        /// <summary>
        /// Изменяет в бд количество контейнеров в ангаре с указанным id. Также меняет значения в таблице site.
        /// </summary>
        /// <param name="id">Id ангара</param>
        /// <param name="N">Изменение контейнеров</param>
        /// <returns>True, если произошло обновление в бд</returns>
        [OperationContract]
        bool ModifyHangar(string id, int N);

        /// <summary>
        /// Получает количество свободных мест на складе
        /// </summary>
        /// <returns>Число свободных мест</returns>
        [OperationContract]
        int GetFreePlacesCount();
    }

    [DataContract]
    public class Site
    {
        string id;
        bool empty;
        int capacity;

        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public bool Empty
        {
            get { return empty; }
            set { empty = value; }
        }

        [DataMember]
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }
    }

    [DataContract]
    public class Hangar
    {
        string id;
        string siteId;
        int capacity;
        int fullness;

        [DataMember]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        [DataMember]
        public int Capacity
        {
            get { return capacity; }
            set { capacity = value; }
        }

        [DataMember]
        public int Fullness
        {
            get { return fullness; }
            set { fullness = value; }
        }
    }
}
