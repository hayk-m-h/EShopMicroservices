namespace Discount.Grpc.Services;

public class DiscountService
    (DiscountContext _dbContext, ILogger<DiscountService> _logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon == null)
        {
            coupon = new Coupon{ ProductName = "No Discount", Amount = 0, Description = "No Discount desc" };
        }
        
        _logger.LogInformation("Getting coupon {CouponId}", coupon.Id);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));
        }
        
        _dbContext.Coupons.Add(coupon);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Created coupon {CouponId}", coupon.Id);

        var couponModel = coupon.Adapt<CouponModel>();
        
        return  couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Coupon is null"));
        }
        
        _dbContext.Coupons.Update(coupon);
        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Updated coupon {CouponId}", coupon.Id);

        var couponModel = coupon.Adapt<CouponModel>();
        
        return  couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon == null)
        {
            coupon = new Coupon{ ProductName = "No Discount", Amount = 0, Description = "No Discount desc" };
        }
        
        _dbContext.Coupons.Remove(coupon);

        await _dbContext.SaveChangesAsync();
        
        _logger.LogInformation("Deleting coupon {CouponId}", coupon.Id);

        return new DeleteDiscountResponse{ Success = true};
    }
}