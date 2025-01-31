using MediatR;
using WebAPI.Core.Models;

namespace WebAPi.Core.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public int Id { get; set; }
    }
}
