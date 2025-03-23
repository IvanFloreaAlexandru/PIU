using System;
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareUser_Memorie
    {
        private List<User> _users;
        private int nrUsers = 0;

        public AdministrareUser_Memorie()
        {
            _users = new List<User>();
        }

        public void AddUser(User user)
        {
            if (user.IdUser <= 0)
            {
                nrUsers++;
                user.IdUser = nrUsers;
            }
            else if (user.IdUser > nrUsers)
            {
                nrUsers = user.IdUser;
            }

            _users.Add(user);
        }

        public User[] GetUsers(out int nrUsers)
        {
            nrUsers = _users.Count;
            return _users.ToArray();
        }

        public User GetUserDupaId(int idUser)
        {
            return _users.Find(u => u.IdUser == idUser);
        }

        public void UpdateUser(User userActualizat)
        {
            var userIndex = _users.FindIndex(u => u.IdUser == userActualizat.IdUser);
            if (userIndex >= 0)
            {
                _users[userIndex] = userActualizat;
            }
        }

        public void RemoveUser(int idUser)
        {
            _users.RemoveAll(u => u.IdUser == idUser);
        }
    }
}