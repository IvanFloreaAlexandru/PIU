using System;
using System.Configuration;
using LibrarieModele;
using NivelStocareDate;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GestionareSpital;

namespace ClinicaAPP
{
    class Program
    {
        private static User utilizatorCurent;

        static void Main()
        {

            utilizatorCurent = Autentificare();

            if (utilizatorCurent == null)
            {
                Console.WriteLine("Autentificare esuata. Aplicatia se va inchide...");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Bine ati venit, {utilizatorCurent.Nume} {utilizatorCurent.Prenume}! Rang: {utilizatorCurent.Rang}");

            string numeFisierPacienti = ConfigurationManager.AppSettings["NumeFisierPacienti"];
            string numeFisierMedici = ConfigurationManager.AppSettings["NumeFisierMedici"];
            string numeFisierProgramari = ConfigurationManager.AppSettings["NumeFisierProgramari"];
            string numeFisierPrescriptii = ConfigurationManager.AppSettings["NumeFisierPrescriptii"];
            string numeFisierDepartamente = ConfigurationManager.AppSettings["NumeFisierDepartamente"];
            string numeFisierUser = ConfigurationManager.AppSettings["NumeFisierUser"];

            AdministrarePacienti_FisierText adminPacienti = new AdministrarePacienti_FisierText(numeFisierPacienti);
            AdministrareMedici_FisierText adminMedici = new AdministrareMedici_FisierText(numeFisierMedici);
            AdministrareProgramari_FisierText adminProgramari = new AdministrareProgramari_FisierText(numeFisierProgramari);
            AdministrarePrescriptii_FisierText adminPrescriptii = new AdministrarePrescriptii_FisierText(numeFisierPrescriptii);
            AdministrareDepartamente_FisierText adminDepartamente = new AdministrareDepartamente_FisierText(numeFisierDepartamente);
            AdministrareUser_FisierText adminUser = new AdministrareUser_FisierText(numeFisierUser);

            Pacient pacientNou = new Pacient();
            Medic medicNou = new Medic();
            Programare programareNoua = new Programare();
            Prescriptie prescriptieNoua = new Prescriptie();
            Departament departamentNou = new Departament();

            int nrPacienti = 0;
            int nrMedici = 0;
            int nrProgramari = 0;
            int nrPrescriptii = 0;
            int nrDepartamente = 0;

            string optiune;
            do
            {
                Console.WriteLine("\n======= SISTEM MANAGEMENT CLINICA =======");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePacienti))
                    Console.WriteLine("1. Gestionare pacienti");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
                    Console.WriteLine("2. Gestionare medici");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                    Console.WriteLine("3. Gestionare programari");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                    Console.WriteLine("4. Gestionare prescriptii");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareDepartamente))
                    Console.WriteLine("5. Gestionare departamente");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareUtilizatori))
                    Console.WriteLine("6. Gestionare utilizatori");

                Console.WriteLine("X. Iesire din aplicatie");
                Console.WriteLine("=========================================");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "1":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePacienti))
                            GestionarePacienti(adminPacienti, adminUser, ref nrPacienti);
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "2":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
                            GestionareMedici(adminMedici, adminDepartamente, ref medicNou, ref nrMedici);
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "3":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                            GestionareProgramari(adminProgramari, adminPacienti, adminMedici, ref programareNoua, ref nrProgramari);
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "4":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                            GestionarePrescriptii(adminPrescriptii, adminPacienti, adminMedici, ref prescriptieNoua, ref nrPrescriptii);
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "5":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareDepartamente))
                            GestionareDepartamente(adminDepartamente, ref departamentNou, ref nrDepartamente);
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "6":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareUtilizatori))
                            GestionareUser();
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "X":
                        Console.WriteLine("Aplicatia se inchide...");
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta. Incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "X");
        }


        private static User Autentificare()
        {
            AdministrareUser_FisierText adminUser = new AdministrareUser_FisierText("User.txt");
            AdministrareUser_Memorie adminUserMemorie = new AdministrareUser_Memorie();

            int nrUtilizatori;
            var utilizatoriDinFisier = adminUser.GetUsers(out nrUtilizatori);

            if (nrUtilizatori == 0)
            {
                Console.WriteLine("Nu exista utilizatori in sistem. Se va crea un utilizator Director implicit.");

                User directorImplicit = new User
                {
                    IdUser = 1,
                    Nume = "Admin",
                    Prenume = "System",
                    Email = "admin@clinica.ro",
                    Parola = "admin",
                    Rang = RangUtilizator.Director
                };

                adminUser.AddUser(directorImplicit);
                Console.WriteLine("Utilizator Director creat. Va rugam sa va autentificati.");
                Console.WriteLine("Email: admin@clinica.ro, Parola: admin");
                Console.WriteLine("Dupa autentificare, va recomandam sa schimbati parola implicita.");
                Console.WriteLine();

                utilizatoriDinFisier = adminUser.GetUsers(out nrUtilizatori);
            }

            foreach (var user in utilizatoriDinFisier)
            {
                adminUserMemorie.AddUser(user);
            }

            int incercari = 0;
            const int MAX_INCERCARI = 3;

            while (incercari < MAX_INCERCARI)
            {
                Console.WriteLine("=== Autentificare ===");
                Console.Write("Email: ");
                string email = Console.ReadLine();
                Console.Write("Parola: ");
                string parola = Console.ReadLine();

                User utilizatorGasit = null;

                foreach (var user in utilizatoriDinFisier)
                {
                    if (user.Email == email && user.Parola == parola)
                    {
                        utilizatorGasit = user;
                        break;
                    }
                }

                if (utilizatorGasit != null)
                {
                    Console.WriteLine("Autentificare reusita!");
                    return utilizatorGasit;
                }

                incercari++;
                Console.WriteLine($"Email sau parola incorecte! Mai aveti {MAX_INCERCARI - incercari} incercari.");
            }

            Console.WriteLine("Prea multe incercari esuate. Aplicatia se va inchide.");
            return null;
        }


        private static void GestionareUser()
        {
            AdministrareUser_FisierText adminUser = new AdministrareUser_FisierText("User.txt");
            AdministrareUser_Memorie adminUserMemorie = new AdministrareUser_Memorie();

            int nrUtilizatori;
            var utilizatoriDinFisier = adminUser.GetUsers(out nrUtilizatori);

            foreach (var user in utilizatoriDinFisier)
            {
                adminUserMemorie.AddUser(user);
            }

            string optiune;
            do
            {
                Console.WriteLine("\n--- Gestionare Utilizatori ---");
                Console.WriteLine("C. Creare user nou");
                Console.WriteLine("M. Modificare user existent");
                Console.WriteLine("S. Stergere user");
                Console.WriteLine("A. Afisare toti utilizatorii");
                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
                        Console.Write("Introduceti numele: ");
                        string nume = Console.ReadLine();
                        Console.Write("Introduceti prenumele: ");
                        string prenume = Console.ReadLine();
                        Console.Write("Introduceti email-ul: ");
                        string email = Console.ReadLine();
                        Console.Write("Introduceti parola: ");
                        string parola = Console.ReadLine();

                        Console.WriteLine("Alegeti rangul: ");
                        foreach (RangUtilizator rang in Enum.GetValues(typeof(RangUtilizator)))
                        {
                            Console.WriteLine($"{(int)rang}. {rang}");
                        }
                        int rangInt;
                        while (!int.TryParse(Console.ReadLine(), out rangInt) || !Enum.IsDefined(typeof(RangUtilizator), rangInt))
                        {
                            Console.WriteLine("Optiune invalida, incercati din nou.");
                        }
                        RangUtilizator rangNou = (RangUtilizator)rangInt;

                        User userNou = new User
                        {
                            IdUser = adminUserMemorie.GetUsers(out _).Length + 1,
                            Nume = nume,
                            Prenume = prenume,
                            Email = email,
                            Parola = parola,
                            Rang = rangNou
                        };

                        adminUserMemorie.AddUser(userNou);
                        adminUser.AddUser(userNou);

                        Console.WriteLine("User creat cu succes!");
                        break;

                    case "M":
                        Console.Write("Introduceti ID-ul userului de modificat: ");
                        int idUserModificat;
                        if (!int.TryParse(Console.ReadLine(), out idUserModificat))
                        {
                            Console.WriteLine("ID invalid.");
                            break;
                        }

                        User userExistent = adminUserMemorie.GetUserDupaId(idUserModificat);
                        if (userExistent == null)
                        {
                            Console.WriteLine("Userul nu exista!");
                            break;
                        }

                        Console.Write("Introduceti noul nume: ");
                        string numeModificat = Console.ReadLine();
                        Console.Write("Introduceti noul prenume: ");
                        string prenumeModificat = Console.ReadLine();
                        Console.Write("Introduceti noul email: ");
                        string emailModificat = Console.ReadLine();
                        Console.Write("Introduceti noua parola: ");
                        string parolaModificata = Console.ReadLine();

                        Console.WriteLine("Alegeti noul rang: ");
                        foreach (RangUtilizator rang in Enum.GetValues(typeof(RangUtilizator)))
                        {
                            Console.WriteLine($"{(int)rang}. {rang}");
                        }
                        int rangModificatInt;
                        while (!int.TryParse(Console.ReadLine(), out rangModificatInt) || !Enum.IsDefined(typeof(RangUtilizator), rangModificatInt))
                        {
                            Console.WriteLine("Optiune invalida, incercati din nou.");
                        }
                        RangUtilizator rangModificat = (RangUtilizator)rangModificatInt;

                        userExistent.Nume = numeModificat;
                        userExistent.Prenume = prenumeModificat;
                        userExistent.Email = emailModificat;
                        userExistent.Parola = parolaModificata;
                        userExistent.Rang = rangModificat;

                        adminUserMemorie.UpdateUser(userExistent);
                        adminUser.UpdateUser(userExistent);

                        Console.WriteLine("User modificat cu succes!");
                        break;

                    case "S":
                        Console.Write("Introduceti ID-ul userului de sters: ");
                        int idUserSters;
                        if (!int.TryParse(Console.ReadLine(), out idUserSters))
                        {
                            Console.WriteLine("ID invalid.");
                            break;
                        }

                        User userDeSters = adminUserMemorie.GetUserDupaId(idUserSters);
                        if (userDeSters == null)
                        {
                            Console.WriteLine("Userul nu exista!");
                            break;
                        }

                        adminUserMemorie.RemoveUser(idUserSters);
                        adminUser.RemoveUser(idUserSters);

                        Console.WriteLine("User sters cu succes!");
                        break;

                    case "A":
                        int nrUtilizatoriMemorie;
                        var usersMemorie = adminUserMemorie.GetUsers(out nrUtilizatoriMemorie);

                        if (nrUtilizatoriMemorie == 0)
                        {
                            Console.WriteLine("Nu exista utilizatori.");
                        }
                        else
                        {
                            Console.WriteLine("Utilizatori in memorie:");
                            foreach (var user in usersMemorie)
                            {
                                Console.WriteLine($"ID: {user.IdUser}, Nume: {user.Nume} {user.Prenume}, Email: {user.Email}, Rang: {user.Rang}");
                            }
                        }
                        break;

                    case "R":
                        return;

                    default:
                        Console.WriteLine("Optiune invalida, incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }


        private static void GestionarePacienti(AdministrarePacienti_FisierText adminPacienti, AdministrareUser_FisierText adminUser, ref int nrPacienti)
        {
            string optiune;
            string numeFisierPacienti = ConfigurationManager.AppSettings["NumeFisierPacienti"];
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisier = Path.Combine(locatieFisierSolutie, numeFisierPacienti);

            do
            {
                Console.WriteLine("\n--- Gestionare Pacienti ---");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugarePacienti))
                    Console.WriteLine("A. Adaugare pacient");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificarePacienti))
                    Console.WriteLine("M. Modificare pacient");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.StergerePacienti))
                    Console.WriteLine("S. Stergere pacient");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePacienti))
                    Console.WriteLine("V. Vizualizare pacienti");

                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "A":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugarePacienti))
                        {
                            Pacient pacientNou = CitirePacientTastatura();


                            int maxId = adminPacienti.GetMaxIdPacient();

                            pacientNou.IdPacient = maxId + 1;

                            nrPacienti = maxId + 1;

                            adminPacienti.AddPacient(pacientNou, caleCompletaFisier);
                            Console.WriteLine("Pacient adaugat cu succes!");

                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "M":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificarePacienti))
                        {
                            Console.Write("Introduceti CNP-ul pacientului pentru actualizare: ");
                            string cnpUpdate = Console.ReadLine();

                            Pacient pacientUpdate = adminPacienti.GetPacientDupaCNP(cnpUpdate);
                            if (pacientUpdate != null)
                            {
                                Console.WriteLine($"Pacient gasit pentru actualizare: ID: {pacientUpdate.IdPacient} - CNP: {pacientUpdate.CNP}");
                                Pacient pacientActualizat = CitirePacientTastatura();
                                pacientActualizat.IdPacient = pacientUpdate.IdPacient;

                                adminPacienti.UpdatePacient(pacientActualizat, caleCompletaFisier);
                                Console.WriteLine("Pacient actualizat cu succes!");
                            }
                            else
                            {
                                Console.WriteLine("Nu exista pacient cu acest CNP!");
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "S":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.StergerePacienti))
                        {
                            Console.Write("Introduceti CNP-ul pacientului pentru stergere: ");
                            string cnpDelete = Console.ReadLine();

                            Pacient pacientDelete = adminPacienti.GetPacientDupaCNP(cnpDelete);
                            if (pacientDelete != null)
                            {
                                Console.WriteLine($"Pacient gasit pentru stergere: ID: {pacientDelete.IdPacient} - CNP: {pacientDelete.CNP}");
                                Console.Write("Confirmati stergerea (D/N): ");
                                string confirmare = Console.ReadLine();

                                if (confirmare.ToUpper() == "D")
                                {
                                    adminPacienti.DeletePacient(pacientDelete, caleCompletaFisier);
                                    Console.WriteLine("Pacient sters cu succes!");
                                }
                                else
                                {
                                    Console.WriteLine("Operatiunea de stergere a fost anulata.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu exista pacient cu acest CNP!");
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "V":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePacienti))
                        {
                            Console.WriteLine("\n--- Optiuni de vizualizare ---");
                            Console.WriteLine("1. Afisare toti pacientii");
                            Console.WriteLine("2. Cautare pacient dupa CNP");
                            Console.Write("Alegeti optiunea: ");
                            string optiuneVizualizare = Console.ReadLine();

                            switch (optiuneVizualizare)
                            {
                                case "1":
                                    Pacient[] pacienti = adminPacienti.GetPacienti(out nrPacienti);
                                    AfisarePacienti(pacienti, nrPacienti);
                                    break;

                                case "2":
                                    Console.Write("Introduceti CNP-ul pacientului: ");
                                    string cnp = Console.ReadLine();

                                    Pacient pacientCautat = adminPacienti.GetPacientDupaCNP(cnp);
                                    if (pacientCautat != null)
                                    {
                                        Pacient[] pacientiGasiti = new Pacient[] { pacientCautat };
                                        AfisarePacienti(pacientiGasiti, 1);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Nu exista pacient cu acest CNP!");
                                    }
                                    break;

                                default:
                                    Console.WriteLine("Optiune invalida!");
                                    break;
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "R":
                        return;

                    default:
                        Console.WriteLine("Optiune invalida, incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }





        private static void GestionareProgramari(AdministrareProgramari_FisierText adminProgramari, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici, ref Programare programareNoua, ref int nrProgramari)
        {
            string optiune;
            string numeFisierProgramari = ConfigurationManager.AppSettings["NumeFisierProgramari"];
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisier = Path.Combine(locatieFisierSolutie, numeFisierProgramari);

            do
            {
                Console.WriteLine("\n--- Gestionare Programari ---");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareProgramari))
                    Console.WriteLine("C. Creare programare noua");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                {
                    Console.WriteLine("A. Afisare toate programarile");
                    Console.WriteLine("P. Afisare programari pentru un pacient");
                    Console.WriteLine("M. Afisare programari pentru un medic");
                    Console.WriteLine("D. Afisare programari pentru o data");
                }

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareProgramari))
                    Console.WriteLine("U. Actualizare status programare");

                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareProgramari))
                        {
                            int nrPacientiTemp, nrMediciTemp;
                            adminPacienti.GetPacienti(out nrPacientiTemp);
                            adminMedici.GetMedici(out nrMediciTemp);

                            if (nrPacientiTemp == 0 || nrMediciTemp == 0)
                            {
                                Console.WriteLine("Nu exista suficienti pacienti sau medici pentru a crea o programare!");
                                break;
                            }

                            programareNoua = CitireProgramareTastatura(adminPacienti, adminMedici);

                            int idProgramare = ++nrProgramari;
                            programareNoua.IdProgramare = idProgramare;
                            adminProgramari.AddProgramare(programareNoua, caleCompletaFisier);

                            Console.WriteLine("Programare creata cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "A":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                        {
                            Programare[] programari = adminProgramari.GetProgramari(out nrProgramari);
                            AfisareProgramari(programari, nrProgramari, adminPacienti, adminMedici);
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "P":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                        {
                            Console.Write("Introduceti CNP-ul pacientului: ");
                            string cnpPacient = Console.ReadLine();

                            Pacient pacient = adminPacienti.GetPacientDupaCNP(cnpPacient);
                            if (pacient != null)
                            {
                                Programare[] programariPacient = adminProgramari.GetProgramariDupaPacient(pacient.IdPacient);
                                if (programariPacient != null && programariPacient.Length > 0)
                                {
                                    Console.WriteLine($"Programari pentru pacientul {pacient.Nume} {pacient.Prenume}:");
                                    for (int i = 0; i < programariPacient.Length; i++)
                                    {
                                        if (programariPacient[i] != null)
                                        {
                                            AfisareProgramare(programariPacient[i], adminPacienti, adminMedici);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nu exista programari pentru acest pacient!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu exista pacient cu acest CNP!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "M":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                        {
                            Console.Write("Introducti ID-ul medicului: ");
                            int idMedic = Convert.ToInt32(Console.ReadLine());

                            Medic medic = adminMedici.GetMedicDupaId(idMedic);
                            if (medic != null)
                            {
                                Programare[] programariMedic = adminProgramari.GetProgramariDupaMedic(idMedic);
                                if (programariMedic != null && programariMedic.Length > 0)
                                {
                                    Console.WriteLine($"Programari pentru medicul {medic.Nume} {medic.Prenume}:");
                                    for (int i = 0; i < programariMedic.Length; i++)
                                    {
                                        if (programariMedic[i] != null)
                                        {
                                            AfisareProgramare(programariMedic[i], adminPacienti, adminMedici);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nu exista programari pentru acest medic!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu exista medic cu acest ID!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "D":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareProgramari))
                        {
                            Console.Write("Introduceti data (format: zz.ll.aaaa): ");
                            string dataString = Console.ReadLine();

                            if (DateTime.TryParse(dataString, out DateTime data))
                            {
                                Programare[] programariData = adminProgramari.GetProgramariDupaData(data);
                                if (programariData != null && programariData.Length > 0)
                                {
                                    Console.WriteLine($"Programari pentru data {data.ToShortDateString()}:");
                                    for (int i = 0; i < programariData.Length; i++)
                                    {
                                        if (programariData[i] != null)
                                        {
                                            AfisareProgramare(programariData[i], adminPacienti, adminMedici);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nu exista programari pentru aceasta data!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Format data invalid!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "U":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareProgramari))
                        {
                            Console.Write("Introduceti ID-ul programarii pentru actualizare: ");
                            int idProgramareUpdate = Convert.ToInt32(Console.ReadLine());

                            Programare programareUpdate = adminProgramari.GetProgramareDupaId(idProgramareUpdate);
                            if (programareUpdate != null)
                            {
                                Console.WriteLine("Status curent: " + programareUpdate.Status);
                                Console.WriteLine("Alegeti noul status:");
                                Console.WriteLine("1. Programat");
                                Console.WriteLine("2. Confirmat");
                                Console.WriteLine("3. In asteptare");
                                Console.WriteLine("4. Finalizat");
                                Console.WriteLine("5. Anulat");

                                int optiuneStatus = Convert.ToInt32(Console.ReadLine());
                                string nouStatus = "Programat";

                                switch (optiuneStatus)
                                {
                                    case 1: nouStatus = "Programat"; break;
                                    case 2: nouStatus = "Confirmat"; break;
                                    case 3: nouStatus = "In asteptare"; break;
                                    case 4: nouStatus = "Finalizat"; break;
                                    case 5: nouStatus = "Anulat"; break;
                                    default:
                                        Console.WriteLine("Optiune invalida. Status neschimbat.");
                                        break;
                                }

                                programareUpdate.Status = nouStatus;
                                adminProgramari.UpdateProgramare(programareUpdate, caleCompletaFisier);
                                Console.WriteLine("Status programare actualizat cu succes!");
                            }
                            else
                            {
                                Console.WriteLine("Nu exista programare cu acest ID!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "R":
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta. Incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }

        private static void GestionarePrescriptii(AdministrarePrescriptii_FisierText adminPrescriptii, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici, ref Prescriptie prescriptieNoua, ref int nrPrescriptii)
        {
            string optiune;
            string numeFisierPrescriptii = ConfigurationManager.AppSettings["NumeFisierPrescriptii"];
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisier = Path.Combine(locatieFisierSolutie, numeFisierPrescriptii);

            do
            {
                Console.WriteLine("\n--- Gestionare Prescriptii ---");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugarePrescriptii))
                    Console.WriteLine("C. Creare prescriptie noua");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                {
                    Console.WriteLine("A. Afisare toate prescriptiile");
                    Console.WriteLine("P. Afisare prescriptii pentru un pacient");
                    Console.WriteLine("M. Afisare prescriptii emise de un medic");
                    Console.WriteLine("D. Afisare prescriptii pentru o data");
                }

                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugarePrescriptii))
                        {
                            int nrPacientiTemp, nrMediciTemp;
                            adminPacienti.GetPacienti(out nrPacientiTemp);
                            adminMedici.GetMedici(out nrMediciTemp);

                            if (nrPacientiTemp == 0 || nrMediciTemp == 0)
                            {
                                Console.WriteLine("Nu exista suficienti pacienti sau medici pentru a crea o prescriptie!");
                                break;
                            }

                            prescriptieNoua = CitirePrescriptieTastatura(adminPacienti, adminMedici);

                            int idPrescriptie = ++nrPrescriptii;
                            prescriptieNoua.IdPrescriptie = idPrescriptie;
                            adminPrescriptii.AddPrescriptie(prescriptieNoua, caleCompletaFisier);

                            Console.WriteLine("Prescriptie creata cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "A":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                        {
                            Prescriptie[] prescriptii = adminPrescriptii.GetPrescriptii(out nrPrescriptii);
                            AfisarePrescriptii(prescriptii, nrPrescriptii, adminPacienti, adminMedici);
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "P":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                        {
                            Console.Write("Introduceti CNP-ul pacientului: ");
                            string cnpPacient = Console.ReadLine();

                            Pacient pacient = adminPacienti.GetPacientDupaCNP(cnpPacient);
                            if (pacient != null)
                            {
                                Prescriptie[] prescriptiiPacient = adminPrescriptii.GetPrescriptiiDupaPacient(pacient.IdPacient);
                                if (prescriptiiPacient != null && prescriptiiPacient.Length > 0)
                                {
                                    Console.WriteLine($"Prescriptii pentru pacientul {pacient.Nume} {pacient.Prenume}:");
                                    for (int i = 0; i < prescriptiiPacient.Length; i++)
                                    {
                                        if (prescriptiiPacient[i] != null)
                                        {
                                            AfisarePrescriptie(prescriptiiPacient[i], adminPacienti, adminMedici);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nu exista prescriptii pentru acest pacient!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu exista pacient cu acest CNP!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "M":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                        {
                            Console.Write("Introduceti ID-ul medicului: ");
                            int idMedic = Convert.ToInt32(Console.ReadLine());

                            Medic medic = adminMedici.GetMedicDupaId(idMedic);
                            if (medic != null)
                            {
                                Prescriptie[] prescriptiiMedic = adminPrescriptii.GetPrescriptiiDupaMedic(idMedic);
                                if (prescriptiiMedic != null && prescriptiiMedic.Length > 0)
                                {
                                    Console.WriteLine($"Prescriptii emise de medicul {medic.Nume} {medic.Prenume}:");
                                    for (int i = 0; i < prescriptiiMedic.Length; i++)
                                    {
                                        if (prescriptiiMedic[i] != null)
                                        {
                                            AfisarePrescriptie(prescriptiiMedic[i], adminPacienti, adminMedici);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nu exista prescriptii emise de acest medic!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nu exista medic cu acest ID!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "D":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizarePrescriptii))
                        {
                            Console.Write("Introduceti data (format: zz.ll.aaaa): ");
                            string dataString = Console.ReadLine();

                            if (DateTime.TryParse(dataString, out DateTime data))
                            {
                                Prescriptie[] prescriptiiData = adminPrescriptii.GetPrescriptiiDupaData(data);
                                if (prescriptiiData != null && prescriptiiData.Length > 0)
                                {
                                    Console.WriteLine($"Prescriptii pentru data {data.ToShortDateString()}:");
                                    for (int i = 0; i < prescriptiiData.Length; i++)
                                    {
                                        if (prescriptiiData[i] != null)
                                        {
                                            AfisarePrescriptie(prescriptiiData[i], adminPacienti, adminMedici);
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Nu exista prescriptii pentru aceasta data!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Format data invalid!");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        }
                        break;

                    case "R":
                        return;

                    default:
                        Console.WriteLine("Optiune inexistenta. Incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }

        private static void GestionareDepartamente(AdministrareDepartamente_FisierText adminDepartamente, ref Departament departamentNou, ref int nrDepartamente)
        {
            string optiune;
            do
            {
                Console.WriteLine("\n--- Gestionare Departamente ---");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareDepartamente))
                    Console.WriteLine("A. Adaugare departament");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareDepartamente))
                    Console.WriteLine("M. Modificare departament");

                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareDepartamente))
                    Console.WriteLine("V. Vizualizare departamente");

                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "A":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareDepartamente))
                        {
                            Departament[] departamente = adminDepartamente.GetDepartamente(out nrDepartamente);
                            AdministrareDepartamente_Memorie.AfisareDepartamente(departamente, nrDepartamente);
                            departamentNou = CitireDepartamentTastatura();
                            int idDepartament = ++nrDepartamente;
                            departamentNou.IdDepartament = idDepartament;

                            string numeFisierDepartamente = ConfigurationManager.AppSettings["NumeFisierDepartamente"];
                            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                            string caleCompletaFisier = Path.Combine(locatieFisierSolutie, numeFisierDepartamente);
                            AdministrareDepartamente_Memorie adminDepartamenteMemorie = new AdministrareDepartamente_Memorie(caleCompletaFisier);

                            adminDepartamenteMemorie.AddDepartament(departamentNou);
                            Console.WriteLine("Departament adaugat in memorie.");
                            adminDepartamente.AddDepartament(departamentNou);
                            Console.WriteLine("Departament adaugat in fisier.");
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "M":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareDepartamente))
                        {
                            string numeFisierDepartamente = ConfigurationManager.AppSettings["NumeFisierDepartamente"];
                            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                            string caleCompletaFisier = Path.Combine(locatieFisierSolutie, numeFisierDepartamente);
                            AdministrareDepartamente_Memorie adminDepartamenteMemorie = new AdministrareDepartamente_Memorie(caleCompletaFisier);

                            Console.Write("Introduceti ID-ul departamentului pentru actualizare: ");
                            int idDepartamentUpdate = Convert.ToInt32(Console.ReadLine());
                            Departament departamentUpdate = adminDepartamenteMemorie.GetDepartamentDupaId(idDepartamentUpdate);
                            if (departamentUpdate != null)
                            {
                                Console.WriteLine($"Departament gasit pentru actualizare: ID: {departamentUpdate.IdDepartament} - {departamentUpdate.Nume} - Locatie: {departamentUpdate.Locatie}");
                                Departament departamentActualizat = CitireDepartamentTastatura();
                                departamentActualizat.IdDepartament = departamentUpdate.IdDepartament;
                                adminDepartamenteMemorie.UpdateDepartament(departamentActualizat);
                                Console.WriteLine("Departament actualizat in memorie.");
                                adminDepartamente.UpdateDepartament(departamentActualizat);
                                Console.WriteLine("Departament actualizat in fisier.");
                            }
                            else
                            {
                                Console.WriteLine("Nu exista departament cu acest ID!");
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "V":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareDepartamente))
                        {
                            Departament[] departamente = adminDepartamente.GetDepartamente(out nrDepartamente);
                            AdministrareDepartamente_Memorie.AfisareDepartamente(departamente, nrDepartamente);
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;

                    case "R":
                        return;

                    default:
                        Console.WriteLine("Optiune invalida, incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }


        private static void GestionareMedici(AdministrareMedici_FisierText adminMedici, AdministrareDepartamente_FisierText adminDepartamente, ref Medic medicNou, ref int nrMedici)
        {
            string optiune;
            string numeFisierMedici = ConfigurationManager.AppSettings["NumeFisierMedici"];

            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string caleCompletaFisier = Path.Combine(locatieFisierSolutie, numeFisierMedici);

            adminMedici = new AdministrareMedici_FisierText(numeFisierMedici);

            Medic[] mediciExistenti = adminMedici.GetMedici(out nrMedici);
            int maxId = 0;

            if (mediciExistenti != null && mediciExistenti.Length > 0)
            {
                for (int i = 0; i < mediciExistenti.Length; i++)
                {
                    if (mediciExistenti[i] != null && mediciExistenti[i].IdUser > maxId)
                    {
                        maxId = mediciExistenti[i].IdUser;
                        Console.WriteLine($"Max ID: {maxId}");
                    }
                }
            }

            do
            {
                Console.WriteLine("\n--- Gestionare Medici ---");
                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareMedici))
                    Console.WriteLine("C. Adaugare medic nou");
                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
                {
                    Console.WriteLine("A. Afisare toti medicii");
                    Console.WriteLine("S. Afisare medici dupa specialitate");
                    Console.WriteLine("D. Afisare medici dupa departament");
                }
                if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareMedici))
                    Console.WriteLine("U. Actualizare informatii medic");
                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.AdaugareMedici))
                        {
                            int nrDepartamente;
                            Departament[] departamente = adminDepartamente.GetDepartamente(out nrDepartamente);

                            if (nrDepartamente == 0)
                            {
                                Console.WriteLine("Nu exista departamente! Adaugati mai intai un departament.");
                                break;
                            }

                            Console.WriteLine("Departamente disponibile:");
                            for (int i = 0; i < nrDepartamente; i++)
                            {
                                Console.WriteLine($"{departamente[i].IdDepartament}. {departamente[i].Nume}");
                            }

                            medicNou = CitireMedicTastatura();

                            maxId++;
                            medicNou.IdUser = maxId;

                            adminMedici.AddMedic(medicNou);
                            adminMedici.SalveazaMediciInFisier();

                            Console.WriteLine("Medic adaugat cu succes!");
                            nrMedici++;
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;
                    case "A":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
                        {
                            Medic[] medici = adminMedici.GetMedici(out nrMedici);
                            AfisareMedici(medici, nrMedici);
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;
                    case "S":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
                        {
                            Console.Write("Introduceti specialitatea: ");
                            string specialitate = Console.ReadLine();

                            Medic[] mediciSpecialitate = adminMedici.GetMediciDupaSpecialitate(specialitate);
                            if (mediciSpecialitate != null && mediciSpecialitate.Length > 0)
                            {
                                Console.WriteLine($"Medici cu specialitatea {specialitate}:");
                                for (int i = 0; i < mediciSpecialitate.Length; i++)
                                {
                                    if (mediciSpecialitate[i] != null)
                                    {
                                        AfisareMedic(mediciSpecialitate[i]);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Nu exista medici cu specialitatea {specialitate}!");
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;
                    case "D":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.VizualizareMedici))
                        {
                            Console.Write("Introduceti ID-ul departamentului: ");
                            int idDepartament = Convert.ToInt32(Console.ReadLine());

                            Medic[] mediciDepartament = adminMedici.GetMediciDupaDepartament(idDepartament);
                            if (mediciDepartament != null && mediciDepartament.Length > 0)
                            {
                                Departament departament = adminDepartamente.GetDepartamentDupaId(idDepartament);
                                string numeDepartament = departament != null ? departament.Nume : "Necunoscut";

                                Console.WriteLine($"Medici din departamentul {numeDepartament}:");
                                for (int i = 0; i < mediciDepartament.Length; i++)
                                {
                                    if (mediciDepartament[i] != null)
                                    {
                                        AfisareMedic(mediciDepartament[i]);
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Nu exista medici in departamentul cu ID-ul {idDepartament}!");
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;
                    case "U":
                        if (VerificarePermisiuni.ArePermisiune(utilizatorCurent, Permisiuni.ModificareMedici))
                        {
                            Console.Write("Introduceti ID-ul medicului pentru actualizare: ");
                            int idMedicUpdate = Convert.ToInt32(Console.ReadLine());

                            Medic medicUpdate = adminMedici.GetMedicDupaId(idMedicUpdate);
                            if (medicUpdate != null)
                            {
                                Medic medicActualizat = CitireMedicTastatura();
                                medicActualizat.IdUser = medicUpdate.IdUser;

                                adminMedici.UpdateMedic(medicActualizat);
                                adminMedici.SalveazaMediciInFisier();

                                Console.WriteLine("Medic actualizat cu succes!");
                            }
                            else
                            {
                                Console.WriteLine("Nu exista medic cu acest ID!");
                            }
                        }
                        else
                            Console.WriteLine("Nu aveti permisiuni pentru aceasta actiune!");
                        break;
                    case "R":
                        return;
                    default:
                        Console.WriteLine("Optiune inexistenta. Incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");

            adminMedici.SalveazaMediciInFisier();
        }


        public static Pacient CitirePacientTastatura()
        {
            Console.WriteLine("\n--- Introducere date pacient ---");

            Console.Write("Nume: ");
            string nume = Console.ReadLine();

            Console.Write("Prenume: ");
            string prenume = Console.ReadLine();

            Console.Write("CNP: ");
            string cnp = Console.ReadLine();

            Console.Write("Data nasterii (zz.ll.aaaa): ");
            DateTime dataNasterii;
            DateTime.TryParse(Console.ReadLine(), out dataNasterii);

            Console.Write("Gen (M/F): ");
            string gen = Console.ReadLine();

            Console.Write("Adresa: ");
            string adresa = Console.ReadLine();

            Console.Write("Telefon: ");
            string telefon = Console.ReadLine();

            Console.Write("Email: ");
            string email = Console.ReadLine();

            Console.Write("Grupa sanguina: ");
            string grupaSanguina = Console.ReadLine();

            Console.Write("Alergii (separate prin virgula): ");
            string alergiiString = Console.ReadLine();
            string[] alergii = alergiiString.Split(',');

            Pacient pacient = new Pacient(0, nume, prenume, cnp, dataNasterii, gen, adresa, telefon, email, grupaSanguina);
            pacient.SetAlergii(alergii);

            return pacient;
        }

        private static Medic CitireMedicTastatura()
        {
            Console.Write("Introduceti numele medicului: ");
            string nume = Console.ReadLine();
            Console.Write("Introduceti prenumele medicului: ");
            string prenume = Console.ReadLine();
            Console.Write("Introduceti CNP-ul medicului: ");
            string cnp = Console.ReadLine();
            Console.Write("Introduceti specialitatea: ");
            string specialitate = Console.ReadLine();
            Console.Write("Introduceti ID-ul departamentului: ");
            int idDepartament = Convert.ToInt32(Console.ReadLine());
            Console.Write("Introduceti telefonul medicului: ");
            string telefon = Console.ReadLine();
            Console.Write("Introduceti email-ul medicului: ");
            string email = Console.ReadLine();
            Console.Write("Introduceti programul medicului: ");
            string program = Console.ReadLine();

            return new Medic(0, nume, prenume, cnp, specialitate, idDepartament, telefon, email, program, RangUtilizator.Medic);
        }

        public static Programare CitireProgramareTastatura(AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici)
        {
            Console.WriteLine("\n--- Introducere date programare ---");

            int nrPacienti;
            Pacient[] pacienti = adminPacienti.GetPacienti(out nrPacienti);

            Console.WriteLine("Pacienti disponibili:");
            for (int i = 0; i < nrPacienti; i++)
            {
                Console.WriteLine($"{pacienti[i].IdPacient}. {pacienti[i].Nume} {pacienti[i].Prenume} (CNP: {pacienti[i].CNP})");
            }

            Console.Write("Selectati ID-ul pacientului: ");
            int idPacient = Convert.ToInt32(Console.ReadLine());

            int nrMedici;
            Medic[] medici = adminMedici.GetMedici(out nrMedici);

            Console.WriteLine("Medici disponibili:");
            for (int i = 0; i < nrMedici; i++)
            {
                Console.WriteLine($"{medici[i].IdUser}. {medici[i].Nume} {medici[i].Prenume} ({medici[i].Specialitate})");
            }

            Console.Write("Selectati ID-ul medicului: ");
            int idMedic = Convert.ToInt32(Console.ReadLine());

            Console.Write("Data si ora programarii (zz.ll.aaaa hh:mm): ");
            DateTime dataOra;
            DateTime.TryParse(Console.ReadLine(), out dataOra);

            Console.Write("Durata (minute): ");
            int durata = Convert.ToInt32(Console.ReadLine());

            TimeSpan durataTimeSpan = TimeSpan.FromMinutes(durata);

            Console.Write("Motiv programare: ");
            string motiv = Console.ReadLine();

            Programare programare = new Programare(0, idPacient, idMedic, dataOra, durataTimeSpan, motiv, "Programat");

            return programare;
        }


        public static Prescriptie CitirePrescriptieTastatura(AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici)
        {
            Console.WriteLine("\n--- Introducere date prescriptie ---");

            int nrPacienti;
            Pacient[] pacienti = adminPacienti.GetPacienti(out nrPacienti);

            Console.WriteLine("Pacienti disponibili:");
            for (int i = 0; i < nrPacienti; i++)
            {
                Console.WriteLine($"{pacienti[i].IdPacient}. {pacienti[i].Nume} {pacienti[i].Prenume} (CNP: {pacienti[i].CNP})");
            }

            Console.Write("Selectati ID-ul pacientului: ");
            int idPacient = Convert.ToInt32(Console.ReadLine());

            int nrMedici;
            Medic[] medici = adminMedici.GetMedici(out nrMedici);

            Console.WriteLine("Medici disponibili:");
            for (int i = 0; i < nrMedici; i++)
            {
                Console.WriteLine($"{medici[i].IdUser}. {medici[i].Nume} {medici[i].Prenume} ({medici[i].Specialitate})");
            }

            Console.Write("Selectati ID-ul medicului: ");
            int idMedic = Convert.ToInt32(Console.ReadLine());

            Console.Write("Data prescriptiei (zz.ll.aaaa): ");
            DateTime data;
            DateTime.TryParse(Console.ReadLine(), out data);

            Console.Write("Diagnostic: ");
            string diagnostic = Console.ReadLine();

            Console.WriteLine("Introducere medicamente (introduceti 'gata' cand ati terminat):");
            List<string> medicamenteList = new List<string>();

            string medicament;
            do
            {
                Console.Write("Medicament (nume + dozaj + indicatii): ");
                medicament = Console.ReadLine();

                if (medicament.ToLower() != "gata")
                {
                    medicamenteList.Add(medicament);
                }
            } while (medicament.ToLower() != "gata");

            string[] medicamente = medicamenteList.ToArray();

            Console.Write("Indicatii: ");
            string indicatii = Console.ReadLine();

            Console.Write("Observatii: ");
            string observatii = Console.ReadLine();

            Prescriptie prescriptie = new Prescriptie(0, idPacient, idMedic, data, diagnostic, medicamente, indicatii, observatii);

            return prescriptie;
        }


        public static Departament CitireDepartamentTastatura()
        {
            Console.WriteLine("\n--- Introducere date departament ---");

            Console.Write("Nume departament: ");
            string nume = Console.ReadLine();

            Console.Write("Descriere: ");
            string descriere = Console.ReadLine();

            Console.Write("Locatie: ");
            string locatie = Console.ReadLine();

            Departament departament = new Departament(0, nume, descriere, locatie);

            return departament;
        }


        public static void AfisarePacienti(Pacient[] pacienti, int nrPacienti)
        {
            if (nrPacienti == 0)
            {
                Console.WriteLine("Nu exista pacienti in sistem.");
                return;
            }

            Console.WriteLine("--- Lista Pacientilor ---");
            foreach (var pacient in pacienti)
            {
                Console.WriteLine($"ID: {pacient.IdPacient} - Nume: {pacient.Nume} - CNP: {pacient.CNP}");
            }
        }


        public static void AfisareMedic(Medic medic)
        {
            if (medic == null) return;

            Console.WriteLine($"\n--- Informatii medic ID #{medic.IdUser} ---");
            Console.WriteLine(medic.Info());
        }

        public static void AfisareMedici(Medic[] medici, int nrMedici)
        {
            if (medici == null || nrMedici == 0)
            {
                Console.WriteLine("Nu exista medici inregistrati!");
                return;
            }

            Console.WriteLine("\n--- Lista medicilor ---");
            for (int i = 0; i < nrMedici; i++)
            {
                Console.WriteLine(medici[i].Info());
            }
        }

        public static void AfisareProgramare(Programare programare, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici)
        {
            if (programare == null) return;
            Pacient pacient = adminPacienti.GetPacientDupaId(programare.IdPacient);
            Medic medic = adminMedici.GetMedicDupaId(programare.IdMedic);

            string numePacient = (pacient != null) ? $"{pacient.Nume} {pacient.Prenume}" : "Pacient necunoscut";
            string numeMedic = (medic != null) ? $"{medic.Nume} {medic.Prenume}" : "Medic necunoscut";

            Console.WriteLine($"\n--- Informatii programare ID #{programare.IdProgramare} ---");
            Console.WriteLine($"Pacient: {numePacient}");
            Console.WriteLine($"Medic: {numeMedic}");
            Console.WriteLine($"Data si ora: {programare.DataOra}");
            Console.WriteLine($"Durata: {programare.Durata} minute");
            Console.WriteLine($"Motiv: {programare.Motiv}");
            Console.WriteLine($"Status: {programare.Status}");
        }

        public static void AfisareProgramari(Programare[] programari, int nrProgramari, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici)
        {
            if (programari == null || nrProgramari == 0)
            {
                Console.WriteLine("Nu exista programari inregistrate!");
                return;
            }

            Console.WriteLine("\n--- Lista programarilor ---");
            for (int i = 0; i < nrProgramari; i++)
            {
                AfisareProgramare(programari[i], adminPacienti, adminMedici);
            }
        }

        public static void AfisarePrescriptie(Prescriptie prescriptie, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici)
        {
            if (prescriptie == null) return;

            Pacient pacient = adminPacienti.GetPacientDupaId(prescriptie.IdPacient);

            Console.WriteLine($"ID pacient: {prescriptie.IdPacient}");

            Medic medic = adminMedici.GetMedicDupaId(prescriptie.IdMedic);

            string numePacient = (pacient != null) ? $"{pacient.Nume} {pacient.Prenume}" : "Pacient necunoscut";
            string numeMedic = (medic != null) ? $"{medic.Nume} {medic.Prenume}" : "Medic necunoscut";

            Console.WriteLine($"\n--- Informatii prescriptie ID #{prescriptie.IdPrescriptie} ---");
            Console.WriteLine($"Pacient: {numePacient}");
            Console.WriteLine($"Medic: {numeMedic}");
            Console.WriteLine($"Data: {prescriptie.DataEmitere.ToShortDateString()}");
            Console.WriteLine($"Diagnostic: {prescriptie.Diagnostic}");

            string[] medicamente = prescriptie.GetMedicamente();
            Console.WriteLine("Medicamente prescrise:");
            if (medicamente != null && medicamente.Length > 0)
            {
                foreach (string medicament in medicamente)
                {
                    if (!string.IsNullOrEmpty(medicament.Trim()))
                    {
                        Console.WriteLine($"- {medicament.Trim()}");
                    }
                }
            }
            else
            {
                Console.WriteLine("- Nu au fost prescrise medicamente");
            }

            Console.WriteLine($"Observatii: {prescriptie.Observatii}");
        }

        public static void AfisarePrescriptii(Prescriptie[] prescriptii, int nrPrescriptii, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici)
        {
            if (prescriptii == null || nrPrescriptii == 0)
            {
                Console.WriteLine("Nu exista prescriptii inregistrate!");
                return;
            }

            Console.WriteLine("\n--- Lista prescriptiilor ---");
            for (int i = 0; i < nrPrescriptii; i++)
            {
                AfisarePrescriptie(prescriptii[i], adminPacienti, adminMedici);
            }
        }


    }
}

