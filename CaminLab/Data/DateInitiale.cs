using CaminLab.Models;

namespace CaminLab.Data
{
    // Nu modifica acest fișier. Tarifele depind de varianta ta.
    public static class DateInitiale
    {
        public static void Scrie(IServiceProvider servicii)
        {
            using (IServiceScope domeniu = servicii.CreateScope())
            {
                CaminDbContext db = domeniu.ServiceProvider.GetRequiredService<CaminDbContext>();
                try
                {
                    if (db.Camere.ToList().Count > 0)
                    {
                        return;
                    }
                    string[] numere = { "101", "102", "103", "201", "202", "203", "301", "302", "401", "402" };
                    int[] locuri = { 2, 3, 2, 4, 2, 3, 2, 4, 3, 2 };
                    for (int i = 0; i < numere.Length; i++)
                    {
                        Camera camera = new Camera();
                        camera.Numar = numere[i];
                        camera.Etaj = int.Parse(numere[i].Substring(0, 1));
                        camera.Locuri = locuri[i];
                        camera.Tarif = 400 + ((Personalizare.Numar + i * 7) % 9) * 50;
                        camera.Libera = (Personalizare.Numar + i) % 3 != 0;
                        db.Camere.Add(camera);
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
