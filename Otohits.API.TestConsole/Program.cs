using Newtonsoft.Json;
using Otohits.API.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Otohits.API.TestConsole
{
    class Program
    {
        /// <summary>
        /// Simple example to get your user info from the API.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            OtohitsRequest.SetCredentials(ConfigurationManager.AppSettings["Otohits:API:Key"], ConfigurationManager.AppSettings["Otohits:API:Secret"]);

            var user = new OtohitsRequest().GetUserInfo();

            Console.WriteLine(JsonConvert.SerializeObject(user, Formatting.Indented));
            Console.WriteLine("Well, that's the end... just press a key and go play with the API!");
            Console.Read();
        }
    }
}
