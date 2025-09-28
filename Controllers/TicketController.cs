using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using perla_metro_ticketService.Models;
using perla_metro_ticketService.Repositories;

namespace perla_metro_ticketService.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar la API de Tickets.
    /// 
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly TicketRepository _repository;

        /// <summary>
        /// Inicializador del controlador con el repositorio de tickets
        /// </summary>
        /// <param name="repository">Repositorio de tickets</param>
        public TicketController(TicketRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Creacion de un nuevo ticket
        /// </summary>
        /// <param name="ticket">ticket a crear</param>
        /// <returns>Http 201 si se creo correctamente, 400 si esta duplicado</returns>
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

        /// <summary>
        /// Obtener todos los tickets en el sistema
        /// </summary>
        /// <summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var tickets = await _repository.GetAllAsync();
            return Ok(tickets);
        }

        /// <summary>
        /// Obtener por id un ticket
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ticket encontrado o 400 de lo contrario</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var ticket = await _repository.GetByIdAsync(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }

        /// <summary>
        /// Editar un ticket existente en sistema
        /// </summary>
        /// <param name="id">Id de ticket a editar</param>
        /// <param name="ticket">el ticket actualizado</param>
        /// <returns>200 si se edito, 400 si no es valido su estado o 401 si no se encuentra</returns>
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

        /// <summary>
        /// Eliminación Soft Delete de un ticket, marcandolo inactivo
        /// </summary>
        /// <param name="id">Id de ticket a desactivar</param>
        /// <returns>200 con mensaje de confirmación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _repository.SoftDeleteAsync(id);
            return Ok("Ticket marcado como inactivo.");
        }

    }
}