using System;
using System.Collections.Generic;
using LibrarieModele;

namespace NivelStocareDate
{
    public class AdministrareProgramari_Memorie
    {
        private List<Programare> _programari;

        public AdministrareProgramari_Memorie()
        {
            _programari = new List<Programare>();
        }

        public void AddProgramare(Programare programare)
        {
            _programari.Add(programare);
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
            }
        }
    }
}
