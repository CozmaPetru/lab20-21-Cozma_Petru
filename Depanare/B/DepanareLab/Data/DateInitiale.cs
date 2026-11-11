using DepanareLab.Models;

namespace DepanareLab.Data
{
    public static class DateInitiale
    {
        public static void Scrie(IServiceProvider servicii)
        {
            try
            {
                using (IServiceScope domeniu = servicii.CreateScope())
                {
                    InventarDbContext db = domeniu.ServiceProvider.GetRequiredService<InventarDbContext>();
                    if (db.Echipamente.ToList().Count > 0)
                    {
                        return;
                    }
                    string[] denumiri = { "Laptop Dell", "Proiector Epson", "Switch TP-Link", "Imprimantă HP", "Monitor LG", "Router Cisco" };
                    decimal[] preturi = { 12500, 8400, 950, 3200, 2750, 4100 };
                    int[] ani = { 2022, 2019, 2021, 2020, 2023, 2024 };
                    for (int i = 0; i < denumiri.Length; i++)
                    {
                        Echipament echipament = new Echipament();
                        echipament.Denumire = denumiri[i];
                        echipament.Serie = "S" + (1000 + i);
                        echipament.Pret = preturi[i];
                        echipament.An = ani[i];
                        db.Echipamente.Add(echipament);
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception eroare)
            {
                Console.WriteLine("Datele de start nu au fost scrise: " + eroare.Message);
            }
        }
    }
}
