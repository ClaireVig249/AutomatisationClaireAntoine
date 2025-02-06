from flask import Flask, request, jsonify
import requests
# importation des fichiers de calculs
import calculs
import suiteSyracuse

app = Flask(__name__)

@app.route('/api/calcul', methods=['POST'])
def calcul():
    # Récupérer les données JSON de la requête
    data = request.json
    valeur = data.get("valeur", 0)
    
    # Fait appel aux fonctions de calculs.py et suiteSyracuse.py
    pair = calculs.est_pair_ou_impair(valeur)
    premier = calculs.est_premier(valeur)
    parfait = calculs.est_parfait(valeur)
    syracuse = suiteSyracuse.syracuse(valeur)

    try:
        # Appel au service C# (Projet C#)
        response = requests.post("http://int_db:80/api/process", json={"Number": valeur, "IsEven": pair, "IsPrime": premier, "IsPerfect": parfait, "Syracuse": syracuse})
        
        # Vérifier si la requête a réussi
        if response.status_code == 200:
            # Retourner la réponse JSON du service C#
            return jsonify(response.json()), 200
        else:
            # Retourner une erreur si la requête a échoué
            return jsonify({"error": "Erreur lors de l'appel au service C#", "status_code": response.status_code}), response.status_code
    except requests.exceptions.RequestException as e:
        # Gérer les erreurs de connexion ou de requête
        return jsonify({"error": "Erreur de connexion au service C#", "details": str(e)}), 500

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=80)