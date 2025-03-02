using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ClinicaApp.Services
{
    public enum Rang
    {
        Medic,
        SefDepartament,
        DirectorSpital
    }

    public class Utilizator
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string CNP { get; set; }
        public string AsigurareMedicala { get; set; }
        public int MedicDeFamilieId { get; set; }
        public string Adresa { get; set; }
        public Rang RangUtilizator { get; set; }
        public int? DepartamentId { get; set; }
    
        public List<Programare> IstoricProgramari { get; set; } = new List<Programare>();
        public List<Prescriptie> IstoricPrescriptii { get; set; } = new List<Prescriptie>();

        public bool ValidareEmail()
        {
            return Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        public bool ValidareContact()
        {
            return Regex.IsMatch(Contact, @"^07[0-9]{8}$");
        }

        public bool ValidareCNP()
        {
            return Regex.IsMatch(CNP, @"^[1-8][0-9]{12}$");
        }
    }

    public class Programare
    {
        public int Id { get; set; }
        public DateTime DataSiOra { get; set; }
    }

    public class Prescriptie
    {
        public int Id { get; set; }
        public string TipPrescriptie { get; set; }
    }

    public class UtilizatorService
    {
        private List<Utilizator> utilizatori = new List<Utilizator>();

        public void AdaugaUtilizator(Utilizator user)
        {
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu poate fi null.");
                throw new ArgumentException("Utilizatorul nu poate fi null.");
            }

            if (!user.ValidareEmail() || !user.ValidareContact() || !user.ValidareCNP())
            {
                Console.WriteLine("Eroare: Datele utilizatorului nu sunt valide.");
                throw new ArgumentException("Email-ul, numarul de telefon sau CNP-ul nu sunt valide.");
            }

            if (utilizatori.Exists(u => u.Email == user.Email || u.Contact == user.Contact || u.CNP == user.CNP))
            {
                Console.WriteLine("Eroare: Utilizatorul exista deja cu acelasi email, numar de telefon sau CNP.");
                throw new InvalidOperationException("Utilizatorul exista deja cu acelasi email, numar de telefon sau CNP.");
            }

            utilizatori.Add(user);
            Console.WriteLine("Utilizator adaugat cu succes.");
        }

        public void ModificaDateUtilizator(Utilizator user, string numeNou, string contactNou)
        {
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu poate fi null.");
                throw new ArgumentException("Utilizatorul nu poate fi null.");
            }
            user.Nume = numeNou;
            user.Contact = contactNou;
            Console.WriteLine("Datele utilizatorului au fost actualizate.");
        }

        public void StergeUtilizator(int userId)
        {
            var user = utilizatori.Find(p => p.Id == userId);
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu a fost gasit.");
                throw new KeyNotFoundException("Utilizatorul nu a fost gasit.");
            }
            utilizatori.Remove(user);
            Console.WriteLine("Utilizator sters cu succes.");
        }

        public void AfiseazaIstoricProgramari(Utilizator user)
        {
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu poate fi null.");
                throw new ArgumentException("Utilizatorul nu poate fi null.");
            }

            Console.WriteLine("Istoric programari pentru " + user.Nume);
            foreach (var prog in user.IstoricProgramari)
            {
                Console.WriteLine($"Programare {prog.Id} la {prog.DataSiOra}");
            }
        }

        public void AfiseazaIstoricPrescriptii(Utilizator user)
        {
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu poate fi null.");
                throw new ArgumentException("Utilizatorul nu poate fi null.");
            }

            Console.WriteLine("Istoric prescriptii pentru " + user.Nume);
            foreach (var pres in user.IstoricPrescriptii)
            {
                Console.WriteLine($"Prescriptie {pres.Id} - Tip: {pres.TipPrescriptie}");
            }
        }

        public void GestionareAsigurari(Utilizator user, string asigurare)
        {
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu poate fi null.");
                throw new ArgumentException("Utilizatorul nu poate fi null.");
            }
            user.AsigurareMedicala = asigurare;
            Console.WriteLine("Asigurare medicala actualizata cu succes.");
        }

        public void AsociereMedicDeFamilie(Utilizator user, int medicFamilieId)
        {
            if (user == null)
            {
                Console.WriteLine("Eroare: Utilizatorul nu poate fi null.");
                throw new ArgumentException("Utilizatorul nu poate fi null.");
            }
            user.MedicDeFamilieId = medicFamilieId;
            Console.WriteLine("Medic de familie asociat cu succes.");
        }
    }

}
