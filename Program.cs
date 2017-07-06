using System;
using System.IO;
using System.Net;
using Ionic.Zip;
using System.Threading;

namespace SKProCH_Updater
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();
            string base_url = "ftp://updater:thisispassword@31.25.29.138/usb1_1/minecraft/DontTouchThisFolder/";
            string appdata_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appdata_launcher_path = appdata_path + @"\scproch_updater\";
            string temp_path = appdata_launcher_path + @"Temp\";
            string launcher_install_path = @"C:\Program Files\SKProCH Updater\";

            Console.WriteLine("Сканирование новых версий для программы автоматического обновления...");

            //Обновление лаунчера

            string name = "L_Version.txt";
            string url = base_url + name;
            wc.DownloadFile(url, temp_path + name);

            // Закончена структура запроса и сохранения файла номера версии лаунчера с удалённого сервера

            string new_l_v = File.ReadAllText(temp_path + "L_Version.txt");
            string local_l_v = File.ReadAllText(appdata_launcher_path + "L_Version.txt");
            if (local_l_v == new_l_v)
            {
                Console.WriteLine("Сканирование завершено. \n Новых версий не обнаружено.");
            }
            else
            {
                System.Diagnostics.Process.Start(launcher_install_path + @"Dont_Touch_This_EXE.exe");
                return;
            }
            Console.WriteLine("Проверка правильности пути папки Minecraft'a.");
            string Path = File.ReadAllText(appdata_launcher_path + "MCPath.txt");
            if (Directory.Exists(Path))
            { Console.WriteLine("Путь указан правильно."); }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("ВЫ, ВЕРОЯТНО, УКАЗАЛИ НЕПРАВИЛЬНЫЙ ПУТЬ К ПАПКЕ MINECRAFT\n Потому, что такого пути не существует.\nНажмите любую клавишу и откроется инструкция.");
                Console.ReadKey(true);
                System.Diagnostics.Process.Start(@"https://cdn.discordapp.com/attachments/236018668889309185/331019888443260928/unknown.png");
                Console.WriteLine("Выполните необходимые действия и нажмите любую клавишу. Приложение будет закрыто.");
                Console.ReadKey(true);
                return;
            }
            Console.WriteLine("Сканирование новых версий новых версий клиента Minecraft...");
            string url1 = base_url + "M_Version.txt";
            string save_path1 = temp_path;
            string name1 = "M_Version.txt";
            wc.DownloadFile(url1, save_path1 + name1);
            string new_m_v = File.ReadAllText(temp_path + "M_Version.txt");
            string local_m_v = File.ReadAllText(appdata_launcher_path + "M_Version.txt");
            if (local_m_v == new_m_v)
            {
                Console.WriteLine("Сканирование завершено. \n Новых версий не обнаружено.");
                Console.Write("Нажмите любую клавишу для завершения...");
                Console.ReadKey(true);
                return;
            }
            else
            {
                Console.WriteLine("Новые версии обнаружены.\nНачинаем загрузку...\nЭто может продлится долго.");
                string ModsPath;
                ModsPath = Path + @"\Mods";
                Console.WriteLine("Папка вашего клиента - " + ModsPath);
                Directory.Delete(@ModsPath, true);
                Directory.CreateDirectory(@ModsPath);
                string url2 = base_url + "Client.zip";
                string save_path2 = temp_path;
                string name2 = "Client.zip";
                Download(url2, save_path2, name2);
                Console.WriteLine("Распаковываем клиент...");
                ZipFile zf = new ZipFile(temp_path + "Client.zip");
                zf.ExtractAll(ModsPath);
                Console.WriteLine("Завершено");
                Console.WriteLine("Переносим ваши моды...");
                ZipFile NM = new ZipFile(temp_path + "NM.zip");
                NM.AddDirectory(appdata_launcher_path + "Needed Mods");
                NM.Save();
                ZipFile NM2 = new ZipFile(temp_path + "MN.zip");
                NM2.ExtractAll(ModsPath);
                File.Copy(temp_path + "M_Version.txt", appdata_launcher_path + "M_Version.txt", true);
                Console.WriteLine("Завершено");
                Console.Write("Нажмите любую клавишу для завершения...");
                Console.ReadKey(true);
                return;
            }
        }

        static void Download(string url, string path, string name)
        {
            Console.CursorVisible = false;
            int pos = Console.CursorTop;
            long totalBytes;
            int Time = 1;
            bool ccomp = true;
            bool DCompleted = false;
            int TExpired = 0;
            int TExpired1 = 0;
            int TNeed = 0;
            int DSpeed = 1;
            int PER1 = 3;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Загружаем " + name + "...");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("◄——————————————————————————————————————————————————►    % Завершено...");
            var webClient = new WebClient();
            var request = (FtpWebRequest)WebRequest.Create(url);
            request.Method = WebRequestMethods.Ftp.GetFileSize;
            using (var response = request.GetResponse())
            {
                totalBytes = response.ContentLength;
            }
            webClient.DownloadProgressChanged += (s, e) =>
            {
                int Per = (int)(e.BytesReceived * 100 / totalBytes);
                if (ccomp == true)
                {
                    ccomp = false;
                    DSpeed = (int)e.BytesReceived / Time / 1024;
                    int KbitRecieved = (int)e.BytesReceived / 8 / 1024;
                    int KbitTotal = (int)totalBytes / 8 / 1024;
                    long BytesRemaining = totalBytes - e.BytesReceived;
                    int pointwrited = Per / 2;
                    if (TExpired == 60)
                    {
                        TExpired = TExpired - 60;
                        ++TExpired1;
                    }
                    if (PER1 + 2 < Time)
                    {
                        TNeed = (int)e.BytesReceived / Time ;
                        PER1 = Time;
                    }
                    Console.SetCursorPosition(0, pos + 2);
                    Console.Write("Скачано " + KbitRecieved + " Кбайт из " + KbitTotal + " Кбайт. Средняя скорость - " + DSpeed + "КБайт/С");
                    Console.SetCursorPosition(0, pos + 3);
                    Console.Write("Прошло времени - " + TExpired1 + " мин. " + TExpired + " сек. Осталось - " + TNeed + " сек.");
                    if (Per < 25)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    else if (Per < 50)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (Per < 75)
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                    else if (Per < 100)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.SetCursorPosition(1, pos + 1);
                    Console.Write(new string('█', pointwrited));
                    if (pointwrited < 50)
                    {
                        Random rnd = new Random();
                        int value = rnd.Next(0, 13);
                        if (value == 0) { Console.ForegroundColor = ConsoleColor.Blue; }
                        if (value == 1) { Console.ForegroundColor = ConsoleColor.Cyan; }
                        if (value == 2) { Console.ForegroundColor = ConsoleColor.DarkBlue; }
                        if (value == 3) { Console.ForegroundColor = ConsoleColor.DarkCyan; }
                        if (value == 4) { Console.ForegroundColor = ConsoleColor.DarkGray; }
                        if (value == 5) { Console.ForegroundColor = ConsoleColor.DarkGreen; }
                        if (value == 6) { Console.ForegroundColor = ConsoleColor.DarkMagenta; }
                        if (value == 7) { Console.ForegroundColor = ConsoleColor.DarkRed; }
                        if (value == 8) { Console.ForegroundColor = ConsoleColor.DarkYellow; }
                        if (value == 9) { Console.ForegroundColor = ConsoleColor.Green; }
                        if (value == 10) { Console.ForegroundColor = ConsoleColor.Magenta; }
                        if (value == 11) { Console.ForegroundColor = ConsoleColor.Red; }
                        if (value == 12) { Console.ForegroundColor = ConsoleColor.White; }
                        if (value == 13) { Console.ForegroundColor = ConsoleColor.Yellow; }
                        Console.Write(">");
                    }
                    Console.SetCursorPosition(53, pos + 1);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(Per);
                    ccomp = true;
                }

            };
            webClient.DownloadFileCompleted += (s, e) =>
            {
                Thread.Sleep(3000);
                Console.SetCursorPosition(0, pos);
                Console.Write(new string(' ', 320));
                Console.SetCursorPosition(0, pos);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("_-=<-(}[");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Скачивание ");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(name);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(" завершено за " + TExpired1 + "мин. " + TExpired + "сек!");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("]{)->=-_");
                DCompleted = true;

            };
            webClient.DownloadFileAsync(new Uri(url), path + name);
            do
            {
                Thread.Sleep(1000);
                ++Time;
                ++TExpired;
            } while (DCompleted != true);
            Console.CursorVisible = true;
        }
    }
}


