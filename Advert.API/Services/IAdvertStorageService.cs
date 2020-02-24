using Advert.API.Models;
using System.Threading.Tasks;

namespace Advert.API.Services
{
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel model);

        Task Confirm(ConfirmModel model);

        Task<bool> CheckHeathAsync();
    }
}
