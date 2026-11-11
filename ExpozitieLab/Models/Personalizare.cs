using System.Text;

namespace ExpozitieLab.Models
{
    // Datele tale personale se calculează din numele repozitoriului.
    // Nu modifica acest fișier.
    public static class Personalizare
    {
        public static string Repozitoriu = "lab20-21-exemplu";
        public static int Numar = 0;

        static Personalizare()
        {
            string nume = CitesteNumele();
            if (nume != "")
            {
                Repozitoriu = nume;
            }
            int suma = 0;
            for (int i = 0; i < Repozitoriu.Length; i++)
            {
                suma = suma + (i + 1) * (int)Repozitoriu[i];
            }
            Numar = suma % 1000;
        }

        // Limita de mărime pentru fișiere, în MB: 1, 2 sau 3.
        public static int LimitaMb
        {
            get { return 1 + Numar % 3; }
        }

        // A treia extensie permisă: .gif sau .webp.
        public static string ExtensieSuplimentara
        {
            get
            {
                if (Numar % 2 == 0)
                {
                    return ".gif";
                }
                return ".webp";
            }
        }

        // Proiectul de depanare: A, B, C sau D.
        public static string Depanare
        {
            get { return "ABCD".Substring(Numar % 4, 1); }
        }

        public static string Cod
        {
            get { return "V" + Numar.ToString("000"); }
        }

        private static string CitesteNumele()
        {
            DirectoryInfo folder = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (folder != null)
            {
                string cale = Path.Combine(folder.FullName, ".git", "config");
                if (File.Exists(cale))
                {
                    foreach (string rand in File.ReadAllLines(cale))
                    {
                        string text = rand.Trim();
                        if (text.StartsWith("url"))
                        {
                            string adresa = text.Substring(text.IndexOf('=') + 1).Trim().TrimEnd('/');
                            if (adresa.EndsWith(".git"))
                            {
                                adresa = adresa.Substring(0, adresa.Length - 4);
                            }
                            int pozitie = adresa.LastIndexOfAny(new char[] { '/', '\\', ':' });
                            return adresa.Substring(pozitie + 1);
                        }
                    }
                }
                folder = folder.Parent;
            }
            return "";
        }
    }
}
