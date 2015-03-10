using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DoWorkGym.Infrastructure
{
    public class BaseMongo
    {
        //private const string ConnectionString = "mongodb://localhost"; // 27017
        private const string DbName = "dowork";

        public MongoDatabase GetDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["dowork-mongodb"].ConnectionString;

            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            return server.GetDatabase(DbName);
        }
    }
}
