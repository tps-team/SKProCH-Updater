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
            string appdata_launcher_path = appdata_path + @"\SKProCH Lab\MC Updater\";
            string temp_path = appdata_launcher_path + @"Temp\";
            string launcher_install_path = @"C:\Program Files\SKProCH Lab\MC Updater\";

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
                RemoveTemp();
            }
            else
            {
                System.Diagnostics.Process.Start(launcher_install_path + @"Dont Touch This EXE.exe");
                return;
            }
            Console.WriteLine("Проверка правильности пути папки Minecraft'a.");
            string Path = File.ReadAllText(appdata_launcher_path + "MCPath.txt");
            if (Path != null)
            {
                if (Directory.Exists(Path))
                { Console.WriteLine("Путь указан правильно."); }
                else
                {
                    if (File.Exists(appdata_launcher_path + "MCPath.granted"))
                    {
                        Console.WriteLine("Возможно путь указан неправильно, однако вы подтвердили его. Установка продолжается!");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("ВЫ, ВЕРОЯТНО, УКАЗАЛИ НЕПРАВИЛЬНЫЙ ПУТЬ К ПАПКЕ MINECRAFT\n Потому, что такого пути не существует(Либо в нем есть русские символы).");
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Нажмите N для того, что бы прочитать инструкцию еще раз и ввести новый путь.\n Нажмите Y, что бы подтвердить, что все правильно.\n В любом случае вы можете изменить путь самостоятельно.");
                        int CPos1 = Console.CursorTop;
                        var x = Console.ReadKey();
                        Console.SetCursorPosition(0, CPos1);
                        switch (Convert.ToString(x.KeyChar))
                        {
                            case "y":
                            case "н":
                                File.Create(appdata_launcher_path + "MCPath.granted");
                                break;
                            case "n":
                            case "т":
                                Console.ForegroundColor = ConsoleColor.Red;
                                string newlv = File.ReadAllText(appdata_launcher_path + @"Temp\ForgeVErsion.txt");
                                Console.WriteLine("Для того, что бы правильно подготовить рабочую папку Minecraft создайте новый модпак в Curse(Twitch) или MultiMC. Установите Forge");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine(newlv);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("Далее зайдите в папку лаунчера, в Modpacks (Instances), в в папку созданного модпака, там, где находятся директории Mods и Config.");
                                Console.WriteLine("Скопируйте адрес данной папки... Нажмите ПКМ на название консоли, выберите <Изменить> и Вставить.");
                                Console.CursorVisible = true;
                                Console.ForegroundColor = ConsoleColor.Red;
                                string MCPath = Console.ReadLine();
                                File.WriteAllText(appdata_launcher_path + "MCPath.txt", MCPath);
                                File.Delete(appdata_launcher_path + @"Temp\ForgeVersion.txt");
                                Console.WriteLine("Файл с путем к папке Minecraft'a находится тут: " + appdata_launcher_path + "MCPath.txt");
                                Console.WriteLine("Потом вы можете вручную изменить путь, открыв данный файл.");
                                Console.ReadKey(true);
                                break;
                        }
                        
                    }
                }
            }
            else
            {
                wc.DownloadFile(base_url + "ForgeVersion.txt", appdata_launcher_path + @"Temp\ForgeVersion.txt");
                Console.ForegroundColor = ConsoleColor.Red;
                string newlv = File.ReadAllText(appdata_launcher_path + @"Temp\ForgeVersion.txt");
                Console.WriteLine("Для того, что бы правильно подготовить рабочую папку Minecraft создайте новый модпак в Curse(Twitch) или MultiMC. Установите Forge");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(newlv);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Далее зайдите в папку лаунчера, в Modpacks (Instances), в в папку созданного модпака, там, где находятся директории Mods и Config.");
                Console.WriteLine("Скопируйте адрес данной папки... Нажмите ПКМ на название консоли, выберите <Изменить> и Вставить.");
                Console.CursorVisible = true;
                Console.ForegroundColor = ConsoleColor.Red;
                string MCPath = Console.ReadLine();
                File.WriteAllText(appdata_launcher_path + "MCPath.txt", MCPath);
                File.Delete(appdata_launcher_path + @"Temp\ForgeVersion.txt");
                Console.WriteLine("Файл с путем к папке Minecraft'a находится тут: " + appdata_launcher_path + "MCPath.txt");
                Console.WriteLine("Потом вы можете вручную изменить путь, открыв данный файл.");
                Console.ReadKey(true);
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
                RemoveTemp();
                Console.ReadKey(true);
                return;
            }
            else
            {
                Console.WriteLine("Новые версии обнаружены.\nНачинаем загрузку...\nЭто может продлится долго.");
                string ModsPath;
                ModsPath = Path + @"\Mods";
                Console.WriteLine("Папка вашего клиента - " + Path);
                Directory.Delete(@ModsPath, true);
                Directory.CreateDirectory(@ModsPath);
                Directory.Delete(Path + @"\Config", true);
                Directory.CreateDirectory(Path + @"\Config");
                string url2 = base_url + "Client.zip";
                string save_path2 = temp_path;
                string name2 = "Client.zip";
                Download(url2, save_path2, name2);
                Console.WriteLine("Распаковываем клиент...");
                ZipFile zf = new ZipFile(temp_path + "Client.zip");
                zf.ExtractAll(Path);
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
                RemoveTemp();
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
                        TNeed = (int)BytesRemaining / (int)e.BytesReceived * Time;
                        PER1 = Time;
                    }
                    int pointwritedE = pointwrited;
                    Console.SetCursorPosition(0, pos);
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Загружаем " + name + "...");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 1);
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("◄");
                    if (Per < 30)
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                    else if (Per < 66)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else if (Per < 90)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
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
                        --pointwritedE;
                    }
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(new string('—', 50 - pointwritedE));
                    Console.Write("► ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write(Per + "% Завершено!");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 2);
                    Console.Write("Скачано " + KbitRecieved + " Кбайт из " + KbitTotal + " Кбайт. Средняя скорость - " + DSpeed + "КБайт/С");
                    Console.Write(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 3);
                    Console.Write("Прошло времени - " + TExpired1 + " мин. " + TExpired + " сек. Осталось - " + TNeed + " сек.");
                    Console.WriteLine(new string(' ', Console.WindowWidth - Console.CursorLeft));
                    Console.SetCursorPosition(0, pos + 4);
                    Console.WriteLine(new string(' ', 400));
                    ccomp = true;
                }

            };
            webClient.DownloadFileCompleted += (s, e) =>
            {
                Thread.Sleep(3000);
                Console.SetCursorPosition(0, pos);
                Console.Write(new string(' ', 800));
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
        static void RemoveTemp ()
        {
            string appdata_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appdata_launcher_path = appdata_path + @"\SKProCH Lab\MC Updater\";
            string temp_path = appdata_launcher_path + @"Temp\";
            Directory.Delete(temp_path, true);
            Directory.CreateDirectory(temp_path);
        }
    }
}