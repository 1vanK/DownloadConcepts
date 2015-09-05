// Утилита скачивает все концепты с http://www.igorstshirts.com/blog/
// Это хранилище для:
// http://conceptrobots.blogspot.com
// http://conceptships.blogspot.com
// http://conceptvehicles.blogspot.com
// http://conceptaliens.blogspot.com
// http://conceptguns.blogspot.com
// http://conceptfish.blogspot.com
// http://concepttanks.blogspot.com
// Для статьи http://ru.blender.wikia.com/wiki/Техника


using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;


static class Program
{
    static WebClient Клиент = new WebClient();

    static string СоздатьДиректорию(string ссылка)
    {
        // Пример ссыки: http://www.igorstshirts.com/blog/concepttanks/
        ссылка = ссылка.Replace("http://", "");
        int индекс = ссылка.IndexOf('/');
        string путь = "Result" + ссылка.Substring(индекс);
        Directory.CreateDirectory(путь);
        return путь;
    }
    
    static string ОпределитьРасширение(string имяФайла)
    {
        int индекс = имяФайла.LastIndexOf('.');
        if (индекс == -1)
            return "";
        return имяФайла.Substring(индекс + 1);
    }

    static void РекурсивнаяЗагрузка(string ссылка)
    {
        Console.WriteLine(ссылка);
        string путь = СоздатьДиректорию(ссылка);
        string страница = Клиент.DownloadString(ссылка);
        Regex шаблон = new Regex("<a href=\"(.+)\">");
        var соответствия = шаблон.Matches(страница);
        foreach (Match соответствие in соответствия)
        {
            string элемент = соответствие.Groups[1].Value;
            if (элемент.StartsWith("/")) // ссылка возврата из директории
                continue;
            if (элемент[элемент.Length - 1] == '/') // ссылка на поддиректорию
            {
                РекурсивнаяЗагрузка(ссылка + элемент);
                continue;
            }
            string расширение = ОпределитьРасширение(элемент);
            if (расширение == "db" || расширение == "lnk" || расширение == "php" ||
                расширение == "mp3" || расширение == "mov" || расширение == "" ||
                расширение == "xml" || расширение == "fla") // пропускаем всякий мусор
                continue;
            Console.WriteLine(ссылка + элемент);
            Клиент.DownloadFile(ссылка + элемент, путь + элемент);
        }
    }

    static void Main(string[] args)
    {
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/conceptaliens/");
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/conceptfish/");
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/conceptguns/");
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/conceptrobots/");
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/conceptships/");
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/concepttanks/");
        РекурсивнаяЗагрузка("http://www.igorstshirts.com/blog/conceptvehicles/");
        Console.WriteLine("Нажмите ENTER...");
        Console.ReadLine();
    }
}
