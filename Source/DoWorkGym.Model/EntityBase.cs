using MongoDB.Bson;

namespace DoWorkGym.Model
{
    /// <summary>
    /// A non-instantiable base entity which defines 
    /// members available across all entities.
    /// </summary>
    public abstract class EntityBase
    {
        public ObjectId Id { get; set; }
    }
}
