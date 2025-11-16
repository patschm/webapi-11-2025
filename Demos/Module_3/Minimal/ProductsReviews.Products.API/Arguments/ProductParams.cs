namespace ProductsReviews.Products.API.Arguments;

public record struct ProductParams(int page = 1, int count = 10);
