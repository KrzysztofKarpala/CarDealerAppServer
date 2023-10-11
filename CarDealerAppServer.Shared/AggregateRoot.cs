using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Shared
{
    public abstract class AggregateRoot
    {
        [BsonIgnoreIfDefault]
        public ObjectId Id { get; set; }
        public int Version { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
        protected void IncrementVersion()
        {
            Version++;
        }
        protected void SetCreationDate() => CreationDate = DateTime.UtcNow;
        protected void SetModificationDate() => ModificationDate = DateTime.UtcNow;
    }
}
