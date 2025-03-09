using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LibrarieModele
{
    public enum Rang { Angajat, SefDepartament, DirectorSpital, Medic }

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
        public int DepartamentId { get; set; }

        public List<Programare> IstoricProgramari { get; set; } = new List<Programare>();
        public List<Prescriptie> IstoricPrescriptii { get; set; } = new List<Prescriptie>();

        public Utilizator()
        {
            Nume = Contact = Email = CNP = AsigurareMedicala = Adresa = string.Empty;
        }

        public string Info()
        {
            return $"Id: {Id}, Nume: {Nume}, Contact: {Contact}, Email: {Email}, CNP: {CNP}, Asigurare: {AsigurareMedicala}, MedicDeFamilieId: {MedicDeFamilieId}, Adresa: {Adresa}, Rang: {RangUtilizator}, DepartamentId: {DepartamentId}";
        }

        public bool ValidareEmail() => Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        public bool ValidareContact() => Regex.IsMatch(Contact, @"^07[0-9]{8}$");
        public bool ValidareCNP() => Regex.IsMatch(CNP, @"^[1-8][0-9]{12}$");
    }
}