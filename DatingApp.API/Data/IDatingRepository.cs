using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Helper;
using DatingApp.API.Models;

namespace DatingApp.API.Data {
    public interface IDatingRepository {
        void Add<T> (T entity) where T : class;

        void Delete<T> (T entity) where T : class;

        Task<bool> SaveAll ();

        Task<PageList<User>> GetUsers (UserParms userparms);

        Task<User> GetUser (int id);

        Task<Photo> GetPhoto (int id);

        Task<Photo> GetMainPhotoForUser (int userId);

        Task<Like> GetLike (int userId, int recipientId);

        Task<Message> GetMessage (int id);

        Task<PageList<Message>> GetMessagesForUse (MessageParams messageParams);

        Task<IEnumerable<Message>> GetMessageThred (int userId, int recipitentId);
    }
}