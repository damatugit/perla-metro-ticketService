using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace perla_metro_ticketService.Models
{
    public class Ticket
    {
        /// <summary>
        /// Identificador unico de ticket
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketId { get; set; } = null!;

        /// <summary>
        /// Identificador unico de usuario/pasajero del ticket
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string PassengerId { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de emision de ticket
        /// </summary>
        [BsonElement("issueDate")]
        public DateTime EmissionDate { get; set; } = DateTime.UtcNow; 

        /// <summary>
        /// Tipo de ticket: ida o vuelta
        /// </summary>
        [BsonElement("ticketType")]
        public string TicketType { get; set; } = "ida"; 

        /// <summary>
        /// Estado del ticket: activo | usado | caducado | inactivo (soft delete)
        /// </summary>
        [BsonElement("status")]
        public string Status { get; set; } = "activo"; //Estado del ticket: 

        /// <summary>
        /// Precio pagado del ticket
        /// </summary>
        [BsonElement("amountPaid")]
        public decimal AmountPaid { get; set; } // Precio de ticket
        
        /// <summary>
        /// Indicador del estado del ticket: activo o inactivo para soft delete
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}