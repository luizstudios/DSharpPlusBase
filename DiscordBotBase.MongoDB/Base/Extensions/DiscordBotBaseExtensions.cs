using DiscordBotBase.Core;
using DiscordBotBase.Extensions;
using DSharpPlus;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBotBase.MongoDB.Extensions
{
    public static class DiscordBotBaseExtensions
    {
        private static MongoClient _mongoClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botBase"></param>
        /// <param name="mongoClient"></param>
        public static void MongoClientSetup(this BotBase botBase, MongoClient mongoClient)
        {
            if (botBase == null) 
                throw new ArgumentNullException("The BotBase can't be null!");

            _mongoClient = mongoClient ?? throw new ArgumentNullException("The MongoClient can't be null!");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_"></param>
        /// <returns></returns>
        public static MongoClient GetMongoClient(this BotBase _) => _mongoClient ?? 
                                                                    throw new NullReferenceException("The MongoClient is null! Call the MongoClientSetup!");
    }
}