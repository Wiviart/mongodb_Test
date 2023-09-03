using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms;
using Realms.Schema;
using Realms.Weaving;

public partial class PlayerData :IRealmObject
{
    [BsonElement("_id")] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [BsonElement("UserID")] public string UserID { get; set; }
    [BsonElement("name")] public string Name { get; set; }
    [BsonElement("rank")] public int Rank { get; set; }
    [BsonElement("gold")] public int Gold { get; set; }
    [BsonElement("level")] public int Level { get; set; }
}
