using System;
using System.Collections.Generic;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class UtilizatorService
    {
        private readonly List<Utilizator> utilizatori = new List<Utilizator>();

        public void AdaugaUtilizator(Utilizator user)
        {
            if (user == null)
                throw new ArgumentException("Utilizatorul nu poate fi null.");

            if (!user.ValidareEmail() || !user.ValidareContact() || !user.ValidareCNP())
                throw new ArgumentException("Datele utilizatorului nu sunt valide.");

            if (utilizatori.Any(u => u.Email == user.Email || u.CNP == user.CNP))
                throw new InvalidOperationException("Utilizatorul exista deja.");

            utilizatori.Add(user);
            Console.WriteLine("Utilizator adaugat cu succes.");
        }

        public void ModificaDateUtilizator(Utilizator user, string numeNou, string contactNou)
        {
            if (user == null)
                throw new ArgumentException("Utilizatorul nu poate fi null.");

            user.Nume = numeNou;
            user.Contact = contactNou;
            Console.WriteLine("Datele utilizatorului au fost actualizate.");
        }

        public void StergeUtilizator(int userId)
        {
            var user = utilizatori.FirstOrDefault(p => p.Id == userId) ?? throw new KeyNotFoundException("Utilizatorul nu a fost gasit.");
            utilizatori.Remove(user);
            Console.WriteLine("Utilizator sters cu succes.");
        }

        public Utilizator CautaUtilizatorDupaId(int id) => utilizatori.FirstOrDefault(u => u.Id == id);
        public void AfiseazaUtilizatori() => utilizatori.ForEach(u => Console.WriteLine(u.Info()));
    }
}