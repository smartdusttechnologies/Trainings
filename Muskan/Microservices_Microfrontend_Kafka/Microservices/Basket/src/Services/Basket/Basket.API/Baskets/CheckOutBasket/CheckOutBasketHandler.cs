using System.Text.Json;
using RabbitMQConnectionFactory = RabbitMQ.Client.IConnectionFactory;

namespace Basket.API.Baskets.CheckOutBasket
{
     public record CheckOutBasketCommand(BssketCheckOutDto BasketCheckOutDto) : ICommand<CheckOutBasketResult>;
     public record CheckOutBasketResult(bool isSuccess);

     public class CheckOutBasketValidator : AbstractValidator<CheckOutBasketCommand>
     {
          public CheckOutBasketValidator()
          {
               RuleFor(x => x.BasketCheckOutDto).NotNull().WithMessage("Basket CheckOut DTO is not null");
               RuleFor(x => x.BasketCheckOutDto.Username).NotEmpty().WithMessage("Basket CheckOut Username is not empty");
          }
     }

     public class CheckOutBasketHandler : ICommandHandler<CheckOutBasketCommand, CheckOutBasketResult>
     {
          private readonly IBasketRepository _repository;
          private readonly IMapper _mapper;
          private readonly ILoggingService<CheckOutBasketHandler> _logger;
          private readonly IProducerServices _producerService;
          private readonly RabbitMQConnectionFactory _factorr;
          private const string TopicName = "basket_checkout_topic";

          public CheckOutBasketHandler(IBasketRepository repository,    IProducerServices producerService,  IMapper mapper, ILoggingService<CheckOutBasketHandler> logger, RabbitMQConnectionFactory factorr)
          {
               _repository = repository;
               _mapper = mapper;
               _logger = logger;
               _producerService = producerService;
               _factorr = factorr;
          }

          public async Task<CheckOutBasketResult> Handle(CheckOutBasketCommand command, CancellationToken cancellationToken)
          {
               // Get the existing basket with total price 
               var basket = await _repository.GetBasket(command.BasketCheckOutDto.Username, cancellationToken);
               if (basket == null)
               {
                    await _logger.LogInformationAsync($"Basket is null");
                    return new CheckOutBasketResult(false);
               }

               //var producer = new BasketCheckOutEventProducer(_factorr, "basket_checkout_exchange", "basket_checkout_queue");

               // Map BasketCheckOutDto to BasketCheckOutEvent
               var eventMessage = _mapper.Map<BasketCheckOutEvents>(command.BasketCheckOutDto);
               eventMessage.TotalPrice = basket.TotalPrice;

               var jsonMessage = JsonSerializer.Serialize(eventMessage);
               await _logger.LogInformationAsync($"Basket Checkout event send : {jsonMessage}");
               await _producerService.ProduceAsync(TopicName, jsonMessage);
               // Publish the basket checkout event to RabbitMQ using the RabbitMQ Producer
               //await producer.Publish(eventMessage);

               // Delete the basket
               await _repository.DeleteBasket(command.BasketCheckOutDto.Username, cancellationToken);

               // Return result as true
               return new CheckOutBasketResult(true);
          }
     }

     //public class BasketCheckOutEventProducer : RabbitMqProducerBase<BasketCheckOutEvents>
     //{
     //     public BasketCheckOutEventProducer(RabbitMQConnectionFactory factory, string exchange, string queue)
     //         : base(factory, exchange, queue)
     //     {
     //     }
     //}


}
