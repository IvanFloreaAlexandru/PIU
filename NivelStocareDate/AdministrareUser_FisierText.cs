using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareUser_FisierText
    {
        private readonly string _numeFisier;
        private List<User> _users;
        private int nrUsers;

        public AdministrareUser_FisierText(string numeFisier)
        {
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            _numeFisier = Path.Combine(locatieFisierSolutie, numeFisier);
            _users = new List<User>();
            if (File.Exists(_numeFisier))
                IncarcaUtilizatoriDinFisier();
            else
                File.Create(_numeFisier).Close();
        }

        public void AddUser(User user)
        {
            _users.Add(user);
            SalveazaUsersInFisier();
        }

        public User[] GetUsers(out int nrUsers)
        {
            if (_users.Count == 0)
            {
                IncarcaUtilizatoriDinFisier();
            }

            nrUsers = _users.Count;
            return _users.ToArray();
        }


        public void UpdateUser(User userActualizat)
        {
            var userIndex = _users.FindIndex(u => u.IdUser == userActualizat.IdUser);
            if (userIndex >= 0)
            {
                _users[userIndex] = userActualizat;
                SalveazaUsersInFisier();
            }
        }

        public void SalveazaUsersInFisier()
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
            {
                foreach (var user in _users)
                {
                    writer.WriteLine($"{user.IdUser},{user.Nume},{user.Prenume},{user.Email},{user.Parola},{user.Rang}");
                }
            }
        }


        public void IncarcaUtilizatoriDinFisier()
        {
            _users.Clear();

            if (File.Exists(_numeFisier) && new FileInfo(_numeFisier).Length > 0)
            {
                var linii = File.ReadAllLines(_numeFisier);
                int maxId = 0;

                foreach (var linie in linii)
                {
                    var valori = linie.Split(',');
                    if (valori.Length == 6)
                    {
                        var user = new User
                        {
                            IdUser = int.Parse(valori[0]),
                            Nume = valori[1],
                            Prenume = valori[2],
                            Email = valori[3],
                            Parola = valori[4]
                        };

                        if (Enum.TryParse(valori[5], out RangUtilizator rang))
                        {
                            user.Rang = rang;
                        }

                        _users.Add(user);

                        if (user.IdUser > maxId)
                            maxId = user.IdUser;
                    }
                }

                nrUsers = maxId;
            }
        }
        public void RemoveUser(int idUser)
        {
            _users.RemoveAll(u => u.IdUser == idUser);

            SalveazaUsersInFisier();
        }


    }
}