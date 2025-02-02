﻿using MediatR;
using WebAPI.Core.Models;

namespace WebAPI.Core.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}
