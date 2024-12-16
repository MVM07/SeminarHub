using Microsoft.EntityFrameworkCore;
using SeminarHub.Contracts;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Data.ValidationConstants;
using SeminarHub.Models.Category;
using SeminarHub.Models.Seminar;
using System.Globalization;

namespace SeminarHub.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly SeminarHubDbContext context;

        public SeminarService(SeminarHubDbContext _context)
        {
            context = _context;
        }

        public async Task AddSeminarAsync(SeminarFormViewModel model, string currentUserId)
        {
            DateTime seminarDate;

            if (!(DateTime.TryParseExact(model.DateAndTime, ValidationConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out seminarDate)))
            {
                throw new ArgumentException($"Incorrect date format. Date format should be {ValidationConstants.DateFormat}", nameof(model.DateAndTime));
            }

            Seminar seminar = new Seminar()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                DateAndTime = seminarDate,
                CategoryId = model.CategoryId,
                OrganiserId = currentUserId,
                Duration = model.Duration
            };

            await context.Seminars.AddAsync(seminar);
            await context.SaveChangesAsync();
        }

        public async Task DeleteSeminarAsync(string currentUserId, int id)
        {
            var seminarToDelete = await context.Seminars
                .FindAsync(id);

            if (seminarToDelete != null && seminarToDelete.OrganiserId == currentUserId)
            {
                context.Seminars.Remove(seminarToDelete);
                await context.SaveChangesAsync();
            }
        }

        public async Task EditEventAsync(SeminarFormViewModel model, int id, string currentUserId)
        {
            var seminarToEdit = await context.Seminars.FindAsync(id);

            if (seminarToEdit == null)
            {
                throw new ArgumentNullException("No such Seminar exist.");
            }

            if (currentUserId != seminarToEdit.OrganiserId)
            {
                throw new ArgumentException("You can not edit this Seminar.");
            }

            DateTime seminarDate;

            if (!(DateTime.TryParseExact(model.DateAndTime, ValidationConstants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out seminarDate)))
            {
                throw new ArgumentException($"Incorrect date format. Date format should be {ValidationConstants.DateFormat}", nameof(model.DateAndTime));
            }

            seminarToEdit.Topic = model.Topic;
            seminarToEdit.Lecturer = model.Lecturer;
            seminarToEdit.Details = model.Details;
            seminarToEdit.DateAndTime = seminarDate;
            seminarToEdit.CategoryId = model.CategoryId;
            seminarToEdit.OrganiserId = currentUserId;
            seminarToEdit.Duration = model.Duration;

            await context.SaveChangesAsync();            
        }

        public async Task<IEnumerable<SeminarAllViewModel>> GetAllSeminarsAsync()
        {
            var seminars = await context.Seminars
                .Select(s => new SeminarAllViewModel()
                {
                    Id = s.Id,
                    Topic = s.Topic,
                    Lecturer = s.Lecturer,
                    DateAndTime = s.DateAndTime,
                    Organizer = s.Organiser.UserName,
                    Category = s.Category.Name
                })
                .ToListAsync();

            return seminars;
        }

        public async Task<IEnumerable<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await context.Categories
                .Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<IEnumerable<SeminarJoinedViewModel>> GetJoinedSeminarsAsync(string currentUserId)
        {
            var joinedSeminars = await context.SeminarsParticipants
                .Where(sp => sp.ParticipantId == currentUserId)
                 .Select(s => new SeminarJoinedViewModel()
                 {
                     Id = s.SeminarId,
                     Topic = s.Seminar.Topic,
                     Lecturer = s.Seminar.Lecturer,
                     DateAndTime = s.Seminar.DateAndTime,
                     Organizer = s.Seminar.OrganiserId
                 })
                .ToListAsync();

            return joinedSeminars;
        }

        public async Task<SeminarDetailsViewModel> GetSeminarDetailsAsync(int id)
        {
            var seminar = await context.Seminars
                .Include(s => s.Category)
                .Include(s => s.Organiser)
                .Where(s => s.Id == id)
                .FirstOrDefaultAsync();

            if (seminar == null)
            {
                throw new ArgumentNullException("The Seminar doesn't exist");
            }

            SeminarDetailsViewModel model = new SeminarDetailsViewModel()
            {
                Id = seminar.Id,
                Topic = seminar.Topic,
                Lecturer = seminar.Lecturer,
                Duration = seminar.Duration,
                Details = seminar.Details,
                DateAndTime = seminar.DateAndTime.ToString(ValidationConstants.DateFormat),
                Category = seminar.Category.Name,
                Organizer = seminar.Organiser.UserName
            };

            return model;
        }

        public async Task<SeminarFormViewModel> GetSeminarToEditAsync(string currentUserId, int id)
        {
            var categories = await GetCategoriesAsync();

            var seminarToEdit = await context.Seminars
                .FindAsync(id);

            if (seminarToEdit == null)
            {
                throw new ArgumentNullException("The Seminar doesn't exist");
            }

            if (seminarToEdit.OrganiserId != currentUserId)
            {
                throw new Exception("You are not the organiser of this Seminar.");
            }

            SeminarFormViewModel model = new SeminarFormViewModel()
            {
                Topic = seminarToEdit.Topic,
                Lecturer = seminarToEdit.Lecturer,
                DateAndTime = seminarToEdit.DateAndTime.ToString(ValidationConstants.DateFormat),
                Details = seminarToEdit.Details,
                Duration = seminarToEdit.Duration,
                CategoryId = seminarToEdit.CategoryId,
                Categories = categories
            };

            return model;
        }

        public async Task JoinSeminarAsync(string currentUserId, int id)
        {
            var seminarToJoin = await context.SeminarsParticipants
                .Where(s => s.SeminarId == id && s.ParticipantId == currentUserId)
                .FirstOrDefaultAsync();

            if (seminarToJoin == null)
            {
                SeminarParticipant sp = new SeminarParticipant()
                {
                    SeminarId = id,
                    ParticipantId = currentUserId,
                };

                await context.SeminarsParticipants.AddAsync(sp);
                await context.SaveChangesAsync();
            }
        }

        public async Task LeaveSeminarAsync(string currentUserId, int id)
        {
            var seminarToLeave = await context.SeminarsParticipants
                .Where(sp => sp.SeminarId == id && sp.ParticipantId == currentUserId)
                .FirstOrDefaultAsync();

            if (seminarToLeave == null)
            {
                throw new ArgumentNullException("The Seminar doesn't exist");
            }

            context.SeminarsParticipants.Remove(seminarToLeave);
            await context.SaveChangesAsync();
        }
    }
}
