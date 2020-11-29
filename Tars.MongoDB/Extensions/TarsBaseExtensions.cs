using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Reflection;
using Tars.Core;

namespace Tars.MongoDB.Extensions
{
    /// <summary>
    /// Class to extend the standard <see cref="TarsBase"/> methods.
    /// </summary>
    public static class TarsBaseExtensions
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
            if (botBase is null)
                throw new ArgumentNullException("The BotBase can be null!");

            _mongoClient = mongoClient ?? throw new ArgumentNullException("The MongoClient can be null!");

            (typeof(TarsBase).GetField("_services", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(botBase) as IServiceCollection).AddSingleton(_mongoClient);
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