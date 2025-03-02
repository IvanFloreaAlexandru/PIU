using System;
using ClinicaApp.Services;

namespace ClinicaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UtilizatorService utilizatorService = new UtilizatorService();
                UserService clinicaService = new UserService();

                Utilizator director = new Utilizator
                {
                    Id = 1,
                    Nume = "Maria Ionescu",
                    Contact = "0722334455",
                    Email = "maria.ionescu@example.com",
                    CNP = "2871234567890",
                    Adresa = "Str. Independentei, Nr. 12",
                    AsigurareMedicala = "CAS 2024",
                    RangUtilizator = Rang.DirectorSpital
                };

                Utilizator sefDepartament = new Utilizator
                {
                    Id = 2,
                    Nume = "George Popa",
                    Contact = "0733445566",
                    Email = "george.popa@example.com",
                    CNP = "1804567890123",
                    Adresa = "Str. Mihai Viteazu, Nr. 45",
                    RangUtilizator = Rang.SefDepartament,
                    DepartamentId = 1
                };

                Utilizator medic = new Utilizator
                {
                    Id = 3,
                    Nume = "Ana Vasilescu",
                    Contact = "0744556677",
                    Email = "ana.vasilescu@example.com",
                    CNP = "2924567890123",
                    Adresa = "Str. Aviatorilor, Nr. 32",
                    RangUtilizator = Rang.Medic,
                    MedicDeFamilieId = 2,
                    DepartamentId = 1
                };

                clinicaService.AdaugaUtilizator(director, director);
                clinicaService.AdaugaUtilizator(sefDepartament, director);
                clinicaService.AdaugaUtilizator(medic, sefDepartament);

                clinicaService.ModificaUtilizator(medic, sefDepartament, "Ana Ionescu", "0766778899");

                utilizatorService.GestionareAsigurari(medic, "CAS 2025");
                utilizatorService.AsociereMedicDeFamilie(medic, 5);

                clinicaService.CreareDepartament("Cardiologie", director);
                clinicaService.ModificaDepartament(1, "Cardiologie Avansata", director);

                clinicaService.AfiseazaStatistici(director);

                medic.IstoricProgramari.Add(new Programare { Id = 101, DataSiOra = DateTime.Now });
                utilizatorService.AfiseazaIstoricProgramari(medic);

                medic.IstoricPrescriptii.Add(new Prescriptie { Id = 201, TipPrescriptie = "Antibiotic" });
                utilizatorService.AfiseazaIstoricPrescriptii(medic);

                try
                {
                    Utilizator duplicat = new Utilizator
                    {
                        Id = 4,
                        Nume = "George Popa",
                        Contact = "0733445566",
                        Email = "george.popa@example.com",
                        CNP = "1804567890123",
                        RangUtilizator = Rang.Medic
                    };
                    clinicaService.AdaugaUtilizator(duplicat, director);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eroare la adaugare utilizator duplicat: {ex.Message}");
                }

                clinicaService.StergeUtilizator(3, sefDepartament);

                try
                {
                    clinicaService.StergeUtilizator(99, director);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Eroare la stergere utilizator inexistent: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare generala: {ex.Message}");
            }
        }
    }
}
