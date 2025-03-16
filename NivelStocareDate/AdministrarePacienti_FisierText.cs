using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrarePacienti_FisierText
    {
        private readonly string _numeFisier;
        private List<Pacient> _pacienti;

        public AdministrarePacienti_FisierText(string numeFisier)
        {
            _numeFisier = numeFisier ?? throw new ArgumentException("Calea fisierului nu poate fi null sau goala.", nameof(numeFisier));

            _pacienti = new List<Pacient>();

            if (File.Exists(_numeFisier))
            {
                IncarcaPacientiDinFisier();
            }
            else
            {
                File.Create(_numeFisier).Close();
            }
        }

        public void AddPacient(Pacient pacient)
        {
            _pacienti.Add(pacient);
            SalveazaPacientiInFisier();
        }

        public Pacient[] GetPacienti(out int nrPacienti)
        {
            nrPacienti = _pacienti.Count;
            return _pacienti.ToArray();
        }

        public Pacient GetPacientDupaCNP(string cnp)
        {
            return _pacienti.Find(p => p.CNP == cnp);
        }

        public void UpdatePacient(Pacient pacientActualizat)
        {
            var pacientIndex = _pacienti.FindIndex(p => p.IdPacient == pacientActualizat.IdPacient);
            if (pacientIndex >= 0)
            {
                _pacienti[pacientIndex] = pacientActualizat;
                SalveazaPacientiInFisier();
            }
        }

        private void SalveazaPacientiInFisier()
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
            {
                foreach (var pacient in _pacienti)
                {
                    writer.WriteLine(pacient.ConversieLaSir_PentruFisier());
                }
            }
        }

        private void IncarcaPacientiDinFisier()
        {
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    Pacient pacient = new Pacient(linie);
                    _pacienti.Add(pacient);
                }
            }
        }
        public Pacient GetPacientDupaId(int idPacient)
        {
            return _pacienti.Find(p => p.IdPacient == idPacient);
        }
    }
}
