
using BuildingBlock.Messaging.Events;
using MassTransit;

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
    public class CheckOutBasketHandler(IBasketRepository repository , IPublishEndpoint publishEndpoint) : ICommandHandler<CheckOutBasketCommand, CheckOutBasketResult>
    {   
        public async Task<CheckOutBasketResult> Handle(CheckOutBasketCommand command, CancellationToken cancellationToken)
        {
            //get the existing basket with total price 
            var basket = await repository.GetBasket(command.BasketCheckOutDto.Username, cancellationToken);
           if (basket == null)
            {
                return new CheckOutBasketResult(false);
            }
           
           // set the toatal price as basketcheckout event message
            var eventmessage = command.BasketCheckOutDto.Adapt<BasketCheckOutEvents>();
            eventmessage.TotalPrice = basket.TotalPrice;

            // send basket checkout event to rabbitmq using mass transit 
            await publishEndpoint.Publish(eventmessage, cancellationToken);
           
            // delete the basket
            await repository.DeleteBasket(command.BasketCheckOutDto.Username, cancellationToken);
          
            // return the result true
            return new CheckOutBasketResult(true);
        }
    }
}
