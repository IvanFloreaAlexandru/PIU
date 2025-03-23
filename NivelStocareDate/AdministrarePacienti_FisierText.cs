using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrarePacienti_FisierText
    {
        private List<Pacient> _pacienti;
        private readonly string _numeFisier;

        public AdministrarePacienti_FisierText(string numeFisier)
        {
            _numeFisier = numeFisier ?? throw new ArgumentException("Calea fisierului nu poate fi null sau goala.", nameof(numeFisier));
            _pacienti = new List<Pacient>();

            string projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName;
            string filePath = Path.Combine(projectDirectory, numeFisier);

            if (File.Exists(filePath))
            {
                IncarcaPacientiDinFisier(filePath);
            }
            else
            {
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                File.Create(filePath).Close();
            }
        }



        public Pacient[] GetPacienti(out int nrPacienti)
        {
            nrPacienti = 0;
            List<Pacient> pacienti = new List<Pacient>();
            string locatieFisierSolutie = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string filePath = Path.Combine(locatieFisierSolutie, _numeFisier);
            Console.WriteLine(filePath);
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string linie;
                    while ((linie = reader.ReadLine()) != null)
                    {
                        Pacient pacient = new Pacient(linie);
                        pacienti.Add(pacient);
                        nrPacienti++;
                    }
                }
            }

            return pacienti.ToArray();
        }


        public Pacient GetPacientDupaCNP(string cnp)
        {
            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, _numeFisier);

            if (File.Exists(filePath))
            {
                List<Pacient> pacienti = IncarcaPacientiDinFisier(filePath);

                return pacienti.Find(p => p.CNP.Trim() == cnp.Trim());
            }
            return null;
        }


        public void UpdatePacient(Pacient pacientActualizat, string filePath)
        {
            List<Pacient> pacientiDinFisier = IncarcaPacientiDinFisier(filePath);

            var pacientIndex = pacientiDinFisier.FindIndex(p => p.IdPacient == pacientActualizat.IdPacient || p.CNP == pacientActualizat.CNP);

            if (pacientIndex >= 0)
            {
                pacientiDinFisier[pacientIndex] = pacientActualizat;

                SalveazaPacientiInFisier(filePath, pacientiDinFisier);

                _pacienti = pacientiDinFisier;

                Console.WriteLine("Pacient actualizat cu succes!");
            }
            else
            {
                Console.WriteLine("Pacientul nu a fost gasit pentru actualizare.");
            }
        }



        public Pacient GetPacientDupaId(int id)
        {
            Console.WriteLine($"Cautam pacientul cu ID {id}");

            string filePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, _numeFisier);
            List<Pacient> pacientiDinFisier = IncarcaPacientiDinFisier(filePath);

            foreach (var pacient in pacientiDinFisier)
            {
                if (pacient.IdPacient == id)
                {
                    return pacient;
                }
            }
            Console.WriteLine("Pacientul nu a fost gasit");
            return null;
        }




        public void AddPacient(Pacient pacient, string filePath)
        {
            List<Pacient> pacientiDinFisier = IncarcaPacientiDinFisier(filePath);

            if (!pacientiDinFisier.Exists(p => p.CNP == pacient.CNP))
            {
                pacientiDinFisier.Add(pacient);

                SalveazaPacientiInFisier(filePath, pacientiDinFisier);
                Console.WriteLine("Pacient adaugat cu succes!");
            }
            else
            {
                Console.WriteLine("Pacientul cu acest CNP exista deja.");
            }
        }

        private void SalveazaPacientiInFisier(string filePath, List<Pacient> pacienti)
        {
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (var pacient in pacienti)
                {
                    writer.WriteLine(pacient.ConversieLaSir_PentruFisier());
                }
            }
        }

        public List<Pacient> IncarcaPacientiDinFisier(string filePath)
        {
            List<Pacient> pacienti = new List<Pacient>();

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string linie;
                    while ((linie = reader.ReadLine()) != null)
                    {
                        Pacient pacient = new Pacient(linie);
                        pacienti.Add(pacient);
                    }
                }
            }
            return pacienti;
        }

        public void DeletePacient(Pacient pacient, string caleFisier)
        {
            if (!File.Exists(caleFisier))
            {
                Console.WriteLine("Fisierul nu exista!");
                return;
            }

            List<string> linii = File.ReadAllLines(caleFisier).ToList();
            string cnpPacient = pacient.CNP;

            int initialCount = linii.Count;
            linii = linii.Where(linie => !linie.Contains(cnpPacient)).ToList();

            if (linii.Count < initialCount)
            {
                File.WriteAllLines(caleFisier, linii);
                Console.WriteLine($"Pacientul cu CNP-ul {cnpPacient} a fost sters.");
            }
            else
            {
                Console.WriteLine("Pacientul nu a fost gasit in fisier.");
            }
        }
        public int GetMaxIdPacient()
        {
            Pacient[] pacienti = GetPacienti(out int nrPacienti);

            if (nrPacienti == 0)
                return 0;

            int maxId = 0;
            foreach (var pacient in pacienti)
            {
                if (pacient.IdPacient > maxId)
                    maxId = pacient.IdPacient;
            }

            return maxId;
        }


    }
}
