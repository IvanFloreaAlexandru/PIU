using System;

namespace LibrarieModele
{
    public class User
    {
        public int IdUser { get; set; }
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }
        public string Parola { get; set; }
        public RangUtilizator Rang { get; set; }

        public User()
        {
            IdUser = 0;
            Nume = string.Empty;
            Prenume = string.Empty;
            Email = string.Empty;
            Telefon = string.Empty;
            Rang = RangUtilizator.Pacient;
        }

        public User(int id, string nume, string prenume, string email, string telefon, RangUtilizator rang)
        {
            IdUser = id;
            Nume = nume;
            Prenume = prenume;
            Email = email;
            Telefon = telefon;
            Rang = rang;
        }


        public virtual string ConversieLaSir_PentruFisier()
        {
            return $"{IdUser},{Nume},{Prenume},{Email},{Telefon},{Rang}";
        }

        public virtual string Info()
        {
            return $"ID: {IdUser}\nNume: {Nume} {Prenume}\nEmail: {Email}\nTelefon: {Telefon}\nRang: {Rang}";
        }
    }
}
