from flask import Flask, request, jsonify
import requests

app = Flask(__name__)

@app.route('/api/calcul', methods=['POST'])
def calcul():
    data = request.json
    valeur = data.get("valeur", 0)

    # Appel au service C# (Projet C#)
    response = requests.post("http://int_db:80/api/process", json={"valeur": valeur * 2})
    
    return jsonify(response.json())

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80)