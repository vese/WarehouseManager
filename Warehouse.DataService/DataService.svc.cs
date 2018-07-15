using System;
using System.IO;
using System.Linq;
using Warehouse.Data;

namespace DataService
{
    public class DataService : IDataService
    {
        static void LogException(Exception ex, DateTime time)
        {
            using (StreamWriter sw = new StreamWriter(Settings.LogFileName, true))
            {
                sw.WriteLine(new string('-', 10));
                sw.WriteLine(ex.ToString());
                sw.WriteLine(time.ToString());
                sw.WriteLine(new string('-', 10));
            }
        }

        DataFunctions.LogExceptionFunction log = new DataFunctions.LogExceptionFunction(LogException);

        public string ShowStatus(out bool success, out string exceptionString)
        {
            string table = Settings.TableHeaders.Length >= 4 ? $"{Settings.TableHeaders[0],-Settings.ColumnWidth}{Settings.TableHeaders[1],-Settings.ColumnWidth}{Settings.TableHeaders[2],-Settings.ColumnWidth}{Settings.TableHeaders[3],-Settings.ColumnWidth}" : "";
            var sites = DataFunctions.GetSites(true, true, log, out success, out exceptionString);
            if (!success)
            {
                return exceptionString;
            }
            foreach (var site in sites)
            {
                var hangars = DataFunctions.GetHangars(site.id, true, true, log, out success, out exceptionString);
                if (!success)
                {
                    return exceptionString;
                }
                foreach (var hangar in hangars)
                {
                    table += Environment.NewLine + $"{hangar.site_id,-Settings.ColumnWidth}{hangar.id,-Settings.ColumnWidth}{hangar.capacity,-Settings.ColumnWidth}{hangar.fullness,-Settings.ColumnWidth}";
                }
            }
            return table;
        }

        ushort Place(ushort N, out bool success, out string exceptionString, bool emptySites = false, bool emptyHangars = false)
        {
            if (N == 0)
            {
                success = true;
                exceptionString = null;
                return 0;
            }
            var sites = DataFunctions.GetSites(emptySites, true, log, out success, out exceptionString);
            if (!success)
            {
                return N;
            }
            for (int i = 0; i < sites.Count && N > 0; i++)
            {
                var hangars = DataFunctions.GetHangars(sites[i].id, emptyHangars, false, log, out success, out exceptionString);
                if (!success)
                {
                    return N;
                }
                for (int j = 0; j < hangars.Count && N > 0; j++)
                {
                    ushort hN = Math.Min((ushort)(hangars[j].capacity - hangars[j].fullness), N);
                    bool modified = DataFunctions.ModifyHangar(hangars[j].id, hN, log, out success, out exceptionString);
                    if (!success)
                    {
                        return N;
                    }
                    if (modified)
                    {
                        N -= hN;
                    }
                }
            }
            return N;
        }

        public string PlaceContainers(ushort N, out bool success, out string exceptionString)
        {
            var s = DataFunctions.GetSites(true, false, log, out success, out exceptionString);
            if (!success)
            {
                return exceptionString;
            }
            int freePlaces = s.Sum(e => e.capacity);
            if (freePlaces >= N)
            {
                ushort oldN = N;
                N = Place(N, out success, out exceptionString);
                if (!success)
                {
                    var placed = oldN - N;
                    return placed > 0 ? exceptionString : Settings.PartiallyPlacedMessage + placed.ToString() + Environment.NewLine + exceptionString;
                }
                N = Place(N, out success, out exceptionString, false, true);
                if (!success)
                {
                    var placed = oldN - N;
                    return placed > 0 ? exceptionString : Settings.PartiallyPlacedMessage + placed.ToString() + Environment.NewLine + exceptionString;
                }
                N = Place(N, out success, out exceptionString, true, true);
                if (!success)
                {
                    var placed = oldN - N;
                    return placed > 0 ? exceptionString : Settings.PartiallyPlacedMessage + placed.ToString() + Environment.NewLine + exceptionString;
                }
                return Settings.PlacedMessage + oldN.ToString();
            }
            else if (freePlaces == 0)
            {
                return Settings.WarehouseIsFilledMessage;
            }
            else
            {
                return Settings.CanNotPlaceContainersMessage;
            }
        }

        public string FreeContainers(ushort N, string hangarId, out bool success, out string exceptionString)
        {
            bool exist;
            var hangar = DataFunctions.GetHangar(hangarId, log, out exist, out success, out exceptionString);
            if (!success)
            {
                return exceptionString;
            }
            if (!exist)
            {
                return Settings.HangarDoesNotExistMessage;
            }

            if (hangar.fullness < N)
            {
                return Settings.NotEnoughContainersMessage;
            }
            else
            {
                DataFunctions.ModifyHangar(hangar.id, -N, log, out success, out exceptionString);
                if (!success)
                {
                    return exceptionString;
                }
                return Settings.ContainersTakenAwayMessage;
            }
        }
    }
}
