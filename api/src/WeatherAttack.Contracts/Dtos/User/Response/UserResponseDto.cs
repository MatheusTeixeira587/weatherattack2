﻿namespace WeatherAttack.Contracts.Dtos.User.Response
{
    public sealed class UserResponseDto
    {
        public long Id { get; set; }

        public string Email { get;  set; }

        public string Username { get;  set; }

        public byte PermissionLevel { get; set; }

        public Character.CharacterDto Character { get; set; }

        public UserResponseDto(string email, string username)
        {
            Email = email;
            Username = username;
        }

        public UserResponseDto(string email, string username, Character.CharacterDto character)
        {
            Email = email;
            Username = username;
            Character = character;
        }

        public UserResponseDto() { }
    }
}
