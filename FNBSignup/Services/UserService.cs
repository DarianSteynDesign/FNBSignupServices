using FNBSignup.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace FNBSignup.Services
{
    public class UserService
    {
        private readonly IMongoCollection<UserModel> _userCollection;

        public UserService(IOptions<DatabaseSettings> dabaseSettings)
        {
            var mongoClient = new MongoClient(dabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(dabaseSettings.Value.DatabaseName);

            _userCollection = mongoDatabase.GetCollection<UserModel>("Users");
        }

        public async Task<List<UserModel>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        public async Task<UserModel?> GetAsync(string id) =>
            await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(UserModel newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        public async Task UpdateAsync(string id, UserModel newUser) =>
            await _userCollection.ReplaceOneAsync(x => x.Id == id, newUser);

        public async Task RemoveAsync(string id) => await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}
