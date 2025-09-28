using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using perla_metro_ticketService.Data;
using perla_metro_ticketService.Models;

namespace perla_metro_ticketService.Repositories
{
    /// <summary>
    /// Repositorio que gestiona los tickets en base de datos MongoDB
    /// </summary>
    public class TicketRepository
    {
        private readonly MongoDbContext _context;

        /// <summary>
        /// Inicializar el repositorio con el contexto de MongoDB.
        /// </summary>
        /// <param name="context">Contexto de base de datos MongoDB.</param>
        public TicketRepository(MongoDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtener todos los tickets activos del sistema.
        /// </summary>
        /// <returns>Lista de tickets activos.</returns>
        public async Task<List<Ticket>> GetAllAsync() =>
            await _context.Tickets.Find(t => t.IsActive).ToListAsync();

        /// <summary>
        /// Obtenre un ticket activo por su identificador.
        /// </summary>
        /// <param name="id">Identificador único del ticket.</param>
        /// <returns>Ticket encontrado o null si no existe.</returns>
        public async Task<Ticket?> GetByIdAsync(string id) =>
            await _context.Tickets.Find(t => t.TicketId == id && t.IsActive).FirstOrDefaultAsync();

        
        /// <summary>
        /// Busca un ticket activo por pasajero y fecha de emisión.
        /// </summary>
        /// <param name="passengerId">Identificador del pasajero.</param>
        /// <param name="date">Fecha de emisión.</param>
        /// <returns>Ticket encontrado o null si no existe.</returns>
        public async Task<Ticket?> GetByPassengerAndDateAsync(string passengerId, DateTime date) =>
            await _context.Tickets
                .Find(t => t.PassengerId == passengerId && t.EmissionDate.Date == date.Date && t.IsActive)
                .FirstOrDefaultAsync();

        /// <summary>
        /// Crear un nuevo ticket en la base de datos.
        /// </summary>
        /// <param name="ticket">Ticket a crear.</param>
        public async Task CreateAsync(Ticket ticket) =>
            await _context.Tickets.InsertOneAsync(ticket);

        /// <summary>
        /// Actualizar un ticket existente en la base de datos.
        /// </summary>
        /// <param name="id">Identificador del ticket a actualizar.</param>
        /// <param name="ticket">Ticket con nuevos valores.</param>
        public async Task UpdateAsync(string id, Ticket ticket) =>
            await _context.Tickets.ReplaceOneAsync(t => t.TicketId == id, ticket);

        /// <summary>
        /// Realizar un soft delete sobre un ticket, marcándolo como inactivo.
        /// </summary>
        /// <param name="id">Identificador del ticket a eliminar.</param>
        public async Task SoftDeleteAsync(string id)
        {
            var update = Builders<Ticket>.Update.Set(t => t.IsActive, false);
            await _context.Tickets.UpdateOneAsync(t => t.TicketId == id, update);
        }
    }
}