using System;
using System.IO;
using System.Net;
using Ionic.Zip;

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

            Console.WriteLine("Сканирование новых версий для программы автоматического обновления...");
            //Обновление лаунчера
            string url = base_url + "L_Version.txt";
            string save_path = temp_path;
            string name = "L_Version.txt";
            wc.DownloadFile(url, save_path + name);
            string new_l_v = File.ReadAllText(temp_path + "L_Version.txt");
            string local_l_v = File.ReadAllText(appdata_launcher_path + "L_Version.txt");
            if (local_l_v == new_l_v)
            {
                Console.WriteLine("Сканирование завершено. \n Новых версий не обнаружено.");
            }
            else
            {
                System.Diagnostics.Process.Start(@"C:\Program Files\SKProCH Updater\Dont Touch This EXE.exe");
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
                wc.DownloadFile(url2, save_path2 + name2);
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
                Console.WriteLine("Завершено");
                return;
            }
        }
    }
}
