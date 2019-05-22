using System;
using System.Text.RegularExpressions;

namespace TekuSP.Utilities
{
    public static class SmartConsole
    {
        public static class Input
        {
            public static void WriteLine(object objectToWrite) => Console.WriteLine(objectToWrite.ToString());
            public static void WriteLine(ConsoleColor color, object objectToWrite)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                WriteLine(objectToWrite);
                Console.ForegroundColor = color;
            }
            public static void Write(object objectToWrite) => Console.Write(objectToWrite.ToString());
            public static void Write(ConsoleColor color, object objectToWrite)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Write(objectToWrite);
                Console.ForegroundColor = color;
            }
            public static void WriteCentered(object objectToWrite) => Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (objectToWrite.ToString().Length / 2)) + "}", objectToWrite.ToString()));
            public static void WriteCentered(ConsoleColor color, object objectToWrite)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                WriteCentered(objectToWrite);
                Console.ForegroundColor = color;
            }

            public static string ReadString(string header = "", ConsoleColor headerColor = ConsoleColor.White,int minLenght = 0, int maxLenght = int.MaxValue)
            {
                Write(headerColor,header);
                string read = Console.ReadLine();
                if (read.Length < minLenght || read.Length > maxLenght)
                {
                    if (Menu(new string[] { Translate.Messages.Yes, Translate.Messages.No }, Translate.Errors.WrongInputString, Translate.Messages.InputTextRule(minLenght, maxLenght)) == 0)
                        return ReadString(header, headerColor,minLenght, maxLenght);
                    else
                        return null;
                }
                return read;
            }
            public static int? ReadInt(string header = "", ConsoleColor headerColor = ConsoleColor.White,int minLenght = 0, int maxLenght = int.MaxValue)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!int.TryParse(read, out int result) || result < minLenght || result > maxLenght)
                {
                    if (Menu(new string[] { Translate.Messages.Yes, Translate.Messages.No }, Translate.Errors.WrongInputNumber, Translate.Messages.InputNumberRule(minLenght, maxLenght)) == 0)
                        return ReadInt(header,headerColor, minLenght, maxLenght);
                    else
                        return null;
                }
                return result;
            }
            public static DateTime? ReadTime(string header = "", ConsoleColor headerColor = ConsoleColor.White)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!new Regex(@"^(?i)(0?[1-9]|1[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?( AM| PM)?$").IsMatch(read))
                {
                    if (Menu(new string[] { Translate.Messages.Yes, Translate.Messages.No }, Translate.Errors.WrongInputTime) == 0)
                        return ReadTime(header, headerColor);
                    else
                        return null;
                }
                var split = read.Split(':');
                int hour = int.Parse(split[0]);
                int minute = int.Parse(split[1].Replace("AM", "").Replace("PM", "").Trim());
                int second = 0;
                if (split.Length == 3)
                    second = int.Parse(split[2].Replace("AM", "").Replace("PM", "").Trim());
                if (read.Contains("PM"))
                    hour += 12;
                if (hour > 24)
                {
                    if (Menu(new string[] { Translate.Messages.Yes, Translate.Messages.No }, Translate.Errors.WrongInputTime) == 0)
                        return ReadTime(header, headerColor);
                    else
                        return null;
                }
                return new DateTime(1, 1, 1, hour, minute, second);
            }
            public static DateTime? ReadDate(DateTime min,DateTime max,string header = "", ConsoleColor headerColor = ConsoleColor.White)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!DateTime.TryParse(read, out DateTime result) || result < min || result > max)
                {
                    if (Menu(new string[] { Translate.Messages.Yes, Translate.Messages.No }, Translate.Errors.WrongInputDate, Translate.Messages.InputDateRule(min, max)) == 0)
                        return ReadDate(min, max,header,headerColor);
                    else
                        return null;
                }
                return result;
            }
            public static DateTime? ReadDateTime(DateTime min, DateTime max, string header = "", ConsoleColor headerColor = ConsoleColor.White)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!DateTime.TryParse(read, out DateTime result) || result < min || result > max)
                {
                    if (Menu(new string[] { Translate.Messages.Yes, Translate.Messages.No }, Translate.Errors.WrongInputDateTime, Translate.Messages.InputDateTimeRule(min, max)) == 0)
                        return ReadDate(min, max, header, headerColor);
                    else
                        return null;
                }
                return result;
            }
            public static int Menu(string[] array, string header = null, string subheader = null, ConsoleColor textColor = ConsoleColor.White, ConsoleColor headerColor = ConsoleColor.White, ConsoleColor subHeaderColor = ConsoleColor.White, ConsoleColor bracketColor = ConsoleColor.White)
            {
                ConsoleColor startingColor = System.Console.ForegroundColor;
                int choice = 0;
                ConsoleKeyInfo input;
                while (true)
                {
                    Console.Clear();
                    if (header != null)
                        WriteLine(headerColor, header);
                    if (subheader != null && header != null)
                        WriteLine(subHeaderColor, subheader);
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (i == choice)
                        {
                            Write(bracketColor, "[ ");
                            Write(textColor, array[i]);
                            WriteLine(bracketColor, " ]");
                        }
                        else
                            WriteLine(textColor, string.Format("  {0}  ", array[i]));
                    }
                    input = Console.ReadKey();
                    switch (input.Key)
                    {
                        case ConsoleKey.UpArrow:
                            choice--;
                            if (choice < 0) { choice = array.Length - 1; }
                            break;

                        case ConsoleKey.DownArrow:
                            choice++;
                            if (choice > array.Length - 1) { choice = 0; }
                            break;

                        case ConsoleKey.Enter:
                            Console.ForegroundColor = startingColor;
                            Console.Clear();
                            return choice;
                    }
                }
            }
        }
        public static class Translate
        {
            public static class Errors
            {
                public static string WrongInputNumber => "You have inputed invalid number. Do you want try again?";
                public static string WrongInputString => "You have inputed invalid text. Do you want try again?";
                public static string WrongInputDateTime => "You have inputed invalid date and time. Do you want try again?";
                public static string WrongInputDate => "You have inputed invalid date. Do you want try again?";
                public static string WrongInputTime => "You have inputed invalid time. Do you want try again?";
            }
            public static class Messages
            {
                public static string Yes => "Yes";
                public static string No => "No";
                public static string InputTextRule(int min, int max) => string.Format("Input has to be minimally {0} long and maximally {1} long.", min, max);
                public static string InputNumberRule(int min, int max) => string.Format("Input has to be number from {0} to {1}.", min, max);
                public static string InputDateRule(DateTime min, DateTime max) => string.Format("Input has to be Date from {0} to {1}.", min.ToShortDateString(), max.ToShortDateString());
                public static string InputDateTimeRule(DateTime min, DateTime max) => string.Format("Input has to be Date and Time from {0} to {1}.", min, max);

            }
        }
    }
}
