using System;

namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new ApiClient("https://localhost:5001/");
            Console.WriteLine("Get or Schedule here. Or exit");
            var input = Console.ReadLine();

            while (input != "exit")
            {
                var parts = input.Split(' ');
                if (parts.Length < 1)
                {
                    continue;
                }

                try
                {
                    var command = parts[0].ToLower();
                    if (command.Equals("get"))
                    {
                        HandleGet(client, parts);
                    }
                    else if (command.Equals("schedule"))
                    {
                        var municipality = parts[1];

                        var tax = parts[2];
                        string startDate;
                        if (parts.Length < 4)
                        {
                            startDate = "2020-01-01";
                        }
                        else
                        {
                            startDate = parts[3];
                        }

                        string endDate;
                        if (parts.Length < 5)
                        {
                            endDate = "2020-12-31";
                        }
                        else
                        {
                            endDate = parts[3];
                        }

                        var response = client.ScheduleTax(municipality, double.Parse(tax), startDate, endDate);
                        Console.WriteLine("Responded: " + response);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e.Message);
                    Console.Error.WriteLine(e.StackTrace);
                }

                input = Console.ReadLine();

            }
        }

        private static void HandleGet(ApiClient client, string[] parts)
        {
            string date;
            if (parts.Length < 3)
            {
                date = "2020-02-02";
            }
            else
            {
                date = parts[2];
            }

            var response = client.GetTax(parts[1], date);
            Console.WriteLine("Got tax " + response.Tax + " for municipality " + response.Municipality);
        }
    }
}
