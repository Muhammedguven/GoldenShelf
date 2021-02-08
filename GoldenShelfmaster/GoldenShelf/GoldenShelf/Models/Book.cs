using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldenShelf.Models
{
    class Book
    {

        [BsonId]
        public string bookID { get; set; }
        [BsonElement("Category")]
        public string category { get; set; }

        [BsonElement("BookName")]
        public string bookName { get; set; }

        [BsonElement("Author")]
        public string author { get; set; }

        [BsonElement("Condition")]
        public string condition { get; set; }
        

        


    }
}
