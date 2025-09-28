using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using perla_metro_ticketService.Models;

namespace perla_metro_ticketService.Data
{
    /// <summary>
    /// Contexto de base de datos para MongoDB.
    /// Administra la conexión y da acceso a las colecciones de la base de datos.
    /// </summary>
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        /// <summary>
        /// Inicializar el contexto de MongoDB usando la configuración provista.
        /// </summary>
        /// <param name="options">Opciones de configuración para la conexión a MongoDB.</param>
        public MongoDbContext(IOptions<MongoDbSettings> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        /// <summary>
        /// Representar la colección de tickets dentro de la base de datos MongoDB.
        /// </summary>
        public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("Tickets");
    }

    /// <summary>
    /// Clase de configuración para los parámetros de conexión a MongoDB.
    /// Se utiliza junto con IOptions para inyectar configuración desde appsettings.json.
    /// </summary>
    public class MongoDbSettings
    {
         /// <summary>
        /// Cadena de conexión a MongoDB.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Nombre de la base de datos en MongoDB.
        /// </summary>
        public string DatabaseName { get; set; } = string.Empty;
    }
}