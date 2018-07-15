using System;

namespace Warehouse.ConsoleClient
{
    class Program
    {
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
                InvalidArgumentsMessage = "Invalid argumets! Try again.";
            string HelpMessage = "Commands' info:" + Environment.NewLine +
                $"{HelpCommand,-CommandsLength}Shows commands' info." + Environment.NewLine +
                $"{ExitCommand,-CommandsLength}Closes app." + Environment.NewLine +
                $"{ShowstatusCommand,-CommandsLength}Showes table with current statuses of all hangars." + Environment.NewLine +
                $"{StoreCommand + " <N>",-CommandsLength}Places N containers in warehouse. N is natural number." + Environment.NewLine +
                $"{FreeCommand + " <N> <H_ID>",-CommandsLength}Takes away N containers from hangar H_ID. N is natural number.";
            #endregion

            string command = "";
            bool needEnterSuggestion = true;
            bool success;
            string exceptionString;

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
                    using (DataService.DataServiceClient wm = new DataService.DataServiceClient())
                    {
                        Console.WriteLine(wm.ShowStatus(out success, out exceptionString));
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
                        using (DataService.DataServiceClient wm = new DataService.DataServiceClient())
                        {
                            Console.WriteLine(wm.PlaceContainers(N, out success, out exceptionString));
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
                    if (comArgs.Length == 3 && ushort.TryParse(comArgs[1], out N))
                    {
                        using (DataService.DataServiceClient wm = new DataService.DataServiceClient())
                        {
                            Console.WriteLine(wm.FreeContainers(N, comArgs[2], out success, out exceptionString));
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
