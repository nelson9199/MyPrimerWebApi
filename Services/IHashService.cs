using MyPrimerWebApi.Models;

namespace MyPrimerWebApi.Services
{
    public interface IHashService
    {
        HashResult HashData(string input);
        HashResult HashData(string input, string salt);

    }
}