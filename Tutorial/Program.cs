using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TekuSP.Utilities; //USING
//using static TekuSP.Utilities.SmartConsole.Input; //CAN BE USED FOR EASIER ACCESS
namespace Tutorial
{
    class Program
    {
        static void Main(string[] args)
        {
            SmartConsole.Input.WriteLine("Thank you for using this!"); //Write text + line break
            SmartConsole.Input.WriteLine(ConsoleColor.Red, "RED"); //Write text in color + line break
            SmartConsole.Input.WriteLine(ConsoleColor.Green, "GREEN"); //Write text in color + line break
            SmartConsole.Input.WriteLine(ConsoleColor.Blue, "BLUE"); //Write text in color + line break

            SmartConsole.Input.WriteCentered("CENTERED"); //Write text centered + line break
            SmartConsole.Input.WriteCentered(ConsoleColor.Red, "RED CENTERED"); //Write text centered in color + line break

            DateTime? time = SmartConsole.Input.ReadTime("Please write time: "); //READ TIME with HEADER
            SmartConsole.Input.WriteLine(time.GetValueOrDefault().ToShortTimeString()); //WRITE TIME

            DateTime? date = SmartConsole.Input.ReadDate(DateTime.MinValue, DateTime.MaxValue, "Please write date: "); //READ TIME with HEADER
            SmartConsole.Input.WriteLine(date.GetValueOrDefault().ToShortDateString()); //WRITE DATE

            DateTime? dateTime = SmartConsole.Input.ReadDateTime(DateTime.MinValue, DateTime.MaxValue, "Please write date time: "); //READ DATETIME with HEADER
            SmartConsole.Input.WriteLine(dateTime.GetValueOrDefault()); //WRITE DATETIME

            int? integer = SmartConsole.Input.ReadInt("Please write number from 5 to 20: ", minLenght: 5, maxLenght: 20); //Read int with specified number range
            SmartConsole.Input.WriteLine(integer); //WRITE INT

            string text = SmartConsole.Input.ReadString("Please write text from 5 to 10 lenght: ", minLenght: 5, maxLenght: 10); //Read string with min lenght 5 and max lenght 10
            SmartConsole.Input.WriteLine(text); //WRITE STRING

            SmartConsole.Input.translate.errors = new ErrorsDifferent(); //Replace translate errors class with our class
            integer = SmartConsole.Input.ReadInt("Write number and make mistake: "); //Example how translate works

            SmartConsole.Input.translate.errors = new SmartConsole.Translate.Errors(); //Fix to original translate class
            integer = SmartConsole.Input.ReadInt("Write number and make mistake: "); //Example how translate works

            int result = SmartConsole.Input.Menu(new string[] { "Option 1", "Option 2", "Option 3" }, "Choose an option!", "Please!"); //Menu, first array is for options, then header, then subheader, then colors, result is array index
            switch (result)
            {
                case 0:
                    SmartConsole.Input.WriteLine("Option 1 was selected");
                    break;
                case 1:
                    SmartConsole.Input.WriteLine("Option 2 was selected");
                    break;
                case 2:
                    SmartConsole.Input.WriteLine("Option 3 was selected");
                    break;
            }
        }
        class ErrorsDifferent : SmartConsole.Translate.Errors //INHERIT
        {
            public override string WrongInputNumber => "BLAH BLAH BLAH"; //OVERRIDE TARGET STRINGS
        }

    }
}
