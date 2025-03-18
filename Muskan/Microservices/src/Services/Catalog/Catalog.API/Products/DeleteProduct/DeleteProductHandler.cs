

using Catalog.API.Exceptions;
using Catalog.API.Products.UpdateProduct;
using FluentValidation;

namespace Catalog.API.Products.DeleteProduct
{
    public record DeleteProductCommand(Guid id ) : ICommand<DeleteProductResult>;
    public record DeleteProductResult(bool isSuccess);
    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
          }
    }
    internal class DeleteProductHandler(IDocumentSession session) : ICommandHandler<DeleteProductCommand, DeleteProductResult>
    {
        public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            var product = await session.LoadAsync<Product>(command.id, cancellationToken);
            if (product == null)
            {
                throw new ProductNotFoundException(command.id);
            }

            session.Delete(product);
            await session.SaveChangesAsync(cancellationToken);

            Console.WriteLine($"Product {command.id} deleted successfully.");
            return new DeleteProductResult(true);

        }


    }
}
