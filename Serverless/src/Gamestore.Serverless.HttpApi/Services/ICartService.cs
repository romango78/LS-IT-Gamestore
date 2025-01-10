using Gamestore.Serverless.HttpApi.Models;

namespace Gamestore.Serverless.HttpApi.Services;

public interface ICartService
{
    Task SubmitAsync(Cart cart, CancellationToken cancellationToken);
}