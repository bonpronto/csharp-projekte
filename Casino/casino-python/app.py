"""
Casino als Webseite: Python (Flask) liefert die Seite aus,
die Würfel-Logik kommt aus dem C#-Programm CasinoLogik.exe.

Start:   python app.py
Öffnen:  http://localhost:5000
"""
import json
import math
import os
import shutil
import subprocess
import sys

from flask import Flask, jsonify, render_template, request, session

VERSION = "0.2.0-test"
START_GUTHABEN = 500
GEWINN_FAKTOR = 2          # Auszahlung = Einsatz * Faktor

# Pfad zur fertig gebauten C#-Logik (per Umgebungsvariable CASINO_EXE änderbar)
BASIS = os.path.dirname(os.path.abspath(__file__))
CASINO_EXE = os.environ.get("CASINO_EXE", os.path.join(BASIS, "CasinoLogik.exe"))

app = Flask(__name__)
app.secret_key = "nur-fuer-den-test"   # für die Session (Guthaben pro Besucher)


def wuerfeln(versuche):
    """Ruft die C#-Logik auf und gibt das geparste JSON zurück."""
    befehl = [CASINO_EXE, str(versuche)]
    # Auf Linux/Mac braucht eine .NET-Framework-exe 'mono'
    if not sys.platform.startswith("win"):
        mono = shutil.which("mono")
        if mono and CASINO_EXE.lower().endswith(".exe"):
            befehl.insert(0, mono)

    ergebnis = subprocess.run(befehl, capture_output=True, text=True, timeout=10)
    if ergebnis.returncode != 0:
        raise RuntimeError("CasinoLogik meldet einen Fehler: " + ergebnis.stderr.strip())
    return json.loads(ergebnis.stdout)


def guthaben():
    return session.get("guthaben", START_GUTHABEN)


@app.route("/")
def start():
    return render_template("index.html", version=VERSION, start_guthaben=START_GUTHABEN)


@app.route("/api/konto")
def konto():
    return jsonify(guthaben=guthaben())


@app.route("/api/neustart", methods=["POST"])
def neustart():
    session["guthaben"] = START_GUTHABEN
    return jsonify(guthaben=START_GUTHABEN)


@app.route("/api/drehen", methods=["POST"])
def drehen():
    daten = request.get_json(silent=True) or {}

    # ---------- Eingaben prüfen (nie dem Browser vertrauen) ----------
    try:
        versuche = int(daten.get("versuche"))
        einsatz = float(daten.get("einsatz"))
    except (TypeError, ValueError):
        return jsonify(fehler="Ungültige Eingabe."), 400

    if versuche < 1 or versuche > 3:
        return jsonify(fehler="Bitte 1, 2 oder 3 Versuche wählen."), 400
    if not math.isfinite(einsatz) or einsatz <= 0:
        return jsonify(fehler="Ungültiger Einsatz. Bitte eine Zahl größer 0 eingeben."), 400
    if einsatz > guthaben():
        return jsonify(fehler="Der Einsatz ist höher als dein Guthaben."), 400

    # ---------- Würfeln (C#) ----------
    try:
        antwort = wuerfeln(versuche)
    except FileNotFoundError:
        return jsonify(fehler="CasinoLogik.exe nicht gefunden: " + CASINO_EXE), 500
    except Exception as fehler:
        return jsonify(fehler=str(fehler)), 500

    if "fehler" in antwort:
        return jsonify(fehler=antwort["fehler"]), 500

    # ---------- Konto berechnen (Python) ----------
    neues_guthaben = guthaben() - einsatz          # Einsatz sofort abziehen
    jackpot = any(antwort["gewonnen"])
    if jackpot:
        neues_guthaben += einsatz * GEWINN_FAKTOR
    session["guthaben"] = neues_guthaben

    return jsonify(
        ergebnis=antwort["ergebnis"],
        gewonnen=antwort["gewonnen"],
        jackpot=jackpot,
        guthaben=neues_guthaben,
    )


if __name__ == "__main__":
    # host="0.0.0.0": auch vom Handy im selben WLAN erreichbar
    app.run(host="0.0.0.0", port=5000, debug=False)
