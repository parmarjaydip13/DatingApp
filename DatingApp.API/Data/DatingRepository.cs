using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helper;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data {
    public class DatingRepository : IDatingRepository {
        private readonly DataContext _context;

        public DatingRepository (DataContext context) {
            _context = context;
        }
        public void Add<T> (T entity) where T : class {
            _context.Add (entity);
        }

        public void Delete<T> (T entity) where T : class {
            _context.Remove (entity);
        }

        public async Task<User> GetUser (int id) {
            var user = await _context.Users.Include (p => p.Photos).FirstOrDefaultAsync (u => u.Id == id);
            return user;
        }

        public async Task<Photo> GetPhoto (int id) {
            var photo = await _context.Photos.FirstOrDefaultAsync (u => u.Id == id);
            return photo;
        }

        public async Task<PageList<User>> GetUsers (UserParms userParms) {

            var users = _context.Users.Include (p => p.Photos).OrderByDescending (u => u.LastActive).AsQueryable ();

            users = users.Where (u => u.Id != userParms.UserId);

            users = users.Where (u => u.Gender == userParms.Gender);

            if (userParms.Likers) {
                var userLiker = await GetUserLikes (userParms.UserId, userParms.Likers);
                users = users.Where (x => userLiker.Contains (x.Id));
            }

            if (userParms.Likees) {
                var userLikees = await GetUserLikes (userParms.UserId, userParms.Likers);
                var lst = userLikees.ToList ();
                users = users.Where (x => userLikees.Contains (x.Id));
            }

            if (userParms.MinAge != 18 || userParms.MaxAge != 99) {

                var minDob = DateTime.Today.AddYears (-userParms.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears (-userParms.MinAge);
                users = users.Where (u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty (userParms.OrderBy)) {

                switch (userParms.OrderBy) {

                    case "created":
                        users = users.OrderBy (u => u.Created);
                        break;
                    default:
                        users = users.OrderByDescending (u => u.LastActive);
                        break;

                }
            }

            return await PageList<User>.CreateAsync (users, userParms.PageNumber, userParms.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes (int id, bool likers) {

            var user = await _context.Users.Include (x => x.Likers).Include (x => x.Likees).FirstOrDefaultAsync (u => u.Id == id);

            if (likers) {
                return user.Likers.Where (x => x.LikeeId == id).Select (x => x.LikerId);
            } else {
                return user.Likees.Where (x => x.LikerId == id).Select (x => x.LikeeId);
            }

        }
        public async Task<bool> SaveAll () {
            return await _context.SaveChangesAsync () > 0;
        }

        public async Task<Photo> GetMainPhotoForUser (int userId) {
            var photo = await _context.Photos.Where (u => u.UserId == userId).FirstOrDefaultAsync (p => p.IsMain);
            return photo;
        }

        public async Task<Like> GetLike (int userId, int recipientId) {
            return await _context.Likes.FirstOrDefaultAsync (u => u.LikerId == userId && u.LikeeId == recipientId);
        }
    }
}