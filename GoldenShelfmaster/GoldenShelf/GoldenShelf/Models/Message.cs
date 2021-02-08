using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldenShelf.Models
{
    public class Message
    {
        [BsonId, BsonElement("MessageID")]
        public Guid Id { get; set; }

        [BsonElement("Name")]
        public string SpecialBookName { get; set; }
        [BsonElement("Sender")]
        public string Sender { get; set; }

        [BsonElement("Receiver")]
        public string Receiver { get; set; }

        [BsonElement("MessageText")]
        public string MessageText { get; set; }

        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime Date { get; set; }
        [BsonElement("Location")]
        public string Location { get; set; }
    }
}
