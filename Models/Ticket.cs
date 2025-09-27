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
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string TicketId { get; set; } = null!; //Id del ticket

        [BsonRepresentation(BsonType.ObjectId)]
        public string PassengerId { get; set; } = string.Empty;
        [BsonElement("issueDate")]
        public DateTime EmissionDate { get; set; } = DateTime.UtcNow; //Fecha y hora de emision de ticket

        [BsonElement("ticketType")]
        public string TicketType { get; set; } = "ida"; //Tipo de viaje del ticket: "ida" | "vuelta"

        [BsonElement("status")]
        public string Status { get; set; } = "activo"; //Estado del ticket: activo | usado | caducado | inactivo (soft delete)

        [BsonElement("amountPaid")]
        public decimal AmountPaid { get; set; } // Precio de ticket
        
        public bool IsActive { get; set; } = true; // Soft delete
    }
}