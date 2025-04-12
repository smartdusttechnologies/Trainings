

using Catalog.API.Exceptions;

namespace Catalog.API.Products.DeleteProduct
{
     public record DeleteProductCommand(Guid id) : ICommand<DeleteProductResult>;
     public record DeleteProductResult(bool isSuccess);
     public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
     {
          public DeleteProductCommandValidator()
          {
               RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
          }
     }
     internal class DeleteProductHandler(IProductRepository _productRepository, ILoggingService<DeleteProductHandler> logger) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
     {
          public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
          {
               try
               {
                    await logger.LogInformationAsync("Deleting a product...");
                    var result = await _productRepository.DeleteProductAsync(command.id, cancellationToken);

                    if (!result)
                    {
                         await logger.LogErrorAsync("Product not found", new ProductNotFoundException(command.id));
                         throw new ProductNotFoundException(command.id);
                    }
                    //Console.WriteLine($"Product {command.id} deleted successfully.");
                    return new DeleteProductResult(true);
               }
               catch (Exception ex)
               {
                    await logger.LogErrorAsync("An Unhandled exception occured ", ex);
                    //Console.WriteLine("Error in delting the product");
                    return new DeleteProductResult(false);
               }

          }


     }
}
