using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using perla_metro_ticketService.Models;
using perla_metro_ticketService.Repositories;

namespace perla_metro_ticketService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketRepository _repository;
        public TicketController(TicketRepository repository)
        {
            _repository = repository;
        }

        // Crear nuevo Ticket
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Ticket ticket)
        {
            // Validar duplicado: pasajero + fecha
            var existing = await _repository.GetByPassengerAndDateAsync(ticket.PassengerId, ticket.EmissionDate);
            if (existing != null)
            {
                return BadRequest("Ya existe un ticket para este pasajero en la misma fecha.");
            }

            await _repository.CreateAsync(ticket);
            return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
        }

        // Ver todos los tickets en sistema
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _repository.GetAllAsync();
            return Ok(tickets);
        }

        // Ver ticket por ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        // Editar ticket existente
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] Ticket ticket)
        {
            var current = await _repository.GetByIdAsync(id);
            if (current == null) return NotFound("Ticket no encontrado.");

            // Validar estado
            if (current.Status == "caducado" && ticket.Status == "activo")
                return BadRequest("No se puede volver a activo un ticket caducado.");

            ticket.TicketId = id; // asegurar que conserve el mismo ID
            await _repository.UpdateAsync(id, ticket);
            return Ok(ticket);
        }

        // Soft Delete/Eliminar ticket
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.SoftDeleteAsync(id);
            return Ok("Ticket marcado como inactivo.");
        }

    }
}