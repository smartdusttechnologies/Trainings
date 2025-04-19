

using Catalog.API.Exceptions;

namespace Catalog.API.Products.UpdateProduct
{
     public record UpdateProductCommand(Guid Id, string Name, List<string> Category, string Description, string ImageFile, decimal Price)
      : ICommand<UpdateProductResult>;
     public record UpdateProductResult(bool IsSuccess);
     public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
     {
          public UpdateProductCommandValidator()
          {
               RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
               RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
               RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must greater than 0");
          }
     }
     internal class UpdateProductCommandHandler
         (IProductRepository _productRepository,
          ILoggingService<UpdateProductCommandHandler> logger)
         : ICommandHandler<UpdateProductCommand, UpdateProductResult>
     {
          public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
          {
               try
               {
                    await logger.LogInformationAsync($"Get the Command from Update Product {command} ");

                    var product = await _productRepository.GetProductByIdAsync(command.Id, cancellationToken);
                    await logger.LogInformationAsync($"Product with Id : {product} ");
                    if (product is null)
                    {
                         await logger.LogErrorAsync($"Product is not found  ", new ProductNotFoundException(command.Id));
                         throw new ProductNotFoundException(command.Id);

                    }

                    // Update the product's properties
                    product.Name = command.Name;
                    product.Category = command.Category;
                    product.Description = command.Description;
                    product.ImageFile = command.ImageFile;
                    product.Price = command.Price;

                    // Save changes via the repository
                    var updatedProduct = await _productRepository.UpdateProductAsync(product, cancellationToken);

               }
               catch (ProductNotFoundException ex)
               {
                    await logger.LogErrorAsync("error occured in update products ", new ProductNotFoundException(command.Id));


                    return new UpdateProductResult(false);
               }


               return new UpdateProductResult(true);
          }
     }
}