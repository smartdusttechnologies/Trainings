using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly DiscountDbContext _context;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(DiscountDbContext context, ILogger<DiscountService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async override Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context
                .Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon == null)
            {
                coupon = new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
            }
            var couponModel = coupon.Adapt<CouponModel>();
            return couponModel;
        }

        public async override Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var coupon = request.Coupon.Adapt<Coupon>();
                if (coupon is null)
                {
                    throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request"));
                }

                _context.Coupons.Add(coupon);
                await _context.SaveChangesAsync();
                var couponModel = coupon.Adapt<CouponModel>();
                _logger.LogInformation("Discount is created successfully");
                return couponModel;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database update error: {Message}", ex.InnerException?.Message ?? ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);

            }
       
        }
        public async override Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Received UpdateDiscount request: {@Request}", request);

            var coupon = request.Coupon.Adapt<Coupon>();

            if (coupon == null || coupon.Id == 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request: Coupon ID is required."));
            }

            var existingCoupon = await _context.Coupons.FindAsync(coupon.Id);
            if (existingCoupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, "Coupon not found"));
            }
            existingCoupon.ProductName = coupon.ProductName;
            existingCoupon.Description = coupon.Description;
            existingCoupon.Amount = coupon.Amount;
            await _context.SaveChangesAsync();

            var couponModel = existingCoupon.Adapt<CouponModel>();
            _logger.LogInformation("Discount is updated successfully");
            return couponModel;
        }
   
        

        public async override Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _context
               .Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
            if (coupon is null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName {request.ProductName} is not found."));
            }
            _context.Coupons.Remove(coupon);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Discount is deleted successfully");

            return new DeleteDiscountResponse { Success = true };
        }
    }
}
