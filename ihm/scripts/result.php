<?php

// Vérifier que la méthode HTTP utilisée est POST et que le champ "number" est défini
if ($_SERVER['REQUEST_METHOD'] === 'POST' && isset($_POST['number'])) {
    // URL du service calc (Python)
    $url = 'http://calc:80/api/calcul';

    // Validation et assainissement de l'entrée utilisateur
    $number = filter_var($_POST['number'], FILTER_SANITIZE_NUMBER_FLOAT, FILTER_FLAG_ALLOW_FRACTION);

    // Préparer les données à envoyer au service
    $data = ['valeur' => $number];

    // Configuration de la requête HTTP
    $options = array(
        'http' => array(
            "header" => "Content-Type: application/json\r\n",
            "method" => "POST",
            "content" => json_encode($data),
            'timeout' => 5 // Timeout pour éviter les blocages
        )
    );

    // Créer le contexte pour la requête
    $context = stream_context_create($options);

    // Effectuer la requête et gérer les erreurs
    try {
        $response = file_get_contents($url, false, $context);

        // Décoder la réponse JSON
        $result = json_decode($response, true);

        // Vérifier si la réponse contient une erreur
        if (isset($result['error'])) {
            // Gérer les erreurs et afficher un message utilisateur
            $error = "Erreur : " . htmlspecialchars($result['error']);
        } else {
            // Récupérer les données du résultat
            if (!isset($result['result'])) {
                throw new Exception("Résultat non disponible");
            }
        
            $isEven = $result['result']['isEven'];
            $isPerfect = $result['result']['isPerfect'];
            $isPrime = $result['result']['isPrime'];
            $syracuse = $result['result']['syracuse'];
        }
    } catch (Exception $e) {
        // Gérer les erreurs et afficher un message utilisateur
        $error = "Erreur : " . htmlspecialchars($e->getMessage());
    }
}
