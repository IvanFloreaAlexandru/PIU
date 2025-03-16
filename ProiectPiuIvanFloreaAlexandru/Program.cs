using System;
using System.Configuration;
using LibrarieModele;
using NivelStocareDate;
using System.Collections.Generic;

namespace ClinicaAPP
{
    class Program
    {
        static void Main()
        {
            string numeFisierPacienti = ConfigurationManager.AppSettings["NumeFisierPacienti"];
            string numeFisierMedici = ConfigurationManager.AppSettings["NumeFisierMedici"];
            string numeFisierProgramari = ConfigurationManager.AppSettings["NumeFisierProgramari"];
            string numeFisierPrescriptii = ConfigurationManager.AppSettings["NumeFisierPrescriptii"];
            string numeFisierDepartamente = ConfigurationManager.AppSettings["NumeFisierDepartamente"];

            AdministrarePacienti_FisierText adminPacienti = new AdministrarePacienti_FisierText(numeFisierPacienti);
            AdministrareMedici_FisierText adminMedici = new AdministrareMedici_FisierText(numeFisierMedici);
            AdministrareProgramari_FisierText adminProgramari = new AdministrareProgramari_FisierText(numeFisierProgramari);
            AdministrarePrescriptii_FisierText adminPrescriptii = new AdministrarePrescriptii_FisierText(numeFisierPrescriptii);
            AdministrareDepartamente_FisierText adminDepartamente = new AdministrareDepartamente_FisierText(numeFisierDepartamente);

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
                Console.WriteLine("1. Gestionare pacienti");
                Console.WriteLine("2. Gestionare medici");
                Console.WriteLine("3. Gestionare programari");
                Console.WriteLine("4. Gestionare prescriptii");
                Console.WriteLine("5. Gestionare departamente");
                Console.WriteLine("X. Iesire din aplicatie");
                Console.WriteLine("=========================================");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "1":
                        GestionarePacienti(adminPacienti, ref nrPacienti);

                        break;
                    case "2":
                        GestionareMedici(adminMedici, adminDepartamente, ref medicNou, ref nrMedici);
                        break;
                    case "3":
                        GestionareProgramari(adminProgramari, adminPacienti, adminMedici, ref programareNoua, ref nrProgramari);
                        break;
                    case "4":
                        GestionarePrescriptii(adminPrescriptii, adminPacienti, adminMedici, ref prescriptieNoua, ref nrPrescriptii);
                        break;
                    case "5":
                        GestionareDepartamente(adminDepartamente, ref departamentNou, ref nrDepartamente);
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

        private static void GestionarePacienti(AdministrarePacienti_FisierText adminPacienti, ref int nrPacienti)
        {
            string optiune;
            do
            {
                Console.WriteLine("\n--- Gestionare Pacienti ---");
                Console.WriteLine("C. Adaugare pacient nou");
                Console.WriteLine("A. Afisare toti pacientii");
                Console.WriteLine("I. Afisare informatii pacient dupa CNP");
                Console.WriteLine("U. Actualizare informatii pacient");
                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
                        Pacient pacientNou = CitirePacientTastatura();


                        int idPacient = ++nrPacienti;
                        pacientNou.IdPacient = idPacient;

                        adminPacienti.AddPacient(pacientNou);

                        Console.WriteLine("Pacient adaugat cu succes!");
                        break;

                    case "A":
                        Pacient[] pacienti = adminPacienti.GetPacienti(out nrPacienti);
                        AfisarePacienti(pacienti, nrPacienti);
                        break;

                    case "I":
                        Console.Write("Introduceti CNP-ul pacientului: ");
                        string cnp = Console.ReadLine();

                        Pacient pacientCautat = adminPacienti.GetPacientDupaCNP(cnp);
                        if (pacientCautat != null)
                        {
                            Pacient[] pacientiGasiti = new Pacient[] { pacientCautat };
                            AfisarePacienti(pacientiGasiti, nrPacienti);
                        }
                        else
                        {
                            Console.WriteLine("Nu exista pacient cu acest CNP!");
                        }
                        break;

                    case "U":
                        Console.Write("Introduceti CNP-ul pacientului pentru actualizare: ");
                        string cnpUpdate = Console.ReadLine();

                        Pacient pacientUpdate = adminPacienti.GetPacientDupaCNP(cnpUpdate);
                        if (pacientUpdate != null)
                        {
                            Pacient pacientActualizat = CitirePacientTastatura();
                            pacientActualizat.IdPacient = pacientUpdate.IdPacient;

                            adminPacienti.UpdatePacient(pacientActualizat);
                            Console.WriteLine("Pacient actualizat cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Nu exista pacient cu acest CNP!");
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




        private static void GestionareProgramari(AdministrareProgramari_FisierText adminProgramari, AdministrarePacienti_FisierText adminPacienti, AdministrareMedici_FisierText adminMedici, ref Programare programareNoua, ref int nrProgramari)
        {
            string optiune;
            do
            {
                Console.WriteLine("\n--- Gestionare Programari ---");
                Console.WriteLine("C. Creare programare noua");
                Console.WriteLine("A. Afisare toate programarile");
                Console.WriteLine("P. Afisare programari pentru un pacient");
                Console.WriteLine("M. Afisare programari pentru un medic");
                Console.WriteLine("D. Afisare programari pentru o data");
                Console.WriteLine("U. Actualizare status programare");
                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
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
                        adminProgramari.AddProgramare(programareNoua);

                        Console.WriteLine("Programare creata cu succes!");
                        break;
                    case "A":
                        Programare[] programari = adminProgramari.GetProgramari(out nrProgramari);
                        AfisareProgramari(programari, nrProgramari, adminPacienti, adminMedici);
                        break;
                    case "P":
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
                        break;
                    case "M":
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
                        break;
                    case "D":
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
                        break;
                    case "U":
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
                            adminProgramari.UpdateProgramare(programareUpdate);
                            Console.WriteLine("Status programare actualizat cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Nu exista programare cu acest ID!");
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
            do
            {
                Console.WriteLine("\n--- Gestionare Prescriptii ---");
                Console.WriteLine("C. Creare prescriptie noua");
                Console.WriteLine("A. Afisare toate prescriptiile");
                Console.WriteLine("P. Afisare prescriptii pentru un pacient");
                Console.WriteLine("M. Afisare prescriptii emise de un medic");
                Console.WriteLine("D. Afisare prescriptii pentru o data");
                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
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
                        adminPrescriptii.AddPrescriptie(prescriptieNoua);

                        Console.WriteLine("Prescriptie creata cu succes!");
                        break;
                    case "A":
                        Prescriptie[] prescriptii = adminPrescriptii.GetPrescriptii(out nrPrescriptii);
                        AfisarePrescriptii(prescriptii, nrPrescriptii, adminPacienti, adminMedici);
                        break;
                    case "P":
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
                        break;
                    case "M":
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
                        break;
                    case "D":
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
                        break;
                    case "R":
                        return;
                    default:
                        Console.WriteLine("Optiune inexistenta. incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }

        private static void GestionareDepartamente(AdministrareDepartamente_FisierText adminDepartamente, ref Departament departamentNou, ref int nrDepartamente)
        {
            string optiune;
            Departament[] departamente = new Departament[0];
            do
            {
                Console.WriteLine("\n--- Gestionare Departamente ---");
                Console.WriteLine("C. Creare departament nou");
                Console.WriteLine("A. Afisare toate departamentele");
                Console.WriteLine("U. Actualizare informatii departament");
                Console.WriteLine("R. Revenire la meniul principal");
                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();
                switch (optiune.ToUpper())
                {
                    case "C":
                        departamente = adminDepartamente.GetDepartamente(out nrDepartamente);
                        AdministrareDepartamente_Memorie.AfisareDepartamente(departamente, nrDepartamente);
                        departamentNou = CitireDepartamentTastatura();
                        int idDepartament = ++nrDepartamente;
                        departamentNou.IdDepartament = idDepartament;
                        adminDepartamente.AddDepartament(departamentNou);
                        Console.WriteLine("Departament creat cu succes!");
                        break;
                    case "A":
                        departamente = adminDepartamente.GetDepartamente(out nrDepartamente);
                        AdministrareDepartamente_Memorie.AfisareDepartamente(departamente, nrDepartamente);
                        break;
                    case "U":
                        Console.Write("Introduceti ID-ul departamentului pentru actualizare: ");
                        int idDepartamentUpdate = Convert.ToInt32(Console.ReadLine());
                        Departament departamentUpdate = adminDepartamente.GetDepartamentDupaId(idDepartamentUpdate);
                        if (departamentUpdate != null)
                        {
                            Departament departamentActualizat = CitireDepartamentTastatura();
                            departamentActualizat.IdDepartament = departamentUpdate.IdDepartament;
                            adminDepartamente.UpdateDepartament(departamentActualizat);
                            Console.WriteLine("Departament actualizat cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Nu exista departament cu acest ID!");
                        }
                        break;
                    case "R":
                        return;
                    default:
                        Console.WriteLine("Optiune inexistenta. incercati din nou.");
                        break;
                }
            } while (optiune.ToUpper() != "R");
        }

        private static void GestionareMedici(AdministrareMedici_FisierText adminMedici, AdministrareDepartamente_FisierText adminDepartamente, ref Medic medicNou, ref int nrMedici)
        {
            string optiune;
            do
            {
                Console.WriteLine("\n--- Gestionare Medici ---");
                Console.WriteLine("C. Adaugare medic nou");
                Console.WriteLine("A. Afisare toti medicii");
                Console.WriteLine("S. Afisare medici dupa specialitate");
                Console.WriteLine("D. Afisare medici dupa departament");
                Console.WriteLine("U. Actualizare informatii medic");
                Console.WriteLine("R. Revenire la meniul principal");

                Console.Write("\nAlegeti o optiune: ");
                optiune = Console.ReadLine();

                switch (optiune.ToUpper())
                {
                    case "C":
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

                        int idMedic = ++nrMedici;
                        medicNou.IdMedic = idMedic;
                        adminMedici.AddMedic(medicNou);

                        Console.WriteLine("Medic adaugat cu succes!");
                        break;
                    case "A":
                        Medic[] medici = adminMedici.GetMedici(out nrMedici);
                        AfisareMedici(medici, nrMedici);
                        break;
                    case "S":
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
                        break;
                    case "D":
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
                        break;
                    case "U":
                        Console.Write("Introduceti ID-ul medicului pentru actualizare: ");
                        int idMedicUpdate = Convert.ToInt32(Console.ReadLine());

                        Medic medicUpdate = adminMedici.GetMedicDupaId(idMedicUpdate);
                        if (medicUpdate != null)
                        {
                            Medic medicActualizat = CitireMedicTastatura();
                            medicActualizat.IdMedic = medicUpdate.IdMedic;

                            adminMedici.UpdateMedic(medicActualizat);
                            Console.WriteLine("Medic actualizat cu succes!");
                        }
                        else
                        {
                            Console.WriteLine("Nu exista medic cu acest ID!");
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

            return new Medic(0, nume, prenume, cnp, specialitate, idDepartament, telefon, email, program); }


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
                Console.WriteLine($"{medici[i].IdMedic}. {medici[i].Nume} {medici[i].Prenume} ({medici[i].Specialitate})");
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
                Console.WriteLine($"{medici[i].IdMedic}. {medici[i].Nume} {medici[i].Prenume} ({medici[i].Specialitate})");
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

        public static void AfisarePacient(Pacient pacient)
        {
            if (pacient == null) return;

            Console.WriteLine($"\n--- Informatii pacient ID #{pacient.IdPacient} ---");
            Console.WriteLine(pacient.Info());

            string[] alergii = pacient.GetAlergii();
            Console.WriteLine("Alergii:");
            if (alergii != null && alergii.Length > 0)
            {
                foreach (string alergie in alergii)
                {
                    if (!string.IsNullOrEmpty(alergie.Trim()))
                    {
                        Console.WriteLine($"- {alergie.Trim()}");
                    }
                }
            }
            else
            {
                Console.WriteLine("- Nu are alergii cunoscute");
            }
        }

        public static void AfisarePacienti(Pacient[] pacienti, int nrPacienti)
        {
            if (pacienti == null || nrPacienti == 0)
            {
                Console.WriteLine("Nu exista pacienti inregistrati!");
                return;
            }

            Console.WriteLine("\n--- Lista pacientilor ---");
            for (int i = 0; i < nrPacienti; i++)
            {
                Console.WriteLine(pacienti[i].InfoScurt());
            }
        }

        public static void AfisareMedic(Medic medic)
        {
            if (medic == null) return;

            Console.WriteLine($"\n--- Informatii medic ID #{medic.IdMedic} ---");
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
                Console.WriteLine(medici[i].InfoScurt());
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

        public static void AfisareDepartament(Departament departament)
        {
            if (departament == null) return;

            Console.WriteLine($"\n--- Informatii departament ID #{departament.IdDepartament} ---");
            Console.WriteLine(departament.Info());
        }

        public static void AfisareDepartamente(Departament[] departamente, int nrDepartamente)
        {
            if (departamente == null || nrDepartamente == 0)
            {
                Console.WriteLine("Nu exista departamente inregistrate!");
                return;
            }

            Console.WriteLine("\n--- Lista departamentelor ---");
            for (int i = 0; i < nrDepartamente; i++)
            {
                Console.WriteLine(departamente[i].InfoScurt());
            }
        }

    }
}