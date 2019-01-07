﻿
using WeatherAttack.Domain.Entities;

namespace WeatherAttack.Application.Contracts.Dtos.User.Response
{
    public class UserResponseDto : EntityBase
    {
        public string Email { get;  set; }

        public string Username { get;  set; }

        public Character Character { get; set; }

        public UserResponseDto(string email, string username)
        {
            Email = email;
            Username = username;
        }

        public UserResponseDto(string email, string username, Character character)
        {
            Email = email;
            Username = username;
            Character = character;
        }

        public UserResponseDto() { }
    }
}