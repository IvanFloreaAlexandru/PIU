using System;

namespace LibrarieModele
{
    public class Prescriptie
    {
        private static readonly int indexIdPrescriptie = 0;
        private static readonly int indexIdPacient = 1;
        private static readonly int indexIdMedic = 2;
        private static readonly int indexDataEmitere = 3;
        private static readonly int indexDiagnostic = 4;
        private static readonly int indexMedicamente = 5;
        private static readonly int indexIndicatii = 6;
        private static readonly int indexDescriere = 7;

        public int IdPrescriptie { get; set; }
        public int IdPacient { get; set; }
        public int IdMedic { get; set; }
        public DateTime DataEmitere { get; set; }
        public string Diagnostic { get; set; }
        public string [] Medicamente { get; set; }
        public string Indicatii { get; set; }
        public string Descriere { get; set; }
        public string Observatii { get; set; }

        public Prescriptie()
        {
            Diagnostic = string.Empty;
            Medicamente = new string[0];
            Indicatii = string.Empty;
            Descriere = string.Empty;
            Observatii = string.Empty;
        }


        public Prescriptie(int idPrescriptie, int idPacient, int idMedic, DateTime dataEmitere, string diagnostic, string[] medicamente, string indicatii, string descriere)
        {
            IdPrescriptie = idPrescriptie;
            IdPacient = idPacient;
            IdMedic = idMedic;
            DataEmitere = dataEmitere;
            Diagnostic = diagnostic;
            Medicamente = medicamente;
            Indicatii = indicatii;
            Descriere = descriere;
        }

        public Prescriptie(string linieFisier)
        {
            string[] datePrescriptie = linieFisier.Split(',');

            IdPrescriptie = Convert.ToInt32(datePrescriptie[indexIdPrescriptie]);
            IdPacient = Convert.ToInt32(datePrescriptie[indexIdPacient]);
            IdMedic = Convert.ToInt32(datePrescriptie[indexIdMedic]);
            DataEmitere = Convert.ToDateTime(datePrescriptie[indexDataEmitere]);
            Diagnostic = datePrescriptie[indexDiagnostic];
            Medicamente = datePrescriptie[indexMedicamente].Split(';');
            Indicatii = datePrescriptie[indexIndicatii];
            Descriere = datePrescriptie[indexDescriere];
        }

        public string ConversieLaSir_PentruFisier()
        {
            string medicamenteString = string.Join(";", Medicamente);
            return $"{IdPrescriptie},{IdPacient},{IdMedic},{DataEmitere:yyyy-MM-dd},{Diagnostic},{medicamenteString},{Indicatii},{Descriere}";
        }

        public string Info()
        {
            string medicamenteString = string.Join(", ", Medicamente);
            return $"ID: {IdPrescriptie}\nData emitere: {DataEmitere:dd.MM.yyyy}\nDiagnostic: {Diagnostic}\nMedicamente: {medicamenteString}\nIndicatii: {Indicatii}\nObservatii: {Observatii}";
        }

        public string[] GetMedicamente()
        {
            return Medicamente;
        }

        public void SetMedicamente(string[] medicamente)
        {
            Medicamente = medicamente;
        }
    }
}