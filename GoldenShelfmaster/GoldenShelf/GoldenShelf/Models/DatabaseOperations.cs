using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoldenShelf.Models
{
    public class DatabaseOperations
    {
        private IMongoDatabase db;
        public DatabaseOperations(string database)
        {
            var client = new MongoClient("mongodb://mguven7006:mguven@cluster0-shard-00-00.hscqt.mongodb.net:27017,cluster0-shard-00-01.hscqt.mongodb.net:27017,cluster0-shard-00-02.hscqt.mongodb.net:27017/<dbname>?ssl=true&replicaSet=atlas-rlcztw-shard-0&authSource=admin&retryWrites=true&w=majority");
            db = client.GetDatabase(database);
        }
        public void InsertRecord<T>(string table, T record)
        {
            var collection = db.GetCollection<T>(table);
            collection.InsertOne(record);
        }
    }
}
