﻿using DatingSite.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingSite.API.Data {
    public interface IAuthRepository {

        Task<User> Register(User user, string password);

        Task<User> Login(string username, string password);

        Task<bool> UserExist(string username);
    }
}
