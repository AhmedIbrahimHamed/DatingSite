﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingSite.API.Helpers;
using DatingSite.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingSite.API.Data {
    public class DatingRepository : IDatingRepository {

        private readonly DataContext _context;

        public DatingRepository(DataContext context) {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add<T>(T entity) where T : class {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class {
            _context.Remove(entity);
        }

        public async Task<Like> GetLike(int userId, int recipentId) {
            return await _context.Likes.FirstOrDefaultAsync(u => u.LikerId == userId && u.LikeeId == recipentId);
        }

        public async Task<Photo> GetMainPhotoForUser(int userId) {
            return await _context.Photos.Where(p => p.UserId == userId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<Photo> GetPhoto(int id) {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);

            return photo;
        }

        public async Task<User> GetUser(int id) {
            var user = await _context.Users.Include(u => u.Photos).FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<PagedList<User>> GetUsers(UserParams userParams) {
            // Filtering users of possible matches with gender and age.
            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            var users = _context.Users.Include(i => i.Photos)
                .Where(u => u.Id != userParams.UserId)
                .Where(u => u.Gender == userParams.Gender)
                .Where(u => u.DateOfBirth >= minDob
                    && u.DateOfBirth <= maxDob)
                .OrderByDescending(u => u.LastActive)
                .AsQueryable();

            if (userParams.Likers) {
                var userLikers = await GetUserLikes(userParams.UserId,
                    userParams.Likers);

                users = users.Where(u => userLikers.Contains(u.Id));
            }

            if (userParams.Likees) {
                var userLikees = await GetUserLikes(userParams.UserId,
                    userParams.Likers);

                users = users.Where(u => userLikees.Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(userParams.OrderBy)) {
                switch (userParams.OrderBy) {
                    case "created":
                        users = users.OrderByDescending(u => u.CreatedAt);
                        break;
                    default:
                        users = users.OrderByDescending(u => u.LastActive);
                        break;
                }
            }

            return await PagedList<User>.CreateAsync(users, userParams.PageNumber, userParams.PageSize);
        }

        private async Task<IEnumerable<int>> GetUserLikes(int id, bool likers) {
            var user = await _context.Users
                .Include(u => u.Likers)
                .Include(u => u.Likees)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (likers) {
                return user.Likers.Where(u => u.LikeeId == id).Select(u => u.LikerId);
            }

            return user.Likees.Where(u => u.LikerId == id).Select(u => u.LikeeId);
        }

        public async Task<bool> SaveAll() {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id) {
            return await _context.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedList<Message>> GetMessagesForUser(MessageParams messageParams) {
            var messages = _context.Messages
                .Include(m => m.Sender)
                .ThenInclude(u => u.Photos)
                .Include(m => m.Recipient)
                .ThenInclude(u => u.Photos)
                .AsQueryable();

            switch (messageParams.MessageContainer) {
                case "Inbox":
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false);
                    break;
                case "Outbox":
                    messages = messages.Where(m => m.SenderId == messageParams.UserId && m.SenderDeleted == false);
                    break;
                default:
                    messages = messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDeleted == false && m.IsRead == false );
                    break;
            }

            messages = messages.OrderByDescending(m => m.MessageSent);
            return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId) {
            var messages = await _context.Messages
                .Include(m => m.Sender)
                .ThenInclude(u => u.Photos)
                .Include(m => m.Recipient)
                .ThenInclude(u => u.Photos)
                .Where(m => m.RecipientId == userId && m.RecipientDeleted == false && m.SenderId == recipientId ||
                       m.RecipientId == recipientId && m.SenderId == userId && m.SenderDeleted == false)
                .OrderByDescending(m => m.MessageSent)
                .ToListAsync();

            return messages;
        }

        public void UpdateMessageIsRead(int userId, int recipientId) {
            
        }
    }
}
 