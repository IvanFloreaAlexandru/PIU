using System;
using System.Collections.Generic;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrarePacienti_Memorie
    {
        private List<Pacient> _pacienti;

        public AdministrarePacienti_Memorie()
        {
            _pacienti = new List<Pacient>();
        }

        public void AddPacient(Pacient pacient)
        {
            _pacienti.Add(pacient);
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

        public Pacient GetPacientDupaId(int idPacient)
        {
            return _pacienti.Find(p => p.IdPacient == idPacient);
        }

        public void UpdatePacient(Pacient pacientActualizat)
        {
            var pacientIndex = _pacienti.FindIndex(p => p.IdPacient == pacientActualizat.IdPacient);
            if (pacientIndex >= 0)
            {
                _pacienti[pacientIndex] = pacientActualizat;
            }
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
                Console.WriteLine(pacient.InfoScurt());
            }
        }
    }
}
