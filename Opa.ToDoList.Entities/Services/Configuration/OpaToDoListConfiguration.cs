
namespace Opa.ToDoList.Entities.Services.Configuration
{
    using System;
    using Microsoft.Extensions.Configuration;

    public class OpaToDoListConfiguration
    {       
        private static OpaToDoListConfiguration instance;
        
        private IConfiguration configuration;
                
        private OpaToDoListConfiguration()
        {
            this.LoadConfiguration();
        }
        
        public static OpaToDoListConfiguration Instance =>
            instance ?? (instance = new OpaToDoListConfiguration());

        /// <summary>
        /// Gets the Integrador cs.
        /// </summary>
        public string Connection =>
            this.configuration.GetConnectionString(this.GetAppSettings<string>("Connection"));
        
        public T GetAppSettings<T>(string key)
        {
            return this.configuration.GetSection("AppSettings").GetValue<T>(key);
        }

        /// <summary>
        /// The load configuration.
        /// </summary>
        private void LoadConfiguration()
        {
            this.configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appSettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
                .Build();
        }
    }
}
