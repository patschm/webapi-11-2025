namespace ProductsReviews.Reviews.API;

public record struct ReviewParams(int page = 1, int count = 10);
