using System;
using System.Collections.Generic;
using System.Linq;

namespace ClinicaApp.Services
{

    public class Departament
    {
        public int Id { get; set; }
        public string Nume { get; set; }
        public List<Utilizator> Angajati { get; set; } = new List<Utilizator>();
    }

    public class UserService
    {
        private List<Utilizator> utilizatori = new List<Utilizator>();
        private List<Departament> departamente = new List<Departament>();

        private void VerificaPermisiune(Utilizator utilizator, Rang rangMinim)
        {
            if (utilizator == null || utilizator.RangUtilizator < rangMinim)
                throw new UnauthorizedAccessException("Acces interzis!");
        }

        public void AdaugaUtilizator(Utilizator utilizator, Utilizator operatorCurent)
        {
            VerificaPermisiune(operatorCurent, Rang.SefDepartament);

            if (utilizatori.Any(u => u.Email == utilizator.Email || u.CNP == utilizator.CNP))
                throw new InvalidOperationException("Utilizatorul exista deja.");

            utilizatori.Add(utilizator);
            Console.WriteLine("Utilizator adaugat cu succes.");
        }

        public void StergeUtilizator(int utilizatorId, Utilizator operatorCurent)
        {
            VerificaPermisiune(operatorCurent, Rang.SefDepartament);

            var utilizator = utilizatori.FirstOrDefault(u => u.Id == utilizatorId);
            if (utilizator == null) throw new KeyNotFoundException("Utilizatorul nu a fost gasit.");

            if (operatorCurent.RangUtilizator == Rang.SefDepartament && utilizator.DepartamentId != operatorCurent.DepartamentId)
                throw new UnauthorizedAccessException("Nu poți sterge utilizatori din alt departament.");

            utilizatori.Remove(utilizator);
            Console.WriteLine("Utilizator sters cu succes.");
        }

        public void ModificaUtilizator(Utilizator utilizator, Utilizator operatorCurent, string numeNou, string contactNou)
        {
            VerificaPermisiune(operatorCurent, Rang.SefDepartament);

            if (operatorCurent.RangUtilizator == Rang.SefDepartament && utilizator.DepartamentId != operatorCurent.DepartamentId)
                throw new UnauthorizedAccessException("Nu poti modifica utilizatori din alt departament.");

            utilizator.Nume = numeNou;
            utilizator.Contact = contactNou;
            Console.WriteLine("Datele utilizatorului au fost actualizate.");
        }

        public void CreareDepartament(string numeDepartament, Utilizator operatorCurent)
        {
            VerificaPermisiune(operatorCurent, Rang.DirectorSpital);

            departamente.Add(new Departament { Id = departamente.Count + 1, Nume = numeDepartament });
            Console.WriteLine("Departament creat cu succes.");
        }

        public void ModificaDepartament(int idDepartament, string numeNou, Utilizator operatorCurent)
        {
            VerificaPermisiune(operatorCurent, Rang.DirectorSpital);

            var departament = departamente.FirstOrDefault(d => d.Id == idDepartament);
            if (departament == null) throw new KeyNotFoundException("Departamentul nu a fost gasit.");

            departament.Nume = numeNou;
            Console.WriteLine("Departamentul a fost actualizat.");
        }

        public void AfiseazaStatistici(Utilizator operatorCurent)
        {
            VerificaPermisiune(operatorCurent, Rang.DirectorSpital);

            Console.WriteLine($"Total utilizatori: {utilizatori.Count}");
            Console.WriteLine($"Total departamente: {departamente.Count}");

            foreach (var dep in departamente)
            {
                Console.WriteLine($"{dep.Nume}: {dep.Angajati.Count} angajati");
            }
        }
    }
}
