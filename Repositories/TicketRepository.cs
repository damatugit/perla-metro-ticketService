using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using perla_metro_ticketService.Data;
using perla_metro_ticketService.Models;

namespace perla_metro_ticketService.Repositories
{
    public class TicketRepository
    {
        private readonly MongoDbContext _context;

        public TicketRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Ticket>> GetAllAsync() =>
            await _context.Tickets.Find(t => t.IsActive).ToListAsync();

        public async Task<Ticket?> GetByIdAsync(string id) =>
            await _context.Tickets.Find(t => t.TicketId == id && t.IsActive).FirstOrDefaultAsync();

        public async Task<Ticket?> GetByPassengerAndDateAsync(string passengerId, DateTime date) =>
            await _context.Tickets
                .Find(t => t.PassengerId == passengerId && t.EmissionDate.Date == date.Date && t.IsActive)
                .FirstOrDefaultAsync();

        public async Task CreateAsync(Ticket ticket) =>
            await _context.Tickets.InsertOneAsync(ticket);

        public async Task UpdateAsync(string id, Ticket ticket) =>
            await _context.Tickets.ReplaceOneAsync(t => t.TicketId == id, ticket);

        public async Task SoftDeleteAsync(string id)
        {
            var update = Builders<Ticket>.Update.Set(t => t.IsActive, false);
            await _context.Tickets.UpdateOneAsync(t => t.TicketId == id, update);
        }
    }
}