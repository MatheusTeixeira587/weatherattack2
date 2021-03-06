﻿
using Valit;
using WeatherAttack.Domain.EntityValidation.Rules;
using WeatherAttack.Domain.Notifications;

namespace WeatherAttack.Domain.Entities
{
    public sealed class User : EntityBase
    {
        public string Email { get; private set; }

        public string Username { get; private set; }

        public string Password { get; private set; }

        public Character Character { get; private set; }

        public byte PermissionLevel { get; private set; } = Rules.User.PermissionLevel.User;

        public User(long id) : base(id) { }

        public User(long id, string username) : base(id)
        {
            Username = username;
        }

        public User(long id, string username, Character character) : base(id)
        {
            Username = username;
            Character = character;
        }

        public User(long id, string email, string username) : base(id)
        {
            Email = email;
            Username = username;
        }

        public User(long id, string username, byte permissionLevel, string password) : base(id)
        {
            Id = id;
            Username = username;
            PermissionLevel = permissionLevel;
            Password = password;
        }

        public void SetPassword(string password) => Password = password;

        public void SetCharacter(Character character) => Character = character;

        protected override bool Validate()
        {
            var result = ValitRules<User>
                .Create()
                .Ensure(u => u.Email, _ => _
                    .Required()
                        .WithMessage(WeatherAttackNotifications.User.EmailIsRequired)
                    .Satisfies(u => u.Trim().Length > Rules.User.Email.MinLength)
                        .WithMessage(WeatherAttackNotifications.User.InvalidEmail)
                    .Email()
                        .WithMessage(WeatherAttackNotifications.User.InvalidEmail)
                    .MaxLength(Rules.User.Email.MaxLength)
                        .WithMessage(WeatherAttackNotifications.User.InvalidEmail))
                 .Ensure(u => u.Username, _ => _
                    .Required()
                        .WithMessage(WeatherAttackNotifications.User.UsernameIsRequired)
                    .Satisfies(u => u.Trim().Length > Rules.User.Username.MinLength)
                        .WithMessage(WeatherAttackNotifications.User.InvalidUsername)
                    .MaxLength(Rules.User.Username.MaxLength)
                        .WithMessage(WeatherAttackNotifications.User.InvalidUsername))
                .Ensure(u => u.Password, _ => _
                    .Required()
                        .WithMessage(WeatherAttackNotifications.User.PasswordIsRequired))
                .For(this)
                .Validate();

            AddNotification(WeatherAttackNotifications.Get(result.ErrorMessages));

            return !HasNotification();
        }
    }
}