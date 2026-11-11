using ExpozitieLab.Models;

namespace ExpozitieLab.Data
{
    // Nu modifica acest fișier.
    public static class DateInitiale
    {
        public static void Scrie(IServiceProvider servicii)
        {
            using (IServiceScope domeniu = servicii.CreateScope())
            {
                ExpozitieDbContext db = domeniu.ServiceProvider.GetRequiredService<ExpozitieDbContext>();
                try
                {
                    if (db.Lucrari.ToList().Count > 0)
                    {
                        return;
                    }
                    string[] titluri = { "Toamna la Cahul", "Podul vechi", "Natură statică", "Lacul Beleu", "Portret de bunic", "Sat în ninsoare" };
                    string[] autori = { "Ana Cebotari", "Dan Rotaru", "Elena Munteanu", "Mihai Popa", "Irina Sandu", "Victor Botnaru" };
                    for (int i = 0; i < titluri.Length; i++)
                    {
                        Lucrare lucrare = new Lucrare();
                        lucrare.Titlu = titluri[i];
                        lucrare.Autor = autori[i];
                        lucrare.Grupa = "AAW" + (2341 + i % 2);
                        lucrare.An = 2024 + i % 3;
                        db.Lucrari.Add(lucrare);
                    }
                    db.SaveChanges();
                }
                catch (Microsoft.Data.Sqlite.SqliteException)
                {
                    Console.WriteLine("Baza de date nu există încă. Rulează Update-Database.");
                }
            }
        }
    }
}
