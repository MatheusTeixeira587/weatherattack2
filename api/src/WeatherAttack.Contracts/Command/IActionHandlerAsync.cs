﻿using System.Threading.Tasks;

namespace WeatherAttack.Contracts.Command
{
    public interface IActionHandlerAsync<T> where T : CommandBase
    {
        Task<T> ExecuteActionAsync(T command);
    }
}
