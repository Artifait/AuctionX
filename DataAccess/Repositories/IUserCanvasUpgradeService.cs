using System.Threading.Tasks;

namespace AucX.DataAccess.Repositories
{
    public interface IUserCanvasUpgradeService
    {
        Task<bool> UpgradeCanvasAsync(string userId);
    }
}
