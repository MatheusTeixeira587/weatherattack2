﻿using System.Collections.Generic;
using WeatherAttack.Contracts.Dtos.SpellRule.Request;

namespace WeatherAttack.Contracts.Dtos.Spell.Response
{
    public sealed class SpellResponseDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public int BaseDamage { get; set; }

        public int BaseManaCost { get; set; }

        public string Element { get; set; }

        public IEnumerable<SpellRuleRequestDto> Rules { get; set; }
    }
}
