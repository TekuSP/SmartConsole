using System;
using System.Text.RegularExpressions;

namespace TekuSP.Utilities
{
    public static class SmartConsole
    {
        public static class Input
        {
            public static Translate translate = new Translate();
            /// <summary>
            /// Writes with line break to terminal/console output
            /// </summary>
            /// <param name="objectToWrite">Target object to write</param>
            public static void WriteLine(object objectToWrite) => Console.WriteLine(objectToWrite.ToString());
            /// <summary>
            /// Writes with line break to terminal/console output
            /// </summary>
            /// <param name="color">Color to text be written in</param>
            /// <param name="objectToWrite">Target object to write</param>
            public static void WriteLine(ConsoleColor color, object objectToWrite)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                WriteLine(objectToWrite);
                Console.ForegroundColor = color;
            }
            /// <summary>
            /// Writes to terminal/console output
            /// </summary>
            /// <param name="objectToWrite">Target object to write</param>
            public static void Write(object objectToWrite) => Console.Write(objectToWrite.ToString());
            /// <summary>
            /// Writes to terminal/console output
            /// </summary>
            /// <param name="color">Color to text be written in</param>
            /// <param name="objectToWrite">Target object to write</param>
            public static void Write(ConsoleColor color, object objectToWrite)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                Write(objectToWrite);
                Console.ForegroundColor = color;
            }
            /// <summary>
            /// Writes centered text on screen with line break to terminal/console output
            /// </summary>
            /// <param name="objectToWrite">Target object to write</param>
            public static void WriteCentered(object objectToWrite) => Console.WriteLine(string.Format("{0," + ((Console.WindowWidth / 2) + (objectToWrite.ToString().Length / 2)) + "}", objectToWrite.ToString()));
            /// <summary>
            /// Writes centered text on screen with line break to terminal/console output
            /// </summary>
            /// <param name="color">Color to text be written in</param>
            /// <param name="objectToWrite">Target object to write</param>
            public static void WriteCentered(ConsoleColor color, object objectToWrite)
            {
                ConsoleColor oldColor = Console.ForegroundColor;
                Console.ForegroundColor = color;
                WriteCentered(objectToWrite);
                Console.ForegroundColor = color;
            }

            /// <summary>
            /// Reads line from terminal/console input and parses string with minimal lenght and maximal lenght with specified header and checks on invalid input, asking user to try again.
            /// </summary>
            /// <param name="header">Header text before reading input</param>
            /// <param name="headerColor">Color to header be written in</param>
            /// <param name="minLenght">Minimal lenght of target string</param>
            /// <param name="maxLenght">Maximal lenght of target string</param>
            /// <returns>Returns null if user cancels operation or string with read text</returns>
            public static string ReadString(string header = "", ConsoleColor headerColor = ConsoleColor.White, int minLenght = 0, int maxLenght = int.MaxValue)
            {
                Write(headerColor, header);
                string read = Console.ReadLine();
                if (read.Length < minLenght || read.Length > maxLenght)
                {
                    if (Menu(new string[] { translate.messages.Yes, translate.messages.No }, translate.errors.WrongInputString, translate.messages.InputTextRule(minLenght, maxLenght)) == 0)
                        return ReadString(header, headerColor, minLenght, maxLenght);
                    else
                        return null;
                }
                return read;
            }
            /// <summary>
            /// Read line from terminal/console input and parses int with minimal size and maximal size with specified header and checks on invalid input, asking user to try again.
            /// </summary>
            /// <param name="header">Header text before reading input</param>
            /// <param name="headerColor">Color to header be written in</param>
            /// <param name="minLenght">Minimal number to be accepted</param>
            /// <param name="maxLenght">Maximal number to be accepted</param>
            /// <returns>Returns null if user cancels operation or int with target number</returns>
            public static int? ReadInt(string header = "", ConsoleColor headerColor = ConsoleColor.White, int minLenght = 0, int maxLenght = int.MaxValue)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!int.TryParse(read, out int result) || result < minLenght || result > maxLenght)
                {
                    if (Menu(new string[] { translate.messages.Yes, translate.messages.No }, translate.errors.WrongInputNumber, translate.messages.InputNumberRule(minLenght, maxLenght)) == 0)
                        return ReadInt(header, headerColor, minLenght, maxLenght);
                    else
                        return null;
                }
                return result;
            }
            /// <summary>
            /// Read line from terminal/console input and parses DateTime with only Time with specified header and checks on invalid input, asking user to try again.
            /// </summary>
            /// <param name="header">Header text before reading input</param>
            /// <param name="headerColor">Color to header be written in</param>
            /// <returns>Returns null if user cancels operation or DateTime with Time filled in</returns>
            public static DateTime? ReadTime(string header = "", ConsoleColor headerColor = ConsoleColor.White)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!new Regex(@"^(?i)(0?[1-9]|1[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?( AM| PM)?$").IsMatch(read))
                {
                    if (Menu(new string[] { translate.messages.Yes, translate.messages.No }, translate.errors.WrongInputTime) == 0)
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
                    if (Menu(new string[] { translate.messages.Yes, translate.messages.No }, translate.errors.WrongInputTime) == 0)
                        return ReadTime(header, headerColor);
                    else
                        return null;
                }
                return new DateTime(1, 1, 1, hour, minute, second);
            }
            /// <summary>
            /// Read line from terminal/console input and parses DateTime with only Date with minimal accepted Date and maximal accepted Date specified header and checks on invalid input, asking user to try again.
            /// </summary>
            /// <param name="min">Minimal date to be accepted</param>
            /// <param name="max">Maximal date to be accepted</param>
            /// <param name="header">Header text before reading input</param>
            /// <param name="headerColor">Color to header be written in</param>
            /// <returns>Returns null if user cancels operation or DateTime with Date filled in</returns>
            public static DateTime? ReadDate(DateTime min, DateTime max, string header = "", ConsoleColor headerColor = ConsoleColor.White)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!DateTime.TryParse(read, out DateTime result) || result < min || result > max)
                {
                    if (Menu(new string[] { translate.messages.Yes, translate.messages.No }, translate.errors.WrongInputDate, translate.messages.InputDateRule(min, max)) == 0)
                        return ReadDate(min, max, header, headerColor);
                    else
                        return null;
                }
                return result;
            }
            /// <summary>
            /// Read line from terminal/console input and parses DateTime with minimal accepted Date and maximal accepted Date specified header and checks on invalid input, asking user to try again.
            /// </summary>
            /// <param name="min">Minimal DateTime to be accepted</param>
            /// <param name="max">Maximal DateTime to be accepted</param>
            /// <param name="header">Header text before reading input</param>
            /// <param name="headerColor">Color to header be written in</param>
            /// <returns>Returns null if user cancels operation or DateTime</returns>
            public static DateTime? ReadDateTime(DateTime min, DateTime max, string header = "", ConsoleColor headerColor = ConsoleColor.White)
            {
                Write(headerColor, header);
                string read = ReadString();
                if (!DateTime.TryParse(read, out DateTime result) || result < min || result > max)
                {
                    if (Menu(new string[] { translate.messages.Yes, translate.messages.No }, translate.errors.WrongInputDateTime, translate.messages.InputDateTimeRule(min, max)) == 0)
                        return ReadDate(min, max, header, headerColor);
                    else
                        return null;
                }
                return result;
            }
            /// <summary>
            /// Creates Console Menu
            /// </summary>
            /// <param name="array">Menu options</param>
            /// <param name="header">Title of menu</param>
            /// <param name="subheader">Subtitle of menu</param>
            /// <param name="centered">Should be menu centered on screen?</param>
            /// <param name="useFixedBrackets">If brackets should be fixed on largest object, or different on every item</param>
            /// <param name="textColor">Color of menu options</param>
            /// <param name="headerColor">Color of title</param>
            /// <param name="subHeaderColor">Color of subtitle</param>
            /// <param name="bracketColor">Color of selected bracket</param>
            /// <returns>Returns index of which item in array was selected</returns>
            public static int Menu(string[] array, string header = null, string subheader = null, string footer = null, bool centered = false, ConsoleColor textColor = ConsoleColor.White, ConsoleColor headerColor = ConsoleColor.White, ConsoleColor subHeaderColor = ConsoleColor.White, ConsoleColor footerColor = ConsoleColor.White, ConsoleColor bracketColor = ConsoleColor.White)
            {
                Console.CursorVisible = false;
                ConsoleColor startingColor = System.Console.ForegroundColor;
                int choice = 0;
                ConsoleKeyInfo input;
                int largestItem = 0;
                while (true)
                {
                    Console.Clear();
                    if (header != null)
                        if (centered)
                            WriteCentered(headerColor, header);
                        else
                            WriteLine(headerColor, header);
                    if (subheader != null && header != null)
                        if (centered)
                            WriteCentered(subHeaderColor, subheader);
                        else
                            WriteLine(subHeaderColor, subheader);
                    if (header != null)
                    {
                        int length = header.Length;
                        bool hdr = length < subheader?.Length;
                        if (hdr)
                            length = subheader.Length;
                        string buff = "";
                        for (int i = 0; i < length; i++)
                            buff += "-";
                        if (centered)
                            WriteCentered(hdr ? headerColor : subHeaderColor, buff);
                        else
                            WriteLine(hdr ? headerColor : subHeaderColor, buff);
                    }
                    for (int i = 0; i < array.Length; i++)
                    {
                        if (i == choice)
                        {
                            if (centered)
                            {
                                double times = ((Console.WindowWidth)/2 + ($"[ {array[i]} ]".ToString().Length / 2) - ($"[ {array[i]} ]".ToString().Length));
                                for (int y = 0; y < times; y++)
                                    Write(" ");
                            }
                            Write(bracketColor, "[ ");
                            Write(textColor, array[i]);
                            WriteLine(bracketColor, " ]");
                        }
                        else
                            if (centered)
                            WriteCentered(textColor, array[i]);
                        else
                            WriteLine(textColor, string.Format("  {0}  ", array[i]));
                    }
                    if (header != null)
                    {
                        int length = header.Length;
                        bool hdr = length < subheader?.Length;
                        if (hdr)
                            length = subheader.Length;
                        string buff = "";
                        for (int i = 0; i < length; i++)
                            buff += "-";
                        if (centered)
                            WriteCentered(hdr ? headerColor : subHeaderColor, buff);
                        else
                            WriteLine(hdr ? headerColor : subHeaderColor, buff);
                    }
                    if (footer != null) 
                    {
                        if (centered)
                            WriteCentered(footerColor, footer);
                        else
                            Write(footerColor, footer);
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
                            Console.CursorVisible = true;
                            return choice;
                    }
                }
            }
            /// <summary>
            /// Password input
            /// </summary>
            /// <param name="visible">If should be * or spaces</param>
            /// <returns>Password</returns>
            public static string PasswordInput(bool visible)
            {
                int left = System.Console.CursorLeft;
                int top = System.Console.CursorTop;
                string password = "";
                System.ConsoleKeyInfo input;
                while (true)
                {
                    input = System.Console.ReadKey();

                    if (input.Key != System.ConsoleKey.Enter)
                    {
                        if (input.Key != System.ConsoleKey.Backspace)
                        {
                            password += input.KeyChar;
                            System.Console.SetCursorPosition(left, top);
                            for (int x = 0; x < password.Length; x++)
                            {
                                if (visible)
                                    System.Console.Write("*");
                                else
                                    System.Console.Write(" ");
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(password))
                            {
                                password = password.Substring(0, password.Length - 1);
                                System.Console.SetCursorPosition(left + password.Length, top);
                                System.Console.Write(" ");
                                System.Console.SetCursorPosition(left, top);
                                for (int x = 0; x < password.Length; x++)
                                {
                                    if (visible)
                                        System.Console.Write("*");
                                    else
                                        System.Console.Write(" ");
                                }
                            }
                            else
                            {
                                System.Console.SetCursorPosition(left, top);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < password.Length; i++)
                        {
                            if (visible)
                                System.Console.Write("*");
                            else
                                System.Console.Write(" ");
                        }
                        System.Console.WriteLine();
                        return password;
                    }
                }
            }
        }
        /// <summary>
        /// This class is used to translate every message written by SmartConsole. You can replace these messages by inheritng subclasses and then overriding to have different messages.
        /// </summary>
        public class Translate
        {
            public Errors errors;
            public Messages messages;
            public Translate()
            {
                errors = new Errors();
                messages = new Messages();
            }
            public class Errors
            {
                public virtual string WrongInputNumber => "You have inputed invalid number. Do you want try again?";
                public virtual string WrongInputString => "You have inputed invalid text. Do you want try again?";
                public virtual string WrongInputDateTime => "You have inputed invalid date and time. Do you want try again?";
                public virtual string WrongInputDate => "You have inputed invalid date. Do you want try again?";
                public virtual string WrongInputTime => "You have inputed invalid time. Do you want try again?";
            }
            public class Messages
            {
                public virtual string Yes => "Yes";
                public virtual string No => "No";
                public virtual string InputTextRule(int min, int max) => string.Format("Input has to be minimally {0} long and maximally {1} long.", min, max);
                public virtual string InputNumberRule(int min, int max) => string.Format("Input has to be number from {0} to {1}.", min, max);
                public virtual string InputDateRule(DateTime min, DateTime max) => string.Format("Input has to be Date from {0} to {1}.", min.ToShortDateString(), max.ToShortDateString());
                public virtual string InputDateTimeRule(DateTime min, DateTime max) => string.Format("Input has to be Date and Time from {0} to {1}.", min, max);

            }
        }
    }
}
