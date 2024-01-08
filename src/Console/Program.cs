// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Shubham Gogna">
// Copyright (c) Shubham Gogna
// </copyright>
// -----------------------------------------------------------------------

namespace VerifoneCommander.PriceBookManager.Console
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using VerifoneCommander.PriceBookManager.Core;

    public class Program
    {
        public static void Main(string[] args)
        {
            _ = args ?? throw new NullReferenceException(nameof(args));

            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            MainAsync(password).GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async Task MainAsync(
            string password)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddSimpleConsole(c => c.SingleLine = true));
            var logger = loggerFactory.CreateLogger<Program>();

            try
            {
                using HttpClientHttpRequestSender requestSender = new();

                var credentialsProvider = new SapphireCredentialProvider(
                    requestSender,
                    loggerFactory.CreateLogger<SapphireCredentialProvider>());

                var sapphireClient = new SapphireClient(
                    requestSender,
                    credentialsProvider,
                    loggerFactory.CreateLogger<SapphireClient>());

                credentialsProvider.SetLoginCredentials(
                    hostName: "192.168.31.11",
                    username: "manager",
                    password: password);

                var plus = await sapphireClient.GetPriceLookUpsAsync(default).ConfigureAwait(false);
                logger.LogInformation($"Found {plus.Count} PLUs");

                foreach (var plu in plus)
                {
                    logger.LogInformation("PLU has {UPC} and {description}", plu.Ean13.ToString("D14"), plu.Description);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception");
            }
        }
    }
}
