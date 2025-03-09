using System;
using LibrarieModele;
using NivelStocareDate;

namespace ClinicaApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                UtilizatorService utilizatorService = new UtilizatorService();

                while (true)
                {
                    Console.WriteLine("\nMeniu:");
                    Console.WriteLine("1. Adauga utilizator");
                    Console.WriteLine("2. Afiseaza utilizatori");
                    Console.WriteLine("3. Cauta utilizator dupa ID");
                    Console.WriteLine("4. Iesire");
                    Console.Write("Alege o optiune: ");

                    string optiune = Console.ReadLine();

                    switch (optiune)
                    {
                        case "1":
                            AdaugaUtilizator(utilizatorService);
                            break;
                        case "2":
                            utilizatorService.AfiseazaUtilizatori();
                            break;
                        case "3":
                            CautaUtilizator(utilizatorService);
                            break;
                        case "4":
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Optiune invalida. Incercati din nou.");
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Eroare generala: {ex.Message}");
            }
        }

        private static void AdaugaUtilizator(UtilizatorService utilizatorService)
        {
            Console.WriteLine("Introduceti detaliile utilizatorului:");

            Console.Write("Nume: ");
            string nume = Console.ReadLine();

            Console.Write("Contact: ");
            string contact = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("CNP: ");
            string cnp = Console.ReadLine();

            Console.Write("Adresa: ");
            string adresa = Console.ReadLine();

            Console.Write("Asigurare Medicala: ");
            string asigurare = Console.ReadLine();

            Console.Write("Rang (0 - Angajat, 1 - SefDepartament, 2 - DirectorSpital, 3 - Medic): ");
            Rang rang = (Rang)Enum.Parse(typeof(Rang), Console.ReadLine());

            Utilizator utilizatorNou = new Utilizator
            {
                Id = new Random().Next(1, 1000),
                Nume = nume,
                Contact = contact,
                Email = email,
                CNP = cnp,
                Adresa = adresa,
                AsigurareMedicala = asigurare,
                RangUtilizator = rang
            };

            utilizatorService.AdaugaUtilizator(utilizatorNou);
        }

        private static void CautaUtilizator(UtilizatorService utilizatorService)
        {
            Console.Write("Introduceti un ID pentru cautare: ");
            int idCautat = int.Parse(Console.ReadLine());

            var utilizatorGasit = utilizatorService.CautaUtilizatorDupaId(idCautat);

            if (utilizatorGasit != null)
            {
                Console.WriteLine("Utilizator gasit:");
                Console.WriteLine(utilizatorGasit.Info());
            }
            else
            {
                Console.WriteLine("Utilizatorul nu a fost gasit.");
            }
        }
    }
}