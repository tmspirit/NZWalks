﻿using FluentValidation;

namespace NZWalksAPI.Validators
{
    public class AddWalkRequestValidator: AbstractValidator<Models.DTO.AddWalkRequest>
    {
        public AddWalkRequestValidator() {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Length).GreaterThan(0);
        }
    }
}
