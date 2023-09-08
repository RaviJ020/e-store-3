using MongoDB.Driver;
using ShoppingCartService;
using ShoppingCartService.Config;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCartServiceTests
{
    public class MongoDbFixture : IDisposable
    {
        private const string ImageName = "mongo_test";
        private const string MongoInPort = "27017";
        private const string MongoOutPort = "1111";

        private static string ConnectionString = $"mongodb://localhost:{MongoOutPort}";
        private static readonly TimeSpan TestTimeout = TimeSpan.FromSeconds(60);
        
        private Process _process;

        public ShoppingCartDatabaseSettings DataBaseSettings => new ShoppingCartDatabaseSettings
        {
            DatabaseName = "ShoppingCartDb",
            ConnectionString = ConnectionString,
            CollectionName = "ShoppingCart",
        };

        public MongoDbFixture() 
        {
            // Create database.
            string command = $"run --name {ImageName} -p {MongoOutPort}:{MongoInPort} mongo";
            _process = Process.Start("docker", command);

            // Check for succes.
            bool isAlive = WaitForMongoDbConnection(ConnectionString, "admin");
            if (!isAlive)
            {
                throw new Exception("Could not start docker container with Mongo.");
            }
        }

        private static bool WaitForMongoDbConnection(string connectionString, string dbName)
        {
            Console.Out.WriteLine("Waiting for Mongo to respond");
            var probeTask = Task.Run(() =>
            {
                var isAlive = false;
                var client = new MongoClient(connectionString);

                for (var i = 0; i < 3000; i++)
                {
                    client.GetDatabase(dbName);
                    var server = client.Cluster.Description.Servers.FirstOrDefault();
                    isAlive = server != null &&
                             server.HeartbeatException == null &&
                             server.State == MongoDB.Driver.Core.Servers.ServerState.Connected;

                    if (isAlive)
                    {
                        break;
                    }

                    Thread.Sleep(100);
                }

                return isAlive;
            });
            
            probeTask.Wait();

            return probeTask.Result;
        }

        public void Dispose()
        {
            if (_process != null)
            {
                _process.Dispose();
                _process = null;
            }

            var processStop = Process.Start("docker", $"stop {ImageName}");
            processStop?.WaitForExit();

            var processRm = Process.Start("docker", $"rm {ImageName}");
            processRm?.WaitForExit();
        }
    }
}
