using System;
using System.IO;

public class Program
{
    public static int Main(string[] args)
    {
        string operation;
        string input;
        string output;

        if (args.Length > 0)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Too few arguments.");
                return -1;
            }

            switch (args[0])
            {
                case "encode":
                case "decode":
                    break;
                default:
                    Console.WriteLine($"{args[0]} is not a valid operation.");
                    return -2;
            }

            operation = args[0];
            input = args[1];
            output = args[2];
        }
        else
        {
            Console.Write("Do you wish to encode (e) or decode (d) a file?: ");
            char option;
            do
            {
                try
                {
                    option = char.ToUpper(Convert.ToChar(Console.ReadLine()));
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid option!");
                    option = default;
                }
            }
            while (option != 'E' && option != 'D');

            operation = option == 'E' ? "encode" : "decode";

            Console.Write($"Enter the path to the file you wish to {operation}: ");
            input = Console.ReadLine();
            Console.Write($"Enter the path to where you want to save the {operation}d file: ");
            output = Console.ReadLine();
        }

        if (operation == "encode")
        {
            ImageMessage.Encode(File.ReadAllText(input), output);
        }
        else
        {
            File.WriteAllText(output, ImageMessage.Decode(input));
        }

        Console.WriteLine("Operation completed!");
        Console.ReadKey();
        return 0;
    }
}
