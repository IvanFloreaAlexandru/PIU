using System;
using System.Collections.Generic;
using System.IO;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareProgramari_FisierText
    {
        private readonly string _numeFisier;
        private List<Programare> _programari;
        private static readonly int indexIdProgramare = 0;
        private static readonly int indexIdPacient = 1;
        private static readonly int indexIdMedic = 2;
        private static readonly int indexDataOra = 3;
        private static readonly int indexDurata = 4;
        private static readonly int indexMotiv = 5;
        private static readonly int indexStatus = 6;

        public AdministrareProgramari_FisierText(string numeFisier)
        {
            _numeFisier = numeFisier;
            _programari = new List<Programare>();

            if (File.Exists(numeFisier))
            {
                IncarcaProgramariDinFisier();
            }
            else
            {
                File.Create(numeFisier).Close();
            }
        }

        public void AddProgramare(Programare programare)
        {
            _programari.Add(programare);
            SalveazaProgramariInFisier();
        }

        public Programare[] GetProgramari(out int nrProgramari)
        {
            nrProgramari = _programari.Count;
            return _programari.ToArray();
        }

        public Programare GetProgramareDupaId(int idProgramare)
        {
            return _programari.Find(p => p.IdProgramare == idProgramare);
        }

        public Programare[] GetProgramariDupaPacient(int idPacient)
        {
            return _programari.FindAll(p => p.IdPacient == idPacient).ToArray();
        }

        public Programare[] GetProgramariDupaMedic(int idMedic)
        {
            return _programari.FindAll(p => p.IdMedic == idMedic).ToArray();
        }

        public Programare[] GetProgramariDupaData(DateTime data)
        {
            return _programari.FindAll(p => p.DataOra.Date == data.Date).ToArray();
        }

        public void UpdateProgramare(Programare programareActualizata)
        {
            var programareIndex = _programari.FindIndex(p => p.IdProgramare == programareActualizata.IdProgramare);
            if (programareIndex >= 0)
            {
                _programari[programareIndex] = programareActualizata;
                SalveazaProgramariInFisier();
            }
        }

        private void SalveazaProgramariInFisier()
        {
            using (StreamWriter writer = new StreamWriter(_numeFisier, false))
            {
                foreach (var programare in _programari)
                {
                    writer.WriteLine(programare.ConversieLaSir_PentruFisier());
                }
            }
        }

        private void IncarcaProgramariDinFisier()
        {
            using (StreamReader reader = new StreamReader(_numeFisier))
            {
                string linie;
                while ((linie = reader.ReadLine()) != null)
                {
                    string[] dateProgramare = linie.Split(',');

                    int idProgramare = Convert.ToInt32(dateProgramare[indexIdProgramare]);
                    int idPacient = Convert.ToInt32(dateProgramare[indexIdPacient]);
                    int idMedic = Convert.ToInt32(dateProgramare[indexIdMedic]);
                    DateTime dataOra = Convert.ToDateTime(dateProgramare[indexDataOra]);
                    TimeSpan durata = TimeSpan.Parse(dateProgramare[indexDurata]);
                    string motiv = dateProgramare[indexMotiv];
                    string status = dateProgramare[indexStatus];

                    Programare programare = new Programare(idProgramare, idPacient, idMedic, dataOra, durata, motiv, status);
                    _programari.Add(programare);
                }
            }
        }
    }
}
