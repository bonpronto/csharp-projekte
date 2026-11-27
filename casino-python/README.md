# 🎰 C# Casino – Browser-Version über Python

Ein textbasiertes Konsolen-Casino in **C#**, das über einen lokalen **Python-Webserver** mit einer **HTML/CSS-Oberfläche** im Browser spielbar gemacht wurde.

## 🧩 Funktionsweise (Architektur)

Das Projekt verbindet drei Technologien miteinander:
1. **C# (`Program.cs` / `Casino.exe`):** Enthält die komplette Spiellogik, die Zufallszahlen-Generierung (`Random`), die Guthabenverwaltung sowie die ASCII-Art-Ausgaben.
2. **Python (`app.py`):** Dient als lokaler Webserver (ohne externe Bibliotheken) und als Schnittstelle. Python startet die kompilierte `Casino.exe` im Hintergrund (`subprocess`) und leitet Ein- und Ausgaben zwischen dem C#-Programm und dem Browser weiter.
3. **HTML / CSS / JS (`index.html`):** Stellt das Spiel im Stil einer modernen Windows-Konsole im Browser dar und sendet Tastatureingaben über `fetch()` an den Python-Server.

---

## 📁 Projektstruktur

```text
├── Program.cs      # C#-Quellcode des Spiels
├── Casino.exe      # Kompilierte Windows-Anwendung
├── app.py          # Python-Webserver (Brücke zwischen C# und HTML)
├── index.html      # Benutzeroberfläche für den Browser
└── README.md       # Projektbeschreibung
```

---

## 🚀 Anleitung zum Starten

### Voraussetzungen
* **Windows** (zum Ausführen der `Casino.exe`)
* **Python 3** installiert (keine zusätzlichen Pakete wie Flask nötig)

### Schritte
1. Repository herunterladen oder klonen:
   ```bash
   git clone https://github.com
   ```
2. In den Projektordner wechseln, in dem `Casino.exe`, `app.py` und `index.html` liegen.
3. Den Python-Server über das Terminal / die Eingabeaufforderung starten:
   ```bash
   python app.py
   ```
4. Im Browser folgende Adresse öffnen:
   ```text
   http://localhost:8000
   ```

---

## 🎮 Spielregeln

* **Startguthaben:** 500 €
* **Versuche:** Pro Runde können **1 bis 3 Versuche** gewählt werden.
* **Symbole:** `*`, `~`, `#` (Gewinnsymbole) und `x` (Niete).
* **Gewinn:**
  * **3 gleiche Symbole** (außer `x`): **Jackpot** – Auszahlung `Einsatz × 3,5`
  * **Ist noch nicht verfügbar(2 gleiche Symbole**: Einsatz zurück)
  * **Niete (`x`) / ungültig:** Einsatz verloren
* Das Spiel läuft, bis das Guthaben auf **0 €** fällt, und startet anschließend eine neue Runde.
