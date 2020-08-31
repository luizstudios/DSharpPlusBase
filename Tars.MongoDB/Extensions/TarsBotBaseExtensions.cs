using Tars.Core;
using Tars.Extensions;
using DSharpPlus;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tars.MongoDB.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="TarsBase"/> methods.
    /// </summary>
    public static class TarsBotBaseExtensions
    {
        private static MongoClient _mongoClient;

        /// <summary>
        /// Method for configuring MongoDB.
        /// </summary>
        /// <param name="botBase"></param>
        /// <param name="mongoClient">Mongo class instance.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void MongoClientSetup(this TarsBase botBase, MongoClient mongoClient)
        {
            if (botBase == null)
                throw new ArgumentNullException("The BotBase can't be null!");

            _mongoClient = mongoClient ?? throw new ArgumentNullException("The MongoClient can't be null!");
        }

        /// <summary>
        /// Method to get the current Mongo instance.
        /// </summary>
        /// <param name="_"></param>
        /// <returns>The current instance of <see cref="MongoClient"/>.</returns>
        /// <exception cref="NullReferenceException"></exception>
        public static MongoClient GetMongoClient(this TarsBase _) => _mongoClient ?? throw new NullReferenceException("The MongoClient is null! Call the MongoClientSetup!");
    }
}