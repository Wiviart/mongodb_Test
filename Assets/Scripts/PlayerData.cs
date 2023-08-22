using System;
using System.Collections;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Realms;
using Realms.Schema;
using Realms.Weaving;

public partial class PlayerData :IRealmObjectBase
{
    [BsonElement("_id")] public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
    [BsonElement("UserID")] public string UserID { get; set; }
    [BsonElement("name")] public string Name { get; set; }
    [BsonElement("rank")] public int Rank { get; set; }
    [BsonElement("gold")] public int Gold { get; set; }
    [BsonElement("level")] public int Level { get; set; }

    public IRealmAccessor Accessor => throw new NotImplementedException();

    public bool IsManaged => throw new NotImplementedException();

    public bool IsValid => throw new NotImplementedException();

    public bool IsFrozen => throw new NotImplementedException();

    public Realm Realm => throw new NotImplementedException();

    public ObjectSchema ObjectSchema => throw new NotImplementedException();

    public DynamicObjectApi DynamicApi => throw new NotImplementedException();

    public int BacklinksCount => throw new NotImplementedException();

    public void SetManagedAccessor(IRealmAccessor accessor, IRealmObjectHelper helper = null, bool update = false, bool skipDefaults = false)
    {
        throw new NotImplementedException();
    }
}
