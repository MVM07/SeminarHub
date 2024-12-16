using SeminarHub.Models.Category;
using SeminarHub.Models.Seminar;

namespace SeminarHub.Contracts
{
    public interface ISeminarService
    {
        Task AddSeminarAsync(SeminarFormViewModel model, string currentUserId);

        Task<IEnumerable<SeminarAllViewModel>> GetAllSeminarsAsync();

        Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync();

        Task<IEnumerable<SeminarJoinedViewModel>> GetJoinedSeminarsAsync(string currentUserId);

        Task JoinSeminarAsync(string currentUserId, int id);

        Task<SeminarFormViewModel> GetSeminarToEditAsync(string currentUserId, int id);

        Task EditEventAsync(SeminarFormViewModel model, int id, string currentUserId);

        Task LeaveSeminarAsync(string currentUserId, int id);

        Task<SeminarDetailsViewModel> GetSeminarDetailsAsync(int id);

        Task DeleteSeminarAsync(string currentUserId, int id);
    }
}
