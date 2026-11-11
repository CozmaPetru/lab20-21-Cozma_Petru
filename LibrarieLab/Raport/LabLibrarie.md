# Raport · Laboratorul „Librăria Online”

**Numele și prenumele:** Cozma Petru  
**Grupa:** AAW2341  
**Data:** 11.11.2026

## Tabelul de testare

| Nr. | Ce am făcut | Ce trebuia să apară | Ce a apărut | ✓ / ✗ |
|---|---|---|---|---|
| 1 | `Update-Database` | `Done.` și fișierul `librarie.db` | A apărut mesajul „Done.” în consolă și s-a creat fișierul librarie.db în proiect | ✓ |
| 2 | Am adăugat o carte fără copertă | rândul nou în tabel | Cartea a fost adăugată și este afișată în tabel cu imaginea implicită „Fără copertă” | ✓ |
| 3 | Am trimis formularul gol | mesajele de validare | Au apărut mesajele de eroare în roșu: „Introduceți titlul”, „Introduceți autorul”, „Introduceți anul”, „Introduceți prețul” | ✓ |
| 4 | Am adăugat o carte cu copertă | coperta în tabel; fișierul în `wwwroot/incarcari` | Coperta a apărut pe prima coloană în tabel, iar fișierul cu nume GUID s-a salvat în wwwroot/incarcari | ✓ |
| 5 | Am deschis pagina de start | catalogul în carduri | La accesarea paginii principale s-a deschis direct catalogul cu cărțile aranjate câte 4 pe rând | ✓ |
| 6 | Am repornit aplicația | cărțile și copertele au rămas | Toate datele și imaginile coperțelor s-au păstrat intacte din baza de date SQLite | ✓ |

## Capturile

- S1: `Capturi/S1_migratie.png`
- S2: `Capturi/S2_tabel.png`
- S3: `Capturi/S3_coperta.png`
- S4: `Capturi/S4_carduri.png`
