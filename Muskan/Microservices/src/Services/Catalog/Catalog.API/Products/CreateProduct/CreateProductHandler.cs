

using FluentValidation;

namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, decimal Price , string Image ) : ICommand<CreateProductResult>;
    public record CreateProductResult(Guid Id);
  public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");    
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must greater than 0");
            RuleFor(x => x.Image).NotEmpty().WithMessage("Image is required");
        }
    }
    internal class CreateProductHandler(IDocumentSession session ) : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // Create product entity from command
           
            var product = new Product
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                Price = command.Price,
                ImageFile = command.Image
            };

            // Save product entity to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            // Assuming SaveProductAsync is a method that saves the product and returns the product ID


            // Return the product iddocker-compose up --build

            return new CreateProductResult(product.Id);
        }


    }
}
