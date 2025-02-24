using MediatR;

namespace Products.Core.Commands.CurrencyCommands
{
    public class AddCurrencyCommand : IRequest<int>
    {
        public string Code { get; }
        public string Name { get; }

        public AddCurrencyCommand(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
