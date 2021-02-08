using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldenShelf
{
    public class Advert
    {
        [BsonId,BsonElement("AdvertID")]
        public Guid AdvertID { get; set; }
        
        [BsonElement("BookName")]
        public string BookName { get; set; }
        
        [BsonElement("BookAuthor")]
        public string BookAuthor { get; set; }
       
        [BsonElement("BookCategory")]
        public string BookCategory { get; set; }
       
        [BsonElement("Image")]
        public byte [] Image { get; set; }

        [BsonElement("ShareType")]
        public string ShareType { get; set; }
       
        [BsonElement("Condition")]
        public string Condition { get; set; }

        [BsonElement("PublisherEmail")]
        public string PublisherEmail { get; set; }
        [BsonElement("Description")]
        public string Description { get; set; }



    }
}
