﻿using WeatherAttack.Domain.Entities;

namespace WeatherAttack.Application.Contracts.Dtos.User.Request
{
    public class UserRequestDto : EntityBase
    {
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public UserRequestDto(string email, string username, string password)
        {
            Email = email;
            Username = username;
            Password = password;
        }
    }
}