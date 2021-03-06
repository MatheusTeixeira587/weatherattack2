﻿using WeatherAttack.Contracts.Dtos.SpellRule.Request;
using WeatherAttack.Contracts.Mapper;
using WeatherAttack.Domain.Enums;
using Entities = WeatherAttack.Domain.Entities;

namespace WeatherAttack.Application.Mapper.SpellRule
{
    public sealed class SpellRuleEntityMapper : IMapper<Entities.SpellRule, SpellRuleRequestDto, SpellRuleRequestDto>
    {
        public SpellRuleRequestDto ToDto(Entities.SpellRule entity)
        {
            return new SpellRuleRequestDto()
            {
                Id = entity.Id,
                Operator = (byte)entity.Operator,
                WeatherCondition = (byte)entity.WeatherCondition,
                Value = entity.Value,
            };
        }

        public Entities.SpellRule ToEntity(SpellRuleRequestDto request)
        {
            return new Entities.SpellRule(
                request.Id,
                request.SpellId,
                request.Value,
                (Operator)request.Operator,
                (WeatherCondition)request.WeatherCondition
            );
        }
    }
}
