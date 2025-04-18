using AutoMapper;
using MongoDB.Driver;
using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Configurations;
using UserService.Infrastructure.Models;

namespace UserService.Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<UserProfileMongo> _users;
        private readonly IMapper _mapper;

        public UserRepository(IMongoClient client, MongoDbSettings settings, IMapper mapper)
        {
            var database = client.GetDatabase(settings.DatabaseName);
            _users = database.GetCollection<UserProfileMongo>("Users");
            _mapper = mapper;
            CreateIndexes();
        }

        private void CreateIndexes()
        {
            var indexKeys = Builders<UserProfileMongo>.IndexKeys
                .Ascending(u => u.Email)
                .Ascending(u => u.UserName);

            _users.Indexes.CreateOne(new CreateIndexModel<UserProfileMongo>(indexKeys));
        }

        public async Task<UserProfile?> GetByEmailAsync(string email)
        {
            var mongoProfile = await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
            return mongoProfile != null ? _mapper.Map<UserProfile>(mongoProfile) : null;
        }

        public async Task<UserProfile?> GetByIdAsync(Guid id)
        {
            var mongoProfile = await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
            return mongoProfile != null ? _mapper.Map<UserProfile>(mongoProfile) : null;
        }

        public async Task<UserProfile?> GetByUserIdAsync(string userId)
        {
            var mongoProfile = await _users.Find(u => u.UserId == userId).FirstOrDefaultAsync();
            return mongoProfile != null ? _mapper.Map<UserProfile>(mongoProfile) : null;
        }

        public async Task CreateAsync(UserProfile user)
        {
            var mongoProfile = _mapper.Map<UserProfileMongo>(user);
            await _users.InsertOneAsync(mongoProfile);
        }

        public async Task UpdateAsync(UserProfile user)
        {
            var mongoProfile = _mapper.Map<UserProfileMongo>(user);
            await _users.ReplaceOneAsync(u => u.Id == user.Id, mongoProfile);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _users.DeleteOneAsync(u => u.Id == id);
        }
    }
}
