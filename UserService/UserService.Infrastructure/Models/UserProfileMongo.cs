using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace UserService.Infrastructure.Models
{
    internal class UserProfileMongo
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        public string UserId { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
