namespace LibrarieModele
{
    public class Departament
    {
        public int Id { get; set; }
        public string Nume { get; set; }

        public Departament()
        {
            Nume = string.Empty;
        }

        public Departament(int id, string nume)
        {
            Id = id;
            Nume = nume;
        }

        public string Info()
        {
            return $"Id: {Id} Nume: {Nume}";
        }
    }
}
