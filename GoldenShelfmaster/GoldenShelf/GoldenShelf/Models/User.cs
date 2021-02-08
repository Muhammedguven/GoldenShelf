using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static GoldenShelf.UserViewModel;

namespace GoldenShelf
{
    public class User
    {

        [BsonId,BsonElement("Email")]
        public string email { get; set; }
        [BsonElement("Name")]
        public string name { get; set; }

        [BsonElement("Password")]
        public string password { get; set; }

        [BsonElement("City")]
        public string city { get; set; }

        [BsonElement("District")]
        public string district { get; set; }

        [BsonElement("Image")]
        public byte[] image { get; set; }





    }
}
