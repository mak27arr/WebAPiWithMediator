using MediatR;
using OrderService.Application.Features.Commands;
using OrderService.Domain.Entities;
using OrderService.Domain.Interface.Repository;
using Products.Common.Kafka;
using Products.Common.Kafka.EventArg;

namespace OrderService.Application.Features.CommandHandler
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _repository;
        private readonly IKafkaProducer _producer;

        public CreateOrderCommandHandler(IOrderRepository repository, IKafkaProducer kafkaProducer)
        {
            _repository = repository;
            _producer = kafkaProducer;
        }

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow
            };

            //TODO: Add price

            await _repository.AddAsync(order);

            var orderEvent = new OrderCreatedEvent 
            { 
                OrderId = order.Id,
                ProductId = order.ProductId,
                Quantity = order.Quantity 
            };
            await _producer.ProduceAsync(KafkaOrderTopics.OrderCreated, orderEvent);

            return order.Id;
        }
    }
}
