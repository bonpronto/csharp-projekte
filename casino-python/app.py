import subprocess
import threading
import json
from http.server import BaseHTTPRequestHandler, HTTPServer

prozess = subprocess.Popen(
    ["Casino.exe"],
    stdin=subprocess.PIPE,
    stdout=subprocess.PIPE,
    stderr=subprocess.STDOUT,
    text=True,
    encoding="utf-8",
    bufsize=1
)

ausgabe_puffer = []
lock = threading.Lock()

def lese_output():
    while True:
        zeichen = prozess.stdout.read(1)
        if not zeichen:
            break
        with lock:
            ausgabe_puffer.append(zeichen)

threading.Thread(target=lese_output, daemon=True).start()

class CasinoServer(BaseHTTPRequestHandler):
    def do_GET(self):
        if self.path == "/":
            with open("index.html", "r", encoding="utf-8") as f:
                html = f.read()
            self.send_response(200)
            self.send_header("Content-type", "text/html; charset=utf-8")
            self.end_headers()
            self.wfile.write(html.encode("utf-8"))

        elif self.path == "/output":
            with lock:
                text = "".join(ausgabe_puffer)
                ausgabe_puffer.clear()
            self.send_response(200)
            self.send_header("Content-type", "application/json")
            self.end_headers()
            self.wfile.write(json.dumps({"text": text}).encode("utf-8"))

    def do_POST(self):
        if self.path == "/input":
            laenge = int(self.headers["Content-Length"])
            daten = json.loads(self.rfile.read(laenge))
            prozess.stdin.write(daten["eingabe"] + "\n")
            prozess.stdin.flush()
            self.send_response(200)
            self.end_headers()

    def log_message(self, format, *args):
        pass

print("Läuft auf http://localhost:8000")
HTTPServer(("localhost", 8000), CasinoServer).serve_forever()
