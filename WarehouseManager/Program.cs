using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManager
{
    class Program
    {
        static ushort PlaceContainers(ushort N, WMDBService.WMDBServiceClient wm, bool empty = false, bool full = false)
        {
            List<WMDBService.Site> sites = wm.GetSites(empty, full).ToList();
            for (int i = 0; i < sites.Count && N > 0; i++)
            {
                List<WMDBService.Hangar> hangars = wm.GetHangars(sites[i].Id, empty, full).ToList();
                for (int j = 0; j < hangars.Count && N > 0; j++)
                {
                    ushort hN = Math.Min((ushort)(hangars[j].Capacity - hangars[j].Fullness), N);
                    wm.ModifyHangar(hangars[j].Id, hN);
                    N -= hN;
                }
            }
            return N;
        }
        static void Main(string[] args)
        {
            #region consts
            const string HelpCommand = "help",
                ExitCommand = "exit",
                ShowstatusCommand = "showstatus",
                StoreCommand = "store",
                FreeCommand = "free";
            const int CommandsLength = 18;
            const string EnterSuggestion = "Please, enter a command. Enter \"" + HelpCommand + "\" to get all commands' info.",
                UnknownCommandMessage = "Unknown command! Please, enter command again.",
                InvalidArgumentsMessage = "Invalid argumets! Try again.",
                WarehouseIsFilledMessage = "Warehouse is filled.",
                CanNotPlaceContainersMessage = "There is not enough free place in warehouse.",
                ContainersPlacedMessage = "Containers are placed in the warehouse.",
                ContainersTakenAwayMessage = "Containers were taken away.",
                NotEnoughContainersMessage = "There are not enough containers in the hangar.";
            string HelpMessage = "Commands' info:" + Environment.NewLine +
                $"{HelpCommand,-CommandsLength}Shows commands' info." + Environment.NewLine +
                $"{ExitCommand,-CommandsLength}Closes app." + Environment.NewLine +
                $"{ShowstatusCommand,-CommandsLength}Showes table with current statuses of all hangars." + Environment.NewLine +
                $"{StoreCommand + " <N>",-CommandsLength}Places N containers in warehouse. N is natural number." + Environment.NewLine +
                $"{FreeCommand + " <N> <H_ID>",-CommandsLength}Takes away N containers from hangar H_ID. N is natural number.";
            const int ColumnWidth = 18;
            string ColumnHeader = $"{"Site ID",-ColumnWidth}{"Hangar ID",-ColumnWidth}{"Hangar capacity",-ColumnWidth}{"Stored containers",-ColumnWidth}";
            #endregion

            WMDBService.WMDBServiceClient wm = new WMDBService.WMDBServiceClient(); //Ссылка на службу для работы с бд
            string command = "";
            bool needEnterSuggestion = true;

            while (true)
            {
                if (needEnterSuggestion)
                {
                    Console.WriteLine(EnterSuggestion);
                }
                else
                {
                    needEnterSuggestion = true;
                }
                command = Console.ReadLine();
                //EXIT – завершение работы
                if (command.ToLower() == ExitCommand)
                {
                    break;
                }
                //HELP – отображение отформатированной справки по всем поддерживаемым командам
                else if (command.ToLower() == HelpCommand)
                {
                    Console.WriteLine(HelpMessage);
                }
                //SHOWSTATUS – отображение отформатированной таблицы загруженности всех ангаров на всех площадках склада
                else if (command.ToLower() == ShowstatusCommand)
                {
                    Console.WriteLine(ColumnHeader);
                    foreach (var site in wm.GetAllSites())
                    {
                        foreach (var hangar in wm.GetAllHangars(site.Id))
                        {
                            Console.WriteLine($"{hangar.SiteId,-ColumnWidth}{hangar.Id,-ColumnWidth}{hangar.Capacity,-ColumnWidth}{hangar.Fullness,-ColumnWidth}");
                        }
                    }
                }
                //STORE <N> – разместить N контейнеров на складе, N – натуральное число
                else if (command.ToLower().StartsWith(StoreCommand + ' '))
                {
                    bool valid = true;
                    ushort N;
                    string[] comArgs = command.Split(' ');
                    if (comArgs.Length == 2 && ushort.TryParse(comArgs[1], out N))
                    {
                        int freePlaces = wm.GetFreePlacesCount();
                        if (freePlaces >= N)
                        {
                            N = PlaceContainers(N, wm);
                            N = PlaceContainers(N, wm, true);
                            Console.WriteLine(ContainersPlacedMessage);
                        }
                        else if (freePlaces == 0)
                        {
                            Console.WriteLine(WarehouseIsFilledMessage);
                        }
                        else
                        {
                            Console.WriteLine(CanNotPlaceContainersMessage);
                        }
                    }
                    else
                    {
                        valid = false;
                    }

                    if (!valid)
                    {
                        Console.WriteLine(InvalidArgumentsMessage);
                    }
                }
                //FREE <N> <H_ID> – выгрузить N контейнеров из ангара H_ID на складе, N – натуральное число
                else if (command.ToLower().StartsWith(FreeCommand + ' '))
                {
                    bool valid = true;
                    ushort N;
                    string[] comArgs = command.Split(' ');
                    if (comArgs.Length == 3 && ushort.TryParse(comArgs[1], out N) && wm.HangarExists(comArgs[2]))
                    {
                        WMDBService.Hangar hangar = wm.GetHangar(comArgs[2]);
                        if (hangar.Fullness < N)
                        {
                            Console.WriteLine(NotEnoughContainersMessage);
                        }
                        else
                        {
                            wm.ModifyHangar(hangar.Id, -N);
                            Console.WriteLine(ContainersTakenAwayMessage);
                        }
                    }
                    else
                    {
                        valid = false;
                    }

                    if (!valid)
                    {
                        Console.WriteLine(InvalidArgumentsMessage);
                    }
                }
                else
                {
                    Console.WriteLine(UnknownCommandMessage);
                    needEnterSuggestion = false;
                }
                Console.WriteLine();
            }
        }
    }
}
