using Agromatrix.Data;
using Agromatrix.Models;
using Microsoft.AspNetCore.Identity;

namespace Agromatrix.Services
{
    //Authentication service
    public class AuthService
    {
        private readonly ApplicationDbContext _db;
        private readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public bool IsUserAdmin { get; private set; } = false;

        // Event to notify state changes
        public event Action? OnChange;

        public AuthService(ApplicationDbContext db)
        {
            _db = db;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        //Login method
        public void Login(string username, string password)
        {
            IsUserAdmin = false;

            var user = _db.Users.SingleOrDefault(u => u.Username == username);
            if (user == null)
            {
                NotifyStateChanged();
                return;
            }

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result == PasswordVerificationResult.Success ||
                result == PasswordVerificationResult.SuccessRehashNeeded)
            {
                IsUserAdmin = user.IsAdmin;
            }

            NotifyStateChanged();
        }

        //Logout method
        public void Logout()
        {
            IsUserAdmin = false;
            NotifyStateChanged();
        }
    }
}
